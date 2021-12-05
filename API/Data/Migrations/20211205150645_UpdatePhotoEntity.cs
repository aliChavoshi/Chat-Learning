using Microsoft.EntityFrameworkCore.Migrations;

namespace API.Data.Migrations
{
    public partial class UpdatePhotoEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Photo_Users_UsersId",
                table: "Photo");

            migrationBuilder.DropIndex(
                name: "IX_Photo_UsersId",
                table: "Photo");

            migrationBuilder.DropColumn(
                name: "UsersId",
                table: "Photo");

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "Photo",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Photo_UserId",
                table: "Photo",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Photo_Users_UserId",
                table: "Photo",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Photo_Users_UserId",
                table: "Photo");

            migrationBuilder.DropIndex(
                name: "IX_Photo_UserId",
                table: "Photo");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Photo");

            migrationBuilder.AddColumn<int>(
                name: "UsersId",
                table: "Photo",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Photo_UsersId",
                table: "Photo",
                column: "UsersId");

            migrationBuilder.AddForeignKey(
                name: "FK_Photo_Users_UsersId",
                table: "Photo",
                column: "UsersId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }
    }
}
