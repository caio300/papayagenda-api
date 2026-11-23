using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PapayAgenda.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AdicionaPermiteApenasUmPorAgendamento : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "EBrindeOuIncluso",
                table: "Servicos",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "PermiteApenasUmPorAgendamento",
                table: "CategoriasServico",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EBrindeOuIncluso",
                table: "Servicos");

            migrationBuilder.DropColumn(
                name: "PermiteApenasUmPorAgendamento",
                table: "CategoriasServico");
        }
    }
}
