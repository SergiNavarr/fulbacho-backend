using Fulbacho.Application.Modules.B2C.DTOs;
using Fulbacho.Application.Modules.B2C.Interfaces;
using Fulbacho.Shared;
using Fulbacho.Shared.Entities;
using Microsoft.EntityFrameworkCore;

namespace Fulbacho.Application.Modules.B2C.Services
{
    public class ReservaService : IReservaService
    {
        private readonly FulbachoDbContext _context;

        // Porcentaje de seña aplicado sobre el monto total de la reserva.
        private const decimal PorcentajeSena = 0.30m;

        public ReservaService(FulbachoDbContext context)
        {
            _context = context;
        }

        public async Task<int> CrearReservaAsync(CrearReservaDto dto, int idUsuario)
        {
            var cancha = await VerificarCanchaExisteAsync(dto.IdCancha);

            // El DTO trae la hora en local AR; convertimos a UTC una sola vez para que la query de
            // disponibilidad y el INSERT (columnas timestamptz) compartan exactamente el mismo valor.
            DateTime inicioUtc = ZonaHorariaArgentina.ConvertirAUtc(dto.FechaHoraInicio);
            DateTime finUtc = ZonaHorariaArgentina.ConvertirAUtc(dto.FechaHoraFin);

            VerificarRangoHorarioValido(inicioUtc, finUtc);
            await VerificarDisponibilidadAsync(dto.IdCancha, inicioUtc, finUtc);
            int idEstadoPendiente = await ObtenerEstadoReservaPendienteAsync();

            decimal montoTotal = CalcularMontoTotal(cancha.PrecioPorHora, inicioUtc, finUtc);
            decimal montoSena = CalcularMontoSena(montoTotal);

            var reserva = new Reserva
            {
                IdCancha = dto.IdCancha,
                FechaHoraInicio = inicioUtc,
                FechaHoraFin = finUtc,
                MontoTotal = montoTotal,
                MontoSena = montoSena,
                Notas = dto.Notas ?? string.Empty,
                IdUsuarioCreador = idUsuario,
                IdEstadoReserva = idEstadoPendiente,
                IdDesafio = dto.IdDesafio
            };

            _context.Reservas.Add(reserva);
            await _context.SaveChangesAsync();
            return reserva.Id;
        }

        public async Task<Reserva?> ObtenerPorIdAsync(int id)
        {
            return await _context.Reservas
                .Include(r => r.Cancha)
                .Include(r => r.Estado)
                .FirstOrDefaultAsync(r => r.Id == id);
        }

        // --- Validaciones de negocio extraídas (trazabilidad 1:1 con self-messages UML) ---

        private async Task<Cancha> VerificarCanchaExisteAsync(int idCancha)
        {
            var cancha = await _context.Canchas
                .FirstOrDefaultAsync(c => c.Id == idCancha && c.EsActivo);
            if (cancha == null)
                throw new Exception("La cancha seleccionada no existe o no está activa.");
            return cancha;
        }

        private static void VerificarRangoHorarioValido(DateTime inicio, DateTime fin)
        {
            if (fin <= inicio)
                throw new Exception("El horario de fin debe ser posterior al de inicio.");
        }

        private async Task VerificarDisponibilidadAsync(int idCancha, DateTime inicio, DateTime fin)
        {
            // Una reserva bloquea el horario salvo que esté Cancelada.
            // Hay solapamiento si: inicioExistente < finNuevo && inicioNuevo < finExistente.
            bool haySolapamiento = await _context.Reservas
                .AnyAsync(r => r.IdCancha == idCancha
                    && r.Estado!.Descripcion != "Cancelada"
                    && r.FechaHoraInicio < fin
                    && inicio < r.FechaHoraFin);

            if (haySolapamiento)
                throw new Exception("La cancha ya se encuentra reservada en ese horario.");
        }

        private async Task<int> ObtenerEstadoReservaPendienteAsync()
        {
            // El estado inicial es "Pendiente". Se resuelve por descripción contra los EstadosReserva
            // seedeados en FulbachoDbContext (evitamos el número mágico).
            var estado = await _context.EstadosReserva
                .FirstOrDefaultAsync(e => e.Descripcion == "Pendiente")
                ?? throw new Exception("No se encontró el estado de reserva 'Pendiente'.");
            return estado.Id;
        }

        private static decimal CalcularMontoTotal(decimal precioPorHora, DateTime inicio, DateTime fin)
        {
            var horas = (decimal)(fin - inicio).TotalHours;
            return Math.Round(precioPorHora * horas, 2);
        }

        private static decimal CalcularMontoSena(decimal montoTotal)
            => Math.Round(montoTotal * PorcentajeSena, 2);
    }
}
