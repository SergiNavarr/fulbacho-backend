using Application.Modules.B2C.DTOs;
using Fulbacho.Shared.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Fulbacho.Application.Modules.B2C.Interfaces
{
    public interface IPredioService
    {
        Task<IEnumerable<PredioResponseDto>> ObtenerTodosLosPrediosAsync();
        Task<IEnumerable<PredioResponseDto>> FiltrarPrediosAsync(string? nombre, string? zona);
    }
}