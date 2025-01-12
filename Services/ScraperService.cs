using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PRUEBA_TECNICA_EY.DTOs.Screening;
using PRUEBA_TECNICA_EY.Interfaces;

namespace PRUEBA_TECNICA_EY.Services
{
    public class ScraperService : IScraperService
    {
        private readonly IEnumerable<IScraper> _scrapers;
        public ScraperService(IEnumerable<IScraper> scrapers)
        {
            _scrapers = scrapers;
        }

        public async Task<ScreeningResponseDto> ScreenEntityAsync(string entityName)
        {
            var tasks = _scrapers.Select(scraper => scraper.ScrapeAsync(entityName).ContinueWith(task => new
            {
                SourceName = scraper.SourceName,
                Results = task.Result
            }));

            var scraperResults = await Task.WhenAll(tasks);

            var sources = scraperResults.Select(sr => new SourceResultDto
            {
                SourceName = sr.SourceName,
                NumberOfHits = sr.Results.Count,
                Results = sr.Results
            }).ToList();

            return new ScreeningResponseDto
            {
                Sources = sources
            };
        }
    }
}