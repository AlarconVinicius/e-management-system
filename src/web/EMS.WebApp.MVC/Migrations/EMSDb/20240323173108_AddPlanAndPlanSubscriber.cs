using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EMS.WebApp.MVC.Migrations.EMSDb
{
    /// <inheritdoc />
    public partial class AddPlanAndPlanSubscriber : Migration
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
                    SubTitle = table.Column<string>(type: "varchar(400)", nullable: false),
                    Price = table.Column<double>(type: "float", nullable: false),
                    Benefits = table.Column<string>(type: "Text", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Plans", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PlanSubscribers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PlanId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SubscriberId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserName = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false),
                    UserEmail = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false),
                    UserCpf = table.Column<string>(type: "varchar(11)", maxLength: 11, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlanSubscribers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PlanSubscribers_Plans_PlanId",
                        column: x => x.PlanId,
                        principalTable: "Plans",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PlanSubscribers_PlanId",
                table: "PlanSubscribers",
                column: "PlanId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PlanSubscribers");

            migrationBuilder.DropTable(
                name: "Plans");
        }
    }
}
