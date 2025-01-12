using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PRUEBA_TECNICA_EY.Models;

namespace PRUEBA_TECNICA_EY.Interfaces
{
    public interface IScraper
    {
        string SourceName { get; }
        Task<List<SearchResult>> ScrapeAsync(string entityName);
    }
}