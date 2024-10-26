using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PostsApp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Changedbookshelfdeletebehaviorforuser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bookshelves_Users_UserId",
                table: "Bookshelves");

            migrationBuilder.AddForeignKey(
                name: "FK_Bookshelves_Users_UserId",
                table: "Bookshelves",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bookshelves_Users_UserId",
                table: "Bookshelves");

            migrationBuilder.AddForeignKey(
                name: "FK_Bookshelves_Users_UserId",
                table: "Bookshelves",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id");
        }
    }
}
