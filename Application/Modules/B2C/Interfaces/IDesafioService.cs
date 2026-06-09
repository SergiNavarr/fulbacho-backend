using Fulbacho.Application.Modules.B2C.DTOs;
using Fulbacho.Application.Modules.B2C.Patterns.Observer;
using Fulbacho.Shared.Entities;

namespace Fulbacho.Application.Modules.B2C.Interfaces
{
    public interface IDesafioService
    {
        Task<Desafio?> ObtenerPorIdAsync(int idDesafio);
        Task<List<DesafioResponseDto>> ObtenerDesafiosPorEquipoAsync(int idEquipo);
        Task<int> CrearDesafioAsync(CrearDesafioDto dto, int idCapitan);
        Task AceptarDesafioAsync(int idDesafio);
        Task RechazarDesafioAsync(int idDesafio);
        Task<IEnumerable<Equipo>> BuscarRivalesAsync(int idEquipoBuscador);
        void Suscribir(IObservadorDesafio observador);
        void Desuscribir(IObservadorDesafio observador);
    }
}
