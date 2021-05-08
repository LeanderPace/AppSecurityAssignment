using Microsoft.EntityFrameworkCore.Migrations;

namespace ShoppingCart.Data.Migrations
{
    public partial class AddedPrivateAndPublicKeyAndLecturerEmail : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "LecturerEmail",
                table: "Members",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PrivateKey",
                table: "Members",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PublicKey",
                table: "Members",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LecturerEmail",
                table: "Members");

            migrationBuilder.DropColumn(
                name: "PrivateKey",
                table: "Members");

            migrationBuilder.DropColumn(
                name: "PublicKey",
                table: "Members");
        }
    }
}
