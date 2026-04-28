using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fulbacho.Shared.Entities
{
    [Table("Canchas")]
    public class Cancha
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Nombre { get; set; } = string.Empty;

        // El agregado comercial estratégico
        [Required]
        [Column(TypeName = "decimal(10, 2)")]
        public decimal PrecioPorHora { get; set; }

        [Required]
        public bool EsActivo { get; set; } = true;

        // --- Claves Foráneas ---
        [Required]
        public int IdPredio { get; set; }

        [ForeignKey("IdPredio")]
        public virtual Predio? Predio { get; set; }

        [Required]
        public int IdTipoCancha { get; set; }

        [ForeignKey("IdTipoCancha")]
        public virtual TipoCancha? TipoCancha { get; set; }

        [Required]
        public int IdSuperficie { get; set; }

        [ForeignKey("IdSuperficie")]
        public virtual Superficie? Superficie { get; set; }

        // --- Propiedades de Navegación ---
        public virtual ICollection<Reserva> Reservas { get; set; } = new List<Reserva>();
    }
}