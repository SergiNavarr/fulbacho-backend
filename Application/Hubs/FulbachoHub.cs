using Microsoft.AspNetCore.SignalR;

namespace Fulbacho.Application.Hubs
{
    // Hub definido en Application (no en API) para que SignalRObserver pueda inyectar
    // IHubContext<FulbachoHub> sin crear una referencia circular API → Application → API.
    public class FulbachoHub : Hub
    {
        // Sprint 4: métodos de tiempo real para desafíos y reservas
        public async Task UnirseAGrupoEquipo(int idEquipo)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, $"equipo-{idEquipo}");
        }
    }
}
