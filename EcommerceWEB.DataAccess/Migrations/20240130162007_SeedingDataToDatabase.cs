using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace EcommerceWEB.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class SeedingDataToDatabase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CatergoryName",
                table: "Categories");

            migrationBuilder.AddColumn<string>(
                name: "CategoryName",
                table: "Categories",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "CategoryName", "DisplayOrder" },
                values: new object[,]
                {
                    { new Guid("56f9a9c9-5966-4dee-8c59-af196da962d7"), "SciFi", 2 },
                    { new Guid("5a13196b-bd85-4e02-83ea-67fa1896b75d"), "Action", 1 },
                    { new Guid("fc2f05fe-f484-4b63-b222-62a9a6560cb6"), "History", 3 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("56f9a9c9-5966-4dee-8c59-af196da962d7"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("5a13196b-bd85-4e02-83ea-67fa1896b75d"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("fc2f05fe-f484-4b63-b222-62a9a6560cb6"));

            migrationBuilder.DropColumn(
                name: "CategoryName",
                table: "Categories");

            migrationBuilder.AddColumn<string>(
                name: "CatergoryName",
                table: "Categories",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
