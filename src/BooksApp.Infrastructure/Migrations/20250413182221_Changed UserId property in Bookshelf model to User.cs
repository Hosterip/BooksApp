using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BooksApp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ChangedUserIdpropertyinBookshelfmodeltoUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Relationships_Users_FollowerId",
                table: "Relationships");

            migrationBuilder.DropForeignKey(
                name: "FK_Relationships_Users_UserId",
                table: "Relationships");

            migrationBuilder.CreateIndex(
                name: "IX_Bookshelves_UserId",
                table: "Bookshelves",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Bookshelves_Users_UserId",
                table: "Bookshelves",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Relationships_Users_FollowerId",
                table: "Relationships",
                column: "FollowerId",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Relationships_Users_UserId",
                table: "Relationships",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bookshelves_Users_UserId",
                table: "Bookshelves");

            migrationBuilder.DropForeignKey(
                name: "FK_Relationships_Users_FollowerId",
                table: "Relationships");

            migrationBuilder.DropForeignKey(
                name: "FK_Relationships_Users_UserId",
                table: "Relationships");

            migrationBuilder.DropIndex(
                name: "IX_Bookshelves_UserId",
                table: "Bookshelves");

            migrationBuilder.AddForeignKey(
                name: "FK_Relationships_Users_FollowerId",
                table: "Relationships",
                column: "FollowerId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Relationships_Users_UserId",
                table: "Relationships",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }
    }
}
