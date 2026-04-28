using Fulbacho.Application.Modules.B2C.DTOs;
using Fulbacho.Application.Modules.B2C.Services;
using Fulbacho.Application.Modules.B2C.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Fulbacho.API.Controllers
{
    [ApiController]
    [Route("api/b2c/[controller]")]
    public class EquiposController : ControllerBase
    {
        private readonly IEquipoService _equipoService;

        public EquiposController(IEquipoService equipoService)
        {
            _equipoService = equipoService;
        }

        [HttpPost]
        public async Task<IActionResult> CrearEquipo([FromBody] CrearEquipoDto dto)
        {
            try
            {
                int idCapitanMock = 1;

                await _equipoService.CrearEquipoAsync(dto, idCapitanMock);

                return Ok(new { mensaje = "¡Equipo creado con éxito!" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> ObtenerEquipo(int id)
        {
            try
            {
                int idCapitanMock = 1;
                var equipo = await _equipoService.ObtenerEquipoPorIdAsync(id, idCapitanMock);

                if (equipo == null) return NotFound(new { error = "Equipo no encontrado" });

                return Ok(new
                {
                    id = equipo.Id,
                    nombre = equipo.Nombre,
                    escudoUrl = equipo.EscudoUrl,
                    idNivel = equipo.IdNivel,
                    nivel = equipo.NivelCompetitivo?.Descripcion
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> ActualizarEquipo(int id, [FromBody] ActualizarEquipoDto dto)
        {
            try
            {
                int idCapitanMock = 1;
                await _equipoService.ActualizarEquipoAsync(id, dto, idCapitanMock);
                return Ok(new { mensaje = "¡Equipo actualizado correctamente!" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpGet("mis-equipos")]
        public async Task<IActionResult> ObtenerMisEquipos()
        {
            try
            {
                int idCapitanMock = 1;
                var equipos = await _equipoService.ObtenerEquiposPorCapitanAsync(idCapitanMock);

                var resultado = equipos.Select(e => new {
                    id = e.Id,
                    nombre = e.Nombre,
                    escudoUrl = e.EscudoUrl,
                    nivel = e.NivelCompetitivo?.Descripcion
                });

                return Ok(resultado);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
    }
}