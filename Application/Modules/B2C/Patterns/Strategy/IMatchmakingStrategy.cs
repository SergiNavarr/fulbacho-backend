using Fulbacho.Shared.Entities;

namespace Fulbacho.Application.Modules.B2C.Patterns.Strategy
{
    public interface IMatchmakingStrategy
    {
        Task<IEnumerable<Equipo>> BuscarRivalesAsync(
            Equipo equipoBuscador,
            IEnumerable<Equipo> todosLosCandidatos);
    }
}
