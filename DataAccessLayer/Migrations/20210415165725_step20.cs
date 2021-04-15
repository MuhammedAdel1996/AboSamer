using Microsoft.EntityFrameworkCore.Migrations;

namespace DataAccessLayer.Migrations
{
    public partial class step20 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "useraction",
                table: "Order",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "useraction",
                table: "Checks",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "useraction",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "useraction",
                table: "Checks");
        }
    }
}
