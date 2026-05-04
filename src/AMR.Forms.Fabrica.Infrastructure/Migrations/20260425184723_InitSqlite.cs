using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AMR.Forms.Fabrica.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitSqlite : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "rds");

            migrationBuilder.CreateTable(
                name: "BUSINESS_UNIT",
                schema: "rds",
                columns: table => new
                {
                    CD_BUSINESS_UNIT = table.Column<string>(type: "TEXT", maxLength: 20, nullable: false),
                    NO_BUSINESS_UNIT = table.Column<string>(type: "TEXT", maxLength: 100, nullable: true),
                    CD_COMPANHIA = table.Column<string>(type: "TEXT", maxLength: 20, nullable: true),
                    CD_ADDRESS_NUMBER = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BUSINESS_UNIT", x => x.CD_BUSINESS_UNIT);
                });

            migrationBuilder.CreateTable(
                name: "DEPARTAMENTO",
                schema: "rds",
                columns: table => new
                {
                    CD_DEPARTAMENTO = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CD_FILIAL = table.Column<int>(type: "INTEGER", nullable: false),
                    NO_DEPARTAMENTO = table.Column<string>(type: "TEXT", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DEPARTAMENTO", x => x.CD_DEPARTAMENTO);
                });

            migrationBuilder.CreateTable(
                name: "FICHA",
                schema: "rds",
                columns: table => new
                {
                    CD_FICHA = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CD_FILIAL = table.Column<int>(type: "INTEGER", nullable: false),
                    CD_PLACA_VEICULO = table.Column<string>(type: "TEXT", maxLength: 10, nullable: true),
                    CD_BUSINESS_UNIT = table.Column<string>(type: "TEXT", maxLength: 20, nullable: true),
                    CD_TIPO_OPERACAO = table.Column<int>(type: "INTEGER", nullable: true),
                    CD_PASSO_ATUAL = table.Column<int>(type: "INTEGER", nullable: true),
                    CD_LOTID = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true),
                    DT_FICHA = table.Column<DateOnly>(type: "TEXT", nullable: true),
                    DT_SAIDA = table.Column<DateOnly>(type: "TEXT", nullable: true),
                    DT_INTERFACE_JDE = table.Column<DateOnly>(type: "TEXT", nullable: true),
                    NO_MOTORISTA = table.Column<string>(type: "TEXT", maxLength: 100, nullable: true),
                    CD_CONTRATO_MANIFESTO = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true),
                    CD_ADDRESS_NUMBER_TRA = table.Column<int>(type: "INTEGER", nullable: true),
                    CD_PRODUTO_DEPTO = table.Column<int>(type: "INTEGER", nullable: true),
                    CD_SOLICITACAO_TRANSP = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true),
                    CD_TIPO_DOCTO_JDE = table.Column<string>(type: "TEXT", maxLength: 10, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FICHA", x => x.CD_FICHA);
                });

            migrationBuilder.CreateTable(
                name: "FICHA_BALANCA",
                schema: "rds",
                columns: table => new
                {
                    CD_FICHA = table.Column<int>(type: "INTEGER", nullable: false),
                    CD_FILIAL = table.Column<int>(type: "INTEGER", nullable: false),
                    CD_PESAGEM = table.Column<int>(type: "INTEGER", nullable: true),
                    IC_ORIGEM_DESTINO = table.Column<string>(type: "TEXT", maxLength: 1, nullable: true),
                    VL_PESO_1_PESAGEM = table.Column<decimal>(type: "TEXT", precision: 18, scale: 4, nullable: true),
                    VL_PESO_2_PESAGEM = table.Column<decimal>(type: "TEXT", precision: 18, scale: 4, nullable: true)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "FICHA_LOAD_DETALHE",
                schema: "rds",
                columns: table => new
                {
                    CD_FICHA = table.Column<int>(type: "INTEGER", nullable: false),
                    CD_FILIAL = table.Column<int>(type: "INTEGER", nullable: false),
                    CD_BUSINESS_UNIT = table.Column<string>(type: "TEXT", maxLength: 20, nullable: true),
                    CD_PRODUTO = table.Column<int>(type: "INTEGER", nullable: true),
                    QT_PRODUTO = table.Column<decimal>(type: "TEXT", precision: 18, scale: 4, nullable: true),
                    CD_UM = table.Column<string>(type: "TEXT", maxLength: 10, nullable: true),
                    CD_ADDRESS_NUMBER = table.Column<int>(type: "INTEGER", nullable: true),
                    CD_TIPO_DOCTO_JDE = table.Column<string>(type: "TEXT", maxLength: 10, nullable: true)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "FILIAL",
                schema: "rds",
                columns: table => new
                {
                    CD_FILIAL = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    NO_FILIAL = table.Column<string>(type: "TEXT", maxLength: 100, nullable: true),
                    CD_BU_DEPOSITO = table.Column<string>(type: "TEXT", maxLength: 20, nullable: true),
                    CD_TIPO_IMPRESSAO_NF = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FILIAL", x => x.CD_FILIAL);
                });

            migrationBuilder.CreateTable(
                name: "LOG_SISTEMA",
                schema: "rds",
                columns: table => new
                {
                    CD_FILIAL = table.Column<int>(type: "INTEGER", nullable: false),
                    TipoLog = table.Column<int>(type: "INTEGER", nullable: true),
                    IC_PENDENTE = table.Column<int>(type: "INTEGER", nullable: true),
                    DT_LOG = table.Column<DateTime>(type: "TEXT", nullable: true),
                    Usuario = table.Column<string>(type: "TEXT", nullable: true),
                    DS_MENSAGEM = table.Column<string>(type: "TEXT", maxLength: 500, nullable: true),
                    CodigoFicha = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "NOTA_FISCAL",
                schema: "rds",
                columns: table => new
                {
                    CD_NOTA_FISCAL = table.Column<int>(type: "INTEGER", nullable: false),
                    CD_SER_NOTA_FISCAL = table.Column<string>(type: "TEXT", maxLength: 10, nullable: false),
                    CD_FILIAL = table.Column<int>(type: "INTEGER", nullable: false),
                    CD_FICHA = table.Column<int>(type: "INTEGER", nullable: true),
                    CD_BUSINESS_UNIT = table.Column<string>(type: "TEXT", maxLength: 20, nullable: true),
                    DT_EMISSAO_NF = table.Column<DateOnly>(type: "TEXT", nullable: true),
                    CD_CHAVE_NFE = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true),
                    CD_PROTOCOLO = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true),
                    NO_CLIENTE = table.Column<string>(type: "TEXT", maxLength: 200, nullable: true),
                    CD_CNPJ_CLIENTE = table.Column<string>(type: "TEXT", maxLength: 20, nullable: true),
                    VL_TRANSMISSAO = table.Column<decimal>(type: "TEXT", precision: 18, scale: 4, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NOTA_FISCAL", x => new { x.CD_NOTA_FISCAL, x.CD_SER_NOTA_FISCAL });
                });

            migrationBuilder.CreateTable(
                name: "NOTA_FISCAL_DETALHE",
                schema: "rds",
                columns: table => new
                {
                    CD_NOTA_FISCAL = table.Column<int>(type: "INTEGER", nullable: false),
                    CD_SER_NOTA_FISCAL = table.Column<string>(type: "TEXT", maxLength: 10, nullable: false),
                    CD_FILIAL = table.Column<int>(type: "INTEGER", nullable: false),
                    CD_PRODUTO = table.Column<int>(type: "INTEGER", nullable: true),
                    QT_PRODUTO = table.Column<decimal>(type: "TEXT", precision: 18, scale: 4, nullable: true),
                    UnidadeMedidaComercial = table.Column<string>(type: "TEXT", nullable: true),
                    UnidadeMedida = table.Column<string>(type: "TEXT", nullable: true),
                    VL_PRECO_UNITARIO = table.Column<decimal>(type: "TEXT", precision: 18, scale: 4, nullable: true),
                    VL_TOTAL = table.Column<decimal>(type: "TEXT", precision: 18, scale: 4, nullable: true),
                    VL_ALIQUOTA_ICMS = table.Column<decimal>(type: "TEXT", precision: 18, scale: 4, nullable: true),
                    VL_BASE_REDUCAO_ICMS = table.Column<decimal>(type: "TEXT", precision: 18, scale: 4, nullable: true),
                    VL_ALIQUOTA_IPI = table.Column<decimal>(type: "TEXT", precision: 18, scale: 4, nullable: true),
                    VL_IPI = table.Column<decimal>(type: "TEXT", precision: 18, scale: 4, nullable: true),
                    VL_ICMS_ST = table.Column<decimal>(type: "TEXT", precision: 18, scale: 4, nullable: true),
                    VL_PIS = table.Column<decimal>(type: "TEXT", precision: 18, scale: 4, nullable: true),
                    VL_COFINS = table.Column<decimal>(type: "TEXT", precision: 18, scale: 4, nullable: true),
                    CodigoCfo = table.Column<int>(type: "INTEGER", nullable: true),
                    SufixoCfo = table.Column<string>(type: "TEXT", nullable: true),
                    CodigoEan = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "PASSO",
                schema: "rds",
                columns: table => new
                {
                    CD_PASSO = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    NO_PASSO = table.Column<string>(type: "TEXT", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PASSO", x => x.CD_PASSO);
                });

            migrationBuilder.CreateTable(
                name: "PEDIDO",
                schema: "rds",
                columns: table => new
                {
                    CD_PEDIDO = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CD_FILIAL = table.Column<int>(type: "INTEGER", nullable: false),
                    CD_BUSINESS_UNIT = table.Column<string>(type: "TEXT", maxLength: 20, nullable: true),
                    CD_ADDRESS_NUMBER = table.Column<int>(type: "INTEGER", nullable: true),
                    DT_PEDIDO = table.Column<DateOnly>(type: "TEXT", nullable: true),
                    CD_TIPO_DOCTO_JDE = table.Column<string>(type: "TEXT", maxLength: 10, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PEDIDO", x => x.CD_PEDIDO);
                });

            migrationBuilder.CreateTable(
                name: "PEDIDO_ITEM",
                schema: "rds",
                columns: table => new
                {
                    CD_PEDIDO = table.Column<int>(type: "INTEGER", nullable: false),
                    CD_FILIAL = table.Column<int>(type: "INTEGER", nullable: false),
                    CD_PRODUTO = table.Column<int>(type: "INTEGER", nullable: true),
                    QT_PRODUTO = table.Column<decimal>(type: "TEXT", precision: 18, scale: 4, nullable: true),
                    CD_UM = table.Column<string>(type: "TEXT", maxLength: 10, nullable: true),
                    CodigoBusinessUnit = table.Column<string>(type: "TEXT", nullable: true),
                    CodigoAddressNumber = table.Column<int>(type: "INTEGER", nullable: true),
                    CodigoTipoDoctoJde = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "PRODUTO",
                schema: "rds",
                columns: table => new
                {
                    CD_PRODUTO = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CD_BUSINESS_UNIT = table.Column<string>(type: "TEXT", maxLength: 20, nullable: true),
                    CD_PRODUTO_LONGO = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true),
                    NO_PRODUTO = table.Column<string>(type: "TEXT", maxLength: 200, nullable: true),
                    CD_EAN = table.Column<string>(type: "TEXT", maxLength: 30, nullable: true),
                    CD_UMC = table.Column<string>(type: "TEXT", maxLength: 10, nullable: true),
                    CD_UM = table.Column<string>(type: "TEXT", maxLength: 10, nullable: true),
                    CodigoCtf = table.Column<string>(type: "TEXT", nullable: true),
                    CodigoClf = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PRODUTO", x => x.CD_PRODUTO);
                });

            migrationBuilder.CreateTable(
                name: "TIPO_OPERACAO",
                schema: "rds",
                columns: table => new
                {
                    CD_TIPO_OPERACAO = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CD_FILIAL = table.Column<int>(type: "INTEGER", nullable: false),
                    NO_TIPO_OPERACAO = table.Column<string>(type: "TEXT", maxLength: 100, nullable: true),
                    CD_EXPEDICAO_RECEPCAO = table.Column<string>(type: "TEXT", maxLength: 1, nullable: true),
                    CD_INTERFACES = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TIPO_OPERACAO", x => x.CD_TIPO_OPERACAO);
                });

            migrationBuilder.CreateTable(
                name: "TIPO_OPERACAO_PASSO",
                schema: "rds",
                columns: table => new
                {
                    CD_TIPO_OPERACAO = table.Column<int>(type: "INTEGER", nullable: false),
                    CD_PASSO = table.Column<int>(type: "INTEGER", nullable: false),
                    CodigoFilial = table.Column<int>(type: "INTEGER", nullable: false),
                    NR_ORDEM = table.Column<int>(type: "INTEGER", nullable: true),
                    TipoPasso = table.Column<int>(type: "INTEGER", nullable: true),
                    CodigoPassoFlutuante = table.Column<int>(type: "INTEGER", nullable: true),
                    CodigoPassoRetorno = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TIPO_OPERACAO_PASSO", x => new { x.CD_TIPO_OPERACAO, x.CD_PASSO });
                });

            migrationBuilder.CreateTable(
                name: "TIPO_OPERACAO_PASSO_CFG",
                schema: "rds",
                columns: table => new
                {
                    CodigoFilial = table.Column<int>(type: "INTEGER", nullable: false),
                    CD_TIPO_OPERACAO = table.Column<int>(type: "INTEGER", nullable: false),
                    NomeTabela = table.Column<string>(type: "TEXT", nullable: true),
                    NomeCampo = table.Column<string>(type: "TEXT", nullable: true),
                    Tipo = table.Column<string>(type: "TEXT", nullable: true),
                    Habilitado = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "VEICULO",
                schema: "rds",
                columns: table => new
                {
                    CD_PLACA_VEICULO = table.Column<string>(type: "TEXT", maxLength: 10, nullable: false),
                    CD_FILIAL = table.Column<int>(type: "INTEGER", nullable: false),
                    CD_UF_VEICULO = table.Column<string>(type: "TEXT", maxLength: 2, nullable: true),
                    CD_RNTC_VEICULO = table.Column<string>(type: "TEXT", maxLength: 30, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VEICULO", x => x.CD_PLACA_VEICULO);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BUSINESS_UNIT",
                schema: "rds");

            migrationBuilder.DropTable(
                name: "DEPARTAMENTO",
                schema: "rds");

            migrationBuilder.DropTable(
                name: "FICHA",
                schema: "rds");

            migrationBuilder.DropTable(
                name: "FICHA_BALANCA",
                schema: "rds");

            migrationBuilder.DropTable(
                name: "FICHA_LOAD_DETALHE",
                schema: "rds");

            migrationBuilder.DropTable(
                name: "FILIAL",
                schema: "rds");

            migrationBuilder.DropTable(
                name: "LOG_SISTEMA",
                schema: "rds");

            migrationBuilder.DropTable(
                name: "NOTA_FISCAL",
                schema: "rds");

            migrationBuilder.DropTable(
                name: "NOTA_FISCAL_DETALHE",
                schema: "rds");

            migrationBuilder.DropTable(
                name: "PASSO",
                schema: "rds");

            migrationBuilder.DropTable(
                name: "PEDIDO",
                schema: "rds");

            migrationBuilder.DropTable(
                name: "PEDIDO_ITEM",
                schema: "rds");

            migrationBuilder.DropTable(
                name: "PRODUTO",
                schema: "rds");

            migrationBuilder.DropTable(
                name: "TIPO_OPERACAO",
                schema: "rds");

            migrationBuilder.DropTable(
                name: "TIPO_OPERACAO_PASSO",
                schema: "rds");

            migrationBuilder.DropTable(
                name: "TIPO_OPERACAO_PASSO_CFG",
                schema: "rds");

            migrationBuilder.DropTable(
                name: "VEICULO",
                schema: "rds");
        }
    }
}
