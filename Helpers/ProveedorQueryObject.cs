using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PRUEBA_TECNICA_EY.Helpers
{
    public class ProveedorQueryObject
    {
        public string SearchValue { get; set; } = "";
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;        
        public List<string>? Paises { get; set; } = null;   
        public string? SortBy { get; set; } = null;
        public bool IsDescending { get; set; } = false;
    }
}