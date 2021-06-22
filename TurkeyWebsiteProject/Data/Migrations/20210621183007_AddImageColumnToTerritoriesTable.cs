using Microsoft.EntityFrameworkCore.Migrations;

namespace TurkeyWebsiteProject.Data.Migrations
{
    public partial class AddImageColumnToTerritoriesTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Image",
                table: "Territories",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Image",
                table: "Territories");
        }
    }
}
