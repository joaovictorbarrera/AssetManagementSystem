using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ThreatlockerAssetManagementSystem.Migrations
{
    /// <inheritdoc />
    public partial class SeedInitialUsers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Active", "CreatedAt", "EmailAddress", "LastLoginAt", "Role" },
                values: new object[,]
                {
                    { new Guid("11111111-1111-1111-1111-111111111111"), true, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "admin@test.com", null, "Admin" },
                    { new Guid("22222222-2222-2222-2222-222222222222"), true, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "manager@test.com", null, "AssetManager" },
                    { new Guid("33333333-3333-3333-3333-333333333333"), true, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "employee@test.com", null, "Employee" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"));

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222222"));

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("33333333-3333-3333-3333-333333333333"));
        }
    }
}
