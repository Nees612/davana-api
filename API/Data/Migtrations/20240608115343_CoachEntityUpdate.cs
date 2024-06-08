using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Data.Migtrations
{
    /// <inheritdoc />
    public partial class CoachEntityUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AboutMe",
                table: "Coaches",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ClosestWorkAddress",
                table: "Coaches",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProfileImageURL",
                table: "Coaches",
                type: "TEXT",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AboutMe",
                table: "Coaches");

            migrationBuilder.DropColumn(
                name: "ClosestWorkAddress",
                table: "Coaches");

            migrationBuilder.DropColumn(
                name: "ProfileImageURL",
                table: "Coaches");
        }
    }
}
