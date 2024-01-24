using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace EcommerceWeb.Migrations
{
    /// <inheritdoc />
    public partial class SeedCategoryTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "CatergoryName", "DisplayOrder" },
                values: new object[,]
                {
                    { new Guid("47da6937-10db-42e2-9e30-124e336ea6fb"), "SciFi", 2 },
                    { new Guid("4c951be7-fb69-49b2-b9b2-4d585ce967e4"), "Action", 1 },
                    { new Guid("5c9914ad-68c2-4c3f-8acd-b66ae429f7fe"), "History", 3 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("47da6937-10db-42e2-9e30-124e336ea6fb"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("4c951be7-fb69-49b2-b9b2-4d585ce967e4"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("5c9914ad-68c2-4c3f-8acd-b66ae429f7fe"));
        }
    }
}
