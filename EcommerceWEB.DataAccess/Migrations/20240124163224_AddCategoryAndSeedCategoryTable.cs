using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace EcommerceWEB.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddCategoryAndSeedCategoryTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CatergoryName = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    DisplayOrder = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "CatergoryName", "DisplayOrder" },
                values: new object[,]
                {
                    { new Guid("6770dd02-f8ec-461a-b11a-7f75ee499069"), "History", 3 },
                    { new Guid("aa495720-ee02-4f1b-af24-a91e3ad6573f"), "Action", 1 },
                    { new Guid("f08a733e-62a9-4405-b592-4382a9d5e29f"), "SciFi", 2 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Categories");
        }
    }
}
