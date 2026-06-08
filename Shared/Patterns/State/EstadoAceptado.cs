using Fulbacho.Shared.Entities;

namespace Fulbacho.Shared.Patterns.State
{
    // Supuesto: Id=2 para "Aceptado". No existe seed data en las migraciones para Estados_desafio.
    public class EstadoAceptado : IEstadoDesafio
    {
        public int Id => 2;
        public string Descripcion => "Aceptado";

        public void Aceptar(Desafio desafio) =>
            throw new InvalidOperationException("El desafío ya fue aceptado.");
        public void Rechazar(Desafio desafio) =>
            throw new InvalidOperationException("No se puede rechazar un desafío aceptado.");
        public void Confirmar(Desafio desafio) => desafio.CambiarEstado(new EstadoConfirmado());
    }
}
