using System;
using System.Linq;
using System.Threading.Tasks;
using Fulbacho.Application.Modules.B2C.DTOs;
using Fulbacho.Application.Modules.B2C.Patterns.Strategy;
using Fulbacho.Application.Modules.B2C.Services;
using Fulbacho.Shared;
using Fulbacho.Tests.TestSupport;
using Xunit;

namespace Fulbacho.Tests
{
    // Tests del flujo de creación de desafío ejercitando el método público
    // DesafioService.CrearDesafioAsync(CrearDesafioDto dto, int idCapitan), que
    // dispara internamente todas las validaciones privadas.
    public class DesafioServiceTests
    {
        private static readonly DateTime Fecha = new(2026, 7, 1);

        // Construye el DesafioService real con sus dependencias reales:
        //   - FulbachoDbContext sobre EF Core InMemory
        //   - MotorMatchmaking con la estrategia MatchmakingPorNivel (la registrada en Program.cs)
        // No se suscribe ningún observador (eso ocurre en el controller), por lo que
        // NotificarAsync recorre una lista vacía y no se necesita SignalR.
        private static DesafioService CrearServicio(FulbachoDbContext ctx)
            => new(ctx, new MotorMatchmaking(new MatchmakingPorNivel()));

        private static CrearDesafioDto DtoValido(TimeSpan inicio, TimeSpan fin) => new()
        {
            IdEquipoLocal = FabricaDeContexto.IdEquipoLocal,
            IdEquipoVisitante = FabricaDeContexto.IdEquipoVisitante,
            FechaPropuesta = Fecha,
            HoraInicio = inicio,
            HoraFin = fin,
            IdZona = FabricaDeContexto.IdZona,
            IdCanchaSugerida = FabricaDeContexto.IdCancha
        };

        [Fact]
        public async Task CrearDesafioAsync_DatosValidos_CreaEnEstadoPendienteYDevuelveId()
        {
            using var ctx = FabricaDeContexto.CrearContextoEnMemoria();
            FabricaDeContexto.SembrarEscenarioBase(ctx);
            var servicio = CrearServicio(ctx);

            var dto = DtoValido(new TimeSpan(20, 0, 0), new TimeSpan(21, 0, 0));

            var id = await servicio.CrearDesafioAsync(dto, FabricaDeContexto.IdCapitan);

            Assert.True(id > 0);
            var creado = ctx.Desafios.Single(d => d.Id == id);
            Assert.Equal(1, creado.IdEstadoDesafio); // Pendiente
            Assert.Equal(FabricaDeContexto.IdEquipoLocal, creado.IdEquipoLocal);
            Assert.Equal(FabricaDeContexto.IdEquipoVisitante, creado.IdEquipoVisitante);
        }

        [Fact]
        public async Task CrearDesafioAsync_CapitanNoDuenoDelEquipoLocal_LanzaExcepcion()
        {
            using var ctx = FabricaDeContexto.CrearContextoEnMemoria();
            FabricaDeContexto.SembrarEscenarioBase(ctx);
            var servicio = CrearServicio(ctx);

            var dto = DtoValido(new TimeSpan(20, 0, 0), new TimeSpan(21, 0, 0));

            var ex = await Assert.ThrowsAsync<Exception>(
                () => servicio.CrearDesafioAsync(dto, FabricaDeContexto.IdCapitanAjeno));

            Assert.Contains("no te pertenece", ex.Message);
            Assert.Empty(ctx.Desafios); // no se persistió nada
        }

        [Fact]
        public async Task CrearDesafioAsync_EquiposDeDistintoNivel_LanzaExcepcion()
        {
            using var ctx = FabricaDeContexto.CrearContextoEnMemoria();
            // Visitante en un nivel distinto al del equipo local (Amateur).
            FabricaDeContexto.SembrarEscenarioBase(ctx, nivelVisitante: FabricaDeContexto.NivelIntermedio);
            var servicio = CrearServicio(ctx);

            var dto = DtoValido(new TimeSpan(20, 0, 0), new TimeSpan(21, 0, 0));

            var ex = await Assert.ThrowsAsync<Exception>(
                () => servicio.CrearDesafioAsync(dto, FabricaDeContexto.IdCapitan));

            Assert.Contains("mismo nivel competitivo", ex.Message);
            Assert.Empty(ctx.Desafios);
        }

        [Fact]
        public async Task CrearDesafioAsync_CanchaOcupada_LanzaExcepcionDeCanchaOcupada()
        {
            using var ctx = FabricaDeContexto.CrearContextoEnMemoria();
            FabricaDeContexto.SembrarEscenarioBase(ctx);
            // Ya hay un desafío pendiente ocupando 18:00-20:00 en la misma cancha y fecha.
            FabricaDeContexto.SembrarDesafioOcupando(
                ctx, Fecha, new TimeSpan(18, 0, 0), new TimeSpan(20, 0, 0));
            var servicio = CrearServicio(ctx);

            // Nuevo desafío solapado (18:00-19:00).
            var dto = DtoValido(new TimeSpan(18, 0, 0), new TimeSpan(19, 0, 0));

            var ex = await Assert.ThrowsAsync<Exception>(
                () => servicio.CrearDesafioAsync(dto, FabricaDeContexto.IdCapitan));

            Assert.Contains("ocupada", ex.Message);
            Assert.Single(ctx.Desafios); // solo queda el sembrado, no se agregó el nuevo
        }

        // Disponibilidad: con un turno ocupado de 18:00 a 20:00, se prueban bordes y solapamientos.
        // esperaLibre = true  => el turno está libre y el desafío se crea.
        // esperaLibre = false => el turno se solapa y se lanza excepción.
        [Theory]
        [InlineData(20, 21, true)]   // arranca justo cuando termina el ocupado -> libre
        [InlineData(18, 19, false)]  // dentro del ocupado -> solapa
        [InlineData(17, 19, false)]  // entra pisando el inicio del ocupado -> solapa
        [InlineData(17, 18, true)]   // termina justo cuando empieza el ocupado (borde) -> libre
        public async Task CrearDesafioAsync_VerificaDisponibilidadContraTurnoOcupado_1800a2000(
            int horaInicio, int horaFin, bool esperaLibre)
        {
            using var ctx = FabricaDeContexto.CrearContextoEnMemoria();
            FabricaDeContexto.SembrarEscenarioBase(ctx);
            FabricaDeContexto.SembrarDesafioOcupando(
                ctx, Fecha, new TimeSpan(18, 0, 0), new TimeSpan(20, 0, 0));
            var servicio = CrearServicio(ctx);

            var dto = DtoValido(new TimeSpan(horaInicio, 0, 0), new TimeSpan(horaFin, 0, 0));

            if (esperaLibre)
            {
                var id = await servicio.CrearDesafioAsync(dto, FabricaDeContexto.IdCapitan);
                Assert.True(id > 0);
                Assert.Equal(2, ctx.Desafios.Count()); // el ocupado + el nuevo
            }
            else
            {
                var ex = await Assert.ThrowsAsync<Exception>(
                    () => servicio.CrearDesafioAsync(dto, FabricaDeContexto.IdCapitan));
                Assert.Contains("ocupada", ex.Message);
                Assert.Single(ctx.Desafios); // solo el ocupado
            }
        }
    }
}
