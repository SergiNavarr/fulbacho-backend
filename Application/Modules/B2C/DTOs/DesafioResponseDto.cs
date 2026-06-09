namespace Fulbacho.Application.Modules.B2C.DTOs
{
    // Representa un desafío para la bandeja del equipo (HU 2.3).
    // NO se devuelve la entidad cruda: se aplana lo necesario para la UI.
    public class DesafioResponseDto
    {
        public int Id { get; set; }

        // Ids crudos para que el front pueda recalcular perspectiva si lo necesita.
        public int IdEquipoLocal { get; set; }
        public int IdEquipoVisitante { get; set; }

        // Rol del desafío YA calculado server-side respecto del equipo consultado:
        //   "Enviado"   => el equipo consultado es el local (él propuso el desafío).
        //   "Recibido"  => el equipo consultado es el visitante (le llegó el desafío).
        public string Rol { get; set; } = string.Empty;

        // Datos del rival = "el otro equipo" respecto del equipo consultado.
        public string RivalNombre { get; set; } = string.Empty;
        public string RivalEscudoUrl { get; set; } = string.Empty;
        public string RivalNivel { get; set; } = string.Empty;

        // Descripción del estado (ej: "Pendiente", "Aceptado"), no el Id.
        public string Estado { get; set; } = string.Empty;

        // Fecha del partido. Es columna 'date' (wall-clock AR), pero respetamos el contrato
        // de que el back habla UTC: se serializa con Kind=Utc => sufijo 'Z' en el JSON.
        public DateTime FechaPropuesta { get; set; }

        // Horas wall-clock (TimeSpan): van tal cual, sin conversión de zona horaria.
        public TimeSpan HoraInicio { get; set; }
        public TimeSpan HoraFin { get; set; }

        public string Zona { get; set; } = string.Empty;
        public string Cancha { get; set; } = string.Empty;
    }
}
