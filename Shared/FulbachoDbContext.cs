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

        public DbSet<Rol> Roles { get; set; }
        public DbSet<Ciudad> Ciudades { get; set; }
        public DbSet<NivelCompetitivo> NivelesCompetitivos { get; set; }
        public DbSet<Superficie> Superficies { get; set; }
        public DbSet<TipoCancha> TiposCancha { get; set; }
        public DbSet<EstadoReserva> EstadosReserva { get; set; }
        public DbSet<EstadoDesafio> EstadosDesafio { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Zona> Zonas { get; set; }

        public DbSet<Equipo> Equipos { get; set; }
        public DbSet<Predio> Predios { get; set; }
        public DbSet<Cancha> Canchas { get; set; }

        public DbSet<JugadorEquipo> JugadoresEquipos { get; set; }
        public DbSet<Desafio> Desafios { get; set; }
        public DbSet<Reserva> Reservas { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Usuario>()
                .HasIndex(u => u.Email)
                .IsUnique();

            modelBuilder.Entity<Cancha>()
                .ToTable(t => t.HasCheckConstraint("CK_Cancha_PrecioPositivo", "\"PrecioPorHora\" >= 0"));

            modelBuilder.Entity<Reserva>()
                .ToTable(t => t.HasCheckConstraint("CK_Reserva_MontosPositivos", "\"MontoSena\" >= 0 AND \"MontoTotal\" >= 0"));

            modelBuilder.Entity<TipoCancha>()
                .ToTable(t => t.HasCheckConstraint("CK_TipoCancha_JugadoresValidos", "\"CantidadJugadores\" > 0"));

            modelBuilder.Entity<Usuario>()
                .HasIndex(u => u.Username)
                .IsUnique();

            modelBuilder.Entity<JugadorEquipo>()
                .HasKey(je => new { je.IdEquipo, je.IdJugador });

            modelBuilder.Entity<Desafio>()
                .HasOne(d => d.EquipoLocal)
                .WithMany(e => e.DesafiosLocal)
                .HasForeignKey(d => d.IdEquipoLocal)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Desafio>()
                .HasOne(d => d.EquipoVisitante)
                .WithMany(e => e.DesafiosVisitante)
                .HasForeignKey(d => d.IdEquipoVisitante)
                .OnDelete(DeleteBehavior.Restrict);

            foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            {
                relationship.DeleteBehavior = DeleteBehavior.Restrict;
            }
        }
    }
}