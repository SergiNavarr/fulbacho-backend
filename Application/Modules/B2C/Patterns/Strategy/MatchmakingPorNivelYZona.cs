using Fulbacho.Shared.Entities;

namespace Fulbacho.Application.Modules.B2C.Patterns.Strategy
{
    public class MatchmakingPorNivelYZona : IMatchmakingStrategy
    {
        private readonly int _idZona;

        public MatchmakingPorNivelYZona(int idZona)
        {
            _idZona = idZona;
        }

        public Task<IEnumerable<Equipo>> BuscarRivalesAsync(
            Equipo equipoBuscador,
            IEnumerable<Equipo> todosLosCandidatos)
        {
            // TODO Sprint 2: cuando Equipo tenga ZonaPreferidaId,
            // simplificar este filtro directamente sobre Equipo.
            var resultado = todosLosCandidatos.Where(e =>
                e.IdNivel == equipoBuscador.IdNivel &&
                e.Id != equipoBuscador.Id &&
                e.EsActivo &&
                (e.DesafiosLocal.Any(d => d.IdZona == _idZona) ||
                 e.DesafiosVisitante.Any(d => d.IdZona == _idZona)));

            if (!resultado.Any())
            {
                var fallback = new MatchmakingPorNivel();
                return fallback.BuscarRivalesAsync(equipoBuscador, todosLosCandidatos);
            }

            return Task.FromResult(resultado);
        }
    }
}
