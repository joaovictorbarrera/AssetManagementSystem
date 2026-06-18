using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ThreatlockerAssetManagementSystem.Migrations
{
    /// <inheritdoc />
    public partial class CheckoutRequestNewFieldsNewData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ActionedAt",
                table: "CheckoutRequests",
                newName: "ReturnedAt");

            migrationBuilder.AddColumn<DateTime>(
                name: "ApprovedAt",
                table: "CheckoutRequests",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "FulfilledAt",
                table: "CheckoutRequests",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsArchived",
                table: "CheckoutRequests",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "RejectedAt",
                table: "CheckoutRequests",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RequestType",
                table: "CheckoutRequests",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "Assets",
                keyColumn: "Id",
                keyValue: new Guid("a3333333-0000-0000-0000-000000000002"),
                column: "Status",
                value: "Available");

            migrationBuilder.UpdateData(
                table: "Assets",
                keyColumn: "Id",
                keyValue: new Guid("a4444444-0000-0000-0000-000000000002"),
                column: "IsArchived",
                value: true);

            migrationBuilder.UpdateData(
                table: "CheckoutRequests",
                keyColumn: "Id",
                keyValue: new Guid("b1111111-0000-0000-0000-000000000001"),
                columns: new[] { "ApprovedAt", "FulfilledAt", "IsArchived", "RejectedAt", "RequestType", "UpdatedAt" },
                values: new object[] { null, null, false, null, 0, new DateTime(2025, 2, 1, 0, 0, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.UpdateData(
                table: "CheckoutRequests",
                keyColumn: "Id",
                keyValue: new Guid("b1111111-0000-0000-0000-000000000002"),
                columns: new[] { "ApprovedAt", "FulfilledAt", "IsArchived", "RejectedAt", "RequestType", "UpdatedAt" },
                values: new object[] { null, null, false, null, 0, new DateTime(2025, 2, 2, 0, 0, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.UpdateData(
                table: "CheckoutRequests",
                keyColumn: "Id",
                keyValue: new Guid("b1111111-0000-0000-0000-000000000003"),
                columns: new[] { "ApprovedAt", "AssignedAssetId", "FulfilledAt", "IsArchived", "Reason", "RejectedAt", "RequestType", "UpdatedAt" },
                values: new object[] { null, new Guid("a2222222-0000-0000-0000-000000000001"), null, false, "Returning second monitor, no longer needed", null, 1, new DateTime(2025, 2, 3, 0, 0, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.UpdateData(
                table: "CheckoutRequests",
                keyColumn: "Id",
                keyValue: new Guid("b2222222-0000-0000-0000-000000000001"),
                columns: new[] { "ApprovedAt", "FulfilledAt", "IsArchived", "RejectedAt", "RequestType", "ReturnedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 2, 4, 0, 0, 0, 0, DateTimeKind.Utc), null, false, null, 0, null, new DateTime(2025, 2, 4, 0, 0, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.UpdateData(
                table: "CheckoutRequests",
                keyColumn: "Id",
                keyValue: new Guid("b2222222-0000-0000-0000-000000000002"),
                columns: new[] { "ApprovedAt", "FulfilledAt", "IsArchived", "Reason", "RejectedAt", "RequestType", "RequestedByUserId", "ReturnedAt", "ReviewedByUserId", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 2, 5, 0, 0, 0, 0, DateTimeKind.Utc), null, true, "Second monitor requested for home office", null, 0, new Guid("33333333-3333-3333-3333-333333333333"), null, new Guid("22222222-2222-2222-2222-222222222222"), new DateTime(2025, 2, 5, 0, 0, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.UpdateData(
                table: "CheckoutRequests",
                keyColumn: "Id",
                keyValue: new Guid("b3333333-0000-0000-0000-000000000001"),
                columns: new[] { "ApprovedAt", "FulfilledAt", "IsArchived", "RejectedAt", "RequestType", "ReturnedAt", "UpdatedAt" },
                values: new object[] { null, null, false, new DateTime(2025, 2, 6, 0, 0, 0, 0, DateTimeKind.Utc), 0, null, new DateTime(2025, 2, 6, 0, 0, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.UpdateData(
                table: "CheckoutRequests",
                keyColumn: "Id",
                keyValue: new Guid("b4444444-0000-0000-0000-000000000001"),
                columns: new[] { "ApprovedAt", "FulfilledAt", "IsArchived", "RejectedAt", "RequestType", "ReturnedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 1, 9, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 1, 10, 0, 0, 0, 0, DateTimeKind.Utc), false, null, 0, null, new DateTime(2025, 1, 10, 0, 0, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.UpdateData(
                table: "CheckoutRequests",
                keyColumn: "Id",
                keyValue: new Guid("b5555555-0000-0000-0000-000000000001"),
                columns: new[] { "ApprovedAt", "FulfilledAt", "IsArchived", "Reason", "RejectedAt", "RequestType", "UpdatedAt" },
                values: new object[] { null, null, false, "Temporary monitor for project sprint, returning now", null, 1, new DateTime(2025, 1, 20, 0, 0, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.InsertData(
                table: "CheckoutRequests",
                columns: new[] { "Id", "ApprovedAt", "AssetCategory", "AssignedAssetId", "CreatedAt", "FulfilledAt", "IsArchived", "Reason", "RejectedAt", "RequestType", "RequestedByUserId", "ReturnedAt", "ReviewedByUserId", "Status", "UpdatedAt" },
                values: new object[,]
                {
                    { new Guid("b6666666-0000-0000-0000-000000000001"), null, "SecurityKey", new Guid("a4444444-0000-0000-0000-000000000001"), new DateTime(2025, 2, 5, 0, 0, 0, 0, DateTimeKind.Utc), null, false, "Requesting to return security key, no longer assigned to this project", new DateTime(2025, 2, 7, 0, 0, 0, 0, DateTimeKind.Utc), 1, new Guid("33333333-3333-3333-3333-333333333333"), null, new Guid("22222222-2222-2222-2222-222222222222"), "Rejected", new DateTime(2025, 2, 7, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("b7777777-0000-0000-0000-000000000001"), null, "Monitor", new Guid("a2222222-0000-0000-0000-000000000002"), new DateTime(2025, 2, 6, 0, 0, 0, 0, DateTimeKind.Utc), null, false, "Requested to return monitor, changed mind", null, 1, new Guid("22222222-2222-2222-2222-222222222222"), null, null, "Cancelled", new DateTime(2025, 2, 8, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("b8888888-0000-0000-0000-000000000001"), null, "Headset", null, new DateTime(2025, 2, 7, 0, 0, 0, 0, DateTimeKind.Utc), null, false, "Requested headset for calls, no longer needed", null, 0, new Guid("33333333-3333-3333-3333-333333333333"), null, null, "Cancelled", new DateTime(2025, 2, 9, 0, 0, 0, 0, DateTimeKind.Utc) }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "CheckoutRequests",
                keyColumn: "Id",
                keyValue: new Guid("b6666666-0000-0000-0000-000000000001"));

            migrationBuilder.DeleteData(
                table: "CheckoutRequests",
                keyColumn: "Id",
                keyValue: new Guid("b7777777-0000-0000-0000-000000000001"));

            migrationBuilder.DeleteData(
                table: "CheckoutRequests",
                keyColumn: "Id",
                keyValue: new Guid("b8888888-0000-0000-0000-000000000001"));

            migrationBuilder.DropColumn(
                name: "ApprovedAt",
                table: "CheckoutRequests");

            migrationBuilder.DropColumn(
                name: "FulfilledAt",
                table: "CheckoutRequests");

            migrationBuilder.DropColumn(
                name: "IsArchived",
                table: "CheckoutRequests");

            migrationBuilder.DropColumn(
                name: "RejectedAt",
                table: "CheckoutRequests");

            migrationBuilder.DropColumn(
                name: "RequestType",
                table: "CheckoutRequests");

            migrationBuilder.RenameColumn(
                name: "ReturnedAt",
                table: "CheckoutRequests",
                newName: "ActionedAt");

            migrationBuilder.UpdateData(
                table: "Assets",
                keyColumn: "Id",
                keyValue: new Guid("a3333333-0000-0000-0000-000000000002"),
                column: "Status",
                value: "Requested");

            migrationBuilder.UpdateData(
                table: "Assets",
                keyColumn: "Id",
                keyValue: new Guid("a4444444-0000-0000-0000-000000000002"),
                column: "IsArchived",
                value: false);

            migrationBuilder.UpdateData(
                table: "CheckoutRequests",
                keyColumn: "Id",
                keyValue: new Guid("b1111111-0000-0000-0000-000000000001"),
                column: "UpdatedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "CheckoutRequests",
                keyColumn: "Id",
                keyValue: new Guid("b1111111-0000-0000-0000-000000000002"),
                column: "UpdatedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "CheckoutRequests",
                keyColumn: "Id",
                keyValue: new Guid("b1111111-0000-0000-0000-000000000003"),
                columns: new[] { "AssignedAssetId", "Reason", "UpdatedAt" },
                values: new object[] { null, "Setting up a second monitor for new hire", null });

            migrationBuilder.UpdateData(
                table: "CheckoutRequests",
                keyColumn: "Id",
                keyValue: new Guid("b2222222-0000-0000-0000-000000000001"),
                columns: new[] { "ActionedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 2, 4, 0, 0, 0, 0, DateTimeKind.Utc), null });

            migrationBuilder.UpdateData(
                table: "CheckoutRequests",
                keyColumn: "Id",
                keyValue: new Guid("b2222222-0000-0000-0000-000000000002"),
                columns: new[] { "ActionedAt", "Reason", "RequestedByUserId", "ReviewedByUserId", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 2, 5, 0, 0, 0, 0, DateTimeKind.Utc), "Second monitor approved for home office setup", new Guid("22222222-2222-2222-2222-222222222222"), new Guid("11111111-1111-1111-1111-111111111111"), null });

            migrationBuilder.UpdateData(
                table: "CheckoutRequests",
                keyColumn: "Id",
                keyValue: new Guid("b3333333-0000-0000-0000-000000000001"),
                columns: new[] { "ActionedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 2, 6, 0, 0, 0, 0, DateTimeKind.Utc), null });

            migrationBuilder.UpdateData(
                table: "CheckoutRequests",
                keyColumn: "Id",
                keyValue: new Guid("b4444444-0000-0000-0000-000000000001"),
                columns: new[] { "ActionedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 1, 10, 0, 0, 0, 0, DateTimeKind.Utc), null });

            migrationBuilder.UpdateData(
                table: "CheckoutRequests",
                keyColumn: "Id",
                keyValue: new Guid("b5555555-0000-0000-0000-000000000001"),
                columns: new[] { "Reason", "UpdatedAt" },
                values: new object[] { "Temporary monitor for project sprint", null });
        }
    }
}
