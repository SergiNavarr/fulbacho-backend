using Fulbacho.Application.Modules.B2C.DTOs;
using Fulbacho.Application.Modules.B2C.Interfaces;
using Fulbacho.Application.Modules.B2C.Patterns.Observer;
using Fulbacho.Shared;
using Fulbacho.Shared.Entities;
using Microsoft.EntityFrameworkCore;

namespace Fulbacho.Application.Modules.B2C.Services
{
    public class DesafioService : IDesafioService
    {
        private readonly FulbachoDbContext _context;
        private readonly List<IObservadorDesafio> _observadores = new();

        public DesafioService(FulbachoDbContext context)
        {
            _context = context;
        }

        public void Suscribir(IObservadorDesafio observador)   => _observadores.Add(observador);
        public void Desuscribir(IObservadorDesafio observador) => _observadores.Remove(observador);

        public async Task<Desafio?> ObtenerPorIdAsync(int idDesafio)
        {
            return await _context.Desafios
                .Include(d => d.EquipoLocal).ThenInclude(e => e!.NivelCompetitivo)
                .Include(d => d.EquipoVisitante)
                .Include(d => d.Estado)
                .Include(d => d.Zona)
                .FirstOrDefaultAsync(d => d.Id == idDesafio);
        }

        public async Task<int> CrearDesafioAsync(CrearDesafioDto dto, int idEquipoLocal)
        {
            await VerificarEquipoActivoAsync(idEquipoLocal, "local");
            await VerificarEquipoActivoAsync(dto.IdEquipoVisitante, "visitante");
            await VerificarMismoNivelAsync(idEquipoLocal, dto.IdEquipoVisitante);
            await VerificarZonaExisteAsync(dto.IdZona);

            var desafio = new Desafio
            {
                IdEquipoLocal = idEquipoLocal,
                IdEquipoVisitante = dto.IdEquipoVisitante,
                FechaPropuesta = dto.FechaPropuesta,
                HoraInicio = dto.HoraInicio,
                HoraFin = dto.HoraFin,
                IdZona = dto.IdZona,
                IdCanchaSugerida = dto.IdCanchaSugerida,
                IdEstadoDesafio = 1 // Pendiente
            };

            _context.Desafios.Add(desafio);
            await _context.SaveChangesAsync();
            return desafio.Id;
        }

        public async Task AceptarDesafioAsync(int idDesafio)
        {
            var desafio = await ObtenerPorIdAsync(idDesafio)
                ?? throw new Exception("Desafío no encontrado.");
            desafio.Aceptar();
            await _context.SaveChangesAsync();
            await NotificarAsync(desafio, "Aceptado");
        }

        public async Task RechazarDesafioAsync(int idDesafio)
        {
            var desafio = await ObtenerPorIdAsync(idDesafio)
                ?? throw new Exception("Desafío no encontrado.");
            desafio.Rechazar();
            await _context.SaveChangesAsync();
            await NotificarAsync(desafio, "Rechazado");
        }

        private async Task VerificarEquipoActivoAsync(int idEquipo, string rol)
        {
            bool existe = await _context.Equipos.AnyAsync(e => e.Id == idEquipo && e.EsActivo);
            if (!existe)
                throw new Exception($"El equipo {rol} no existe o no está activo.");
        }

        private async Task VerificarMismoNivelAsync(int idEquipoLocal, int idEquipoVisitante)
        {
            var niveles = await _context.Equipos
                .Where(e => e.Id == idEquipoLocal || e.Id == idEquipoVisitante)
                .Select(e => e.IdNivel)
                .ToListAsync();
            if (niveles.Distinct().Count() > 1)
                throw new Exception("Los equipos deben tener el mismo nivel competitivo.");
        }

        private async Task VerificarZonaExisteAsync(int idZona)
        {
            bool existe = await _context.Zonas.AnyAsync(z => z.Id == idZona);
            if (!existe)
                throw new Exception("La zona seleccionada no existe.");
        }

        private async Task NotificarAsync(Desafio desafio, string evento)
        {
            foreach (var obs in _observadores)
                await obs.OnDesafioActualizadoAsync(desafio, evento);
        }
    }
}
