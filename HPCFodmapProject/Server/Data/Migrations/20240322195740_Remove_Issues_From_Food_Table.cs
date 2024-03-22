using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HPCFodmapProject.Server.Data.Migrations
{
    public partial class Remove_Issues_From_Food_Table : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "issues",
                table: "Food");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "issues",
                table: "Food",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
