using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace heroesAPI.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "heroescodefirst");

            migrationBuilder.CreateTable(
                name: "Personajes",
                schema: "heroescodefirst",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Nombre = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Nivel = table.Column<int>(type: "integer", nullable: false),
                    FechaCreacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Gremio = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Personajes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Arqueros",
                schema: "heroescodefirst",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false),
                    Precision = table.Column<double>(type: "double precision", nullable: false),
                    TieneMascota = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Arqueros", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Arqueros_Personajes_Id",
                        column: x => x.Id,
                        principalSchema: "heroescodefirst",
                        principalTable: "Personajes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Clerigos",
                schema: "heroescodefirst",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false),
                    Deidad = table.Column<string>(type: "text", nullable: false),
                    PuntosSanacion = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Clerigos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Clerigos_Personajes_Id",
                        column: x => x.Id,
                        principalSchema: "heroescodefirst",
                        principalTable: "Personajes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Guerreros",
                schema: "heroescodefirst",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false),
                    ArmaPrincipal = table.Column<string>(type: "text", nullable: false),
                    Furia = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Guerreros", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Guerreros_Personajes_Id",
                        column: x => x.Id,
                        principalSchema: "heroescodefirst",
                        principalTable: "Personajes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Magos",
                schema: "heroescodefirst",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false),
                    Mana = table.Column<int>(type: "integer", nullable: false),
                    ElementoPrincipal = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Magos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Magos_Personajes_Id",
                        column: x => x.Id,
                        principalSchema: "heroescodefirst",
                        principalTable: "Personajes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Arqueros",
                schema: "heroescodefirst");

            migrationBuilder.DropTable(
                name: "Clerigos",
                schema: "heroescodefirst");

            migrationBuilder.DropTable(
                name: "Guerreros",
                schema: "heroescodefirst");

            migrationBuilder.DropTable(
                name: "Magos",
                schema: "heroescodefirst");

            migrationBuilder.DropTable(
                name: "Personajes",
                schema: "heroescodefirst");
        }
    }
}
