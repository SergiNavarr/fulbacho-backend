using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Shared.Migrations
{
    /// <inheritdoc />
    public partial class CanchaObligatoriaEnDesafio : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "IdCanchaSugerida",
                table: "Desafios",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            // Seeds idempotentes: las filas ya existen en la base y el HasData del
            // modelo recién entra al snapshot en esta migración. ON CONFLICT evita
            // el 23505 (PK duplicada) y permite recrear la BD desde cero sin error.
            migrationBuilder.Sql(@"
                INSERT INTO ""Superficies"" (""Id"", ""Descripcion"") VALUES
                    (1, 'Césped Natural'),
                    (2, 'Césped Sintético'),
                    (3, 'Parquet'),
                    (4, 'Cemento')
                ON CONFLICT (""Id"") DO NOTHING;
            ");

            migrationBuilder.Sql(@"
                INSERT INTO ""TiposCancha"" (""Id"", ""CantidadJugadores"", ""Nombre"") VALUES
                    (1, 5, 'Fútbol 5'),
                    (2, 7, 'Fútbol 7'),
                    (3, 11, 'Fútbol 11')
                ON CONFLICT (""Id"") DO NOTHING;
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Superficies",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Superficies",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Superficies",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Superficies",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "TiposCancha",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "TiposCancha",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "TiposCancha",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.AlterColumn<int>(
                name: "IdCanchaSugerida",
                table: "Desafios",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");
        }
    }
}
