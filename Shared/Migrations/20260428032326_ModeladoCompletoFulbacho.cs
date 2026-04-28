using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Shared.Migrations
{
    /// <inheritdoc />
    public partial class ModeladoCompletoFulbacho : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Equipos_Niveles_Competitivos_IdNivel",
                table: "Equipos");

            migrationBuilder.DropForeignKey(
                name: "FK_Jugadores_Equipos_Equipos_IdEquipo",
                table: "Jugadores_Equipos");

            migrationBuilder.DropForeignKey(
                name: "FK_Jugadores_Equipos_Usuarios_IdUsuario",
                table: "Jugadores_Equipos");

            migrationBuilder.DropForeignKey(
                name: "FK_Predios_Usuarios_IdAdministrador",
                table: "Predios");

            migrationBuilder.DropTable(
                name: "Usuarios_Roles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Niveles_Competitivos",
                table: "Niveles_Competitivos");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Jugadores_Equipos",
                table: "Jugadores_Equipos");

            migrationBuilder.DropIndex(
                name: "IX_Jugadores_Equipos_IdEquipo",
                table: "Jugadores_Equipos");

            migrationBuilder.DropColumn(
                name: "Ciudad",
                table: "Zonas");

            migrationBuilder.DropColumn(
                name: "Apellido",
                table: "Usuarios");

            migrationBuilder.DropColumn(
                name: "Nombre",
                table: "Usuarios");

            migrationBuilder.DropColumn(
                name: "ImagenUrl",
                table: "Predios");

            migrationBuilder.RenameTable(
                name: "Niveles_Competitivos",
                newName: "Niveles_competitivos");

            migrationBuilder.RenameTable(
                name: "Jugadores_Equipos",
                newName: "Jugadores_equipos");

            migrationBuilder.RenameColumn(
                name: "FechaRegistro",
                table: "Usuarios",
                newName: "RegisterDate");

            migrationBuilder.RenameColumn(
                name: "IdAdministrador",
                table: "Predios",
                newName: "IdAdmin");

            migrationBuilder.RenameIndex(
                name: "IX_Predios_IdAdministrador",
                table: "Predios",
                newName: "IX_Predios_IdAdmin");

            migrationBuilder.RenameColumn(
                name: "IdUsuario",
                table: "Jugadores_equipos",
                newName: "IdJugador");

            migrationBuilder.AddColumn<int>(
                name: "CiudadId",
                table: "Zonas",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "Telefono",
                table: "Usuarios",
                type: "character varying(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "character varying(20)",
                oldMaxLength: 20,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "Usuarios",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(150)",
                oldMaxLength: 150);

            migrationBuilder.AddColumn<string>(
                name: "Estado",
                table: "Usuarios",
                type: "character varying(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "RolId",
                table: "Usuarios",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Username",
                table: "Usuarios",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "Nombre",
                table: "Predios",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "Direccion",
                table: "Predios",
                type: "character varying(200)",
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddColumn<bool>(
                name: "EsActivo",
                table: "Predios",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AlterColumn<DateTime>(
                name: "FechaUnion",
                table: "Jugadores_equipos",
                type: "date",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.AlterColumn<string>(
                name: "EscudoUrl",
                table: "Equipos",
                type: "character varying(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "character varying(500)",
                oldMaxLength: 500,
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Niveles_competitivos",
                table: "Niveles_competitivos",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Jugadores_equipos",
                table: "Jugadores_equipos",
                columns: new[] { "IdEquipo", "IdJugador" });

            migrationBuilder.CreateTable(
                name: "Ciudades",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Nombre = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ciudades", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Estados_desafio",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Descripcion = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Estados_desafio", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Estados_reserva",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Descripcion = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Estados_reserva", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Superficies",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Descripcion = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Superficies", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TiposCancha",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Nombre = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    CantidadJugadores = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TiposCancha", x => x.Id);
                    table.CheckConstraint("CK_TipoCancha_JugadoresValidos", "\"CantidadJugadores\" > 0");
                });

            migrationBuilder.CreateTable(
                name: "Canchas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Nombre = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    PrecioPorHora = table.Column<decimal>(type: "numeric(10,2)", nullable: false),
                    EsActivo = table.Column<bool>(type: "boolean", nullable: false),
                    IdPredio = table.Column<int>(type: "integer", nullable: false),
                    IdTipoCancha = table.Column<int>(type: "integer", nullable: false),
                    IdSuperficie = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Canchas", x => x.Id);
                    table.CheckConstraint("CK_Cancha_PrecioPositivo", "\"PrecioPorHora\" >= 0");
                    table.ForeignKey(
                        name: "FK_Canchas_Predios_IdPredio",
                        column: x => x.IdPredio,
                        principalTable: "Predios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Canchas_Superficies_IdSuperficie",
                        column: x => x.IdSuperficie,
                        principalTable: "Superficies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Canchas_TiposCancha_IdTipoCancha",
                        column: x => x.IdTipoCancha,
                        principalTable: "TiposCancha",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Desafios",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    FechaPropuesta = table.Column<DateTime>(type: "date", nullable: false),
                    HoraInicio = table.Column<TimeSpan>(type: "interval", nullable: false),
                    HoraFin = table.Column<TimeSpan>(type: "interval", nullable: false),
                    IdEquipoLocal = table.Column<int>(type: "integer", nullable: false),
                    IdEquipoVisitante = table.Column<int>(type: "integer", nullable: false),
                    IdEstadoDesafio = table.Column<int>(type: "integer", nullable: false),
                    IdZona = table.Column<int>(type: "integer", nullable: false),
                    IdCanchaSugerida = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Desafios", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Desafios_Canchas_IdCanchaSugerida",
                        column: x => x.IdCanchaSugerida,
                        principalTable: "Canchas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Desafios_Equipos_IdEquipoLocal",
                        column: x => x.IdEquipoLocal,
                        principalTable: "Equipos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Desafios_Equipos_IdEquipoVisitante",
                        column: x => x.IdEquipoVisitante,
                        principalTable: "Equipos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Desafios_Estados_desafio_IdEstadoDesafio",
                        column: x => x.IdEstadoDesafio,
                        principalTable: "Estados_desafio",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Desafios_Zonas_IdZona",
                        column: x => x.IdZona,
                        principalTable: "Zonas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Reservas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    FechaHoraInicio = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    FechaHoraFin = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    MontoSena = table.Column<decimal>(type: "numeric(10,2)", nullable: false),
                    MontoTotal = table.Column<decimal>(type: "numeric(10,2)", nullable: false),
                    Notas = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    IdUsuarioCreador = table.Column<int>(type: "integer", nullable: false),
                    IdCancha = table.Column<int>(type: "integer", nullable: false),
                    IdEstadoReserva = table.Column<int>(type: "integer", nullable: false),
                    IdDesafio = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reservas", x => x.Id);
                    table.CheckConstraint("CK_Reserva_MontosPositivos", "\"MontoSena\" >= 0 AND \"MontoTotal\" >= 0");
                    table.ForeignKey(
                        name: "FK_Reservas_Canchas_IdCancha",
                        column: x => x.IdCancha,
                        principalTable: "Canchas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Reservas_Desafios_IdDesafio",
                        column: x => x.IdDesafio,
                        principalTable: "Desafios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Reservas_Estados_reserva_IdEstadoReserva",
                        column: x => x.IdEstadoReserva,
                        principalTable: "Estados_reserva",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Reservas_Usuarios_IdUsuarioCreador",
                        column: x => x.IdUsuarioCreador,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Zonas_CiudadId",
                table: "Zonas",
                column: "CiudadId");

            migrationBuilder.CreateIndex(
                name: "IX_Usuarios_Email",
                table: "Usuarios",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Usuarios_RolId",
                table: "Usuarios",
                column: "RolId");

            migrationBuilder.CreateIndex(
                name: "IX_Usuarios_Username",
                table: "Usuarios",
                column: "Username",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Jugadores_equipos_IdJugador",
                table: "Jugadores_equipos",
                column: "IdJugador");

            migrationBuilder.CreateIndex(
                name: "IX_Canchas_IdPredio",
                table: "Canchas",
                column: "IdPredio");

            migrationBuilder.CreateIndex(
                name: "IX_Canchas_IdSuperficie",
                table: "Canchas",
                column: "IdSuperficie");

            migrationBuilder.CreateIndex(
                name: "IX_Canchas_IdTipoCancha",
                table: "Canchas",
                column: "IdTipoCancha");

            migrationBuilder.CreateIndex(
                name: "IX_Desafios_IdCanchaSugerida",
                table: "Desafios",
                column: "IdCanchaSugerida");

            migrationBuilder.CreateIndex(
                name: "IX_Desafios_IdEquipoLocal",
                table: "Desafios",
                column: "IdEquipoLocal");

            migrationBuilder.CreateIndex(
                name: "IX_Desafios_IdEquipoVisitante",
                table: "Desafios",
                column: "IdEquipoVisitante");

            migrationBuilder.CreateIndex(
                name: "IX_Desafios_IdEstadoDesafio",
                table: "Desafios",
                column: "IdEstadoDesafio");

            migrationBuilder.CreateIndex(
                name: "IX_Desafios_IdZona",
                table: "Desafios",
                column: "IdZona");

            migrationBuilder.CreateIndex(
                name: "IX_Reservas_IdCancha",
                table: "Reservas",
                column: "IdCancha");

            migrationBuilder.CreateIndex(
                name: "IX_Reservas_IdDesafio",
                table: "Reservas",
                column: "IdDesafio");

            migrationBuilder.CreateIndex(
                name: "IX_Reservas_IdEstadoReserva",
                table: "Reservas",
                column: "IdEstadoReserva");

            migrationBuilder.CreateIndex(
                name: "IX_Reservas_IdUsuarioCreador",
                table: "Reservas",
                column: "IdUsuarioCreador");

            migrationBuilder.AddForeignKey(
                name: "FK_Equipos_Niveles_competitivos_IdNivel",
                table: "Equipos",
                column: "IdNivel",
                principalTable: "Niveles_competitivos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Jugadores_equipos_Equipos_IdEquipo",
                table: "Jugadores_equipos",
                column: "IdEquipo",
                principalTable: "Equipos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Jugadores_equipos_Usuarios_IdJugador",
                table: "Jugadores_equipos",
                column: "IdJugador",
                principalTable: "Usuarios",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Predios_Usuarios_IdAdmin",
                table: "Predios",
                column: "IdAdmin",
                principalTable: "Usuarios",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Usuarios_Roles_RolId",
                table: "Usuarios",
                column: "RolId",
                principalTable: "Roles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Zonas_Ciudades_CiudadId",
                table: "Zonas",
                column: "CiudadId",
                principalTable: "Ciudades",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Equipos_Niveles_competitivos_IdNivel",
                table: "Equipos");

            migrationBuilder.DropForeignKey(
                name: "FK_Jugadores_equipos_Equipos_IdEquipo",
                table: "Jugadores_equipos");

            migrationBuilder.DropForeignKey(
                name: "FK_Jugadores_equipos_Usuarios_IdJugador",
                table: "Jugadores_equipos");

            migrationBuilder.DropForeignKey(
                name: "FK_Predios_Usuarios_IdAdmin",
                table: "Predios");

            migrationBuilder.DropForeignKey(
                name: "FK_Usuarios_Roles_RolId",
                table: "Usuarios");

            migrationBuilder.DropForeignKey(
                name: "FK_Zonas_Ciudades_CiudadId",
                table: "Zonas");

            migrationBuilder.DropTable(
                name: "Ciudades");

            migrationBuilder.DropTable(
                name: "Reservas");

            migrationBuilder.DropTable(
                name: "Desafios");

            migrationBuilder.DropTable(
                name: "Estados_reserva");

            migrationBuilder.DropTable(
                name: "Canchas");

            migrationBuilder.DropTable(
                name: "Estados_desafio");

            migrationBuilder.DropTable(
                name: "Superficies");

            migrationBuilder.DropTable(
                name: "TiposCancha");

            migrationBuilder.DropIndex(
                name: "IX_Zonas_CiudadId",
                table: "Zonas");

            migrationBuilder.DropIndex(
                name: "IX_Usuarios_Email",
                table: "Usuarios");

            migrationBuilder.DropIndex(
                name: "IX_Usuarios_RolId",
                table: "Usuarios");

            migrationBuilder.DropIndex(
                name: "IX_Usuarios_Username",
                table: "Usuarios");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Niveles_competitivos",
                table: "Niveles_competitivos");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Jugadores_equipos",
                table: "Jugadores_equipos");

            migrationBuilder.DropIndex(
                name: "IX_Jugadores_equipos_IdJugador",
                table: "Jugadores_equipos");

            migrationBuilder.DropColumn(
                name: "CiudadId",
                table: "Zonas");

            migrationBuilder.DropColumn(
                name: "Estado",
                table: "Usuarios");

            migrationBuilder.DropColumn(
                name: "RolId",
                table: "Usuarios");

            migrationBuilder.DropColumn(
                name: "Username",
                table: "Usuarios");

            migrationBuilder.DropColumn(
                name: "EsActivo",
                table: "Predios");

            migrationBuilder.RenameTable(
                name: "Niveles_competitivos",
                newName: "Niveles_Competitivos");

            migrationBuilder.RenameTable(
                name: "Jugadores_equipos",
                newName: "Jugadores_Equipos");

            migrationBuilder.RenameColumn(
                name: "RegisterDate",
                table: "Usuarios",
                newName: "FechaRegistro");

            migrationBuilder.RenameColumn(
                name: "IdAdmin",
                table: "Predios",
                newName: "IdAdministrador");

            migrationBuilder.RenameIndex(
                name: "IX_Predios_IdAdmin",
                table: "Predios",
                newName: "IX_Predios_IdAdministrador");

            migrationBuilder.RenameColumn(
                name: "IdJugador",
                table: "Jugadores_Equipos",
                newName: "IdUsuario");

            migrationBuilder.AddColumn<string>(
                name: "Ciudad",
                table: "Zonas",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "Telefono",
                table: "Usuarios",
                type: "character varying(20)",
                maxLength: 20,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(20)",
                oldMaxLength: 20);

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "Usuarios",
                type: "character varying(150)",
                maxLength: 150,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100);

            migrationBuilder.AddColumn<string>(
                name: "Apellido",
                table: "Usuarios",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Nombre",
                table: "Usuarios",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "Nombre",
                table: "Predios",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "Direccion",
                table: "Predios",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(200)",
                oldMaxLength: 200);

            migrationBuilder.AddColumn<string>(
                name: "ImagenUrl",
                table: "Predios",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<DateTime>(
                name: "FechaUnion",
                table: "Jugadores_Equipos",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "date");

            migrationBuilder.AlterColumn<string>(
                name: "EscudoUrl",
                table: "Equipos",
                type: "character varying(500)",
                maxLength: 500,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(255)",
                oldMaxLength: 255);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Niveles_Competitivos",
                table: "Niveles_Competitivos",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Jugadores_Equipos",
                table: "Jugadores_Equipos",
                columns: new[] { "IdUsuario", "IdEquipo" });

            migrationBuilder.CreateTable(
                name: "Usuarios_Roles",
                columns: table => new
                {
                    IdUsuario = table.Column<int>(type: "integer", nullable: false),
                    IdRol = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuarios_Roles", x => new { x.IdUsuario, x.IdRol });
                    table.ForeignKey(
                        name: "FK_Usuarios_Roles_Roles_IdRol",
                        column: x => x.IdRol,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Usuarios_Roles_Usuarios_IdUsuario",
                        column: x => x.IdUsuario,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Jugadores_Equipos_IdEquipo",
                table: "Jugadores_Equipos",
                column: "IdEquipo");

            migrationBuilder.CreateIndex(
                name: "IX_Usuarios_Roles_IdRol",
                table: "Usuarios_Roles",
                column: "IdRol");

            migrationBuilder.AddForeignKey(
                name: "FK_Equipos_Niveles_Competitivos_IdNivel",
                table: "Equipos",
                column: "IdNivel",
                principalTable: "Niveles_Competitivos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Jugadores_Equipos_Equipos_IdEquipo",
                table: "Jugadores_Equipos",
                column: "IdEquipo",
                principalTable: "Equipos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Jugadores_Equipos_Usuarios_IdUsuario",
                table: "Jugadores_Equipos",
                column: "IdUsuario",
                principalTable: "Usuarios",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Predios_Usuarios_IdAdministrador",
                table: "Predios",
                column: "IdAdministrador",
                principalTable: "Usuarios",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
