using Fulbacho.Shared.Entities;
using Microsoft.Extensions.Logging;

namespace Fulbacho.Application.Modules.B2C.Patterns.Observer
{
    public class ReservaObserver : IObservadorDesafio
    {
        private readonly ILogger<ReservaObserver> _logger;

        public ReservaObserver(ILogger<ReservaObserver> logger)
        {
            _logger = logger;
        }

        public async Task OnDesafioActualizadoAsync(Desafio desafio, string evento)
        {
            if (evento == "Aceptado")
            {
                _logger.LogInformation(
                    "Desafio {Id} aceptado. Crear solicitud de Reserva. (TODO Sprint 3)",
                    desafio.Id);
                // TODO Sprint 3: inyectar IReservaService y llamar CrearSolicitudAsync(desafio)
            }
            await Task.CompletedTask;
        }
    }
}
