using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PostsApp.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class Bookshelf : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Bookshelfes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bookshelfes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Bookshelfes_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BookshelfBooks",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BookId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BookshelfId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookshelfBooks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BookshelfBooks_Books_BookId",
                        column: x => x.BookId,
                        principalTable: "Books",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_BookshelfBooks_Bookshelfes_BookshelfId",
                        column: x => x.BookshelfId,
                        principalTable: "Bookshelfes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BookshelfBooks_BookId",
                table: "BookshelfBooks",
                column: "BookId");

            migrationBuilder.CreateIndex(
                name: "IX_BookshelfBooks_BookshelfId",
                table: "BookshelfBooks",
                column: "BookshelfId");

            migrationBuilder.CreateIndex(
                name: "IX_Bookshelfes_UserId",
                table: "Bookshelfes",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BookshelfBooks");

            migrationBuilder.DropTable(
                name: "Bookshelfes");
        }
    }
}
