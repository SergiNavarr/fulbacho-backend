# CONTEXTO DEL PROYECTO: FULBACHO

## 1. Visión General
- **Nombre:** Fulbacho.
- **Propósito:** Plataforma integral de matchmaking y gestión de reservas para fútbol amateur.
- **Módulos:** - B2C (Jugadores/Capitanes): Gestión de identidad, exploración de predios, y matchmaking (desafíos).
  - B2B (Predios Deportivos): Gestión de infraestructura y confirmación de turnos.

## 2. Reglas Académicas Estrictas (¡CRÍTICO PARA LA CÁTEDRA!)
- **Nomenclatura en Español:** Todos los métodos de servicios, interfaces y validaciones internas DEBEN nombrarse en español, manteniendo el sufijo Async (ej. `CrearEquipoAsync`, `ObtenerTodosLosPrediosAsync`, `VerificarNivelAsync`). No uses inglés para la lógica de negocio.
- **Trazabilidad con UML (Código Limpio):** Las validaciones de negocio (ej. verificar si un nivel existe en la BD) deben extraerse obligatoriamente a métodos privados dentro del mismo servicio. Esto es para garantizar la trazabilidad 1:1 con los auto-mensajes (self-messages) de los diagramas de secuencia UML del proyecto.
- **Aislamiento de Capas:** Está prohibido colocar lógica de negocio o de base de datos directamente en los Controladores (Capa de Presentación). El controlador solo recibe la petición HTTP, llama al DTO y delega el trabajo al Servicio correspondiente.

## 3. Arquitectura y Stack Tecnológico
- **Patrón Principal:** Monolito Modular en N-Capas (Presentación, Negocio, Datos). Desacoplamiento lógico estricto entre B2C y B2B.
- **Backend:** C# con .NET 9.
- **Base de Datos:** PostgreSQL con Entity Framework Core (Code-First).
- **Frontend:** React / Next.js y Tailwind CSS (SPA Mobile-first para B2C, Dashboard web para B2B).
- **Tiempo Real & Seguridad:** SignalR (WebSockets) para notificaciones de desafíos y turnos. JWT para autenticación.

## 4. Estructura del Backend (Directorios)
- `Fulbacho.Shared.Entities`: Entidades del dominio (ej. `Equipo`, `Predio`, `Zona`, `NivelCompetitivo`).
- `Fulbacho.Application.Modules.B2C.DTOs`: Objetos de transferencia de datos.
- `Fulbacho.Application.Modules.B2C.Interfaces`: Contratos de los servicios.
- `Fulbacho.Application.Modules.B2C.Services`: Implementación de la lógica de negocio.

## INSTRUCCIONES PARA CLAUDE:
Antes de crear o modificar cualquier archivo, verifica esta estructura. Si vas a generar código C#, asegúrate de cumplir la regla de Nomenclatura en Español y la extracción de métodos privados. Si modificas un servicio, actualiza siempre su interfaz correspondiente.