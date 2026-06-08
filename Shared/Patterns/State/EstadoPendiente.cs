using Fulbacho.Shared.Entities;

namespace Fulbacho.Shared.Patterns.State
{
    // Supuesto: Id=1 para "Pendiente". No existe seed data en las migraciones para Estados_desafio.
    public class EstadoPendiente : IEstadoDesafio
    {
        public int Id => 1;
        public string Descripcion => "Pendiente";

        public void Aceptar(Desafio desafio)  => desafio.CambiarEstado(new EstadoAceptado());
        public void Rechazar(Desafio desafio) => desafio.CambiarEstado(new EstadoRechazado());
        public void Confirmar(Desafio desafio) =>
            throw new InvalidOperationException("Un desafío pendiente no puede confirmarse.");
    }
}
