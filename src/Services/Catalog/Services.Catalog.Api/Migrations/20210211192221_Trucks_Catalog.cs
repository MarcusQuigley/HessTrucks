using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Services.Catalog.Api.Migrations
{
    public partial class Trucks_Catalog : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    CategoryId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsMiniTruck = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    Order = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.CategoryId);
                });

            migrationBuilder.CreateTable(
                name: "Trucks",
                columns: table => new
                {
                    TruckId = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NewID()"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Year = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PreviousPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    Hidden = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    Damaged = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    DefaultPhotoPath = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Trucks", x => x.TruckId);
                });

            migrationBuilder.CreateTable(
                name: "CategoryTruck",
                columns: table => new
                {
                    CategoriesCategoryId = table.Column<int>(type: "int", nullable: false),
                    TrucksTruckId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CategoryTruck", x => new { x.CategoriesCategoryId, x.TrucksTruckId });
                    table.ForeignKey(
                        name: "FK_CategoryTruck_Categories_CategoriesCategoryId",
                        column: x => x.CategoriesCategoryId,
                        principalTable: "Categories",
                        principalColumn: "CategoryId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CategoryTruck_Trucks_TrucksTruckId",
                        column: x => x.TrucksTruckId,
                        principalTable: "Trucks",
                        principalColumn: "TruckId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Photos",
                columns: table => new
                {
                    PhotoId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PhotoPath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TruckId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Photos", x => x.PhotoId);
                    table.ForeignKey(
                        name: "FK_Photos_Trucks_TruckId",
                        column: x => x.TruckId,
                        principalTable: "Trucks",
                        principalColumn: "TruckId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CategoryTruck_TrucksTruckId",
                table: "CategoryTruck",
                column: "TrucksTruckId");

            migrationBuilder.CreateIndex(
                name: "IX_Photos_TruckId",
                table: "Photos",
                column: "TruckId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CategoryTruck");

            migrationBuilder.DropTable(
                name: "Photos");

            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropTable(
                name: "Trucks");
        }
    }
}
