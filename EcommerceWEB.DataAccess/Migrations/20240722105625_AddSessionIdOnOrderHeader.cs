using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace EcommerceWEB.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddSessionIdOnOrderHeader : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("18f93e3d-afcf-404e-a073-6c9ddfc68dba"));

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("b43d1cb5-9c92-4ee4-858b-caeff20e5860"));

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("c79d6bed-a7ea-4361-bfbf-16d33aad95bf"));

            migrationBuilder.AddColumn<string>(
                name: "SessionId",
                table: "OrderHeaders",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "Author", "CategoryId", "Description", "ISBN", "ImageUrl", "ListPrice", "Price", "Price100", "Price50", "Title" },
                values: new object[,]
                {
                    { new Guid("44b18a1a-ca4c-46ce-8589-010a4c5e74ab"), "Billy Spark", 1, "Praesent vitae sodales libero. Praesent molestie orci augue, vitae euismod velit sollicitudin ac. Praesent vestibulum facilisis nibh ut ultricies.\r\n\r\nNunc malesuada viverra ipsum sit amet tincidunt. ", "SWD9999001", "\\images\\product\\3075dc18-442e-40ab-94e9-6fa06e099c7d.jpg", 99.0, 90.0, 80.0, 85.0, "Fortune of Time" },
                    { new Guid("b8ef4ab8-64dd-412e-9b23-c64f036f30d8"), "Julian Button", 3, "Praesent vitae sodales libero. Praesent molestie orci augue, vitae euismod velit sollicitudin ac. Praesent vestibulum facilisis nibh ut ultricies.\r\n\r\nNunc malesuada viverra ipsum sit amet tincidunt. ", "RITO5555501", "\\images\\product\\be8f6b36-ad4a-4603-b3c4-e854eb4ec3d1.jpg", 55.0, 50.0, 35.0, 40.0, "Vanish in the Sunset" },
                    { new Guid("bb6b9ec1-c8bf-4464-be2a-d8e74b846910"), "Nancy Hoover", 2, "Praesent vitae sodales libero. Praesent molestie orci augue, vitae euismod velit sollicitudin ac. Praesent vestibulum facilisis nibh ut ultricies.\r\n\r\nNunc malesuada viverra ipsum sit amet tincidunt. ", "CAW777777701", "\\images\\product\\a512016e-a47b-4e9f-9828-671e4ff9c2bb.jpg", 40.0, 30.0, 20.0, 25.0, "Dark Skies" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("44b18a1a-ca4c-46ce-8589-010a4c5e74ab"));

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("b8ef4ab8-64dd-412e-9b23-c64f036f30d8"));

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("bb6b9ec1-c8bf-4464-be2a-d8e74b846910"));

            migrationBuilder.DropColumn(
                name: "SessionId",
                table: "OrderHeaders");

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "Author", "CategoryId", "Description", "ISBN", "ImageUrl", "ListPrice", "Price", "Price100", "Price50", "Title" },
                values: new object[,]
                {
                    { new Guid("18f93e3d-afcf-404e-a073-6c9ddfc68dba"), "Julian Button", 3, "Praesent vitae sodales libero. Praesent molestie orci augue, vitae euismod velit sollicitudin ac. Praesent vestibulum facilisis nibh ut ultricies.\r\n\r\nNunc malesuada viverra ipsum sit amet tincidunt. ", "RITO5555501", "\\images\\product\\be8f6b36-ad4a-4603-b3c4-e854eb4ec3d1.jpg", 55.0, 50.0, 35.0, 40.0, "Vanish in the Sunset" },
                    { new Guid("b43d1cb5-9c92-4ee4-858b-caeff20e5860"), "Billy Spark", 1, "Praesent vitae sodales libero. Praesent molestie orci augue, vitae euismod velit sollicitudin ac. Praesent vestibulum facilisis nibh ut ultricies.\r\n\r\nNunc malesuada viverra ipsum sit amet tincidunt. ", "SWD9999001", "\\images\\product\\3075dc18-442e-40ab-94e9-6fa06e099c7d.jpg", 99.0, 90.0, 80.0, 85.0, "Fortune of Time" },
                    { new Guid("c79d6bed-a7ea-4361-bfbf-16d33aad95bf"), "Nancy Hoover", 2, "Praesent vitae sodales libero. Praesent molestie orci augue, vitae euismod velit sollicitudin ac. Praesent vestibulum facilisis nibh ut ultricies.\r\n\r\nNunc malesuada viverra ipsum sit amet tincidunt. ", "CAW777777701", "\\images\\product\\a512016e-a47b-4e9f-9828-671e4ff9c2bb.jpg", 40.0, 30.0, 20.0, 25.0, "Dark Skies" }
                });
        }
    }
}
