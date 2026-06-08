using Fulbacho.Application.Modules.B2C.DTOs;
using Fulbacho.Application.Modules.B2C.Interfaces;
using Fulbacho.Application.Modules.B2C.Patterns.Observer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Fulbacho.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/b2c/[controller]")]
    public class DesafiosController : ControllerBase
    {
        private readonly IDesafioService _svc;

        public DesafiosController(IDesafioService svc, SignalRObserver signalR, ReservaObserver reserva)
        {
            _svc = svc;
            _svc.Suscribir(signalR);
            _svc.Suscribir(reserva);
        }

        [HttpPost]
        public async Task<IActionResult> Crear([FromBody] CrearDesafioDto dto)
        {
            var id = await _svc.CrearDesafioAsync(dto, ObtenerIdUsuario());
            return CreatedAtAction(nameof(ObtenerPorId), new { id }, new { id });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> ObtenerPorId(int id)
        {
            var desafio = await _svc.ObtenerPorIdAsync(id);
            if (desafio == null) return NotFound();
            return Ok(desafio);
        }

        [HttpPut("{id}/aceptar")]
        public async Task<IActionResult> Aceptar(int id)
        {
            await _svc.AceptarDesafioAsync(id);
            return NoContent();
        }

        [HttpPut("{id}/rechazar")]
        public async Task<IActionResult> Rechazar(int id)
        {
            await _svc.RechazarDesafioAsync(id);
            return NoContent();
        }

        // GET /api/b2c/Desafios/rivales?idEquipo=5
        [HttpGet("rivales")]
        public async Task<IActionResult> BuscarRivales([FromQuery] int idEquipo)
        {
            try
            {
                var rivales = await _svc.BuscarRivalesAsync(idEquipo);
                var resultado = rivales.Select(e => new
                {
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

        private int ObtenerIdUsuario()
        {
            var valor = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!int.TryParse(valor, out int id))
                throw new UnauthorizedAccessException("Token inválido o sin claim de identidad.");
            return id;
        }
    }
}
