using Microsoft.EntityFrameworkCore.Migrations;

namespace Ogame.Migrations
{
    public partial class ChangeRessourceName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Collect_rate",
                table: "SolarPanels",
                newName: "CollectRate");

            migrationBuilder.AddColumn<float>(
                name: "CollectRate",
                table: "Mines",
                nullable: false,
                defaultValue: 0f);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CollectRate",
                table: "Mines");

            migrationBuilder.RenameColumn(
                name: "CollectRate",
                table: "SolarPanels",
                newName: "Collect_rate");
        }
    }
}
