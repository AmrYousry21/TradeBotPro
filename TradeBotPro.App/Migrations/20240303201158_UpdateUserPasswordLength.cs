using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TradeBotPro.App.Migrations
{
    public partial class UpdateUserPasswordLength : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Password",
                table: "users",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(24)",
                oldMaxLength: 24);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Password",
                table: "users",
                type: "varchar(24)",
                maxLength: 24,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext");
        }
    }
}
