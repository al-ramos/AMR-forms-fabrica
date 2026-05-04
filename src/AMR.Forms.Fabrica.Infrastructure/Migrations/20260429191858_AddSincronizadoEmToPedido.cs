using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AMR.Forms.Fabrica.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddSincronizadoEmToPedido : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "SincronizadoEm",
                schema: "rds",
                table: "PEDIDO",
                type: "TEXT",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SincronizadoEm",
                schema: "rds",
                table: "PEDIDO");
        }
    }
}
