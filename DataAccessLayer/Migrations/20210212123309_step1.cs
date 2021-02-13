using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DataAccessLayer.Migrations
{
    public partial class step1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Phones",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    phone = table.Column<string>(nullable: true),
                    objectid = table.Column<int>(nullable: false),
                    objectname = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Phones", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "RefreshTokens",
                columns: table => new
                {
                    RefreshTokenId = table.Column<string>(nullable: false),
                    RefreshTokenSubject = table.Column<string>(nullable: true),
                    RefreshTokenIssuedUtc = table.Column<DateTime>(nullable: false),
                    RefreshTokenExpiresUtc = table.Column<DateTime>(nullable: false),
                    IsRefreshTokenDeleted = table.Column<bool>(nullable: true),
                    ClientId = table.Column<string>(nullable: true),
                    ClientSecret = table.Column<string>(nullable: true),
                    ClientName = table.Column<string>(nullable: true),
                    ClientApplicationType = table.Column<bool>(nullable: true),
                    ClientActive = table.Column<bool>(nullable: true),
                    ClientRefreshTokenLifeTime = table.Column<int>(nullable: true),
                    ClientAllowedOrigin = table.Column<string>(nullable: true),
                    UserId = table.Column<long>(nullable: false),
                    UserName = table.Column<string>(nullable: true),
                    UserEmail = table.Column<string>(nullable: true),
                    UserPhone = table.Column<string>(nullable: true),
                    IsUserBlocked = table.Column<bool>(nullable: false),
                    IsUserDeleted = table.Column<bool>(nullable: false),
                    IsUserReported = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RefreshTokens", x => x.RefreshTokenId);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    RoleId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    RoleName = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.RoleId);
                });

            migrationBuilder.CreateTable(
                name: "Sector",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sector", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    UserId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    UserName = table.Column<string>(nullable: false),
                    Password = table.Column<string>(nullable: true),
                    LastAccess = table.Column<DateTime>(nullable: false),
                    ObjectName = table.Column<string>(nullable: true),
                    ObjectId = table.Column<string>(nullable: true),
                    active = table.Column<bool>(nullable: false),
                    RoleId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.UserId);
                    table.ForeignKey(
                        name: "FK_Users_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "RoleId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Customer",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    name = table.Column<string>(nullable: true),
                    address = table.Column<string>(nullable: true),
                    field = table.Column<string>(nullable: true),
                    sectorid = table.Column<int>(nullable: false),
                    ownerid = table.Column<int>(nullable: false),
                    count = table.Column<int>(nullable: false),
                    hours = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customer", x => x.id);
                    table.ForeignKey(
                        name: "FK_Customer_Users_ownerid",
                        column: x => x.ownerid,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Customer_Sector_sectorid",
                        column: x => x.sectorid,
                        principalTable: "Sector",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Employee",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    name = table.Column<string>(nullable: true),
                    jobtitle = table.Column<string>(nullable: true),
                    email = table.Column<string>(nullable: true),
                    customerid = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employee", x => x.id);
                    table.ForeignKey(
                        name: "FK_Employee_Customer_customerid",
                        column: x => x.customerid,
                        principalTable: "Customer",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FollowUp",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    customerid = table.Column<int>(nullable: false),
                    ownerid = table.Column<int>(nullable: false),
                    followup = table.Column<bool>(nullable: false),
                    order = table.Column<bool>(nullable: false),
                    create = table.Column<DateTime>(nullable: false),
                    discribtion = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FollowUp", x => x.id);
                    table.ForeignKey(
                        name: "FK_FollowUp_Customer_customerid",
                        column: x => x.customerid,
                        principalTable: "Customer",
                        principalColumn: "id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_FollowUp_Users_ownerid",
                        column: x => x.ownerid,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "Order",
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
                    table.PrimaryKey("PK_Order", x => x.id);
                    table.ForeignKey(
                        name: "FK_Order_Customer_customerid",
                        column: x => x.customerid,
                        principalTable: "Customer",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Order_Users_ownerid",
                        column: x => x.ownerid,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Customer_ownerid",
                table: "Customer",
                column: "ownerid");

            migrationBuilder.CreateIndex(
                name: "IX_Customer_sectorid",
                table: "Customer",
                column: "sectorid");

            migrationBuilder.CreateIndex(
                name: "IX_Employee_customerid",
                table: "Employee",
                column: "customerid");

            migrationBuilder.CreateIndex(
                name: "IX_FollowUp_customerid",
                table: "FollowUp",
                column: "customerid");

            migrationBuilder.CreateIndex(
                name: "IX_FollowUp_ownerid",
                table: "FollowUp",
                column: "ownerid");

            migrationBuilder.CreateIndex(
                name: "IX_Order_customerid",
                table: "Order",
                column: "customerid");

            migrationBuilder.CreateIndex(
                name: "IX_Order_ownerid",
                table: "Order",
                column: "ownerid");

            migrationBuilder.CreateIndex(
                name: "IX_Users_RoleId",
                table: "Users",
                column: "RoleId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Employee");

            migrationBuilder.DropTable(
                name: "FollowUp");

            migrationBuilder.DropTable(
                name: "Order");

            migrationBuilder.DropTable(
                name: "Phones");

            migrationBuilder.DropTable(
                name: "RefreshTokens");

            migrationBuilder.DropTable(
                name: "Customer");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Sector");

            migrationBuilder.DropTable(
                name: "Roles");
        }
    }
}
