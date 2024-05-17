using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ToDo.Data.Migrations
{
    public partial class init2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "ToDos",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "IdentityUser");

            migrationBuilder.CreateIndex(
                name: "IX_ToDos_UserId",
                table: "ToDos",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_ToDos_AspNetUsers_UserId",
                table: "ToDos",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ToDos_AspNetUsers_UserId",
                table: "ToDos");

            migrationBuilder.DropIndex(
                name: "IX_ToDos_UserId",
                table: "ToDos");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "ToDos");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "AspNetUsers");
        }
    }
}
