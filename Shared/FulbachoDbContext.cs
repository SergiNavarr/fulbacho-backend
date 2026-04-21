using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Fulbacho.Shared.Entities;
using Microsoft.EntityFrameworkCore;

namespace Fulbacho.Shared
{
    public class FulbachoDbContext : DbContext
    {
        public FulbachoDbContext(DbContextOptions<FulbachoDbContext> options) : base(options)
        {
        }

        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Rol> Roles { get; set; }
        public DbSet<UsuarioRol> UsuariosRoles { get; set; }
        public DbSet<Zona> Zonas { get; set; }
        public DbSet<NivelCompetitivo> NivelesCompetitivos { get; set; }
        public DbSet<Equipo> Equipos { get; set; }
        public DbSet<JugadorEquipo> JugadoresEquipos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<UsuarioRol>()
                .HasKey(ur => new { ur.IdUsuario, ur.IdRol });

            modelBuilder.Entity<JugadorEquipo>()
                .HasKey(je => new { je.IdUsuario, je.IdEquipo });

            foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            {
                relationship.DeleteBehavior = DeleteBehavior.Restrict;
            }
        }
    }
}
