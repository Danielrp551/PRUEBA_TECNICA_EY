using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PRUEBA_TECNICA_EY.Data;
using PRUEBA_TECNICA_EY.DTOs.Proveedor;
using PRUEBA_TECNICA_EY.Helpers;
using PRUEBA_TECNICA_EY.Interfaces;
using PRUEBA_TECNICA_EY.Models;

namespace PRUEBA_TECNICA_EY.Repository
{
    public class ProveedorRepository : IProveedorRepository
    {
        private readonly ApplicationDBContext _context;
        public ProveedorRepository(ApplicationDBContext context)
        {
            _context = context;
        }

        public async Task<Proveedor?> CreateAsync(Proveedor proveedor)
        {
            await _context.Proveedores.AddAsync(proveedor);
            await _context.SaveChangesAsync();
            return proveedor;
        }

        public async Task<Proveedor?> DeleteAsync(int id)
        {
            var proveedor = await _context.Proveedores.FirstOrDefaultAsync(p => p.Id == id);
            if(proveedor == null)
                return null;

            // Hard delete
            _context.Proveedores.Remove(proveedor);

            await _context.SaveChangesAsync();
            return proveedor;            
        }

        public async Task<List<Proveedor>> GetAllAsync(ProveedorQueryObject queryObject)
        {
            var proveedores = _context.Proveedores.AsQueryable();

            // Filtro por busqueda de texto - barra de busqueda
            if(!string.IsNullOrWhiteSpace(queryObject.SearchValue))
            {
                proveedores = proveedores.Where(p => p.RazonSocial.Contains(queryObject.SearchValue) || p.NombreComercial.Contains(queryObject.SearchValue));
            }

            // Filtro por paises
            if (queryObject.Paises != null && queryObject.Paises.Any())
            {
                proveedores = proveedores.Where(p => queryObject.Paises.Contains(p.Pais));
            }

            // Ordenarlos por fecha de ultima edicion
            proveedores = proveedores.OrderByDescending(p => p.FechaUltimaEdicion);

            var skipNumber = (queryObject.PageNumber - 1) * queryObject.PageSize;
            
            return await proveedores.Skip(skipNumber).Take(queryObject.PageSize).ToListAsync();
        }

        public async Task<Proveedor?> GetByIdAsync(int id)
        {
            return await _context.Proveedores.FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<Proveedor?> UpdateAsync(int id, UpdateProveedorRequestDto proveedor)
        {
            var proveedorUpdate = await _context.Proveedores.FirstOrDefaultAsync(p => p.Id == id);
            if(proveedorUpdate == null)
                return null;

            proveedorUpdate.RazonSocial = proveedor.RazonSocial;
            proveedorUpdate.NombreComercial = proveedor.NombreComercial;
            proveedorUpdate.IdentificacionTributaria = proveedor.IdentificacionTributaria;
            proveedorUpdate.NumeroTelefonico = proveedor.NumeroTelefonico;
            proveedorUpdate.CorreoElectronico = proveedor.CorreoElectronico;
            proveedorUpdate.SitioWeb = proveedor.SitioWeb;
            proveedorUpdate.DireccionFisica = proveedor.DireccionFisica;
            proveedorUpdate.Pais = proveedor.Pais;
            proveedorUpdate.FacturacionAnualUSD = proveedor.FacturacionAnualUSD;
            // Actualizar la fecha de ultima edicion
            proveedorUpdate.FechaUltimaEdicion = DateTime.Now;

            await _context.SaveChangesAsync();
            return proveedorUpdate;

        }
    }
}