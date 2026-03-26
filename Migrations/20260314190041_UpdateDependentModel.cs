using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Genesis_Core_Api.Migrations
{
    /// <inheritdoc />
    public partial class UpdateDependentModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Dependents",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Dependents",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Phone",
                table: "Dependents",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Relationship",
                table: "Dependents",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Dependents");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Dependents");

            migrationBuilder.DropColumn(
                name: "Phone",
                table: "Dependents");

            migrationBuilder.DropColumn(
                name: "Relationship",
                table: "Dependents");
        }
    }
}
