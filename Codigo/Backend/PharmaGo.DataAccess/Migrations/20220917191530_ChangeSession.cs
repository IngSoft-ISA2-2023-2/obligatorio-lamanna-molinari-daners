using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PharmaGo.DataAccess.Migrations
{
    public partial class ChangeSession : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Session_SessionId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_SessionId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "SessionId",
                table: "Users");

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "Session",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Session_UserId",
                table: "Session",
                column: "UserId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Session_Users_UserId",
                table: "Session",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Session_Users_UserId",
                table: "Session");

            migrationBuilder.DropIndex(
                name: "IX_Session_UserId",
                table: "Session");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Session");

            migrationBuilder.AddColumn<int>(
                name: "SessionId",
                table: "Users",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Users_SessionId",
                table: "Users",
                column: "SessionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Session_SessionId",
                table: "Users",
                column: "SessionId",
                principalTable: "Session",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}