using Fulbacho.Shared.Entities;

namespace Fulbacho.Application.Modules.B2C.Patterns.Strategy
{
    public class MotorMatchmaking
    {
        private IMatchmakingStrategy _strategy;

        public MotorMatchmaking(IMatchmakingStrategy strategy)
        {
            _strategy = strategy;
        }

        public void CambiarEstrategia(IMatchmakingStrategy nueva) => _strategy = nueva;

        public Task<IEnumerable<Equipo>> BuscarRivalesAsync(
            Equipo equipoBuscador,
            IEnumerable<Equipo> todosLosCandidatos)
            => _strategy.BuscarRivalesAsync(equipoBuscador, todosLosCandidatos);
    }
}
