using Microsoft.EntityFrameworkCore.Migrations;

namespace SGEP.Migrations
{
    public partial class newone : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Showid",
                table: "Material",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Showid",
                table: "Material");
        }
    }
}
