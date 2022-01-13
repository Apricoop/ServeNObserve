using Microsoft.EntityFrameworkCore.Migrations;

namespace ServeNObserve.Migrations
{
    public partial class SgkField : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Sgk",
                table: "Jobs",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Sgk",
                table: "Jobs");
        }
    }
}
