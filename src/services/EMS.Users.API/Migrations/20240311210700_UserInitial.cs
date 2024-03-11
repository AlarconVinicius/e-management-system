using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EMS.Users.API.Migrations;

/// <inheritdoc />
public partial class UserInitial : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "Users",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                Name = table.Column<string>(type: "varchar(200)", nullable: false),
                Email = table.Column<string>(type: "varchar(254)", nullable: true),
                Cpf = table.Column<string>(type: "varchar(11)", maxLength: 11, nullable: true),
                Deleted = table.Column<bool>(type: "bit", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Users", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "Addresses",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                Street = table.Column<string>(type: "varchar(200)", nullable: false),
                Number = table.Column<string>(type: "varchar(50)", nullable: false),
                Complement = table.Column<string>(type: "varchar(250)", nullable: true),
                Neighborhood = table.Column<string>(type: "varchar(100)", nullable: false),
                ZipCode = table.Column<string>(type: "varchar(20)", nullable: false),
                City = table.Column<string>(type: "varchar(100)", nullable: false),
                State = table.Column<string>(type: "varchar(50)", nullable: false),
                UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Addresses", x => x.Id);
                table.ForeignKey(
                    name: "FK_Addresses_Users_UserId",
                    column: x => x.UserId,
                    principalTable: "Users",
                    principalColumn: "Id");
            });

        migrationBuilder.CreateIndex(
            name: "IX_Addresses_UserId",
            table: "Addresses",
            column: "UserId",
            unique: true);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "Addresses");

        migrationBuilder.DropTable(
            name: "Users");
    }
}
