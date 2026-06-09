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
            // ObtenerIdUsuario() = id del capitán autenticado. El servicio valida que
            // dto.IdEquipoLocal realmente le pertenezca antes de crear el desafío.
            // Capturamos Exception genérica (igual que EquiposController) para devolver las
            // validaciones de negocio como 400 con el mensaje. IDEAL a futuro: usar un tipo
            // de excepción propio (ej. ExcepcionDeNegocio) y no mezclar errores de programación.
            try
            {
                var id = await _svc.CrearDesafioAsync(dto, ObtenerIdUsuario());
                return CreatedAtAction(nameof(ObtenerPorId), new { id }, new { id });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        // GET /api/b2c/Desafios?idEquipo=5
        // Bandeja del equipo (HU 2.3): el front segmenta por el campo "rol"
        // (Enviado = el equipo es local, Recibido = el equipo es visitante).
        [HttpGet]
        public async Task<IActionResult> ObtenerPorEquipo([FromQuery] int idEquipo)
        {
            try
            {
                var desafios = await _svc.ObtenerDesafiosPorEquipoAsync(idEquipo);
                return Ok(desafios);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
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
            try
            {
                await _svc.AceptarDesafioAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpPut("{id}/rechazar")]
        public async Task<IActionResult> Rechazar(int id)
        {
            try
            {
                await _svc.RechazarDesafioAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
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
