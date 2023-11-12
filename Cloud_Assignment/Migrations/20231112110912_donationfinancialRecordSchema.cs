using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Cloud_Assignment.Migrations
{
    /// <inheritdoc />
    public partial class donationfinancialRecordSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FinancialRecord",
                columns: table => new
                {
                    RecordId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RecordType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    CurrencyType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Balance = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    DOR = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FinancialRecord", x => x.RecordId);
                    table.ForeignKey(
                        name: "FK_FinancialRecord_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "InventoryRecord",
                columns: table => new
                {
                    FoodId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FoodName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FoodTotal = table.Column<int>(type: "int", nullable: true),
                    FoodDesc = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InventoryRecord", x => x.FoodId);
                });

            migrationBuilder.CreateTable(
                name: "FoodRecord",
                columns: table => new
                {
                    RecordId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RecordType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    FoodId = table.Column<int>(type: "int", nullable: true),
                    FoodQuantity = table.Column<int>(type: "int", nullable: true),
                    DOR = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FoodRecord", x => x.RecordId);
                    table.ForeignKey(
                        name: "FK_FoodRecord_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_FoodRecord_InventoryRecord_FoodId",
                        column: x => x.FoodId,
                        principalTable: "InventoryRecord",
                        principalColumn: "FoodId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_FinancialRecord_UserId",
                table: "FinancialRecord",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_FoodRecord_FoodId",
                table: "FoodRecord",
                column: "FoodId");

            migrationBuilder.CreateIndex(
                name: "IX_FoodRecord_UserId",
                table: "FoodRecord",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FinancialRecord");

            migrationBuilder.DropTable(
                name: "FoodRecord");

            migrationBuilder.DropTable(
                name: "InventoryRecord");
        }
    }
}
