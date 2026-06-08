using Fulbacho.Shared.Entities;

namespace Fulbacho.Shared.Patterns.State
{
    // Supuesto: Id=3 para "Rechazado". No existe seed data en las migraciones para Estados_desafio.
    public class EstadoRechazado : IEstadoDesafio
    {
        public int Id => 3;
        public string Descripcion => "Rechazado";

        public void Aceptar(Desafio desafio) =>
            throw new InvalidOperationException("Estado final: Rechazado.");
        public void Rechazar(Desafio desafio) =>
            throw new InvalidOperationException("Estado final: Rechazado.");
        public void Confirmar(Desafio desafio) =>
            throw new InvalidOperationException("Estado final: Rechazado.");
    }
}
