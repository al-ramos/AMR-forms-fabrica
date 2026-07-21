using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AMR.Forms.Fabrica.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddManutencaoPreventiva : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PLANO_MANUTENCAO",
                schema: "rds",
                columns: table => new
                {
                    ID_PLANO_MANUTENCAO = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ID_EQUIPAMENTO = table.Column<int>(type: "INTEGER", nullable: false),
                    CD_FILIAL = table.Column<int>(type: "INTEGER", nullable: false),
                    CD_TIPO_MANUTENCAO = table.Column<string>(type: "TEXT", maxLength: 20, nullable: false),
                    DS_DESCRICAO = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    DS_INSTRUCOES = table.Column<string>(type: "TEXT", maxLength: 2000, nullable: true),
                    NR_FREQUENCIA_DIAS = table.Column<int>(type: "INTEGER", nullable: false),
                    NR_DURACAO_EST_HORAS = table.Column<decimal>(type: "TEXT", precision: 8, scale: 2, nullable: false),
                    DT_PROXIMA_EXECUCAO = table.Column<DateTime>(type: "TEXT", nullable: false),
                    DT_ULTIMA_EXECUCAO = table.Column<DateTime>(type: "TEXT", nullable: true),
                    FL_ATIVO = table.Column<bool>(type: "INTEGER", nullable: false, defaultValue: true),
                    DT_CRIADO_EM = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PLANO_MANUTENCAO", x => x.ID_PLANO_MANUTENCAO);
                    table.ForeignKey(
                        name: "FK_PLANO_MANUTENCAO_EQUIPAMENTO_ID_EQUIPAMENTO",
                        column: x => x.ID_EQUIPAMENTO,
                        principalSchema: "rds",
                        principalTable: "EQUIPAMENTO",
                        principalColumn: "ID_EQUIPAMENTO",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ORDEM_MANUTENCAO",
                schema: "rds",
                columns: table => new
                {
                    ID_ORDEM_MANUTENCAO = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ID_PLANO_MANUTENCAO = table.Column<int>(type: "INTEGER", nullable: true),
                    ID_EQUIPAMENTO = table.Column<int>(type: "INTEGER", nullable: false),
                    CD_FILIAL = table.Column<int>(type: "INTEGER", nullable: false),
                    CD_TIPO_MANUTENCAO = table.Column<string>(type: "TEXT", maxLength: 20, nullable: false),
                    DS_DESCRICAO = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    CD_STATUS = table.Column<string>(type: "TEXT", maxLength: 20, nullable: false),
                    DT_PREVISTA = table.Column<DateTime>(type: "TEXT", nullable: false),
                    DT_INICIO = table.Column<DateTime>(type: "TEXT", nullable: true),
                    DT_CONCLUSAO = table.Column<DateTime>(type: "TEXT", nullable: true),
                    NR_DURACAO_REAL_HORAS = table.Column<decimal>(type: "TEXT", precision: 8, scale: 2, nullable: true),
                    CD_TECNICO = table.Column<string>(type: "TEXT", maxLength: 20, nullable: true),
                    DS_OBSERVACAO = table.Column<string>(type: "TEXT", maxLength: 500, nullable: true),
                    DS_MOTIVO_CANCELAMENTO = table.Column<string>(type: "TEXT", maxLength: 500, nullable: true),
                    DT_CRIADO_EM = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ORDEM_MANUTENCAO", x => x.ID_ORDEM_MANUTENCAO);
                    table.ForeignKey(
                        name: "FK_ORDEM_MANUTENCAO_EQUIPAMENTO_ID_EQUIPAMENTO",
                        column: x => x.ID_EQUIPAMENTO,
                        principalSchema: "rds",
                        principalTable: "EQUIPAMENTO",
                        principalColumn: "ID_EQUIPAMENTO",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ORDEM_MANUTENCAO_PLANO_MANUTENCAO_ID_PLANO_MANUTENCAO",
                        column: x => x.ID_PLANO_MANUTENCAO,
                        principalSchema: "rds",
                        principalTable: "PLANO_MANUTENCAO",
                        principalColumn: "ID_PLANO_MANUTENCAO");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ORDEM_MANUTENCAO_CD_FILIAL_CD_STATUS",
                schema: "rds",
                table: "ORDEM_MANUTENCAO",
                columns: new[] { "CD_FILIAL", "CD_STATUS" });

            migrationBuilder.CreateIndex(
                name: "IX_ORDEM_MANUTENCAO_ID_EQUIPAMENTO",
                schema: "rds",
                table: "ORDEM_MANUTENCAO",
                column: "ID_EQUIPAMENTO");

            migrationBuilder.CreateIndex(
                name: "IX_ORDEM_MANUTENCAO_ID_PLANO_MANUTENCAO",
                schema: "rds",
                table: "ORDEM_MANUTENCAO",
                column: "ID_PLANO_MANUTENCAO");

            migrationBuilder.CreateIndex(
                name: "IX_PLANO_MANUTENCAO_CD_FILIAL_DT_PROXIMA_EXECUCAO",
                schema: "rds",
                table: "PLANO_MANUTENCAO",
                columns: new[] { "CD_FILIAL", "DT_PROXIMA_EXECUCAO" });

            migrationBuilder.CreateIndex(
                name: "IX_PLANO_MANUTENCAO_ID_EQUIPAMENTO",
                schema: "rds",
                table: "PLANO_MANUTENCAO",
                column: "ID_EQUIPAMENTO");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ORDEM_MANUTENCAO",
                schema: "rds");

            migrationBuilder.DropTable(
                name: "PLANO_MANUTENCAO",
                schema: "rds");
        }
    }
}
