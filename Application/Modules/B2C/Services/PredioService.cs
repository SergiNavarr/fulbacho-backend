using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fulbacho.Shared;
using Fulbacho.Shared.Entities;
using Fulbacho.Application.Modules.B2C.Interfaces;
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

        public async Task<IEnumerable<Predio>> ObtenerTodosLosPrediosAsync()
        {
            return await _context.Predios
                .Include(p => p.Zona)
                .ToListAsync();
        }

        public async Task<IEnumerable<Predio>> FiltrarPrediosAsync(string? nombre, string? zona)
        {
            var query = _context.Predios.Include(p => p.Zona).AsQueryable();

            if (!string.IsNullOrWhiteSpace(zona))
            {
                query = query.Where(p => p.Zona != null && p.Zona.Nombre.ToLower() == zona.ToLower());
            }

            if (!string.IsNullOrWhiteSpace(nombre))
            {
                query = query.Where(p => p.Nombre.ToLower().Contains(nombre.ToLower()));
            }

            return await query.ToListAsync();
        }
    }
}