using System;
using Fulbacho.Shared;
using Fulbacho.Shared.Entities;
using Microsoft.EntityFrameworkCore;

namespace Fulbacho.Tests.TestSupport
{
    // Centraliza la creación del DbContext real sobre EF Core InMemory y el sembrado
    // del "mundo feliz" para los tests del flujo de desafío.
    //
    // Nota: EnsureCreated() aplica el HasData de FulbachoDbContext (NivelCompetitivo 1-3,
    // EstadoDesafio 1-4, EstadoReserva 1-3, Superficie, TipoCancha). Por eso NO se vuelven
    // a sembrar esas tablas paramétricas: ya existen. Las restricciones relacionales
    // (HasCheckConstraint) y la integridad referencial las ignora el proveedor InMemory.
    public static class FabricaDeContexto
    {
        // Ids fijos del escenario base, para que los tests sean legibles.
        public const int IdCapitan = 10;
        public const int IdCapitanAjeno = 999;
        public const int IdEquipoLocal = 1;
        public const int IdEquipoVisitante = 2;
        public const int IdZona = 1;
        public const int IdCancha = 1;

        public const int NivelAmateur = 1;     // sembrado por HasData
        public const int NivelIntermedio = 2;  // sembrado por HasData

        public static FulbachoDbContext CrearContextoEnMemoria()
        {
            var options = new DbContextOptionsBuilder<FulbachoDbContext>()
                // Nombre único por contexto => aislamiento total entre tests.
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            var context = new FulbachoDbContext(options);
            context.Database.EnsureCreated(); // aplica el seed de HasData
            return context;
        }

        // Siembra ciudad/zona/predio/cancha + equipo local y visitante.
        // El nivel del visitante es parametrizable para el caso "distinto nivel".
        public static void SembrarEscenarioBase(FulbachoDbContext ctx, int nivelVisitante = NivelAmateur)
        {
            ctx.Ciudades.Add(new Ciudad { Id = 1, Nombre = "Córdoba" });
            ctx.Zonas.Add(new Zona { Id = IdZona, Nombre = "Centro", CiudadId = 1 });
            ctx.Predios.Add(new Predio
            {
                Id = 1,
                Nombre = "Predio Test",
                Direccion = "Calle Falsa 123",
                IdZona = IdZona,
                IdAdmin = 50,
                EsActivo = true
            });
            ctx.Canchas.Add(new Cancha
            {
                Id = IdCancha,
                Nombre = "Cancha 1",
                PrecioPorHora = 1000m,
                EsActivo = true,
                IdPredio = 1,
                IdTipoCancha = 1,
                IdSuperficie = 1
            });
            ctx.Equipos.Add(new Equipo
            {
                Id = IdEquipoLocal,
                Nombre = "Local FC",
                EscudoUrl = string.Empty,
                IdCapitan = IdCapitan,
                IdNivel = NivelAmateur,
                EsActivo = true
            });
            ctx.Equipos.Add(new Equipo
            {
                Id = IdEquipoVisitante,
                Nombre = "Visitante FC",
                EscudoUrl = string.Empty,
                IdCapitan = 20,
                IdNivel = nivelVisitante,
                EsActivo = true
            });
            ctx.SaveChanges();
        }

        // Siembra un desafío PENDIENTE que ocupa la cancha base en la fecha dada y el
        // rango [horaInicio, horaFin). Sirve para los tests de solapamiento de turnos.
        public static void SembrarDesafioOcupando(
            FulbachoDbContext ctx, DateTime fecha, TimeSpan horaInicio, TimeSpan horaFin)
        {
            ctx.Desafios.Add(new Desafio
            {
                IdEquipoLocal = IdEquipoLocal,
                IdEquipoVisitante = IdEquipoVisitante,
                FechaPropuesta = fecha.Date,
                HoraInicio = horaInicio,
                HoraFin = horaFin,
                IdZona = IdZona,
                IdCanchaSugerida = IdCancha,
                IdEstadoDesafio = 1 // Pendiente => bloquea el horario
            });
            ctx.SaveChanges();
        }
    }
}
