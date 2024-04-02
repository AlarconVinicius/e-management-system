using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EMS.WebApp.MVC.Migrations;

/// <inheritdoc />
public partial class FixColumnType : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AlterColumn<string>(
            name: "Description",
            table: "Products",
            type: "varchar(1000)",
            nullable: false,
            oldClrType: typeof(string),
            oldType: "Text");

        migrationBuilder.AlterColumn<string>(
            name: "Benefits",
            table: "Plans",
            type: "varchar(1000)",
            nullable: false,
            oldClrType: typeof(string),
            oldType: "Text");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AlterColumn<string>(
            name: "Description",
            table: "Products",
            type: "Text",
            nullable: false,
            oldClrType: typeof(string),
            oldType: "varchar(1000)");

        migrationBuilder.AlterColumn<string>(
            name: "Benefits",
            table: "Plans",
            type: "Text",
            nullable: false,
            oldClrType: typeof(string),
            oldType: "varchar(1000)");
    }
}
