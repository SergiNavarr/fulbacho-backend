using Fulbacho.Shared.Entities;

namespace Fulbacho.Application.Modules.B2C.Patterns.Strategy
{
    public class MatchmakingPorNivel : IMatchmakingStrategy
    {
        public Task<IEnumerable<Equipo>> BuscarRivalesAsync(
            Equipo equipoBuscador,
            IEnumerable<Equipo> todosLosCandidatos)
        {
            var resultado = todosLosCandidatos.Where(e =>
                e.IdNivel == equipoBuscador.IdNivel &&
                e.Id != equipoBuscador.Id &&
                e.EsActivo == true);
            return Task.FromResult(resultado);
        }
    }
}
