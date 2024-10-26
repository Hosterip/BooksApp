using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace PostsApp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class EnsureRoles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { new Guid("509c183d-17ff-4ae4-ace5-b3ed06b09db4"), "admin" },
                    { new Guid("6c427f5f-ebc3-4995-8574-53a620a42609"), "member" },
                    { new Guid("b9ec54dc-01c3-4e43-83a7-b46dc0400102"), "author" },
                    { new Guid("dbffd125-1660-4476-a55d-f2223114120f"), "moderator" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("509c183d-17ff-4ae4-ace5-b3ed06b09db4"));

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("6c427f5f-ebc3-4995-8574-53a620a42609"));

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("b9ec54dc-01c3-4e43-83a7-b46dc0400102"));

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("dbffd125-1660-4476-a55d-f2223114120f"));
        }
    }
}
