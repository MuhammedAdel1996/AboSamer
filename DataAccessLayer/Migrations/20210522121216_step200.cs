using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DataAccessLayer.Migrations
{
    public partial class step200 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Lock",
                table: "Order",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Lock",
                table: "FollowUp",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Lock",
                table: "Checks",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "CheckResult",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    result = table.Column<string>(nullable: true),
                    orderid = table.Column<int>(nullable: false),
                    useraction = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CheckResult", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "CustomerLock",
                columns: table => new
                {
                    id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    customerid = table.Column<int>(nullable: false),
                    objectname = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerLock", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "OrderResult",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    result = table.Column<string>(nullable: true),
                    orderid = table.Column<int>(nullable: false),
                    useraction = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderResult", x => x.id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CheckResult");

            migrationBuilder.DropTable(
                name: "CustomerLock");

            migrationBuilder.DropTable(
                name: "OrderResult");

            migrationBuilder.DropColumn(
                name: "Lock",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "Lock",
                table: "FollowUp");

            migrationBuilder.DropColumn(
                name: "Lock",
                table: "Checks");
        }
    }
}
