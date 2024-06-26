﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EMS.WebApi.Data.Migrations;

/// <inheritdoc />
public partial class AddIsActivePropToService : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<bool>(
            name: "IsActive",
            table: "Services",
            type: "bit",
            nullable: false,
            defaultValue: false);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropColumn(
            name: "IsActive",
            table: "Services");
    }
}
