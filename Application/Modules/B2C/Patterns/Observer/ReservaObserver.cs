using Fulbacho.Application.Modules.B2C.DTOs;
using Fulbacho.Application.Modules.B2C.Interfaces;
using Fulbacho.Shared.Entities;
using Microsoft.Extensions.Logging;

namespace Fulbacho.Application.Modules.B2C.Patterns.Observer
{
    public class ReservaObserver : IObservadorDesafio
    {
        private readonly IReservaService _reservaService;
        private readonly ILogger<ReservaObserver> _logger;

        // Tanto ReservaObserver como IReservaService están registrados como Scoped y se resuelven
        // dentro del mismo scope del request (DesafiosController los inyecta y suscribe al servicio).
        // Por eso podemos inyectar IReservaService directamente sin IServiceScopeFactory:
        // no hay singleton involucrado ni dependencia circular (ReservaService no depende de Desafío).
        public ReservaObserver(IReservaService reservaService, ILogger<ReservaObserver> logger)
        {
            _reservaService = reservaService;
            _logger = logger;
        }

        public async Task OnDesafioActualizadoAsync(Desafio desafio, string evento)
        {
            if (evento != "Aceptado") return;

            // La cancha es obligatoria en el Desafío (HU 2.2), por lo que siempre tenemos una cancha
            // concreta para generar la reserva automática al aceptar.

            // El creador de la reserva es el capitán del equipo local (quien propuso el desafío).
            // EquipoLocal viene incluido por DesafioService.ObtenerPorIdAsync.
            if (desafio.EquipoLocal is null)
            {
                _logger.LogWarning(
                    "Desafío {Id} aceptado sin equipo local cargado: no se generó la reserva automática.",
                    desafio.Id);
                return;
            }

            // Reconstruimos la fecha/hora combinando la fecha propuesta (date) con las horas (TimeSpan).
            var dto = new CrearReservaDto
            {
                IdCancha = desafio.IdCanchaSugerida,
                FechaHoraInicio = desafio.FechaPropuesta.Date + desafio.HoraInicio,
                FechaHoraFin = desafio.FechaPropuesta.Date + desafio.HoraFin,
                Notas = $"Reserva generada automáticamente por la aceptación del desafío #{desafio.Id}.",
                IdDesafio = desafio.Id
            };

            try
            {
                // La reserva nace en estado "Pendiente" (lo resuelve ReservaService).
                int idReserva = await _reservaService.CrearReservaAsync(dto, desafio.EquipoLocal.IdCapitan);
                _logger.LogInformation(
                    "Desafío {IdDesafio} aceptado: se creó la reserva pendiente {IdReserva}.",
                    desafio.Id, idReserva);
            }
            catch (Exception ex)
            {
                // No rompemos el flujo de aceptar el desafío si la reserva falla
                // (ej: la cancha sugerida ya está ocupada en ese horario).
                _logger.LogError(ex,
                    "No se pudo crear la reserva automática para el desafío {Id}.", desafio.Id);
            }
        }
    }
}
