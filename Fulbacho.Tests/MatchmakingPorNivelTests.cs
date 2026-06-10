using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fulbacho.Application.Modules.B2C.Patterns.Strategy;
using Fulbacho.Shared.Entities;
using Xunit;

namespace Fulbacho.Tests
{
    // Tests puros de la estrategia de matchmaking (sin base de datos): opera sobre
    // colecciones de Equipo en memoria.
    public class MatchmakingPorNivelTests
    {
        private static Equipo Equipo(int id, int idNivel, bool activo = true) => new()
        {
            Id = id,
            Nombre = $"Equipo {id}",
            IdNivel = idNivel,
            EsActivo = activo
        };

        [Fact]
        public async Task BuscarRivalesAsync_DevuelveSoloRivalesDelMismoNivel()
        {
            var estrategia = new MatchmakingPorNivel();
            var buscador = Equipo(1, idNivel: 1);
            var candidatos = new List<Equipo>
            {
                Equipo(2, idNivel: 1),  // mismo nivel  -> sí
                Equipo(3, idNivel: 2),  // otro nivel   -> no
                Equipo(4, idNivel: 1),  // mismo nivel  -> sí
            };

            var rivales = (await estrategia.BuscarRivalesAsync(buscador, candidatos)).ToList();

            Assert.Equal(new[] { 2, 4 }, rivales.Select(e => e.Id).OrderBy(x => x));
            Assert.All(rivales, e => Assert.Equal(buscador.IdNivel, e.IdNivel));
        }

        [Fact]
        public async Task BuscarRivalesAsync_ExcluyeAlPropioEquipoBuscador()
        {
            var estrategia = new MatchmakingPorNivel();
            var buscador = Equipo(1, idNivel: 1);
            var candidatos = new List<Equipo>
            {
                buscador,             // el propio buscador, mismo nivel -> debe excluirse
                Equipo(2, idNivel: 1)
            };

            var rivales = (await estrategia.BuscarRivalesAsync(buscador, candidatos)).ToList();

            Assert.DoesNotContain(rivales, e => e.Id == buscador.Id);
            Assert.Equal(new[] { 2 }, rivales.Select(e => e.Id));
        }

        [Fact]
        public async Task BuscarRivalesAsync_ExcluyeEquiposInactivos()
        {
            var estrategia = new MatchmakingPorNivel();
            var buscador = Equipo(1, idNivel: 1);
            var candidatos = new List<Equipo>
            {
                Equipo(2, idNivel: 1, activo: false), // inactivo -> no
                Equipo(3, idNivel: 1, activo: true)   // activo   -> sí
            };

            var rivales = (await estrategia.BuscarRivalesAsync(buscador, candidatos)).ToList();

            Assert.Equal(new[] { 3 }, rivales.Select(e => e.Id));
            Assert.All(rivales, e => Assert.True(e.EsActivo));
        }

        [Fact]
        public async Task BuscarRivalesAsync_SinCoincidencias_DevuelveListaVacia()
        {
            var estrategia = new MatchmakingPorNivel();
            var buscador = Equipo(1, idNivel: 1);
            var candidatos = new List<Equipo>
            {
                Equipo(2, idNivel: 2),
                Equipo(3, idNivel: 3)
            };

            var rivales = await estrategia.BuscarRivalesAsync(buscador, candidatos);

            Assert.Empty(rivales);
        }
    }
}
