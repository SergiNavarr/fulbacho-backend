using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Fulbacho.Shared.Entities
{
    [Table("Jugadores_equipos")]
    [PrimaryKey(nameof(IdEquipo), nameof(IdJugador))]
    public class JugadorEquipo
    {
        // Clave Foránea 1
        public int IdEquipo { get; set; }

        [ForeignKey("IdEquipo")]
        public virtual Equipo? Equipo { get; set; }

        // Clave Foránea 2
        public int IdJugador { get; set; }

        [ForeignKey("IdJugador")]
        public virtual Usuario? Jugador { get; set; }

        [Required]
        [Column(TypeName = "date")]
        public DateTime FechaUnion { get; set; } = DateTime.UtcNow.Date;

        [Required]
        public bool EsActivo { get; set; } = true;
    }
}