using Fulbacho.Application.Modules.B2C.DTOs;
using Fulbacho.Shared.Entities;

namespace Fulbacho.Application.Modules.B2C.Interfaces
{
    public interface IReservaService
    {
        Task<int> CrearReservaAsync(CrearReservaDto dto, int idUsuario);
        Task<Reserva?> ObtenerPorIdAsync(int id);
    }
}
