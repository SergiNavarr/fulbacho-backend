namespace Fulbacho.Application.Modules.B2C.DTOs
{
    public class CrearDesafioDto
    {
        public int IdEquipoVisitante { get; set; }
        public DateTime FechaPropuesta { get; set; }
        public TimeSpan HoraInicio { get; set; }
        public TimeSpan HoraFin { get; set; }
        public int IdZona { get; set; }
        public int? IdCanchaSugerida { get; set; }
    }
}
