using Fulbacho.Shared.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Fulbacho.Application.Modules.B2C.Interfaces
{
    public interface IPredioService
    {
        Task<IEnumerable<Predio>> ObtenerTodosLosPrediosAsync();

        Task<IEnumerable<Predio>> FiltrarPrediosAsync(string? nombre, string? zona);
    }
}