using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EcoBalance.Database.Migrations
{
    /// <inheritdoc />
    public partial class first : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ecobalance_empresas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    Nome = table.Column<string>(type: "NVARCHAR2(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "NVARCHAR2(100)", maxLength: 100, nullable: false),
                    Telefone = table.Column<string>(type: "NVARCHAR2(30)", maxLength: 30, nullable: false),
                    Cnpj = table.Column<string>(type: "NVARCHAR2(30)", maxLength: 30, nullable: false),
                    SenhaHash = table.Column<string>(type: "NVARCHAR2(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ecobalance_empresas", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ecobalance_consumos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    Data = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false),
                    Consumo = table.Column<double>(type: "BINARY_DOUBLE", nullable: false),
                    EmpresaId = table.Column<int>(type: "NUMBER(10)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ecobalance_consumos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ecobalance_consumos_ecobalance_empresas_EmpresaId",
                        column: x => x.EmpresaId,
                        principalTable: "ecobalance_empresas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ecobalance_producoes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    Data = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false),
                    Producao = table.Column<double>(type: "BINARY_DOUBLE", nullable: false),
                    EmpresaId = table.Column<int>(type: "NUMBER(10)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ecobalance_producoes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ecobalance_producoes_ecobalance_empresas_EmpresaId",
                        column: x => x.EmpresaId,
                        principalTable: "ecobalance_empresas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ecobalance_consumos_EmpresaId",
                table: "ecobalance_consumos",
                column: "EmpresaId");

            migrationBuilder.CreateIndex(
                name: "IX_ecobalance_producoes_EmpresaId",
                table: "ecobalance_producoes",
                column: "EmpresaId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ecobalance_consumos");

            migrationBuilder.DropTable(
                name: "ecobalance_producoes");

            migrationBuilder.DropTable(
                name: "ecobalance_empresas");
        }
    }
}
