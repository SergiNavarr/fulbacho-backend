using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.ComponentModel.DataAnnotations.Schema;

namespace Fulbacho.Shared.Entities
{
    [Table("Usuarios_Roles")]
    public class UsuarioRol
    {
        public int IdUsuario { get; set; }
        [ForeignKey(nameof(IdUsuario))]
        public Usuario Usuario { get; set; } = null!;

        public int IdRol { get; set; }
        [ForeignKey(nameof(IdRol))]
        public Rol Rol { get; set; } = null!;
    }
}
