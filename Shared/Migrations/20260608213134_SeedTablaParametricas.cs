using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Shared.Migrations
{
    /// <inheritdoc />
    public partial class SeedTablaParametricas : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                INSERT INTO ""Roles"" (""Id"", ""Nombre"") VALUES
                    (1, 'Jugador'),
                    (2, 'Administrador')
                ON CONFLICT (""Id"") DO NOTHING;
            ");

            migrationBuilder.Sql(@"
                INSERT INTO ""Niveles_competitivos"" (""Id"", ""Descripcion"") VALUES
                    (1, 'Amateur'),
                    (2, 'Intermedio'),
                    (3, 'Competitivo')
                ON CONFLICT (""Id"") DO NOTHING;
            ");

            migrationBuilder.Sql(@"
                INSERT INTO ""Estados_desafio"" (""Id"", ""Descripcion"") VALUES
                    (1, 'Pendiente'),
                    (2, 'Aceptado'),
                    (3, 'Rechazado'),
                    (4, 'Confirmado')
                ON CONFLICT (""Id"") DO NOTHING;
            ");

            migrationBuilder.Sql(@"
                INSERT INTO ""Estados_reserva"" (""Id"", ""Descripcion"") VALUES
                    (1, 'Pendiente'),
                    (2, 'Confirmada'),
                    (3, 'Cancelada')
                ON CONFLICT (""Id"") DO NOTHING;
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Estados_desafio",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Estados_desafio",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Estados_desafio",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Estados_desafio",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Estados_reserva",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Estados_reserva",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Estados_reserva",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Niveles_competitivos",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Niveles_competitivos",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Niveles_competitivos",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 2);
        }
    }
}
