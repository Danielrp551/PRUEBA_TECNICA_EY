using System.Threading.RateLimiting;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using PRUEBA_TECNICA_EY.Data;
using PRUEBA_TECNICA_EY.Identity;
using PRUEBA_TECNICA_EY.Interfaces;
using PRUEBA_TECNICA_EY.Repository;
using PRUEBA_TECNICA_EY.Scrapers;
using PRUEBA_TECNICA_EY.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Logging.ClearProviders();
builder.Logging.AddConsole();

builder.Services.AddControllers().AddNewtonsoftJson(options =>
    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
);

builder.Services.AddDbContext<ApplicationDBContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddIdentity<IdentityUser, IdentityRole>(options => {
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequiredLength = 12;
})
.AddEntityFrameworkStores<ApplicationDBContext>()
.AddErrorDescriber<SpanishIdentityErrorDescriber>();

// rate limiting
builder.Services.AddRateLimiter(options =>
{
    options.RejectionStatusCode = 429;

    options.AddPolicy("ScraperPolicy", context =>
        RateLimitPartition.GetFixedWindowLimiter(
            partitionKey: context.Request.Path.Value ?? string.Empty, // Se aplica a cada ruta individualmente
            factory: key => new FixedWindowRateLimiterOptions
            {
                PermitLimit = 20,                   // Máximo 20 solicitudes
                Window = TimeSpan.FromMinutes(1),     // Por cada 1 minuto
                QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
                QueueLimit = 0                        // No se encola ninguna solicitud extra
            }));
});

builder.Services.AddAuthentication(options => {
    options.DefaultAuthenticateScheme =
    options.DefaultChallengeScheme = 
    options.DefaultForbidScheme = 
    options.DefaultScheme = 
    options.DefaultSignInScheme =
    options.DefaultSignOutScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options => {
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidIssuer = builder.Configuration["JWT:Issuer"],
        ValidateAudience = true,
        ValidAudience = builder.Configuration["JWT:Audience"],
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(
            System.Text.Encoding.UTF8.GetBytes(builder.Configuration["JWT:SigningKey"])
        )
    };
});

// Inyectar dependencias
builder.Services.AddScoped<IProveedorRepository, ProveedorRepository>();
builder.Services.AddScoped<ITokenService, TokenService>();

// Scrapers
builder.Services.AddHttpClient<IScraper, CIIJScraper>(client =>
{
    client.BaseAddress = new Uri("https://offshoreleaks.icij.org/");
    client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (compatible; ScraperBot/1.0)");
});
builder.Services.AddScoped<IScraper, OFACScraper>();
builder.Services.AddScoped<IScraper, WorldBankScraper>();

builder.Services.AddScoped<IScraperService, ScraperService>();

// PARA PROBAR EL JWT EN SWAGGER
builder.Services.AddSwaggerGen(option =>
{
    option.SwaggerDoc("v1", new OpenApiInfo { Title = "Demo API", Version = "v1" });
    option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter a valid token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });
    option.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id="Bearer"
                }
            },
            new string[]{}
        }
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

if (app.Environment.IsProduction())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseRateLimiter();

//CORS
app.UseCors(x => x
    .AllowAnyHeader()
    .AllowAnyMethod()
    //.WithOrigins("http://localhost:3000")
    .AllowCredentials()
    .SetIsOriginAllowed(origin => true)
);

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
