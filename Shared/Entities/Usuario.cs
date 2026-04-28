using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fulbacho.Shared.Entities
{
    [Table("Usuarios")]
    public class Usuario
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Username { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        [MaxLength(100)]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string PasswordHash { get; set; } = string.Empty;

        [Required]
        public DateTime RegisterDate { get; set; } = DateTime.UtcNow;

        [Required]
        public bool EsActivo { get; set; } = true;

        [MaxLength(20)]
        public string Telefono { get; set; } = string.Empty;

        [Required]
        [MaxLength(20)]
        public string Estado { get; set; } = "Activo"; 

        // Clave Foránea hacia Rol
        [Required]
        public int RolId { get; set; }

        [ForeignKey("RolId")]
        public virtual Rol? Rol { get; set; }

        // --- Propiedades de Navegación (Relaciones de Negocio) ---

        // Equipos donde este usuario ejerce el liderazgo como capitán
        public virtual ICollection<Equipo> EquiposCapitaneados { get; set; } = new List<Equipo>();

        // Relación con la tabla intermedia que gestiona la pertenencia a múltiples equipos
        public virtual ICollection<JugadorEquipo> JugadorEquipos { get; set; } = new List<JugadorEquipo>();

        // Predios que pertenecen o son gestionados por este usuario (Rol: Administrador/Dueño)
        public virtual ICollection<Predio> PrediosAdministrados { get; set; } = new List<Predio>();

        // Historial de reservas generadas por este usuario
        public virtual ICollection<Reserva> ReservasCreadas { get; set; } = new List<Reserva>();
    }
}