using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fulbacho.Shared.Entities
{
    [Table("Estados_desafio")]
    public class EstadoDesafio
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Descripcion { get; set; } = string.Empty;

        // Propiedad de navegación
        public virtual ICollection<Desafio> Desafios { get; set; } = new List<Desafio>();
    }
}
