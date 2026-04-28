using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Fulbacho.Shared.Entities
{
    [Table("Predios")]
    public class Predio
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Nombre { get; set; } = string.Empty;

        [MaxLength(200)]
        public string Direccion { get; set; } = string.Empty;

        [Required]
        public bool EsActivo { get; set; } = true;

        // --- Claves Foráneas ---
        [Required]
        public int IdZona { get; set; }

        [ForeignKey("IdZona")]
        public virtual Zona? Zona { get; set; }

        [Required]
        public int IdAdmin { get; set; }

        [ForeignKey("IdAdmin")]
        public virtual Usuario? Administrador { get; set; }

        // --- Propiedades de Navegación ---
        // Todas las canchas que pertenecen a este complejo
        public virtual ICollection<Cancha> Canchas { get; set; } = new List<Cancha>();
    }
}