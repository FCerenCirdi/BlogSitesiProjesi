using Microsoft.EntityFrameworkCore.Migrations;

namespace BlogSitesiProjesi.Migrations
{
    public partial class SecondlyCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ArticleId",
                table: "Topics",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Topics_ArticleId",
                table: "Topics",
                column: "ArticleId");

            migrationBuilder.AddForeignKey(
                name: "FK_Topics_Articles_ArticleId",
                table: "Topics",
                column: "ArticleId",
                principalTable: "Articles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Topics_Articles_ArticleId",
                table: "Topics");

            migrationBuilder.DropIndex(
                name: "IX_Topics_ArticleId",
                table: "Topics");

            migrationBuilder.DropColumn(
                name: "ArticleId",
                table: "Topics");
        }
    }
}
