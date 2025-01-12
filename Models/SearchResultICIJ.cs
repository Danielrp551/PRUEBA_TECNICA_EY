using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PRUEBA_TECNICA_EY.Models
{
    public class SearchResultICIJ : SearchResult
    {
        public string Entity { get; set; }
        public string Jurisdiction { get; set; }
        public string LinkedTo { get; set; }
        public string DataFrom { get; set; }        
    }
}