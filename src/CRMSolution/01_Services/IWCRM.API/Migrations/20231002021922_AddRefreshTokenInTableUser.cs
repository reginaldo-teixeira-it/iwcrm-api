using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IWCRM.API.Migrations
{
    /// <inheritdoc />
    public partial class AddRefreshTokenInTableUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AccessToken",
                table: "User",
                type: "TEXT",
                maxLength: 300,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "RefreshToken",
                table: "User",
                type: "TEXT",
                maxLength: 300,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AccessToken",
                table: "User");

            migrationBuilder.DropColumn(
                name: "RefreshToken",
                table: "User");
        }
    }
}
