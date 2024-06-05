using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Data.Migtrations
{
    /// <inheritdoc />
    public partial class EntitiesUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PasswordHash",
                table: "Users",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Roles",
                table: "Users",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Scopes",
                table: "Users",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PasswordHash",
                table: "Coaches",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Roles",
                table: "Coaches",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Scopes",
                table: "Coaches",
                type: "TEXT",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PasswordHash",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Roles",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Scopes",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "PasswordHash",
                table: "Coaches");

            migrationBuilder.DropColumn(
                name: "Roles",
                table: "Coaches");

            migrationBuilder.DropColumn(
                name: "Scopes",
                table: "Coaches");
        }
    }
}
