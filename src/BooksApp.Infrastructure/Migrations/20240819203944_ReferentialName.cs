using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PostsApp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ReferentialName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BooksGenres_Books_BooksId",
                table: "BooksGenres");

            migrationBuilder.DropForeignKey(
                name: "FK_BookshelfBooks_Bookshelfes_BookshelfId",
                table: "BookshelfBooks");

            migrationBuilder.DropForeignKey(
                name: "FK_Bookshelfes_Users_UserId",
                table: "Bookshelfes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Bookshelfes",
                table: "Bookshelfes");

            migrationBuilder.RenameTable(
                name: "Bookshelfes",
                newName: "Bookshelves");

            migrationBuilder.RenameIndex(
                name: "IX_Bookshelfes_UserId",
                table: "Bookshelves",
                newName: "IX_Bookshelves_UserId");

            migrationBuilder.AddColumn<Guid>(
                name: "BookId",
                table: "BooksGenres",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ReferentialName",
                table: "Books",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ReferentialName",
                table: "Bookshelves",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Bookshelves",
                table: "Bookshelves",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_BooksGenres_BookId",
                table: "BooksGenres",
                column: "BookId");

            migrationBuilder.AddForeignKey(
                name: "FK_BooksGenres_Books_BookId",
                table: "BooksGenres",
                column: "BookId",
                principalTable: "Books",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_BooksGenres_Books_BooksId",
                table: "BooksGenres",
                column: "BooksId",
                principalTable: "Books",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BookshelfBooks_Bookshelves_BookshelfId",
                table: "BookshelfBooks",
                column: "BookshelfId",
                principalTable: "Bookshelves",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Bookshelves_Users_UserId",
                table: "Bookshelves",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BooksGenres_Books_BookId",
                table: "BooksGenres");

            migrationBuilder.DropForeignKey(
                name: "FK_BooksGenres_Books_BooksId",
                table: "BooksGenres");

            migrationBuilder.DropForeignKey(
                name: "FK_BookshelfBooks_Bookshelves_BookshelfId",
                table: "BookshelfBooks");

            migrationBuilder.DropForeignKey(
                name: "FK_Bookshelves_Users_UserId",
                table: "Bookshelves");

            migrationBuilder.DropIndex(
                name: "IX_BooksGenres_BookId",
                table: "BooksGenres");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Bookshelves",
                table: "Bookshelves");

            migrationBuilder.DropColumn(
                name: "BookId",
                table: "BooksGenres");

            migrationBuilder.DropColumn(
                name: "ReferentialName",
                table: "Books");

            migrationBuilder.DropColumn(
                name: "ReferentialName",
                table: "Bookshelves");

            migrationBuilder.RenameTable(
                name: "Bookshelves",
                newName: "Bookshelfes");

            migrationBuilder.RenameIndex(
                name: "IX_Bookshelves_UserId",
                table: "Bookshelfes",
                newName: "IX_Bookshelfes_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Bookshelfes",
                table: "Bookshelfes",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_BooksGenres_Books_BooksId",
                table: "BooksGenres",
                column: "BooksId",
                principalTable: "Books",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_BookshelfBooks_Bookshelfes_BookshelfId",
                table: "BookshelfBooks",
                column: "BookshelfId",
                principalTable: "Bookshelfes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Bookshelfes_Users_UserId",
                table: "Bookshelfes",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id");
        }
    }
}
