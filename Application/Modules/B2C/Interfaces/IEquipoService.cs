using Fulbacho.Application.Modules.B2C.DTOs;
using Fulbacho.Shared.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fulbacho.Application.Modules.B2C.Interfaces
{
    public interface IEquipoService
    {
        Task<bool> CrearEquipoAsync(CrearEquipoDto dto, int idCapitan);
        Task<Equipo?> ObtenerEquipoPorIdAsync(int idEquipo, int idCapitan);
        Task<bool> ActualizarEquipoAsync(int idEquipo, ActualizarEquipoDto dto, int idCapitan);
        Task<IEnumerable<Equipo>> ObtenerEquiposPorCapitanAsync(int idCapitan);
    }
}
