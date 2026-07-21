using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AMR.Forms.Fabrica.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddBomMultinivel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CD_TIPO_PRODUTO",
                schema: "rds",
                table: "PRODUTO",
                type: "TEXT",
                maxLength: 20,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "NR_LEAD_TIME_DIAS",
                schema: "rds",
                table: "PRODUTO",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "VL_CUSTO_PADRAO",
                schema: "rds",
                table: "PRODUTO",
                type: "TEXT",
                precision: 18,
                scale: 4,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.CreateTable(
                name: "BOM_ITEM",
                schema: "rds",
                columns: table => new
                {
                    ID_BOM_ITEM = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CD_PRODUTO_PAI = table.Column<int>(type: "INTEGER", nullable: false),
                    CD_PRODUTO_FILHO = table.Column<int>(type: "INTEGER", nullable: false),
                    QT_QUANTIDADE = table.Column<decimal>(type: "TEXT", precision: 18, scale: 6, nullable: false),
                    NR_NIVEL = table.Column<int>(type: "INTEGER", nullable: false),
                    PC_PERDA = table.Column<decimal>(type: "TEXT", precision: 5, scale: 2, nullable: false, defaultValue: 0m),
                    FL_ATIVO = table.Column<bool>(type: "INTEGER", nullable: false, defaultValue: true),
                    DT_CRIADO_EM = table.Column<DateTime>(type: "TEXT", nullable: false),
                    DT_ATUALIZADO_EM = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BOM_ITEM", x => x.ID_BOM_ITEM);
                    table.ForeignKey(
                        name: "FK_BOM_ITEM_PRODUTO_CD_PRODUTO_FILHO",
                        column: x => x.CD_PRODUTO_FILHO,
                        principalSchema: "rds",
                        principalTable: "PRODUTO",
                        principalColumn: "CD_PRODUTO",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BOM_ITEM_PRODUTO_CD_PRODUTO_PAI",
                        column: x => x.CD_PRODUTO_PAI,
                        principalSchema: "rds",
                        principalTable: "PRODUTO",
                        principalColumn: "CD_PRODUTO",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BOM_ITEM_CD_PRODUTO_FILHO",
                schema: "rds",
                table: "BOM_ITEM",
                column: "CD_PRODUTO_FILHO");

            migrationBuilder.CreateIndex(
                name: "IX_BOM_ITEM_CD_PRODUTO_PAI_CD_PRODUTO_FILHO",
                schema: "rds",
                table: "BOM_ITEM",
                columns: new[] { "CD_PRODUTO_PAI", "CD_PRODUTO_FILHO" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BOM_ITEM",
                schema: "rds");

            migrationBuilder.DropColumn(
                name: "CD_TIPO_PRODUTO",
                schema: "rds",
                table: "PRODUTO");

            migrationBuilder.DropColumn(
                name: "NR_LEAD_TIME_DIAS",
                schema: "rds",
                table: "PRODUTO");

            migrationBuilder.DropColumn(
                name: "VL_CUSTO_PADRAO",
                schema: "rds",
                table: "PRODUTO");
        }
    }
}
