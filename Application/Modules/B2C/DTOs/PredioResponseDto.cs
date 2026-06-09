using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Modules.B2C.DTOs
{
    public class PredioResponseDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Direccion { get; set; } = string.Empty;
        public int IdZona { get; set; }
        public string Zona { get; set; } = string.Empty;

        // La entidad Predio NO tiene una columna ImagenUrl en el modelo actual.
        // Para evitar una migración sorpresa, se expone como opcional y se mapea siempre a null.
        // (El día que el modelo incorpore la imagen del predio, sólo habrá que completar el mapeo.)
        public string? ImagenUrl { get; set; } = null;

        // Canchas del predio. Sólo se completa en el detalle (ObtenerPredioPorIdAsync).
        // En los listados queda como colección vacía para NO romper el contrato actual del front
        // (que sólo espera id, nombre, direccion y zona).
        public ICollection<CanchaResponseDto> Canchas { get; set; } = new List<CanchaResponseDto>();
    }
}
