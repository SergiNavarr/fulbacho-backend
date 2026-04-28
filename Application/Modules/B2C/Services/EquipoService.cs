using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Fulbacho.Application.Modules.B2C.DTOs;
using Fulbacho.Shared;
using Fulbacho.Shared.Entities;
using Microsoft.EntityFrameworkCore;
using Fulbacho.Application.Modules.B2C.Interfaces;

namespace Fulbacho.Application.Modules.B2C.Services
{
    public class EquipoService : IEquipoService
    {
        private readonly FulbachoDbContext _context;

        public EquipoService(FulbachoDbContext context)
        {
            _context = context;
        }

        public async Task<bool> CrearEquipoAsync(CrearEquipoDto dto, int idCapitan)
        {
            bool nivelExiste = await _context.NivelesCompetitivos
                .AnyAsync(n => n.Id == dto.IdNivel);

            if (!nivelExiste)
                throw new Exception("El nivel competitivo seleccionado no es válido.");

            var nuevoEquipo = new Equipo
            {
                Nombre = dto.Nombre,
                EscudoUrl = dto.EscudoUrl,
                IdNivel = dto.IdNivel,
                IdCapitan = idCapitan,
                EsActivo = true
            };

            _context.Equipos.Add(nuevoEquipo);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<Equipo?> ObtenerEquipoPorIdAsync(int idEquipo, int idCapitan)
        {
            return await _context.Equipos
                .Include(e => e.NivelCompetitivo)
                .FirstOrDefaultAsync(e => e.Id == idEquipo && e.IdCapitan == idCapitan && e.EsActivo)!;
        }

        public async Task<bool> ActualizarEquipoAsync(int idEquipo, ActualizarEquipoDto dto, int idCapitan)
        {
            // Buscamos el equipo y verificamos que le pertenezca a este capitán
            var equipo = await _context.Equipos
                .FirstOrDefaultAsync(e => e.Id == idEquipo && e.IdCapitan == idCapitan && e.EsActivo);

            if (equipo == null)
                throw new Exception("Equipo no encontrado o no tenés permisos para editarlo.");

            // Verificamos que el nuevo nivel exista
            bool nivelExiste = await _context.NivelesCompetitivos.AnyAsync(n => n.Id == dto.IdNivel);
            if (!nivelExiste)
                throw new Exception("El nivel competitivo seleccionado no es válido.");

            // Actualizamos las propiedades
            equipo.Nombre = dto.Nombre;
            equipo.EscudoUrl = dto.EscudoUrl;
            equipo.IdNivel = dto.IdNivel;

            // Guardamos los cambios
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<Equipo>> ObtenerEquiposPorCapitanAsync(int idCapitan)
        {
            return await _context.Equipos
                .Include(e => e.NivelCompetitivo)
                .Where(e => e.IdCapitan == idCapitan && e.EsActivo)
                .ToListAsync();
        }
    }
}