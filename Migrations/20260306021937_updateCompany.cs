using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Genesis_Core_Api.Migrations
{
    /// <inheritdoc />
    public partial class updateCompany : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Ruc",
                table: "Companies",
                newName: "Rnc");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Rnc",
                table: "Companies",
                newName: "Ruc");
        }
    }
}
