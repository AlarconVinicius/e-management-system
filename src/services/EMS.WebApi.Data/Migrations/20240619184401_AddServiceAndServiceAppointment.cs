using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EMS.WebApi.Data.Migrations;

/// <inheritdoc />
public partial class AddServiceAndServiceAppointment : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "Services",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                CompanyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                Name = table.Column<string>(type: "varchar(200)", nullable: false),
                Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false, defaultValue: 0m),
                Duration = table.Column<TimeSpan>(type: "time", nullable: false),
                CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Services", x => x.Id);
                table.CheckConstraint("CK_Service_Price", "Price >= 0");
                table.ForeignKey(
                    name: "FK_Services_Companies_CompanyId",
                    column: x => x.CompanyId,
                    principalTable: "Companies",
                    principalColumn: "Id");
            });

        migrationBuilder.CreateTable(
            name: "ServiceAppointments",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                CompanyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                EmployeeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                ClientId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                ServiceId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                AppointmentStart = table.Column<DateTime>(type: "datetime2", nullable: false),
                AppointmentEnd = table.Column<DateTime>(type: "datetime2", nullable: false),
                Status = table.Column<int>(type: "int", nullable: false),
                CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_ServiceAppointments", x => x.Id);
                table.ForeignKey(
                    name: "FK_ServiceAppointments_Clients_ClientId",
                    column: x => x.ClientId,
                    principalTable: "Clients",
                    principalColumn: "Id");
                table.ForeignKey(
                    name: "FK_ServiceAppointments_Companies_CompanyId",
                    column: x => x.CompanyId,
                    principalTable: "Companies",
                    principalColumn: "Id");
                table.ForeignKey(
                    name: "FK_ServiceAppointments_Employees_EmployeeId",
                    column: x => x.EmployeeId,
                    principalTable: "Employees",
                    principalColumn: "Id");
                table.ForeignKey(
                    name: "FK_ServiceAppointments_Services_ServiceId",
                    column: x => x.ServiceId,
                    principalTable: "Services",
                    principalColumn: "Id");
            });

        migrationBuilder.CreateIndex(
            name: "IX_ServiceAppointments_ClientId",
            table: "ServiceAppointments",
            column: "ClientId");

        migrationBuilder.CreateIndex(
            name: "IX_ServiceAppointments_CompanyId",
            table: "ServiceAppointments",
            column: "CompanyId");

        migrationBuilder.CreateIndex(
            name: "IX_ServiceAppointments_EmployeeId",
            table: "ServiceAppointments",
            column: "EmployeeId");

        migrationBuilder.CreateIndex(
            name: "IX_ServiceAppointments_ServiceId",
            table: "ServiceAppointments",
            column: "ServiceId");

        migrationBuilder.CreateIndex(
            name: "IX_Services_CompanyId",
            table: "Services",
            column: "CompanyId");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "ServiceAppointments");

        migrationBuilder.DropTable(
            name: "Services");
    }
}
