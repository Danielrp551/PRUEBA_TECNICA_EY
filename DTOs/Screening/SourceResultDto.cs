using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PRUEBA_TECNICA_EY.Models;

namespace PRUEBA_TECNICA_EY.DTOs.Screening
{
    public class SourceResultDto
    {
        public string SourceName { get; set; }
        public int NumberOfHits { get; set; }
        public List<SearchResult> Results { get; set; }          
    }
}