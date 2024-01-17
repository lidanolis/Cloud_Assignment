using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Cloud_Assignment.Migrations
{
    /// <inheritdoc />
    public partial class UpdateDistributionScheduleImageKey : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
            name: "ImageURL",
            table: "DistributionSchedule",
            type: "nvarchar(max)",
            nullable: true);

            migrationBuilder.AddColumn<string>(
            name: "ImageS3Key",
            table: "DistributionSchedule",
            type: "nvarchar(max)",
            nullable: true);

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageURL",
                table: "DistributionSchedule");

            migrationBuilder.DropColumn(
                name: "ImageS3Key",
                table: "DistributionSchedule");
        }
    }
}
