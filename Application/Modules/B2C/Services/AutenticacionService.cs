using Fulbacho.Application.Modules.B2C.DTOs;
using Fulbacho.Application.Modules.B2C.Interfaces;
using Fulbacho.Shared;
using Fulbacho.Shared.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Fulbacho.Application.Modules.B2C.Services
{
    public class AutenticacionService : IAutenticacionService
    {
        private readonly FulbachoDbContext _context;
        private readonly IConfiguration _configuration;

        public AutenticacionService(FulbachoDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public async Task<RespuestaLoginDto?> LoginAsync(LoginJugadorDto dto)
        {
            var usuario = await VerificarCredencialesAsync(dto.Email, dto.Password);
            if (usuario == null) return null;

            var token = GenerarTokenPrivado(usuario);

            return new RespuestaLoginDto
            {
                Token = token,
                Username = usuario.Username,
                Email = usuario.Email,
                Rol = usuario.Rol?.Nombre ?? string.Empty,
                Expiracion = DateTime.UtcNow.AddHours(8)
            };
        }

        public async Task<RespuestaLoginDto?> RegistrarJugadorAsync(RegistroJugadorDto dto)
        {
            var emailDisponible = await VerificarDisponibilidadEmailAsync(dto.Email);
            if (!emailDisponible) return null;

            var usernameDisponible = await VerificarDisponibilidadUsernameAsync(dto.Username);
            if (!usernameDisponible) return null;

            var rolJugador = await _context.Roles.FirstOrDefaultAsync(r => r.Nombre == "Jugador");
            if (rolJugador == null) return null;

            var nuevoUsuario = new Usuario
            {
                Username = dto.Username,
                Email = dto.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
                Telefono = dto.Telefono ?? string.Empty,
                RolId = rolJugador.Id,
                RegisterDate = DateTime.UtcNow,
                EsActivo = true,
                Estado = "Activo"
            };

            _context.Usuarios.Add(nuevoUsuario);
            await _context.SaveChangesAsync();

            await _context.Entry(nuevoUsuario).Reference(u => u.Rol).LoadAsync();

            var token = GenerarTokenPrivado(nuevoUsuario);

            return new RespuestaLoginDto
            {
                Token = token,
                Username = nuevoUsuario.Username,
                Email = nuevoUsuario.Email,
                Rol = nuevoUsuario.Rol?.Nombre ?? "Jugador",
                Expiracion = DateTime.UtcNow.AddHours(8)
            };
        }

        // --- Métodos Privados (auto-mensajes UML) ---

        private async Task<Usuario?> VerificarCredencialesAsync(string email, string passwordPlana)
        {
            var usuario = await _context.Usuarios
                .Include(u => u.Rol)
                .FirstOrDefaultAsync(u => u.Email == email && u.EsActivo);

            if (usuario == null) return null;
            if (!BCrypt.Net.BCrypt.Verify(passwordPlana, usuario.PasswordHash)) return null;

            return usuario;
        }

        private async Task<bool> VerificarDisponibilidadEmailAsync(string email)
        {
            return !await _context.Usuarios.AnyAsync(u => u.Email == email);
        }

        private async Task<bool> VerificarDisponibilidadUsernameAsync(string username)
        {
            return !await _context.Usuarios.AnyAsync(u => u.Username == username);
        }

        private string GenerarTokenPrivado(Usuario usuario)
        {
            var jwtKey = _configuration["Jwt:Key"]!;
            var jwtIssuer = _configuration["Jwt:Issuer"]!;
            var jwtAudience = _configuration["Jwt:Audience"]!;

            var claveSeguridad = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
            var credenciales = new SigningCredentials(claveSeguridad, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, usuario.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, usuario.Email),
                new Claim(ClaimTypes.Name, usuario.Username),
                new Claim(ClaimTypes.Role, usuario.Rol?.Nombre ?? string.Empty),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var token = new JwtSecurityToken(
                issuer: jwtIssuer,
                audience: jwtAudience,
                claims: claims,
                expires: DateTime.UtcNow.AddHours(8),
                signingCredentials: credenciales
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
