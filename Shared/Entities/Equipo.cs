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

        [MaxLength(255)]
        public string EscudoUrl { get; set; } = string.Empty;

        [Required]
        public bool EsActivo { get; set; } = true;

        [Required]
        public int IdCapitan { get; set; }

        [ForeignKey("IdCapitan")]
        public virtual Usuario? Capitan { get; set; }

        [Required]
        public int IdNivel { get; set; }

        [ForeignKey("IdNivel")]
        public virtual NivelCompetitivo? NivelCompetitivo { get; set; }

        // --- Propiedades de Navegación ---
        public virtual ICollection<JugadorEquipo> Jugadores { get; set; } = new List<JugadorEquipo>();

        // Separamos los desafíos donde juega de local o de visitante
        [InverseProperty("EquipoLocal")]
        public virtual ICollection<Desafio> DesafiosLocal { get; set; } = new List<Desafio>();

        [InverseProperty("EquipoVisitante")]
        public virtual ICollection<Desafio> DesafiosVisitante { get; set; } = new List<Desafio>();
    }
}