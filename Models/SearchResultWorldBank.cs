using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PRUEBA_TECNICA_EY.Models
{
    public class SearchResultWorldBank : SearchResult
    {
        public string FirmName { get; set; }
        public string Address { get; set; }
        public string Country { get; set; }
        public string fromDate { get; set; }
        public string ToDate { get; set; }
        public string Grounds { get; set; }
    }
}