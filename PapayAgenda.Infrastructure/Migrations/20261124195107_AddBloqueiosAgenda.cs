using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PapayAgenda.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddBloqueiosAgenda : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BloqueiosAgenda",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    EstabelecimentoId = table.Column<int>(type: "int", nullable: false),
                    DataHoraInicio = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    DataHoraFim = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    DiaInteiro = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    Motivo = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BloqueiosAgenda", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BloqueiosAgenda_Estabelecimentos_EstabelecimentoId",
                        column: x => x.EstabelecimentoId,
                        principalTable: "Estabelecimentos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_BloqueiosAgenda_EstabelecimentoId",
                table: "BloqueiosAgenda",
                column: "EstabelecimentoId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BloqueiosAgenda");
        }
    }
}
