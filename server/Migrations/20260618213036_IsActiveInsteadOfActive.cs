using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ThreatlockerAssetManagementSystem.Migrations
{
    /// <inheritdoc />
    public partial class IsActiveInsteadOfActive : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Active",
                table: "Users",
                newName: "IsActive");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsActive",
                table: "Users",
                newName: "Active");
        }
    }
}
