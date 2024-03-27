using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HPCFodmapProject.Server.Data.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "userFlagged",
                table: "WhiteList",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "userFlagged",
                table: "WhiteList");
        }
    }
}
