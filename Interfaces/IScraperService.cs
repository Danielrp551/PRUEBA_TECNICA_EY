using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PRUEBA_TECNICA_EY.DTOs.Screening;

namespace PRUEBA_TECNICA_EY.Interfaces
{
    public interface IScraperService
    {
        Task<ScreeningResponseDto> ScreenEntityAsync(string entityName);      
    }
}