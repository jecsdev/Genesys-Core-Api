using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Genesis_Core_Api.Migrations
{
    /// <inheritdoc />
    public partial class AddServicePlanAndPayments : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "PlanStartDate",
                table: "Affiliates",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ServicePlanId",
                table: "Affiliates",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "AffiliatePayments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    PaymentDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DueDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PaymentMethod = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ReferenceNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AffiliateId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AffiliatePayments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AffiliatePayments_Affiliates_AffiliateId",
                        column: x => x.AffiliateId,
                        principalTable: "Affiliates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ServicePlans",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BasePrice = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    IncludedDependents = table.Column<int>(type: "int", nullable: false),
                    ExtraDependentPrice = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServicePlans", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PlanBenefits",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ServicePlanId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlanBenefits", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PlanBenefits_ServicePlans_ServicePlanId",
                        column: x => x.ServicePlanId,
                        principalTable: "ServicePlans",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Affiliates_ServicePlanId",
                table: "Affiliates",
                column: "ServicePlanId");

            migrationBuilder.CreateIndex(
                name: "IX_AffiliatePayments_AffiliateId",
                table: "AffiliatePayments",
                column: "AffiliateId");

            migrationBuilder.CreateIndex(
                name: "IX_PlanBenefits_ServicePlanId",
                table: "PlanBenefits",
                column: "ServicePlanId");

            migrationBuilder.AddForeignKey(
                name: "FK_Affiliates_ServicePlans_ServicePlanId",
                table: "Affiliates",
                column: "ServicePlanId",
                principalTable: "ServicePlans",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Affiliates_ServicePlans_ServicePlanId",
                table: "Affiliates");

            migrationBuilder.DropTable(
                name: "AffiliatePayments");

            migrationBuilder.DropTable(
                name: "PlanBenefits");

            migrationBuilder.DropTable(
                name: "ServicePlans");

            migrationBuilder.DropIndex(
                name: "IX_Affiliates_ServicePlanId",
                table: "Affiliates");

            migrationBuilder.DropColumn(
                name: "PlanStartDate",
                table: "Affiliates");

            migrationBuilder.DropColumn(
                name: "ServicePlanId",
                table: "Affiliates");
        }
    }
}
