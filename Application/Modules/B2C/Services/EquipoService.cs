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
    }
}
