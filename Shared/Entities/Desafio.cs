using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fulbacho.Shared.Entities
{
    [Table("Desafios")]
    public class Desafio
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Column(TypeName = "date")]
        public DateTime FechaPropuesta { get; set; }

        [Required]
        public TimeSpan HoraInicio { get; set; }

        [Required]
        public TimeSpan HoraFin { get; set; }

        // --- Claves Foráneas ---
        [Required]
        public int IdEquipoLocal { get; set; }

        [ForeignKey("IdEquipoLocal")]
        public virtual Equipo? EquipoLocal { get; set; }

        [Required]
        public int IdEquipoVisitante { get; set; }

        [ForeignKey("IdEquipoVisitante")]
        public virtual Equipo? EquipoVisitante { get; set; }

        [Required]
        public int IdEstadoDesafio { get; set; }

        [ForeignKey("IdEstadoDesafio")]
        public virtual EstadoDesafio? Estado { get; set; }

        [Required]
        public int IdZona { get; set; }

        [ForeignKey("IdZona")]
        public virtual Zona? Zona { get; set; }

        // La cancha es opcional (nullable) porque pueden proponer el partido y decidir la cancha después
        public int? IdCanchaSugerida { get; set; }

        [ForeignKey("IdCanchaSugerida")]
        public virtual Cancha? CanchaSugerida { get; set; }

        // Propiedad de Navegación
        public virtual ICollection<Reserva> Reservas { get; set; } = new List<Reserva>();
    }
}