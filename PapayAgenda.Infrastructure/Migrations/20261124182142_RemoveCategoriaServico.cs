using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PapayAgenda.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RemoveCategoriaServico : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Servicos_CategoriasServico_CategoriaServicoId",
                table: "Servicos");

            migrationBuilder.DropTable(
                name: "CategoriasServico");

            migrationBuilder.DropIndex(
                name: "IX_Servicos_CategoriaServicoId",
                table: "Servicos");

            migrationBuilder.DropColumn(
                name: "GoogleCalendarId",
                table: "Estabelecimentos");

            migrationBuilder.RenameColumn(
                name: "CategoriaServicoId",
                table: "Servicos",
                newName: "OrdemExibicao");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "OrdemExibicao",
                table: "Servicos",
                newName: "CategoriaServicoId");

            migrationBuilder.AddColumn<string>(
                name: "GoogleCalendarId",
                table: "Estabelecimentos",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "CategoriasServico",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CategoriaPaiId = table.Column<int>(type: "int", nullable: true),
                    EstabelecimentoId = table.Column<int>(type: "int", nullable: false),
                    Ativa = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    Descricao = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Nome = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    OrdemExibicao = table.Column<int>(type: "int", nullable: false),
                    PermiteApenasUmPorAgendamento = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CategoriasServico", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CategoriasServico_CategoriasServico_CategoriaPaiId",
                        column: x => x.CategoriaPaiId,
                        principalTable: "CategoriasServico",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CategoriasServico_Estabelecimentos_EstabelecimentoId",
                        column: x => x.EstabelecimentoId,
                        principalTable: "Estabelecimentos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_Servicos_CategoriaServicoId",
                table: "Servicos",
                column: "CategoriaServicoId");

            migrationBuilder.CreateIndex(
                name: "IX_CategoriasServico_CategoriaPaiId",
                table: "CategoriasServico",
                column: "CategoriaPaiId");

            migrationBuilder.CreateIndex(
                name: "IX_CategoriasServico_EstabelecimentoId",
                table: "CategoriasServico",
                column: "EstabelecimentoId");

            migrationBuilder.AddForeignKey(
                name: "FK_Servicos_CategoriasServico_CategoriaServicoId",
                table: "Servicos",
                column: "CategoriaServicoId",
                principalTable: "CategoriasServico",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
