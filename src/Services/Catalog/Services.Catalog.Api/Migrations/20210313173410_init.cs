using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Services.Catalog.Api.Migrations
{
    public partial class init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "categories",
                columns: table => new
                {
                    category_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: true),
                    is_mini_truck = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    category_order = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_categories", x => x.category_id);
                });

            migrationBuilder.CreateTable(
                name: "trucks",
                columns: table => new
                {
                    truck_id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    name = table.Column<string>(type: "text", nullable: false),
                    year = table.Column<int>(type: "integer", nullable: false),
                    description = table.Column<string>(type: "text", nullable: false),
                    price = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    previous_price = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: true),
                    quantity = table.Column<int>(type: "integer", nullable: false, defaultValue: 1),
                    hidden = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    damaged = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    default_photo_path = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_trucks", x => x.truck_id);
                });

            migrationBuilder.CreateTable(
                name: "category_truck",
                columns: table => new
                {
                    categories_category_id = table.Column<int>(type: "integer", nullable: false),
                    trucks_truck_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_category_truck", x => new { x.categories_category_id, x.trucks_truck_id });
                    table.ForeignKey(
                        name: "fk_category_truck_categories_categories_category_id",
                        column: x => x.categories_category_id,
                        principalTable: "categories",
                        principalColumn: "category_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_category_truck_trucks_trucks_truck_id",
                        column: x => x.trucks_truck_id,
                        principalTable: "trucks",
                        principalColumn: "truck_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "photos",
                columns: table => new
                {
                    photo_id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    photo_path = table.Column<string>(type: "text", nullable: true),
                    truck_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_photos", x => x.photo_id);
                    table.ForeignKey(
                        name: "fk_photos_trucks_truck_id",
                        column: x => x.truck_id,
                        principalTable: "trucks",
                        principalColumn: "truck_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_category_truck_trucks_truck_id",
                table: "category_truck",
                column: "trucks_truck_id");

            migrationBuilder.CreateIndex(
                name: "ix_photos_truck_id",
                table: "photos",
                column: "truck_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "category_truck");

            migrationBuilder.DropTable(
                name: "photos");

            migrationBuilder.DropTable(
                name: "categories");

            migrationBuilder.DropTable(
                name: "trucks");
        }
    }
}
