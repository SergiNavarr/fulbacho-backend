using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.ComponentModel.DataAnnotations;

namespace Fulbacho.Application.Modules.B2C.DTOs
{
    public class CrearEquipoDto
    {
        [Required(ErrorMessage = "El nombre del equipo es obligatorio.")]
        [MaxLength(100)]
        public string Nombre { get; set; } = string.Empty;

        public string? EscudoUrl { get; set; }

        [Required(ErrorMessage = "El nivel competitivo es obligatorio.")]
        public int IdNivel { get; set; }
    }
}
