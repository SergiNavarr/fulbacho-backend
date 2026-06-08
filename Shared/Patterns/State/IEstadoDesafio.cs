using Fulbacho.Shared.Entities;

namespace Fulbacho.Shared.Patterns.State
{
    public interface IEstadoDesafio
    {
        int Id { get; }
        string Descripcion { get; }
        void Aceptar(Desafio desafio);
        void Rechazar(Desafio desafio);
        void Confirmar(Desafio desafio);
    }
}
