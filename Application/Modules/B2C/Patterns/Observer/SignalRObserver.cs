using Fulbacho.Application.Hubs;
using Fulbacho.Shared.Entities;
using Microsoft.AspNetCore.SignalR;

namespace Fulbacho.Application.Modules.B2C.Patterns.Observer
{
    public class SignalRObserver : IObservadorDesafio
    {
        private readonly IHubContext<FulbachoHub> _hub;

        public SignalRObserver(IHubContext<FulbachoHub> hub)
        {
            _hub = hub;
        }

        public async Task OnDesafioActualizadoAsync(Desafio desafio, string evento)
        {
            await _hub.Clients
                .Group($"equipo-{desafio.IdEquipoVisitante}")
                .SendAsync("DesafioActualizado", new
                {
                    desafio.Id,
                    Estado = evento,
                    desafio.IdEquipoLocal,
                    desafio.IdEquipoVisitante
                });
        }
    }
}
