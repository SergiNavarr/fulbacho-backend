using Fulbacho.Application.Modules.B2C.DTOs;
using Fulbacho.Application.Modules.B2C.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Fulbacho.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/b2c/[controller]")]
    public class ReservasController : ControllerBase
    {
        private readonly IReservaService _svc;

        public ReservasController(IReservaService svc)
        {
            _svc = svc;
        }

        [HttpPost]
        public async Task<IActionResult> Crear([FromBody] CrearReservaDto dto)
        {
            try
            {
                var id = await _svc.CrearReservaAsync(dto, ObtenerIdUsuario());
                return CreatedAtAction(nameof(ObtenerPorId), new { id }, new { id });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> ObtenerPorId(int id)
        {
            var reserva = await _svc.ObtenerPorIdAsync(id);
            if (reserva == null) return NotFound();

            return Ok(new
            {
                id = reserva.Id,
                idCancha = reserva.IdCancha,
                cancha = reserva.Cancha?.Nombre,
                fechaHoraInicio = reserva.FechaHoraInicio,
                fechaHoraFin = reserva.FechaHoraFin,
                montoSena = reserva.MontoSena,
                montoTotal = reserva.MontoTotal,
                estado = reserva.Estado?.Descripcion,
                idDesafio = reserva.IdDesafio
            });
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
