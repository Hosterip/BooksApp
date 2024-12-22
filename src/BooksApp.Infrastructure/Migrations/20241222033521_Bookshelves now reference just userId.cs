using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BooksApp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class BookshelvesnowreferencejustuserId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bookshelves_Users_UserId",
                table: "Bookshelves");

            migrationBuilder.DropIndex(
                name: "IX_Bookshelves_UserId",
                table: "Bookshelves");

            migrationBuilder.AlterColumn<Guid>(
                name: "UserId",
                table: "Bookshelves",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<Guid>(
                name: "UserId",
                table: "Bookshelves",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

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
                onDelete: ReferentialAction.SetNull);
        }
    }
}
