namespace Fulbacho.Shared
{
    // Conversión centralizada entre la hora local de Argentina y UTC.
    // Toda la app persiste/consulta en UTC (columnas timestamptz de PostgreSQL exigen Kind=Utc).
    // El cliente trabaja en hora local AR; acá se hace la traducción ida y vuelta.
    public static class ZonaHorariaArgentina
    {
        // Argentina es UTC-3 todo el año (sin horario de verano desde 2009).
        // Offset fijo => determinista y sin depender de la tz database del sistema operativo.
        private static readonly TimeSpan Offset = TimeSpan.FromHours(-3);

        // Hora local AR (el Kind de entrada se ignora) -> UTC. Para consultar y persistir.
        public static DateTime ConvertirAUtc(DateTime horaLocal)
        {
            var local = DateTime.SpecifyKind(horaLocal, DateTimeKind.Unspecified);
            return DateTime.SpecifyKind(local - Offset, DateTimeKind.Utc);
        }

        // UTC (lo que está guardado en la BD) -> hora local AR. Para mostrar.
        public static DateTime ConvertirALocal(DateTime horaUtc)
        {
            var utc = DateTime.SpecifyKind(horaUtc, DateTimeKind.Utc);
            return DateTime.SpecifyKind(utc + Offset, DateTimeKind.Unspecified);
        }
    }
}
