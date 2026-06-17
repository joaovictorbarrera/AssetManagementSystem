using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ThreatlockerAssetManagementSystem.Migrations
{
    /// <inheritdoc />
    public partial class SeedAssetsAndRequests : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Assets",
                columns: new[] { "Id", "AssetTag", "AssignedToUserId", "Category", "Condition", "CreatedAt", "IsArchived", "Name", "SerialNumber", "Status", "UpdatedAt" },
                values: new object[,]
                {
                    { new Guid("a1111111-0000-0000-0000-000000000001"), "LAP-001", new Guid("33333333-3333-3333-3333-333333333333"), "Laptop", "Good", new DateTime(2025, 1, 2, 0, 0, 0, 0, DateTimeKind.Utc), false, "Dell Latitude 5440", "DL5440-0001", "Assigned", null },
                    { new Guid("a1111111-0000-0000-0000-000000000002"), "LAP-002", null, "Laptop", "New", new DateTime(2025, 1, 2, 0, 0, 0, 0, DateTimeKind.Utc), false, "Dell Latitude 5440", "DL5440-0002", "Available", null },
                    { new Guid("a1111111-0000-0000-0000-000000000003"), "LAP-003", null, "Laptop", "Damaged", new DateTime(2025, 1, 3, 0, 0, 0, 0, DateTimeKind.Utc), false, "MacBook Pro 14\"", "MBP14-0003", "Maintenance", null },
                    { new Guid("a2222222-0000-0000-0000-000000000001"), "MON-001", null, "Monitor", "Good", new DateTime(2025, 1, 4, 0, 0, 0, 0, DateTimeKind.Utc), false, "Dell UltraSharp 27\"", "DU27-0001", "Available", null },
                    { new Guid("a2222222-0000-0000-0000-000000000002"), "MON-002", null, "Monitor", "Good", new DateTime(2025, 1, 4, 0, 0, 0, 0, DateTimeKind.Utc), false, "Dell UltraSharp 27\"", "DU27-0002", "Available", null },
                    { new Guid("a2222222-0000-0000-0000-000000000003"), "MON-003", null, "Monitor", "Lost", new DateTime(2025, 1, 5, 0, 0, 0, 0, DateTimeKind.Utc), true, "LG UltraWide 34\"", "LGUW34-0003", "Retired", null },
                    { new Guid("a3333333-0000-0000-0000-000000000001"), "PHN-001", null, "Phone", "Good", new DateTime(2025, 1, 6, 0, 0, 0, 0, DateTimeKind.Utc), false, "iPhone 14", "IP14-0001", "Available", null },
                    { new Guid("a3333333-0000-0000-0000-000000000002"), "PHN-002", null, "Phone", "New", new DateTime(2025, 1, 6, 0, 0, 0, 0, DateTimeKind.Utc), false, "iPhone 14", "IP14-0002", "Requested", null },
                    { new Guid("a4444444-0000-0000-0000-000000000001"), "KEY-001", null, "SecurityKey", "New", new DateTime(2025, 1, 7, 0, 0, 0, 0, DateTimeKind.Utc), false, "YubiKey 5C", "YK5C-0001", "Available", null },
                    { new Guid("a4444444-0000-0000-0000-000000000002"), "KEY-002", null, "SecurityKey", "Good", new DateTime(2025, 1, 7, 0, 0, 0, 0, DateTimeKind.Utc), false, "YubiKey 5C", "YK5C-0002", "Available", null }
                });

            migrationBuilder.InsertData(
                table: "CheckoutRequests",
                columns: new[] { "Id", "ActionedAt", "AssetCategory", "AssignedAssetId", "CreatedAt", "Reason", "RequestedByUserId", "ReviewedByUserId", "Status", "UpdatedAt" },
                values: new object[,]
                {
                    { new Guid("b1111111-0000-0000-0000-000000000001"), null, "Laptop", null, new DateTime(2025, 2, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Need a backup laptop for travel", new Guid("33333333-3333-3333-3333-333333333333"), null, "Pending", null },
                    { new Guid("b1111111-0000-0000-0000-000000000002"), null, "Phone", null, new DateTime(2025, 2, 2, 0, 0, 0, 0, DateTimeKind.Utc), "Existing phone screen is cracked", new Guid("33333333-3333-3333-3333-333333333333"), null, "Pending", null },
                    { new Guid("b1111111-0000-0000-0000-000000000003"), null, "Monitor", null, new DateTime(2025, 2, 3, 0, 0, 0, 0, DateTimeKind.Utc), "Setting up a second monitor for new hire", new Guid("22222222-2222-2222-2222-222222222222"), null, "Pending", null },
                    { new Guid("b2222222-0000-0000-0000-000000000001"), new DateTime(2025, 2, 4, 0, 0, 0, 0, DateTimeKind.Utc), "SecurityKey", null, new DateTime(2025, 2, 1, 12, 0, 0, 0, DateTimeKind.Utc), "MFA hardware key for new system rollout", new Guid("33333333-3333-3333-3333-333333333333"), new Guid("22222222-2222-2222-2222-222222222222"), "Approved", null },
                    { new Guid("b2222222-0000-0000-0000-000000000002"), new DateTime(2025, 2, 5, 0, 0, 0, 0, DateTimeKind.Utc), "Monitor", null, new DateTime(2025, 2, 2, 12, 0, 0, 0, DateTimeKind.Utc), "Second monitor approved for home office setup", new Guid("22222222-2222-2222-2222-222222222222"), new Guid("11111111-1111-1111-1111-111111111111"), "Approved", null },
                    { new Guid("b3333333-0000-0000-0000-000000000001"), new DateTime(2025, 2, 6, 0, 0, 0, 0, DateTimeKind.Utc), "Tablet", null, new DateTime(2025, 2, 3, 12, 0, 0, 0, DateTimeKind.Utc), "Would like a tablet for note-taking in meetings", new Guid("33333333-3333-3333-3333-333333333333"), new Guid("22222222-2222-2222-2222-222222222222"), "Rejected", null },
                    { new Guid("b4444444-0000-0000-0000-000000000001"), new DateTime(2025, 1, 10, 0, 0, 0, 0, DateTimeKind.Utc), "Laptop", new Guid("a1111111-0000-0000-0000-000000000001"), new DateTime(2025, 1, 8, 0, 0, 0, 0, DateTimeKind.Utc), "Primary laptop for daily work", new Guid("33333333-3333-3333-3333-333333333333"), new Guid("22222222-2222-2222-2222-222222222222"), "Fulfilled", null },
                    { new Guid("b5555555-0000-0000-0000-000000000001"), new DateTime(2025, 1, 20, 0, 0, 0, 0, DateTimeKind.Utc), "Monitor", new Guid("a2222222-0000-0000-0000-000000000003"), new DateTime(2025, 1, 15, 0, 0, 0, 0, DateTimeKind.Utc), "Temporary monitor for project sprint", new Guid("33333333-3333-3333-3333-333333333333"), new Guid("22222222-2222-2222-2222-222222222222"), "Returned", null }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Assets",
                keyColumn: "Id",
                keyValue: new Guid("a1111111-0000-0000-0000-000000000002"));

            migrationBuilder.DeleteData(
                table: "Assets",
                keyColumn: "Id",
                keyValue: new Guid("a1111111-0000-0000-0000-000000000003"));

            migrationBuilder.DeleteData(
                table: "Assets",
                keyColumn: "Id",
                keyValue: new Guid("a2222222-0000-0000-0000-000000000001"));

            migrationBuilder.DeleteData(
                table: "Assets",
                keyColumn: "Id",
                keyValue: new Guid("a2222222-0000-0000-0000-000000000002"));

            migrationBuilder.DeleteData(
                table: "Assets",
                keyColumn: "Id",
                keyValue: new Guid("a3333333-0000-0000-0000-000000000001"));

            migrationBuilder.DeleteData(
                table: "Assets",
                keyColumn: "Id",
                keyValue: new Guid("a3333333-0000-0000-0000-000000000002"));

            migrationBuilder.DeleteData(
                table: "Assets",
                keyColumn: "Id",
                keyValue: new Guid("a4444444-0000-0000-0000-000000000001"));

            migrationBuilder.DeleteData(
                table: "Assets",
                keyColumn: "Id",
                keyValue: new Guid("a4444444-0000-0000-0000-000000000002"));

            migrationBuilder.DeleteData(
                table: "CheckoutRequests",
                keyColumn: "Id",
                keyValue: new Guid("b1111111-0000-0000-0000-000000000001"));

            migrationBuilder.DeleteData(
                table: "CheckoutRequests",
                keyColumn: "Id",
                keyValue: new Guid("b1111111-0000-0000-0000-000000000002"));

            migrationBuilder.DeleteData(
                table: "CheckoutRequests",
                keyColumn: "Id",
                keyValue: new Guid("b1111111-0000-0000-0000-000000000003"));

            migrationBuilder.DeleteData(
                table: "CheckoutRequests",
                keyColumn: "Id",
                keyValue: new Guid("b2222222-0000-0000-0000-000000000001"));

            migrationBuilder.DeleteData(
                table: "CheckoutRequests",
                keyColumn: "Id",
                keyValue: new Guid("b2222222-0000-0000-0000-000000000002"));

            migrationBuilder.DeleteData(
                table: "CheckoutRequests",
                keyColumn: "Id",
                keyValue: new Guid("b3333333-0000-0000-0000-000000000001"));

            migrationBuilder.DeleteData(
                table: "CheckoutRequests",
                keyColumn: "Id",
                keyValue: new Guid("b4444444-0000-0000-0000-000000000001"));

            migrationBuilder.DeleteData(
                table: "CheckoutRequests",
                keyColumn: "Id",
                keyValue: new Guid("b5555555-0000-0000-0000-000000000001"));

            migrationBuilder.DeleteData(
                table: "Assets",
                keyColumn: "Id",
                keyValue: new Guid("a1111111-0000-0000-0000-000000000001"));

            migrationBuilder.DeleteData(
                table: "Assets",
                keyColumn: "Id",
                keyValue: new Guid("a2222222-0000-0000-0000-000000000003"));
        }
    }
}
