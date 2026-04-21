using Fulbacho.Application.Modules.B2C.DTOs;
using Fulbacho.Application.Modules.B2C.Services;
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
                // TODO: Más adelante sacaremos el idCapitan del Token JWT.
                // Por ahora, simulamos (mockeamos) que el Capitán logueado es el Usuario con ID 1.
                int idCapitanMock = 1;

                await _equipoService.CrearEquipoAsync(dto, idCapitanMock);

                return Ok(new { mensaje = "¡Equipo creado con éxito!" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
    }
}
