using Microsoft.EntityFrameworkCore.Migrations;

namespace pruebaDisneyApi.Migrations
{
    public partial class attributeurlImg : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UrlImg",
                table: "Personajes",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UrlImg",
                table: "Peliculas",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UrlImg",
                table: "Personajes");

            migrationBuilder.DropColumn(
                name: "UrlImg",
                table: "Peliculas");
        }
    }
}
