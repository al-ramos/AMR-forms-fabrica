using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AMR.Forms.Fabrica.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddOrdemProducaoRastreabilidade : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ORDEM_PRODUCAO",
                schema: "rds",
                columns: table => new
                {
                    ID_ORDEM_PRODUCAO = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    NR_ORDEM = table.Column<string>(type: "TEXT", maxLength: 30, nullable: false),
                    CD_PRODUTO = table.Column<int>(type: "INTEGER", nullable: false),
                    CD_FILIAL = table.Column<int>(type: "INTEGER", nullable: false),
                    QT_PLANEJADA = table.Column<decimal>(type: "TEXT", precision: 18, scale: 4, nullable: false),
                    QT_PRODUZIDA = table.Column<decimal>(type: "TEXT", precision: 18, scale: 4, nullable: false, defaultValue: 0m),
                    QT_REJEITADA = table.Column<decimal>(type: "TEXT", precision: 18, scale: 4, nullable: false, defaultValue: 0m),
                    CD_STATUS = table.Column<string>(type: "TEXT", maxLength: 20, nullable: false),
                    DT_ABERTURA = table.Column<DateTime>(type: "TEXT", nullable: false),
                    DT_PREVISTA_FIM = table.Column<DateTime>(type: "TEXT", nullable: true),
                    DT_FECHAMENTO = table.Column<DateTime>(type: "TEXT", nullable: true),
                    DS_OBSERVACAO = table.Column<string>(type: "TEXT", maxLength: 500, nullable: true),
                    DS_MOTIVO_CANCELAMENTO = table.Column<string>(type: "TEXT", maxLength: 500, nullable: true),
                    ProdutoCodigo = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ORDEM_PRODUCAO", x => x.ID_ORDEM_PRODUCAO);
                    table.ForeignKey(
                        name: "FK_ORDEM_PRODUCAO_PRODUTO_ProdutoCodigo",
                        column: x => x.ProdutoCodigo,
                        principalSchema: "rds",
                        principalTable: "PRODUTO",
                        principalColumn: "CD_PRODUTO");
                });

            migrationBuilder.CreateTable(
                name: "RASTREABILIDADE_ITEM",
                schema: "rds",
                columns: table => new
                {
                    ID_RASTREABILIDADE = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ID_ORDEM_PRODUCAO = table.Column<int>(type: "INTEGER", nullable: false),
                    CD_PRODUTO = table.Column<int>(type: "INTEGER", nullable: false),
                    CD_LOTE = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true),
                    QT_QUANTIDADE = table.Column<decimal>(type: "TEXT", precision: 18, scale: 4, nullable: false),
                    CD_TIPO_MOVIMENTO = table.Column<string>(type: "TEXT", maxLength: 20, nullable: false),
                    DT_REGISTRO = table.Column<DateTime>(type: "TEXT", nullable: false),
                    CD_OPERADOR = table.Column<string>(type: "TEXT", maxLength: 20, nullable: true),
                    DS_OBSERVACAO = table.Column<string>(type: "TEXT", maxLength: 300, nullable: true),
                    ProdutoCodigo = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RASTREABILIDADE_ITEM", x => x.ID_RASTREABILIDADE);
                    table.ForeignKey(
                        name: "FK_RASTREABILIDADE_ITEM_ORDEM_PRODUCAO_ID_ORDEM_PRODUCAO",
                        column: x => x.ID_ORDEM_PRODUCAO,
                        principalSchema: "rds",
                        principalTable: "ORDEM_PRODUCAO",
                        principalColumn: "ID_ORDEM_PRODUCAO",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RASTREABILIDADE_ITEM_PRODUTO_ProdutoCodigo",
                        column: x => x.ProdutoCodigo,
                        principalSchema: "rds",
                        principalTable: "PRODUTO",
                        principalColumn: "CD_PRODUTO");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ORDEM_PRODUCAO_NR_ORDEM",
                schema: "rds",
                table: "ORDEM_PRODUCAO",
                column: "NR_ORDEM",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ORDEM_PRODUCAO_ProdutoCodigo",
                schema: "rds",
                table: "ORDEM_PRODUCAO",
                column: "ProdutoCodigo");

            migrationBuilder.CreateIndex(
                name: "IX_RASTREABILIDADE_ITEM_CD_LOTE",
                schema: "rds",
                table: "RASTREABILIDADE_ITEM",
                column: "CD_LOTE");

            migrationBuilder.CreateIndex(
                name: "IX_RASTREABILIDADE_ITEM_ID_ORDEM_PRODUCAO",
                schema: "rds",
                table: "RASTREABILIDADE_ITEM",
                column: "ID_ORDEM_PRODUCAO");

            migrationBuilder.CreateIndex(
                name: "IX_RASTREABILIDADE_ITEM_ProdutoCodigo",
                schema: "rds",
                table: "RASTREABILIDADE_ITEM",
                column: "ProdutoCodigo");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RASTREABILIDADE_ITEM",
                schema: "rds");

            migrationBuilder.DropTable(
                name: "ORDEM_PRODUCAO",
                schema: "rds");
        }
    }
}
