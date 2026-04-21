using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.ComponentModel.DataAnnotations.Schema;

namespace Fulbacho.Shared.Entities
{
    [Table("Jugadores_Equipos")]
    public class JugadorEquipo
    {
        public int IdUsuario { get; set; }
        [ForeignKey(nameof(IdUsuario))]
        public Usuario Jugador { get; set; } = null!;

        public int IdEquipo { get; set; }
        [ForeignKey(nameof(IdEquipo))]
        public Equipo Equipo { get; set; } = null!;

        public DateTime FechaUnion { get; set; } = DateTime.UtcNow;
        public bool EsActivo { get; set; } = true; 
    }
}
