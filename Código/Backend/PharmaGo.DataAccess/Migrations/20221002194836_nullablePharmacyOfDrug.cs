using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PharmaGo.DataAccess.Migrations
{
    public partial class nullablePharmacyOfDrug : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Drugs_Pharmacys_pharmacyId",
                table: "Drugs");

            migrationBuilder.RenameColumn(
                name: "pharmacyId",
                table: "Drugs",
                newName: "PharmacyId");

            migrationBuilder.RenameIndex(
                name: "IX_Drugs_pharmacyId",
                table: "Drugs",
                newName: "IX_Drugs_PharmacyId");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "UnitMeasures",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Presentations",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Drugs_Pharmacys_PharmacyId",
                table: "Drugs",
                column: "PharmacyId",
                principalTable: "Pharmacys",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Drugs_Pharmacys_PharmacyId",
                table: "Drugs");

            migrationBuilder.RenameColumn(
                name: "PharmacyId",
                table: "Drugs",
                newName: "pharmacyId");

            migrationBuilder.RenameIndex(
                name: "IX_Drugs_PharmacyId",
                table: "Drugs",
                newName: "IX_Drugs_pharmacyId");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "UnitMeasures",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Presentations",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddForeignKey(
                name: "FK_Drugs_Pharmacys_pharmacyId",
                table: "Drugs",
                column: "pharmacyId",
                principalTable: "Pharmacys",
                principalColumn: "Id");
        }
    }
}
