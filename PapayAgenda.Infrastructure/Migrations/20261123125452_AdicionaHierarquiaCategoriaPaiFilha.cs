using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PapayAgenda.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AdicionaHierarquiaCategoriaPaiFilha : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CategoriaPaiId",
                table: "CategoriasServico",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "OrdemExibicao",
                table: "CategoriasServico",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_CategoriasServico_CategoriaPaiId",
                table: "CategoriasServico",
                column: "CategoriaPaiId");

            migrationBuilder.AddForeignKey(
                name: "FK_CategoriasServico_CategoriasServico_CategoriaPaiId",
                table: "CategoriasServico",
                column: "CategoriaPaiId",
                principalTable: "CategoriasServico",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CategoriasServico_CategoriasServico_CategoriaPaiId",
                table: "CategoriasServico");

            migrationBuilder.DropIndex(
                name: "IX_CategoriasServico_CategoriaPaiId",
                table: "CategoriasServico");

            migrationBuilder.DropColumn(
                name: "CategoriaPaiId",
                table: "CategoriasServico");

            migrationBuilder.DropColumn(
                name: "OrdemExibicao",
                table: "CategoriasServico");
        }
    }
}
