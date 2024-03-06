using System;
using Microsoft.EntityFrameworkCore.Migrations;
using MySql.EntityFrameworkCore.Metadata;

#nullable disable

namespace TradeBotPro.App.Migrations
{
    public partial class InitialSetup : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "clients",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false),
                    Name = table.Column<string>(type: "longtext", nullable: false),
                    Registrationkey = table.Column<string>(type: "varchar(32)", maxLength: 32, nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_clients", x => x.Id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false),
                    FirstName = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    LastName = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    Email = table.Column<string>(type: "longtext", nullable: false),
                    Password = table.Column<string>(type: "varchar(24)", maxLength: 24, nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    UserRole = table.Column<int>(type: "int", nullable: false),
                    ClientId = table.Column<Guid>(type: "char(36)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_users", x => x.Id);
                    table.ForeignKey(
                        name: "FK_users_clients_ClientId",
                        column: x => x.ClientId,
                        principalTable: "clients",
                        principalColumn: "Id");
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "closures",
                columns: table => new
                {
                    PositionId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    OrderId = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    VolumeInUnits = table.Column<double>(type: "double", nullable: false),
                    GrossProfit = table.Column<double>(type: "double", nullable: false),
                    NetProfit = table.Column<double>(type: "double", nullable: false),
                    Margin = table.Column<double>(type: "double", nullable: false),
                    Commissions = table.Column<double>(type: "double", nullable: false),
                    Quantity = table.Column<double>(type: "double", nullable: false),
                    UserId = table.Column<Guid>(type: "char(36)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_closures", x => x.PositionId);
                    table.ForeignKey(
                        name: "FK_closures_users_UserId",
                        column: x => x.UserId,
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "orders",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    BarIndex = table.Column<int>(type: "int", nullable: false),
                    PositionId = table.Column<int>(type: "int", nullable: false),
                    OrderId = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    ZoneId = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    Type = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    Symbol = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    EntryPrice = table.Column<double>(type: "double", nullable: false),
                    EntryTime = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    StopLoss = table.Column<double>(type: "double", nullable: false),
                    TakeProfit = table.Column<double>(type: "double", nullable: false),
                    Pips = table.Column<double>(type: "double", nullable: false),
                    UserId = table.Column<Guid>(type: "char(36)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_orders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_orders_users_UserId",
                        column: x => x.UserId,
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_closures_UserId",
                table: "closures",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_orders_UserId",
                table: "orders",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_users_ClientId",
                table: "users",
                column: "ClientId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "closures");

            migrationBuilder.DropTable(
                name: "orders");

            migrationBuilder.DropTable(
                name: "users");

            migrationBuilder.DropTable(
                name: "clients");
        }
    }
}
