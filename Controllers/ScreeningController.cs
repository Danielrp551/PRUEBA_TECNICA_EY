using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using PRUEBA_TECNICA_EY.DTOs.Screening;
using PRUEBA_TECNICA_EY.Interfaces;

namespace PRUEBA_TECNICA_EY.Controllers
{
    [Route("api/screening")]
    [ApiController]
    [EnableRateLimiting("ScraperPolicy")]
    public class ScreeningController : ControllerBase
    {
        private readonly IScraperService _scraperService;

        public ScreeningController(IScraperService scraperService)
        {
            _scraperService = scraperService;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Get([FromQuery]ScreeningRequestDto requestDto)
        {
            if (string.IsNullOrWhiteSpace(requestDto.EntityName))
            {
                return BadRequest("El nombre de la entidad es obligatorio.");
            }

            var response = await _scraperService.ScreenEntityAsync(requestDto.EntityName);
            return Ok(response);            
        }
    }
}