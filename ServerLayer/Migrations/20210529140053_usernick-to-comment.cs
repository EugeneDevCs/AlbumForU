using Microsoft.EntityFrameworkCore.Migrations;

namespace ServerLayer.Migrations
{
    public partial class usernicktocomment : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserNick",
                table: "Comments",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserNick",
                table: "Comments");
        }
    }
}
