using Application.Modules.B2C.DTOs;
using Fulbacho.Application.Modules.B2C.DTOs;
using Fulbacho.Application.Modules.B2C.Interfaces;
using Fulbacho.Shared;
using Fulbacho.Shared.Entities;
using Microsoft.EntityFrameworkCore;

namespace Fulbacho.Application.Modules.B2C.Services
{
    public class PredioService : IPredioService
    {
        private readonly FulbachoDbContext _context;

        public PredioService(FulbachoDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<PredioResponseDto>> ObtenerTodosLosPrediosAsync()
        {
            return await _context.Predios
                .Include(p => p.Zona)
                .Where(p => p.EsActivo)
                .Select(p => new PredioResponseDto
                {
                    Id = p.Id,
                    Nombre = p.Nombre,
                    Direccion = p.Direccion,
                    Zona = p.Zona != null ? p.Zona.Nombre : string.Empty
                })
                .ToListAsync();
        }

        public async Task<IEnumerable<PredioResponseDto>> FiltrarPrediosAsync(string? nombre, string? zona)
        {
            var query = _context.Predios
                .Include(p => p.Zona)
                .Where(p => p.EsActivo)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(zona))
                query = query.Where(p => p.Zona != null && p.Zona.Nombre.ToLower() == zona.ToLower());

            if (!string.IsNullOrWhiteSpace(nombre))
                query = query.Where(p => p.Nombre.ToLower().Contains(nombre.ToLower()));

            return await query
                .Select(p => new PredioResponseDto
                {
                    Id = p.Id,
                    Nombre = p.Nombre,
                    Direccion = p.Direccion,
                    Zona = p.Zona != null ? p.Zona.Nombre : string.Empty
                })
                .ToListAsync();
        }

        // Método privado para trazabilidad con UML
        // Se usa cuando el objeto Predio ya está cargado en memoria
        private static PredioResponseDto MapearAPredioResponseDto(Predio p) => new()
        {
            Id = p.Id,
            Nombre = p.Nombre,
            Direccion = p.Direccion,
            Zona = p.Zona?.Nombre ?? string.Empty
        };
    }
}