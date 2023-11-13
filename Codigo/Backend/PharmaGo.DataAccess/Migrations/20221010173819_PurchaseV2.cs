using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PharmaGo.DataAccess.Migrations
{
    public partial class PurchaseV2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Purchases_Pharmacys_PharmacyId",
                table: "Purchases");

            migrationBuilder.DropIndex(
                name: "IX_Purchases_PharmacyId",
                table: "Purchases");

            migrationBuilder.DropColumn(
                name: "PharmacyId",
                table: "Purchases");

            migrationBuilder.AddColumn<string>(
                name: "TrackingCode",
                table: "Purchases",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PharmacyId",
                table: "PurchaseDetails",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "PurchaseDetails",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseDetails_PharmacyId",
                table: "PurchaseDetails",
                column: "PharmacyId");

            migrationBuilder.AddForeignKey(
                name: "FK_PurchaseDetails_Pharmacys_PharmacyId",
                table: "PurchaseDetails",
                column: "PharmacyId",
                principalTable: "Pharmacys",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PurchaseDetails_Pharmacys_PharmacyId",
                table: "PurchaseDetails");

            migrationBuilder.DropIndex(
                name: "IX_PurchaseDetails_PharmacyId",
                table: "PurchaseDetails");

            migrationBuilder.DropColumn(
                name: "TrackingCode",
                table: "Purchases");

            migrationBuilder.DropColumn(
                name: "PharmacyId",
                table: "PurchaseDetails");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "PurchaseDetails");

            migrationBuilder.AddColumn<int>(
                name: "PharmacyId",
                table: "Purchases",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Purchases_PharmacyId",
                table: "Purchases",
                column: "PharmacyId");

            migrationBuilder.AddForeignKey(
                name: "FK_Purchases_Pharmacys_PharmacyId",
                table: "Purchases",
                column: "PharmacyId",
                principalTable: "Pharmacys",
                principalColumn: "Id");
        }
    }
}
