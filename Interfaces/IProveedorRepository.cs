using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PRUEBA_TECNICA_EY.DTOs.Proveedor;
using PRUEBA_TECNICA_EY.Helpers;
using PRUEBA_TECNICA_EY.Models;

namespace PRUEBA_TECNICA_EY.Interfaces
{
    public interface IProveedorRepository
    {
        Task<List<Proveedor>> GetAllAsync(ProveedorQueryObject queryObject);
        Task<Proveedor?> GetByIdAsync(int id);
        Task<Proveedor?> CreateAsync(Proveedor proveedor);
        Task<Proveedor?> UpdateAsync(int id, UpdateProveedorRequestDto proveedor);
        Task<Proveedor?> DeleteAsync(int id);
    }
}