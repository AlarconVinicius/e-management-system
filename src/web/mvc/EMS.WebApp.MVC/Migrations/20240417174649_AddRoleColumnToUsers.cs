using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EMS.WebApp.MVC.Migrations;

/// <inheritdoc />
public partial class AddRoleColumnToUsers : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<string>(
            name: "Role",
            table: "Users",
            type: "varchar(100)",
            nullable: true);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropColumn(
            name: "Role",
            table: "Users");
    }
}
