using Fulbacho.Shared.Entities;
using Xunit;

namespace Fulbacho.Tests
{
    // Tests del patrón State embebido en la entidad Desafio. Son unitarios puros:
    // la transición de estado no toca la base de datos, solo el objeto de dominio.
    public class DesafioStateTests
    {
        [Fact]
        public void Aceptar_DesdePendiente_PasaAAceptado()
        {
            // Un Desafio recién instanciado arranca con _estadoPatron = EstadoPendiente.
            var desafio = new Desafio();

            desafio.Aceptar();

            Assert.Equal(2, desafio.IdEstadoDesafio); // EstadoAceptado.Id == 2
        }

        [Fact]
        public void Rechazar_DesdePendiente_PasaARechazado()
        {
            var desafio = new Desafio();

            desafio.Rechazar();

            Assert.Equal(3, desafio.IdEstadoDesafio); // EstadoRechazado.Id == 3
        }
    }
}
