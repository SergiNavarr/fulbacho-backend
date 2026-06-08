using Fulbacho.Shared.Entities;

namespace Fulbacho.Shared.Patterns.State
{
    // Supuesto: Id=4 para "Confirmado". No existe seed data en las migraciones para Estados_desafio.
    public class EstadoConfirmado : IEstadoDesafio
    {
        public int Id => 4;
        public string Descripcion => "Confirmado";

        public void Aceptar(Desafio desafio) =>
            throw new InvalidOperationException("Estado final: Confirmado.");
        public void Rechazar(Desafio desafio) =>
            throw new InvalidOperationException("Estado final: Confirmado.");
        public void Confirmar(Desafio desafio) =>
            throw new InvalidOperationException("Estado final: Confirmado.");
    }
}
