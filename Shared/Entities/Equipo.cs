using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fulbacho.Shared.Entities
{
    [Table("Equipos")]
    public class Equipo
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Nombre { get; set; } = string.Empty;

        [MaxLength(500)]
        public string? EscudoUrl { get; set; }

        public bool EsActivo { get; set; } = true;

        // Foreign Keys
        [Required]
        public int IdCapitan { get; set; }
        [ForeignKey(nameof(IdCapitan))]
        public Usuario Capitan { get; set; } = null!;

        [Required]
        public int IdNivel { get; set; }
        [ForeignKey(nameof(IdNivel))]
        public NivelCompetitivo Nivel { get; set; } = null!;

        public ICollection<JugadorEquipo> Plantilla { get; set; } = new List<JugadorEquipo>();
    }
}