using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fulbacho.Shared.Entities
{
    [Table("Reservas")]
    public class Reserva
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public DateTime FechaHoraInicio { get; set; }

        [Required]
        public DateTime FechaHoraFin { get; set; }

        [Required]
        [Column(TypeName = "decimal(10, 2)")]
        public decimal MontoSena { get; set; }

        [Required]
        [Column(TypeName = "decimal(10, 2)")]
        public decimal MontoTotal { get; set; }

        [MaxLength(500)]
        public string Notas { get; set; } = string.Empty;

        // --- Claves Foráneas ---
        [Required]
        public int IdUsuarioCreador { get; set; }

        [ForeignKey("IdUsuarioCreador")]
        public virtual Usuario? UsuarioCreador { get; set; }

        [Required]
        public int IdCancha { get; set; }

        [ForeignKey("IdCancha")]
        public virtual Cancha? Cancha { get; set; }

        [Required]
        public int IdEstadoReserva { get; set; }

        [ForeignKey("IdEstadoReserva")]
        public virtual EstadoReserva? Estado { get; set; }

        public int? IdDesafio { get; set; }

        [ForeignKey("IdDesafio")]
        public virtual Desafio? Desafio { get; set; }
    }
}