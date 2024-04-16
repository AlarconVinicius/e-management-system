using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EMS.WebApp.MVC.Migrations;

/// <inheritdoc />
public partial class Tenant : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<Guid>(
            name: "TenantId",
            table: "Users",
            type: "uniqueidentifier",
            nullable: false,
            defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

        migrationBuilder.AddColumn<Guid>(
            name: "TenantId",
            table: "Services",
            type: "uniqueidentifier",
            nullable: false,
            defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

        migrationBuilder.AddColumn<Guid>(
            name: "TenantId",
            table: "Products",
            type: "uniqueidentifier",
            nullable: false,
            defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

        migrationBuilder.AddColumn<Guid>(
            name: "TenantId",
            table: "Companies",
            type: "uniqueidentifier",
            nullable: false,
            defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

        migrationBuilder.AddColumn<Guid>(
            name: "TenantId",
            table: "Clients",
            type: "uniqueidentifier",
            nullable: false,
            defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

        migrationBuilder.CreateTable(
            name: "Tenants",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                Name = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false),
                IsActive = table.Column<bool>(type: "bit", nullable: false),
                CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Tenants", x => x.Id);
            });

        migrationBuilder.CreateIndex(
            name: "IX_Users_TenantId",
            table: "Users",
            column: "TenantId");

        migrationBuilder.CreateIndex(
            name: "IX_Services_TenantId",
            table: "Services",
            column: "TenantId");

        migrationBuilder.CreateIndex(
            name: "IX_Products_TenantId",
            table: "Products",
            column: "TenantId");

        migrationBuilder.CreateIndex(
            name: "IX_Companies_TenantId",
            table: "Companies",
            column: "TenantId",
            unique: true);

        migrationBuilder.CreateIndex(
            name: "IX_Clients_TenantId",
            table: "Clients",
            column: "TenantId");

        migrationBuilder.AddForeignKey(
            name: "FK_Clients_Tenants_TenantId",
            table: "Clients",
            column: "TenantId",
            principalTable: "Tenants",
            principalColumn: "Id",
            onDelete: ReferentialAction.Cascade);

        migrationBuilder.AddForeignKey(
            name: "FK_Companies_Tenants_TenantId",
            table: "Companies",
            column: "TenantId",
            principalTable: "Tenants",
            principalColumn: "Id",
            onDelete: ReferentialAction.Restrict);

        migrationBuilder.AddForeignKey(
            name: "FK_Products_Tenants_TenantId",
            table: "Products",
            column: "TenantId",
            principalTable: "Tenants",
            principalColumn: "Id",
            onDelete: ReferentialAction.Cascade);

        migrationBuilder.AddForeignKey(
            name: "FK_Services_Tenants_TenantId",
            table: "Services",
            column: "TenantId",
            principalTable: "Tenants",
            principalColumn: "Id",
            onDelete: ReferentialAction.Cascade);

        migrationBuilder.AddForeignKey(
            name: "FK_Users_Tenants_TenantId",
            table: "Users",
            column: "TenantId",
            principalTable: "Tenants",
            principalColumn: "Id",
            onDelete: ReferentialAction.Cascade);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey(
            name: "FK_Clients_Tenants_TenantId",
            table: "Clients");

        migrationBuilder.DropForeignKey(
            name: "FK_Companies_Tenants_TenantId",
            table: "Companies");

        migrationBuilder.DropForeignKey(
            name: "FK_Products_Tenants_TenantId",
            table: "Products");

        migrationBuilder.DropForeignKey(
            name: "FK_Services_Tenants_TenantId",
            table: "Services");

        migrationBuilder.DropForeignKey(
            name: "FK_Users_Tenants_TenantId",
            table: "Users");

        migrationBuilder.DropTable(
            name: "Tenants");

        migrationBuilder.DropIndex(
            name: "IX_Users_TenantId",
            table: "Users");

        migrationBuilder.DropIndex(
            name: "IX_Services_TenantId",
            table: "Services");

        migrationBuilder.DropIndex(
            name: "IX_Products_TenantId",
            table: "Products");

        migrationBuilder.DropIndex(
            name: "IX_Companies_TenantId",
            table: "Companies");

        migrationBuilder.DropIndex(
            name: "IX_Clients_TenantId",
            table: "Clients");

        migrationBuilder.DropColumn(
            name: "TenantId",
            table: "Users");

        migrationBuilder.DropColumn(
            name: "TenantId",
            table: "Services");

        migrationBuilder.DropColumn(
            name: "TenantId",
            table: "Products");

        migrationBuilder.DropColumn(
            name: "TenantId",
            table: "Companies");

        migrationBuilder.DropColumn(
            name: "TenantId",
            table: "Clients");
    }
}
