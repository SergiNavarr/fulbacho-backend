using Fulbacho.Application.Modules.B2C.DTOs;
using Fulbacho.Application.Modules.B2C.Interfaces;
using Fulbacho.Application.Modules.B2C.Patterns.Observer;
using Microsoft.AspNetCore.Mvc;

namespace Fulbacho.API.Controllers
{
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
            var id = await _svc.CrearDesafioAsync(dto, 1); // idEquipoLocal = 1 (mock, reemplazar con JWT en Sprint 2)
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
    }
}
