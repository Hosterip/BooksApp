using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BooksApp.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class Following : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Relationships_Users_UserId",
                table: "Relationships");

            migrationBuilder.AlterColumn<Guid>(
                name: "UserId",
                table: "Relationships",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<Guid>(
                name: "FollowerId",
                table: "Relationships",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.CreateIndex(
                name: "IX_Relationships_FollowerId",
                table: "Relationships",
                column: "FollowerId");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Relationships_Users_FollowerId",
                table: "Relationships");

            migrationBuilder.DropForeignKey(
                name: "FK_Relationships_Users_UserId",
                table: "Relationships");

            migrationBuilder.DropIndex(
                name: "IX_Relationships_FollowerId",
                table: "Relationships");

            migrationBuilder.AlterColumn<Guid>(
                name: "UserId",
                table: "Relationships",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "FollowerId",
                table: "Relationships",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Relationships_Users_UserId",
                table: "Relationships",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
