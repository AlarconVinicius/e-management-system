using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EMS.WebApp.MVC.Migrations;

/// <inheritdoc />
public partial class Initial : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "Plans",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                Title = table.Column<string>(type: "varchar(250)", nullable: false),
                Subtitle = table.Column<string>(type: "varchar(400)", nullable: false),
                Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                Benefits = table.Column<string>(type: "Text", nullable: false),
                IsActive = table.Column<bool>(type: "bit", nullable: false),
                CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Plans", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "Companies",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                PlanId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                Name = table.Column<string>(type: "varchar(200)", nullable: false),
                Cpf = table.Column<string>(type: "varchar(11)", maxLength: 11, nullable: true),
                IsActive = table.Column<bool>(type: "bit", nullable: false),
                CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Companies", x => x.Id);
                table.ForeignKey(
                    name: "FK_Companies_Plans_PlanId",
                    column: x => x.PlanId,
                    principalTable: "Plans",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "Clients",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                CompanyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                Name = table.Column<string>(type: "varchar(200)", nullable: false),
                LastName = table.Column<string>(type: "varchar(200)", nullable: false),
                Email = table.Column<string>(type: "varchar(254)", nullable: true),
                PhoneNumber = table.Column<string>(type: "varchar(15)", nullable: false),
                Cpf = table.Column<string>(type: "varchar(11)", maxLength: 11, nullable: true),
                IsActive = table.Column<bool>(type: "bit", nullable: false),
                CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Clients", x => x.Id);
                table.ForeignKey(
                    name: "FK_Clients_Companies_CompanyId",
                    column: x => x.CompanyId,
                    principalTable: "Companies",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "Products",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                CompanyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                Title = table.Column<string>(type: "varchar(200)", nullable: false),
                Description = table.Column<string>(type: "Text", nullable: false),
                UnitaryValue = table.Column<decimal>(type: "decimal(18,2)", nullable: false, defaultValue: 0m),
                Image = table.Column<string>(type: "varchar(100)", nullable: true),
                IsActive = table.Column<bool>(type: "bit", nullable: false),
                CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Products", x => x.Id);
                table.CheckConstraint("CK_Product_UnitaryValue", "UnitaryValue >= 0");
                table.ForeignKey(
                    name: "FK_Products_Companies_CompanyId",
                    column: x => x.CompanyId,
                    principalTable: "Companies",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "Services",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                CompanyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                Title = table.Column<string>(type: "varchar(200)", nullable: false),
                Description = table.Column<string>(type: "varchar(1000)", nullable: false),
                Value = table.Column<decimal>(type: "decimal(18,2)", nullable: false, defaultValue: 0m),
                IsActive = table.Column<bool>(type: "bit", nullable: false),
                CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Services", x => x.Id);
                table.CheckConstraint("CK_Service_Value", "Value >= 0");
                table.ForeignKey(
                    name: "FK_Services_Companies_CompanyId",
                    column: x => x.CompanyId,
                    principalTable: "Companies",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "Users",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                CompanyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                Name = table.Column<string>(type: "varchar(200)", nullable: false),
                LastName = table.Column<string>(type: "varchar(200)", nullable: false),
                Email = table.Column<string>(type: "varchar(254)", nullable: true),
                PhoneNumber = table.Column<string>(type: "varchar(15)", nullable: false),
                Cpf = table.Column<string>(type: "varchar(11)", maxLength: 11, nullable: true),
                IsActive = table.Column<bool>(type: "bit", nullable: false),
                CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Users", x => x.Id);
                table.ForeignKey(
                    name: "FK_Users_Companies_CompanyId",
                    column: x => x.CompanyId,
                    principalTable: "Companies",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
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
                UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                ClientId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Addresses", x => x.Id);
                table.ForeignKey(
                    name: "FK_Addresses_Clients_ClientId",
                    column: x => x.ClientId,
                    principalTable: "Clients",
                    principalColumn: "Id");
                table.ForeignKey(
                    name: "FK_Addresses_Users_UserId",
                    column: x => x.UserId,
                    principalTable: "Users",
                    principalColumn: "Id");
            });

        migrationBuilder.CreateIndex(
            name: "IX_Addresses_ClientId",
            table: "Addresses",
            column: "ClientId",
            unique: true,
            filter: "[ClientId] IS NOT NULL");

        migrationBuilder.CreateIndex(
            name: "IX_Addresses_UserId",
            table: "Addresses",
            column: "UserId",
            unique: true,
            filter: "[UserId] IS NOT NULL");

        migrationBuilder.CreateIndex(
            name: "IX_Clients_CompanyId",
            table: "Clients",
            column: "CompanyId");

        migrationBuilder.CreateIndex(
            name: "IX_Companies_PlanId",
            table: "Companies",
            column: "PlanId");

        migrationBuilder.CreateIndex(
            name: "IX_Products_CompanyId",
            table: "Products",
            column: "CompanyId");

        migrationBuilder.CreateIndex(
            name: "IX_Services_CompanyId",
            table: "Services",
            column: "CompanyId");

        migrationBuilder.CreateIndex(
            name: "IX_Users_CompanyId",
            table: "Users",
            column: "CompanyId");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "Addresses");

        migrationBuilder.DropTable(
            name: "Products");

        migrationBuilder.DropTable(
            name: "Services");

        migrationBuilder.DropTable(
            name: "Clients");

        migrationBuilder.DropTable(
            name: "Users");

        migrationBuilder.DropTable(
            name: "Companies");

        migrationBuilder.DropTable(
            name: "Plans");
    }
}
