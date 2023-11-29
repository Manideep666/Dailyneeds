using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DailyNeeds1.Migrations
{
    /// <inheritdoc />
    public partial class NewDbNewOne : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_logins_users_UserId",
                table: "logins");

            migrationBuilder.DropColumn(
                name: "UserID",
                table: "logins");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "logins",
                newName: "UserID");

            migrationBuilder.RenameIndex(
                name: "IX_logins_UserId",
                table: "logins",
                newName: "IX_logins_UserID");

            migrationBuilder.AddForeignKey(
                name: "FK_logins_users_UserID",
                table: "logins",
                column: "UserID",
                principalTable: "users",
                principalColumn: "UserID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_logins_users_UserID",
                table: "logins");

            migrationBuilder.RenameColumn(
                name: "UserID",
                table: "logins",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_logins_UserID",
                table: "logins",
                newName: "IX_logins_UserId");

            migrationBuilder.AddColumn<int>(
                name: "UserID",
                table: "logins",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_logins_users_UserId",
                table: "logins",
                column: "UserId",
                principalTable: "users",
                principalColumn: "UserID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
