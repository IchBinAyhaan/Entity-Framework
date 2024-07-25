using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EFCoreWithEntity.Migrations
{
    /// <inheritdoc />
    public partial class StudentOtherPropertyAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Test",
                table: "Students");

            migrationBuilder.AddColumn<string>(
                name: "OtherPrperty",
                table: "Students",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "salam");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OtherPrperty",
                table: "Students");

            migrationBuilder.AddColumn<string>(
                name: "Test",
                table: "Students",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
