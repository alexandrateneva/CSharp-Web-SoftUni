using Microsoft.EntityFrameworkCore.Migrations;

namespace GrabNReadApp.Data.Migrations
{
    public partial class MakeNullableOrderIdInUserModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Orders_OrderId",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_OrderId",
                table: "AspNetUsers");

            migrationBuilder.AlterColumn<int>(
                name: "OrderId",
                table: "AspNetUsers",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_OrderId",
                table: "AspNetUsers",
                column: "OrderId",
                unique: true,
                filter: "[OrderId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Orders_OrderId",
                table: "AspNetUsers",
                column: "OrderId",
                principalTable: "Orders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Orders_OrderId",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_OrderId",
                table: "AspNetUsers");

            migrationBuilder.AlterColumn<int>(
                name: "OrderId",
                table: "AspNetUsers",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_OrderId",
                table: "AspNetUsers",
                column: "OrderId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Orders_OrderId",
                table: "AspNetUsers",
                column: "OrderId",
                principalTable: "Orders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
