using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace GrabNReadApp.Data.Migrations
{
    public partial class DateToArticle : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "PublishedOn",
                table: "Articles",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PublishedOn",
                table: "Articles");
        }
    }
}
