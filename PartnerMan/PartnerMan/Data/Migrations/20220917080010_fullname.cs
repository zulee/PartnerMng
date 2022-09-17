using Microsoft.EntityFrameworkCore.Migrations;

namespace PartnerMan.Data.Migrations
{
    public partial class fullname : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "Partners");

            migrationBuilder.AddColumn<string>(
                name: "Comment",
                table: "Partners",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                table: "Partners",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "LastName",
                table: "Partners",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "MiddleName",
                table: "Partners",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "Partners",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Comment",
                table: "Partners");

            migrationBuilder.DropColumn(
                name: "FirstName",
                table: "Partners");

            migrationBuilder.DropColumn(
                name: "LastName",
                table: "Partners");

            migrationBuilder.DropColumn(
                name: "MiddleName",
                table: "Partners");

            migrationBuilder.DropColumn(
                name: "Title",
                table: "Partners");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Partners",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
