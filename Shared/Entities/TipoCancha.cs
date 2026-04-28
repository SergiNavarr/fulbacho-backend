using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fulbacho.Shared.Entities
{
    [Table("TiposCancha")]
    public class TipoCancha
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Nombre { get; set; } = string.Empty; 

        [Required]
        public int CantidadJugadores { get; set; }

        public ICollection<Cancha> Canchas { get; set; } = new List<Cancha>();
    }
}