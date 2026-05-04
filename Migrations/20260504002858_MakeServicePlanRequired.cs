using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Genesis_Core_Api.Migrations
{
    /// <inheritdoc />
    public partial class MakeServicePlanRequired : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Affiliates_ServicePlans_ServicePlanId",
                table: "Affiliates");

            migrationBuilder.AlterColumn<int>(
                name: "ServicePlanId",
                table: "Affiliates",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "PlanStartDate",
                table: "Affiliates",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Affiliates_ServicePlans_ServicePlanId",
                table: "Affiliates",
                column: "ServicePlanId",
                principalTable: "ServicePlans",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Affiliates_ServicePlans_ServicePlanId",
                table: "Affiliates");

            migrationBuilder.AlterColumn<int>(
                name: "ServicePlanId",
                table: "Affiliates",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<DateTime>(
                name: "PlanStartDate",
                table: "Affiliates",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddForeignKey(
                name: "FK_Affiliates_ServicePlans_ServicePlanId",
                table: "Affiliates",
                column: "ServicePlanId",
                principalTable: "ServicePlans",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }
    }
}
