using System;
using System.Collections.Generic;
using System.Linq;
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
            await VerificarNivelAsync(dto.IdNivel);

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
            var equipo = await ObtenerEquipoPorIdAsync(idEquipo, idCapitan);

            if (equipo == null)
                throw new Exception("Equipo no encontrado o no tenés permisos para editarlo.");

            await VerificarNivelAsync(dto.IdNivel);

            equipo.Nombre = dto.Nombre;
            equipo.EscudoUrl = dto.EscudoUrl;
            equipo.IdNivel = dto.IdNivel;

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


        private async Task VerificarNivelAsync(int idNivel)
        {
            bool nivelExiste = await _context.NivelesCompetitivos.AnyAsync(n => n.Id == idNivel);
            if (!nivelExiste)
                throw new Exception("El nivel competitivo seleccionado no es válido.");
        }
    }
}