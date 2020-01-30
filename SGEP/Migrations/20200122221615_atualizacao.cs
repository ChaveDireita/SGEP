using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SGEP.Migrations
{
    public partial class atualizacao : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Funcionario_Projeto_ProjetoId",
                table: "Funcionario");

            migrationBuilder.DropIndex(
                name: "IX_Funcionario_ProjetoId",
                table: "Funcionario");

            migrationBuilder.DropColumn(
                name: "ProjetoId",
                table: "Funcionario");

            migrationBuilder.CreateTable(
                name: "ProjetosxFuncionarios",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    IdFuncionario = table.Column<int>(nullable: false),
                    IdProjeto = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjetosxFuncionarios", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProjetosxFuncionarios");

            migrationBuilder.AddColumn<int>(
                name: "ProjetoId",
                table: "Funcionario",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Funcionario_ProjetoId",
                table: "Funcionario",
                column: "ProjetoId");

            migrationBuilder.AddForeignKey(
                name: "FK_Funcionario_Projeto_ProjetoId",
                table: "Funcionario",
                column: "ProjetoId",
                principalTable: "Projeto",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
