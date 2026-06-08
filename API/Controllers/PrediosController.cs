using Microsoft.AspNetCore.Mvc;
using Fulbacho.Application.Modules.B2C.Interfaces;

namespace Fulbacho.API.Controllers
{
    [ApiController]
    [Route("api/b2c/[controller]")]
    public class PrediosController : ControllerBase
    {
        private readonly IPredioService _predioService;

        public PrediosController(IPredioService predioService)
        {
            _predioService = predioService;
        }

        [HttpGet]
        public async Task<IActionResult> GetPredios([FromQuery] string? zona, [FromQuery] string? nombre)
        {
            if (string.IsNullOrWhiteSpace(zona) && string.IsNullOrWhiteSpace(nombre))
            {
                var todos = await _predioService.ObtenerTodosLosPrediosAsync();
                return Ok(todos);
            }

            var filtrados = await _predioService.FiltrarPrediosAsync(nombre, zona);
            return Ok(filtrados);
        }
    }
}