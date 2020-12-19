using Microsoft.EntityFrameworkCore.Migrations;

namespace BookStore.Data.Migrations.BookStore
{
    public partial class AddPermissionsSerialized : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Permissions",
                table: "User",
                type: "nvarchar(2000)",
                maxLength: 2000,
                nullable: false,
                defaultValueSql: "'[]'");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Permissions",
                table: "User");
        }
    }
}
