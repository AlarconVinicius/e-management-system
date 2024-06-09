using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EMS.WebApi.Data.Migrations;

/// <inheritdoc />
public partial class ChangeTypeOfRole : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AlterColumn<short>(
            name: "Role",
            table: "Users",
            type: "SMALLINT",
            nullable: false,
            defaultValue: (short)0,
            oldClrType: typeof(string),
            oldType: "varchar(100)",
            oldNullable: true);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AlterColumn<string>(
            name: "Role",
            table: "Users",
            type: "varchar(100)",
            nullable: true,
            oldClrType: typeof(short),
            oldType: "SMALLINT");
    }
}
