namespace Fulbacho.Application.Modules.B2C.DTOs
{
    public class CrearReservaDto
    {
        // Datos mínimos que el front necesita enviar para reservar una cancha.
        public int IdCancha { get; set; }
        public DateTime FechaHoraInicio { get; set; }
        public DateTime FechaHoraFin { get; set; }

        // Opcional: observaciones del usuario.
        public string? Notas { get; set; }

        // Opcional: si la reserva proviene de un desafío aceptado, se vincula acá.
        public int? IdDesafio { get; set; }
    }
}
