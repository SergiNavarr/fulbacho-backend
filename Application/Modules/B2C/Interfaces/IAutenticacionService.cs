using Fulbacho.Application.Modules.B2C.DTOs;

namespace Fulbacho.Application.Modules.B2C.Interfaces
{
    public interface IAutenticacionService
    {
        Task<RespuestaLoginDto?> LoginAsync(LoginJugadorDto dto);
        Task<RespuestaLoginDto?> RegistrarJugadorAsync(RegistroJugadorDto dto);
    }
}
