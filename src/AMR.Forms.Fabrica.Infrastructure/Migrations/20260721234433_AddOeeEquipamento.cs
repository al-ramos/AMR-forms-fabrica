using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AMR.Forms.Fabrica.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddOeeEquipamento : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EQUIPAMENTO",
                schema: "rds",
                columns: table => new
                {
                    ID_EQUIPAMENTO = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CD_FILIAL = table.Column<int>(type: "INTEGER", nullable: false),
                    NO_EQUIPAMENTO = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    DS_EQUIPAMENTO = table.Column<string>(type: "TEXT", maxLength: 300, nullable: true),
                    CD_AREA = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true),
                    FL_ATIVO = table.Column<bool>(type: "INTEGER", nullable: false, defaultValue: true),
                    DT_CRIADO_EM = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EQUIPAMENTO", x => x.ID_EQUIPAMENTO);
                });

            migrationBuilder.CreateTable(
                name: "REGISTRO_OEE",
                schema: "rds",
                columns: table => new
                {
                    ID_REGISTRO_OEE = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ID_EQUIPAMENTO = table.Column<int>(type: "INTEGER", nullable: false),
                    CD_FILIAL = table.Column<int>(type: "INTEGER", nullable: false),
                    DT_HORA_INICIO = table.Column<DateTime>(type: "TEXT", nullable: false),
                    DT_HORA_FIM = table.Column<DateTime>(type: "TEXT", nullable: false),
                    NR_TEMPO_PLANEJADO_MIN = table.Column<int>(type: "INTEGER", nullable: false),
                    NR_TEMPO_REAL_MIN = table.Column<int>(type: "INTEGER", nullable: false),
                    QT_PRODUZIDA = table.Column<decimal>(type: "TEXT", precision: 18, scale: 4, nullable: false),
                    QT_APROVADA = table.Column<decimal>(type: "TEXT", precision: 18, scale: 4, nullable: false),
                    NR_CICLO_IDEAL_SEG = table.Column<decimal>(type: "TEXT", precision: 10, scale: 3, nullable: false),
                    CD_OPERADOR = table.Column<string>(type: "TEXT", maxLength: 20, nullable: true),
                    DS_OBSERVACAO = table.Column<string>(type: "TEXT", maxLength: 300, nullable: true),
                    DT_CRIADO_EM = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_REGISTRO_OEE", x => x.ID_REGISTRO_OEE);
                    table.ForeignKey(
                        name: "FK_REGISTRO_OEE_EQUIPAMENTO_ID_EQUIPAMENTO",
                        column: x => x.ID_EQUIPAMENTO,
                        principalSchema: "rds",
                        principalTable: "EQUIPAMENTO",
                        principalColumn: "ID_EQUIPAMENTO",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_REGISTRO_OEE_CD_FILIAL",
                schema: "rds",
                table: "REGISTRO_OEE",
                column: "CD_FILIAL");

            migrationBuilder.CreateIndex(
                name: "IX_REGISTRO_OEE_ID_EQUIPAMENTO_DT_HORA_INICIO",
                schema: "rds",
                table: "REGISTRO_OEE",
                columns: new[] { "ID_EQUIPAMENTO", "DT_HORA_INICIO" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "REGISTRO_OEE",
                schema: "rds");

            migrationBuilder.DropTable(
                name: "EQUIPAMENTO",
                schema: "rds");
        }
    }
}
