using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Services.Catalog.Api.Migrations
{
    public partial class defaultnewidforphotophotoid : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<Guid>(
                name: "PhotoId",
                table: "Photos",
                type: "uniqueidentifier",
                nullable: false,
                defaultValueSql: "NewID()",
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<Guid>(
                name: "PhotoId",
                table: "Photos",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldDefaultValueSql: "NewID()");
        }
    }
}
