using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fulbacho.Shared.Entities
{
    [Table("Niveles_Competitivos")]
    public class NivelCompetitivo
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Descripcion { get; set; } = string.Empty;
    }
}