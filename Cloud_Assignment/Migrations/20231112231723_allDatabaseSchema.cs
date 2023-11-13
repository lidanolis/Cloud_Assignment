using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Cloud_Assignment.Migrations
{
    /// <inheritdoc />
    public partial class allDatabaseSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "FoodRecord",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "FinancialRecord",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "RequestRecord",
                columns: table => new
                {
                    RecordId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    requestType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    StaffId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    FoodId = table.Column<int>(type: "int", nullable: true),
                    FoodQuantity = table.Column<int>(type: "int", nullable: true),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    RequestStatus = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RequestDesc = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DOR = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RequestRecord", x => x.RecordId);
                    table.ForeignKey(
                        name: "FK_RequestRecord_AspNetUsers_StaffId",
                        column: x => x.StaffId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RequestRecord_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RequestRecord_InventoryRecord_FoodId",
                        column: x => x.FoodId,
                        principalTable: "InventoryRecord",
                        principalColumn: "FoodId");
                });

            migrationBuilder.CreateTable(
                name: "DistributionSchedule",
                columns: table => new
                {
                    DistributionId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RecordType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RecordId = table.Column<int>(type: "int", nullable: true),
                    DistributionStatus = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DOD = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DistributorId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DistributionSchedule", x => x.DistributionId);
                    table.ForeignKey(
                        name: "FK_DistributionSchedule_AspNetUsers_DistributorId",
                        column: x => x.DistributorId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_DistributionSchedule_RequestRecord_RecordId",
                        column: x => x.RecordId,
                        principalTable: "RequestRecord",
                        principalColumn: "RecordId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_DistributionSchedule_DistributorId",
                table: "DistributionSchedule",
                column: "DistributorId");

            migrationBuilder.CreateIndex(
                name: "IX_DistributionSchedule_RecordId",
                table: "DistributionSchedule",
                column: "RecordId");

            migrationBuilder.CreateIndex(
                name: "IX_RequestRecord_FoodId",
                table: "RequestRecord",
                column: "FoodId");

            migrationBuilder.CreateIndex(
                name: "IX_RequestRecord_StaffId",
                table: "RequestRecord",
                column: "StaffId");

            migrationBuilder.CreateIndex(
                name: "IX_RequestRecord_UserId",
                table: "RequestRecord",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DistributionSchedule");

            migrationBuilder.DropTable(
                name: "RequestRecord");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "FoodRecord");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "FinancialRecord");
        }
    }
}
