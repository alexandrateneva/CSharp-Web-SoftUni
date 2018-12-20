using Microsoft.EntityFrameworkCore.Migrations;

namespace GrabNReadApp.Data.Migrations
{
    public partial class AddNewProperty : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsOrdered",
                table: "Rentals",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsOrdered",
                table: "Purchases",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsOrdered",
                table: "Rentals");

            migrationBuilder.DropColumn(
                name: "IsOrdered",
                table: "Purchases");
        }
    }
}
