using Fulbacho.Application.Modules.B2C.DTOs;
using Fulbacho.Application.Modules.B2C.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Fulbacho.API.Controllers
{
    [ApiController]
    [Route("api/b2c/autenticacion")]
    public class AutenticacionController : ControllerBase
    {
        private readonly IAutenticacionService _autenticacionService;

        public AutenticacionController(IAutenticacionService autenticacionService)
        {
            _autenticacionService = autenticacionService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginJugadorDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var resultado = await _autenticacionService.LoginAsync(dto);

            if (resultado == null)
                return Unauthorized(new { mensaje = "Credenciales inválidas. Verificá tu email y contraseña." });

            return Ok(resultado);
        }

        [HttpPost("registro")]
        public async Task<IActionResult> Registro([FromBody] RegistroJugadorDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var resultado = await _autenticacionService.RegistrarJugadorAsync(dto);

            if (resultado == null)
                return BadRequest(new { mensaje = "El email o nombre de usuario ya se encuentra registrado." });

            return Ok(resultado);
        }
    }
}
