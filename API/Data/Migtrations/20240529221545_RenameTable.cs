using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Data.Migtrations
{
    /// <inheritdoc />
    public partial class RenameTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Appointmets",
                table: "Appointmets");

            migrationBuilder.RenameTable(
                name: "Appointmets",
                newName: "Appointments");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Appointments",
                table: "Appointments",
                column: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Appointments",
                table: "Appointments");

            migrationBuilder.RenameTable(
                name: "Appointments",
                newName: "Appointmets");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Appointmets",
                table: "Appointmets",
                column: "Id");
        }
    }
}
