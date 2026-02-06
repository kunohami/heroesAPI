using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace heroesAPI.Migrations
{
    /// <inheritdoc />
    public partial class addJSON : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Rasgos",
                schema: "heroescodefirst",
                table: "Personajes",
                type: "jsonb",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Rasgos",
                schema: "heroescodefirst",
                table: "Personajes");
        }
    }
}
