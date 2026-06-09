namespace Application.Modules.B2C.DTOs
{
    public class CanchaResponseDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public decimal PrecioPorHora { get; set; }

        // Descripción del tipo de cancha (ej: "Fútbol 5"), tomada de la entidad TipoCancha.
        public string TipoCancha { get; set; } = string.Empty;

        // Descripción de la superficie (ej: "Césped Sintético"), tomada de la entidad Superficie.
        // NOTA: la entidad Cancha NO posee un campo "Techada", por eso no se expone uno.
        public string Superficie { get; set; } = string.Empty;
    }
}
