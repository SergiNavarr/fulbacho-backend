using Fulbacho.Application.Modules.B2C.DTOs;
using Fulbacho.Application.Modules.B2C.Interfaces;
using Fulbacho.Application.Modules.B2C.Patterns.Observer;
using Fulbacho.Application.Modules.B2C.Patterns.Strategy;
using Fulbacho.Shared;
using Fulbacho.Shared.Entities;
using Microsoft.EntityFrameworkCore;

namespace Fulbacho.Application.Modules.B2C.Services
{
    public class DesafioService : IDesafioService
    {
        private readonly FulbachoDbContext _context;
        private readonly MotorMatchmaking _motor;
        private readonly List<IObservadorDesafio> _observadores = new();

        // Id de estados según el seed de FulbachoDbContext (HasData). Comparamos por Id
        // y no por descripción para que un cambio de texto/acento no rompa la validación en silencio.
        private const int EstadoDesafioPendiente = 1;
        private const int EstadoDesafioAceptado = 2;
        private const int EstadoReservaCancelada = 3;

        public DesafioService(FulbachoDbContext context, MotorMatchmaking motor)
        {
            _context = context;
            _motor = motor;
        }

        public void Suscribir(IObservadorDesafio observador)   => _observadores.Add(observador);
        public void Desuscribir(IObservadorDesafio observador) => _observadores.Remove(observador);

        public async Task<Desafio?> ObtenerPorIdAsync(int idDesafio)
        {
            return await _context.Desafios
                .Include(d => d.EquipoLocal).ThenInclude(e => e!.NivelCompetitivo)
                .Include(d => d.EquipoVisitante)
                .Include(d => d.Estado)
                .Include(d => d.Zona)
                .FirstOrDefaultAsync(d => d.Id == idDesafio);
        }

        // Bandeja de desafíos del equipo (HU 2.3). Trae los desafíos donde el equipo
        // participa como local (enviados) o como visitante (recibidos), ordenados por
        // fecha descendente. Se mapea a DTO para no exponer la entidad cruda.
        public async Task<List<DesafioResponseDto>> ObtenerDesafiosPorEquipoAsync(int idEquipo)
        {
            var desafios = await _context.Desafios
                .Include(d => d.EquipoLocal).ThenInclude(e => e!.NivelCompetitivo)
                .Include(d => d.EquipoVisitante).ThenInclude(e => e!.NivelCompetitivo)
                .Include(d => d.Estado)
                .Include(d => d.Zona)
                .Include(d => d.CanchaSugerida)
                .Where(d => d.IdEquipoLocal == idEquipo || d.IdEquipoVisitante == idEquipo)
                .OrderByDescending(d => d.FechaPropuesta)
                .ThenByDescending(d => d.HoraInicio)
                .ToListAsync();

            return desafios.Select(d => MapearADto(d, idEquipo)).ToList();
        }

        // Aplana un Desafio a DTO desde la perspectiva del equipo consultado:
        // calcula el rol (Enviado/Recibido) y los datos del rival (el otro equipo).
        private static DesafioResponseDto MapearADto(Desafio d, int idEquipo)
        {
            bool esLocal = d.IdEquipoLocal == idEquipo;
            var rival = esLocal ? d.EquipoVisitante : d.EquipoLocal;

            return new DesafioResponseDto
            {
                Id = d.Id,
                IdEquipoLocal = d.IdEquipoLocal,
                IdEquipoVisitante = d.IdEquipoVisitante,
                Rol = esLocal ? "Enviado" : "Recibido",
                RivalNombre = rival?.Nombre ?? string.Empty,
                RivalEscudoUrl = rival?.EscudoUrl ?? string.Empty,
                RivalNivel = rival?.NivelCompetitivo?.Descripcion ?? string.Empty,
                Estado = d.Estado?.Descripcion ?? string.Empty,
                // FechaPropuesta es columna 'date'; se etiqueta como UTC para que el JSON
                // salga con sufijo 'Z' (contrato: el back habla UTC). Las horas van tal cual.
                FechaPropuesta = DateTime.SpecifyKind(d.FechaPropuesta, DateTimeKind.Utc),
                HoraInicio = d.HoraInicio,
                HoraFin = d.HoraFin,
                Zona = d.Zona?.Nombre ?? string.Empty,
                Cancha = d.CanchaSugerida?.Nombre ?? string.Empty
            };
        }

        public async Task<int> CrearDesafioAsync(CrearDesafioDto dto, int idCapitan)
        {
            await VerificarEquipoPerteneceACapitanAsync(dto.IdEquipoLocal, idCapitan);
            await VerificarEquipoActivoAsync(dto.IdEquipoVisitante, "visitante");
            await VerificarMismoNivelAsync(dto.IdEquipoLocal, dto.IdEquipoVisitante);
            await VerificarZonaExisteAsync(dto.IdZona);
            await VerificarCanchaExisteAsync(dto.IdCanchaSugerida);
            await VerificarCanchaPerteneceAZonaAsync(dto.IdCanchaSugerida, dto.IdZona);
            await VerificarDisponibilidadCanchaAsync(dto.IdCanchaSugerida, dto.FechaPropuesta, dto.HoraInicio, dto.HoraFin);

            var desafio = new Desafio
            {
                IdEquipoLocal = dto.IdEquipoLocal,
                IdEquipoVisitante = dto.IdEquipoVisitante,
                FechaPropuesta = dto.FechaPropuesta,
                HoraInicio = dto.HoraInicio,
                HoraFin = dto.HoraFin,
                IdZona = dto.IdZona,
                IdCanchaSugerida = dto.IdCanchaSugerida,
                IdEstadoDesafio = 1 // Pendiente
            };

            _context.Desafios.Add(desafio);
            await _context.SaveChangesAsync();
            return desafio.Id;
        }

        public async Task AceptarDesafioAsync(int idDesafio)
        {
            var desafio = await ObtenerPorIdAsync(idDesafio)
                ?? throw new Exception("Desafío no encontrado.");
            desafio.Aceptar();
            await _context.SaveChangesAsync();
            await NotificarAsync(desafio, "Aceptado");
        }

        public async Task RechazarDesafioAsync(int idDesafio)
        {
            var desafio = await ObtenerPorIdAsync(idDesafio)
                ?? throw new Exception("Desafío no encontrado.");
            desafio.Rechazar();
            await _context.SaveChangesAsync();
            await NotificarAsync(desafio, "Rechazado");
        }

        private async Task VerificarEquipoActivoAsync(int idEquipo, string rol)
        {
            bool existe = await _context.Equipos.AnyAsync(e => e.Id == idEquipo && e.EsActivo);
            if (!existe)
                throw new Exception($"El equipo {rol} no existe o no está activo.");
        }

        // El equipo local debe existir, estar activo y pertenecer al capitán autenticado (del token).
        // Esto evita que un capitán cree desafíos en nombre de un equipo que no es suyo.
        private async Task VerificarEquipoPerteneceACapitanAsync(int idEquipoLocal, int idCapitan)
        {
            await VerificarEquipoActivoAsync(idEquipoLocal, "local");
            bool esDelCapitan = await _context.Equipos
                .AnyAsync(e => e.Id == idEquipoLocal && e.IdCapitan == idCapitan);
            if (!esDelCapitan)
                throw new Exception("El equipo seleccionado no te pertenece.");
        }

        private async Task VerificarMismoNivelAsync(int idEquipoLocal, int idEquipoVisitante)
        {
            var niveles = await _context.Equipos
                .Where(e => e.Id == idEquipoLocal || e.Id == idEquipoVisitante)
                .Select(e => e.IdNivel)
                .ToListAsync();
            if (niveles.Distinct().Count() > 1)
                throw new Exception("Los equipos deben tener el mismo nivel competitivo.");
        }

        private async Task VerificarZonaExisteAsync(int idZona)
        {
            bool existe = await _context.Zonas.AnyAsync(z => z.Id == idZona);
            if (!existe)
                throw new Exception("La zona seleccionada no existe.");
        }

        private async Task VerificarCanchaExisteAsync(int idCancha)
        {
            bool existe = await _context.Canchas.AnyAsync(c => c.Id == idCancha && c.EsActivo);
            if (!existe)
                throw new Exception("La cancha seleccionada no existe o no está activa.");
        }

        // La cancha pertenece a un Predio, y el Predio pertenece a una Zona (Cancha.IdPredio -> Predio.IdZona).
        // Por eso podemos chequear que la cancha sugerida esté dentro de la zona del desafío.
        private async Task VerificarCanchaPerteneceAZonaAsync(int idCancha, int idZona)
        {
            bool perteneceAZona = await _context.Canchas
                .AnyAsync(c => c.Id == idCancha && c.Predio!.IdZona == idZona);
            if (!perteneceAZona)
                throw new Exception("La cancha seleccionada no pertenece a la zona del desafío.");
        }

        // No se puede desafiar en una cancha que ya está ocupada en ese rango horario.
        // Mismo criterio de solapamiento que ReservaService.VerificarDisponibilidadAsync:
        // hay solapamiento si  inicioExistente < finNuevo  &&  inicioNuevo < finExistente.
        private async Task VerificarDisponibilidadCanchaAsync(int idCancha, DateTime fechaPropuesta, TimeSpan horaInicio, TimeSpan horaFin)
        {
            // Reserva: trabaja con DateTime, así que reconstruimos el rango propuesto sobre la fecha del desafío.
            // La fecha/hora viene en hora local AR; las columnas de Reservas son timestamptz, así que
            // convertimos a UTC antes de consultar (Npgsql rechaza DateTime con Kind=Unspecified).
            DateTime inicioPropuesto = ZonaHorariaArgentina.ConvertirAUtc(fechaPropuesta.Date + horaInicio);
            DateTime finPropuesto = ZonaHorariaArgentina.ConvertirAUtc(fechaPropuesta.Date + horaFin);

            // Una reserva bloquea el horario salvo que esté Cancelada.
            bool ocupadaPorReserva = await _context.Reservas
                .AnyAsync(r => r.IdCancha == idCancha
                    && r.IdEstadoReserva != EstadoReservaCancelada
                    && r.FechaHoraInicio < finPropuesto
                    && inicioPropuesto < r.FechaHoraFin);

            // Otro desafío bloquea el horario si está Pendiente o Aceptado (no Rechazado).
            // El estado Confirmado ya genera su Reserva asociada, por lo que queda cubierto por el chequeo anterior.
            bool ocupadaPorDesafio = await _context.Desafios
                .AnyAsync(d => d.IdCanchaSugerida == idCancha
                    && d.FechaPropuesta == fechaPropuesta.Date
                    && (d.IdEstadoDesafio == EstadoDesafioPendiente || d.IdEstadoDesafio == EstadoDesafioAceptado)
                    && d.HoraInicio < horaFin
                    && horaInicio < d.HoraFin);

            if (ocupadaPorReserva || ocupadaPorDesafio)
                throw new Exception("La cancha ya está ocupada en ese horario.");
        }

        public async Task<IEnumerable<Equipo>> BuscarRivalesAsync(int idEquipoBuscador)
        {
            var equipoBuscador = await _context.Equipos
                .Include(e => e.NivelCompetitivo)
                .Include(e => e.DesafiosLocal)
                .Include(e => e.DesafiosVisitante)
                .FirstOrDefaultAsync(e => e.Id == idEquipoBuscador && e.EsActivo)
                ?? throw new Exception("Equipo no encontrado o inactivo.");

            var candidatos = await _context.Equipos
                .Include(e => e.NivelCompetitivo)
                .Include(e => e.DesafiosLocal)
                .Include(e => e.DesafiosVisitante)
                .Where(e => e.EsActivo)
                .ToListAsync();

            return await _motor.BuscarRivalesAsync(equipoBuscador, candidatos);
        }

        private async Task NotificarAsync(Desafio desafio, string evento)
        {
            foreach (var obs in _observadores)
                await obs.OnDesafioActualizadoAsync(desafio, evento);
        }
    }
}
