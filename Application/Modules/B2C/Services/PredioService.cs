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
                    IdZona = p.IdZona,
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
                    IdZona = p.IdZona,
                    Zona = p.Zona != null ? p.Zona.Nombre : string.Empty
                })
                .ToListAsync();
        }

        public async Task<PredioResponseDto?> ObtenerPredioPorIdAsync(int id)
        {
            // Sólo predios activos, con sus canchas activas y los datos del tipo de cancha y superficie.
            var predio = await _context.Predios
                .Include(p => p.Zona)
                .Include(p => p.Canchas.Where(c => c.EsActivo))
                    .ThenInclude(c => c.TipoCancha)
                .Include(p => p.Canchas.Where(c => c.EsActivo))
                    .ThenInclude(c => c.Superficie)
                .FirstOrDefaultAsync(p => p.Id == id && p.EsActivo);

            if (predio == null) return null;

            return MapearADetalleConCanchas(predio);
        }

        // Método privado para trazabilidad con UML
        // Se usa cuando el objeto Predio ya está cargado en memoria
        private static PredioResponseDto MapearAPredioResponseDto(Predio p) => new()
        {
            Id = p.Id,
            Nombre = p.Nombre,
            Direccion = p.Direccion,
            IdZona = p.IdZona,
            Zona = p.Zona?.Nombre ?? string.Empty
        };

        // Mapeo del detalle del predio incluyendo sus canchas.
        private static PredioResponseDto MapearADetalleConCanchas(Predio p) => new()
        {
            Id = p.Id,
            Nombre = p.Nombre,
            Direccion = p.Direccion,
            IdZona = p.IdZona,
            Zona = p.Zona?.Nombre ?? string.Empty,
            ImagenUrl = null, // Predio no tiene ImagenUrl en el modelo actual (sin migración).
            Canchas = p.Canchas.Select(MapearACanchaResponseDto).ToList()
        };

        private static CanchaResponseDto MapearACanchaResponseDto(Cancha c) => new()
        {
            Id = c.Id,
            Nombre = c.Nombre,
            PrecioPorHora = c.PrecioPorHora,
            TipoCancha = c.TipoCancha?.Nombre ?? string.Empty,
            Superficie = c.Superficie?.Descripcion ?? string.Empty
        };
    }
}