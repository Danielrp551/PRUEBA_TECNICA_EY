using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PRUEBA_TECNICA_EY.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace PRUEBA_TECNICA_EY.Data
{
    public class ApplicationDBContext : IdentityDbContext
    {
        public ApplicationDBContext(DbContextOptions dbContextOptions) : base(dbContextOptions)
        {
            
        }      

        //TABLAS 
        public DbSet<Proveedor> Proveedores { get; set; } 

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            List<IdentityRole> roles = new List<IdentityRole>
            {
                new IdentityRole
                {
                    Id = "650becbe-be41-4974-95c8-91f331564a36",
                    Name = "Admin",
                    NormalizedName = "ADMIN",
                    ConcurrencyStamp = "cstamp-admin-1"
                },
                new IdentityRole
                {
                    Id = "13a77ea5-4061-40d9-b6e0-5081c21db1e5",
                    Name = "User",
                    NormalizedName = "USER",
                    ConcurrencyStamp = "cstamp-user-1"
                }
            };

            builder.Entity<IdentityRole>().HasData(roles);
        } 
    }
}