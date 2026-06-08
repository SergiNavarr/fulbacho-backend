using Fulbacho.Shared.Entities;

namespace Fulbacho.Application.Modules.B2C.Patterns.Observer
{
    public interface IObservadorDesafio
    {
        Task OnDesafioActualizadoAsync(Desafio desafio, string evento);
    }
}
