using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DataAccessLayer.Migrations
{
    public partial class step10 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "whatsapp",
                table: "Phones",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "email",
                table: "Customer",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "vacancy",
                table: "Customer",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Checks",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    description = table.Column<string>(nullable: true),
                    create = table.Column<DateTime>(nullable: false),
                    customerid = table.Column<int>(nullable: false),
                    ownerid = table.Column<int>(nullable: true),
                    result = table.Column<string>(nullable: true),
                    count = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Checks", x => x.id);
                    table.ForeignKey(
                        name: "FK_Checks_Customer_customerid",
                        column: x => x.customerid,
                        principalTable: "Customer",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Checks_Users_ownerid",
                        column: x => x.ownerid,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Checks_customerid",
                table: "Checks",
                column: "customerid");

            migrationBuilder.CreateIndex(
                name: "IX_Checks_ownerid",
                table: "Checks",
                column: "ownerid");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Checks");

            migrationBuilder.DropColumn(
                name: "whatsapp",
                table: "Phones");

            migrationBuilder.DropColumn(
                name: "email",
                table: "Customer");

            migrationBuilder.DropColumn(
                name: "vacancy",
                table: "Customer");
        }
    }
}
