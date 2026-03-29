using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Genesis_Core_Api.Migrations
{
    /// <inheritdoc />
    public partial class FixAffiliateCompanyRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Affiliates_Companies_CompanyId1",
                table: "Affiliates");

            migrationBuilder.DropIndex(
                name: "IX_Affiliates_CompanyId1",
                table: "Affiliates");

            migrationBuilder.DropColumn(
                name: "CompanyId1",
                table: "Affiliates");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CompanyId1",
                table: "Affiliates",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Affiliates_CompanyId1",
                table: "Affiliates",
                column: "CompanyId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Affiliates_Companies_CompanyId1",
                table: "Affiliates",
                column: "CompanyId1",
                principalTable: "Companies",
                principalColumn: "Id");
        }
    }
}
