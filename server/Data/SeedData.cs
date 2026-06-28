using AssetManagementSystem.Enums;
using AssetManagementSystem.Models.Entities;

namespace AssetManagementSystem.Data
{
    public static class SeedData
    {
        public static readonly Guid AdminUserId = new("11111111-1111-1111-1111-111111111111");
        public static readonly Guid ManagerUserId = new("22222222-2222-2222-2222-222222222222");
        public static readonly Guid EmployeeUserId = new("33333333-3333-3333-3333-333333333333");

        public static User[] Users { get; } =
        {
            new User
            {
                Id           = AdminUserId,
                EmailAddress = "admin@test.com",
                Role         = Role.Admin,
                IsActive     = true,
                LastLoginAt  = null,
                CreatedAt    = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                FirstName    = "Admin",
                LastName     = "User"
            },
            new User
            {
                Id           = ManagerUserId,
                EmailAddress = "manager@test.com",
                Role         = Role.AssetManager,
                IsActive     = true,
                LastLoginAt  = null,
                CreatedAt    = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                FirstName    = "Manager",
                LastName     = "User"
            },
            new User
            {
                Id           = EmployeeUserId,
                EmailAddress = "employee@test.com",
                Role         = Role.Employee,
                IsActive     = true,
                LastLoginAt  = null,
                CreatedAt    = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                FirstName    = "Employee",
                LastName     = "User"
            }
        };

        public static Asset[] Assets { get; } =
        {
            // ── Laptops ──────────────────────────────────────────────────────────
            new Asset  // Assigned → Employee (fulfilled checkout LAP-001)
            {
                Id               = new Guid("a1111111-0000-0000-0000-000000000001"),
                AssetTag         = "LAP-001",
                Name             = "Dell Latitude 5440",
                Category         = AssetCategory.Laptop,
                SerialNumber     = "DL5440-0001",
                Status           = AssetStatus.Assigned,
                Condition        = AssetCondition.Good,
                AssignedToUserId = EmployeeUserId,
                IsArchived       = false,
                CreatedAt        = new DateTime(2025, 1, 2, 0, 0, 0, DateTimeKind.Utc)
            },
            new Asset  // Available
            {
                Id               = new Guid("a1111111-0000-0000-0000-000000000002"),
                AssetTag         = "LAP-002",
                Name             = "Dell Latitude 5440",
                Category         = AssetCategory.Laptop,
                SerialNumber     = "DL5440-0002",
                Status           = AssetStatus.Available,
                Condition        = AssetCondition.New,
                AssignedToUserId = null,
                IsArchived       = false,
                CreatedAt        = new DateTime(2025, 1, 2, 0, 0, 0, DateTimeKind.Utc)
            },
            new Asset  // Maintenance
            {
                Id               = new Guid("a1111111-0000-0000-0000-000000000003"),
                AssetTag         = "LAP-003",
                Name             = "MacBook Pro 14\"",
                Category         = AssetCategory.Laptop,
                SerialNumber     = "MBP14-0003",
                Status           = AssetStatus.Maintenance,
                Condition        = AssetCondition.Damaged,
                AssignedToUserId = null,
                IsArchived       = false,
                CreatedAt        = new DateTime(2025, 1, 3, 0, 0, 0, DateTimeKind.Utc)
            },

            // ── Monitors ─────────────────────────────────────────────────────────
            new Asset  // Assigned → Manager (pending return MON-001)
            {
                Id               = new Guid("a2222222-0000-0000-0000-000000000001"),
                AssetTag         = "MON-001",
                Name             = "Dell UltraSharp 27\"",
                Category         = AssetCategory.Monitor,
                SerialNumber     = "DU27-0001",
                Status           = AssetStatus.Assigned,
                Condition        = AssetCondition.Good,
                AssignedToUserId = ManagerUserId,
                IsArchived       = false,
                CreatedAt        = new DateTime(2025, 1, 4, 0, 0, 0, DateTimeKind.Utc)
            },
            new Asset  // Assigned → Employee (approved return MON-002)
            {
                Id               = new Guid("a2222222-0000-0000-0000-000000000002"),
                AssetTag         = "MON-002",
                Name             = "Dell UltraSharp 27\"",
                Category         = AssetCategory.Monitor,
                SerialNumber     = "DU27-0002",
                Status           = AssetStatus.Assigned,
                Condition        = AssetCondition.Good,
                AssignedToUserId = EmployeeUserId,
                IsArchived       = false,
                CreatedAt        = new DateTime(2025, 1, 4, 0, 0, 0, DateTimeKind.Utc)
            },
            new Asset  // Retired + archived
            {
                Id               = new Guid("a2222222-0000-0000-0000-000000000003"),
                AssetTag         = "MON-003",
                Name             = "LG UltraWide 34\"",
                Category         = AssetCategory.Monitor,
                SerialNumber     = "LGUW34-0003",
                Status           = AssetStatus.Retired,
                Condition        = AssetCondition.Lost,
                AssignedToUserId = null,
                IsArchived       = true,
                CreatedAt        = new DateTime(2025, 1, 5, 0, 0, 0, DateTimeKind.Utc)
            },

            // ── Phones ───────────────────────────────────────────────────────────
            new Asset  // Available
            {
                Id               = new Guid("a3333333-0000-0000-0000-000000000001"),
                AssetTag         = "PHN-001",
                Name             = "iPhone 14",
                Category         = AssetCategory.Phone,
                SerialNumber     = "IP14-0001",
                Status           = AssetStatus.Available,
                Condition        = AssetCondition.Good,
                AssignedToUserId = null,
                IsArchived       = false,
                CreatedAt        = new DateTime(2025, 1, 6, 0, 0, 0, DateTimeKind.Utc)
            },
            new Asset  // Available
            {
                Id               = new Guid("a3333333-0000-0000-0000-000000000002"),
                AssetTag         = "PHN-002",
                Name             = "iPhone 14",
                Category         = AssetCategory.Phone,
                SerialNumber     = "IP14-0002",
                Status           = AssetStatus.Available,
                Condition        = AssetCondition.New,
                AssignedToUserId = null,
                IsArchived       = false,
                CreatedAt        = new DateTime(2025, 1, 6, 0, 0, 0, DateTimeKind.Utc)
            },

            // ── Security Keys ────────────────────────────────────────────────────
            new Asset  // Available
            {
                Id               = new Guid("a4444444-0000-0000-0000-000000000001"),
                AssetTag         = "KEY-001",
                Name             = "YubiKey 5C",
                Category         = AssetCategory.SecurityKey,
                SerialNumber     = "YK5C-0001",
                Status           = AssetStatus.Available,
                Condition        = AssetCondition.New,
                AssignedToUserId = null,
                IsArchived       = false,
                CreatedAt        = new DateTime(2025, 1, 7, 0, 0, 0, DateTimeKind.Utc)
            },
            new Asset  // Assigned → Employee (returned request KEY-002)
            {
                Id               = new Guid("a4444444-0000-0000-0000-000000000002"),
                AssetTag         = "KEY-002",
                Name             = "YubiKey 5C",
                Category         = AssetCategory.SecurityKey,
                SerialNumber     = "YK5C-0002",
                Status           = AssetStatus.Assigned,
                Condition        = AssetCondition.Good,
                AssignedToUserId = EmployeeUserId,
                IsArchived       = false,
                CreatedAt        = new DateTime(2025, 1, 7, 0, 0, 0, DateTimeKind.Utc)
            }
        };

        public static CheckoutRequest[] CheckoutRequests { get; } =
        {
            // ── Pending Checkout ──────────────────────────────────────────────────
            new CheckoutRequest  // Employee wants a backup laptop
            {
                Id                  = new Guid("b1111111-0000-0000-0000-000000000001"),
                RequestType         = CheckoutRequestType.Checkout,
                RequestedByUserId   = EmployeeUserId,
                AssetCategory       = AssetCategory.Laptop,
                Reason              = "Need a backup laptop for travel",
                Status              = CheckoutRequestStatus.Pending,
                ReviewedByUserId    = null,
                AssignedAssetId     = null,
                IsArchived          = false,
                ApprovedAt          = null,
                RejectedAt          = null,
                FulfilledAt         = null,
                ReturnedAt          = null,
                CreatedAt           = new DateTime(2025, 2, 1, 0, 0, 0, DateTimeKind.Utc),
                UpdatedAt           = new DateTime(2025, 2, 1, 0, 0, 0, DateTimeKind.Utc)
            },
            new CheckoutRequest  // Employee wants a replacement phone
            {
                Id                  = new Guid("b1111111-0000-0000-0000-000000000002"),
                RequestType         = CheckoutRequestType.Checkout,
                RequestedByUserId   = EmployeeUserId,
                AssetCategory       = AssetCategory.Phone,
                Reason              = "Existing phone screen is cracked",
                Status              = CheckoutRequestStatus.Pending,
                ReviewedByUserId    = null,
                AssignedAssetId     = null,
                IsArchived          = false,
                ApprovedAt          = null,
                RejectedAt          = null,
                FulfilledAt         = null,
                ReturnedAt          = null,
                CreatedAt           = new DateTime(2025, 2, 2, 0, 0, 0, DateTimeKind.Utc),
                UpdatedAt           = new DateTime(2025, 2, 2, 0, 0, 0, DateTimeKind.Utc)
            },
            new CheckoutRequest  // Employee wants a security key
            {
                Id                  = new Guid("b1111111-0000-0000-0000-000000000003"),
                RequestType         = CheckoutRequestType.Checkout,
                RequestedByUserId   = EmployeeUserId,
                AssetCategory       = AssetCategory.SecurityKey,
                Reason              = "MFA hardware key for new system rollout",
                Status              = CheckoutRequestStatus.Pending,
                ReviewedByUserId    = null,
                AssignedAssetId     = null,
                IsArchived          = false,
                ApprovedAt          = null,
                RejectedAt          = null,
                FulfilledAt         = null,
                ReturnedAt          = null,
                CreatedAt           = new DateTime(2025, 2, 3, 0, 0, 0, DateTimeKind.Utc),
                UpdatedAt           = new DateTime(2025, 2, 3, 0, 0, 0, DateTimeKind.Utc)
            },

            // ── Pending Return ────────────────────────────────────────────────────
            new CheckoutRequest  // Manager returning MON-001 (assigned to Manager)
            {
                Id                  = new Guid("b1111111-0000-0000-0000-000000000004"),
                RequestType         = CheckoutRequestType.Return,
                RequestedByUserId   = ManagerUserId,
                AssetCategory       = AssetCategory.Monitor,
                Reason              = "Returning second monitor, no longer needed",
                Status              = CheckoutRequestStatus.Pending,
                ReviewedByUserId    = null,
                AssignedAssetId     = new Guid("a2222222-0000-0000-0000-000000000001"), // MON-001 → ManagerUserId
                IsArchived          = false,
                ApprovedAt          = null,
                RejectedAt          = null,
                FulfilledAt         = null,
                ReturnedAt          = null,
                CreatedAt           = new DateTime(2025, 2, 4, 0, 0, 0, DateTimeKind.Utc),
                UpdatedAt           = new DateTime(2025, 2, 4, 0, 0, 0, DateTimeKind.Utc)
            },

            // ── Approved Checkout ─────────────────────────────────────────────────
            new CheckoutRequest  // Approved, asset not yet assigned
            {
                Id                  = new Guid("b2222222-0000-0000-0000-000000000001"),
                RequestType         = CheckoutRequestType.Checkout,
                RequestedByUserId   = EmployeeUserId,
                AssetCategory       = AssetCategory.Laptop,
                Reason              = "Laptop for new project assignment",
                Status              = CheckoutRequestStatus.Approved,
                ReviewedByUserId    = ManagerUserId,
                AssignedAssetId     = null,
                IsArchived          = false,
                ApprovedAt          = new DateTime(2025, 2, 5, 0, 0, 0, DateTimeKind.Utc),
                RejectedAt          = null,
                FulfilledAt         = null,
                ReturnedAt          = null,
                CreatedAt           = new DateTime(2025, 2, 3, 0, 0, 0, DateTimeKind.Utc),
                UpdatedAt           = new DateTime(2025, 2, 5, 0, 0, 0, DateTimeKind.Utc)
            },

            // ── Approved Return ───────────────────────────────────────────────────
            new CheckoutRequest  // Employee returning MON-002 (assigned to Employee)
            {
                Id                  = new Guid("b2222222-0000-0000-0000-000000000002"),
                RequestType         = CheckoutRequestType.Return,
                RequestedByUserId   = EmployeeUserId,
                AssetCategory       = AssetCategory.Monitor,
                Reason              = "Second monitor for home office, returning now",
                Status              = CheckoutRequestStatus.Approved,
                ReviewedByUserId    = ManagerUserId,
                AssignedAssetId     = new Guid("a2222222-0000-0000-0000-000000000002"), // MON-002 → EmployeeUserId
                IsArchived          = false,
                ApprovedAt          = new DateTime(2025, 2, 6, 0, 0, 0, DateTimeKind.Utc),
                RejectedAt          = null,
                FulfilledAt         = null,
                ReturnedAt          = null,
                CreatedAt           = new DateTime(2025, 2, 4, 0, 0, 0, DateTimeKind.Utc),
                UpdatedAt           = new DateTime(2025, 2, 6, 0, 0, 0, DateTimeKind.Utc)
            },

            // ── Rejected Checkout ─────────────────────────────────────────────────
            new CheckoutRequest
            {
                Id                  = new Guid("b3333333-0000-0000-0000-000000000001"),
                RequestType         = CheckoutRequestType.Checkout,
                RequestedByUserId   = EmployeeUserId,
                AssetCategory       = AssetCategory.Tablet,
                Reason              = "Would like a tablet for note-taking in meetings",
                Status              = CheckoutRequestStatus.Rejected,
                ReviewedByUserId    = ManagerUserId,
                AssignedAssetId     = null,
                IsArchived          = false,
                ApprovedAt          = null,
                RejectedAt          = new DateTime(2025, 2, 7, 0, 0, 0, DateTimeKind.Utc),
                FulfilledAt         = null,
                ReturnedAt          = null,
                CreatedAt           = new DateTime(2025, 2, 5, 0, 0, 0, DateTimeKind.Utc),
                UpdatedAt           = new DateTime(2025, 2, 7, 0, 0, 0, DateTimeKind.Utc)
            },

            // ── Rejected Return ───────────────────────────────────────────────────
            new CheckoutRequest  // Employee tried to return KEY-002, rejected
            {
                Id                  = new Guid("b3333333-0000-0000-0000-000000000002"),
                RequestType         = CheckoutRequestType.Return,
                RequestedByUserId   = EmployeeUserId,
                AssetCategory       = AssetCategory.SecurityKey,
                Reason              = "No longer assigned to this project",
                Status              = CheckoutRequestStatus.Rejected,
                ReviewedByUserId    = ManagerUserId,
                AssignedAssetId     = new Guid("a4444444-0000-0000-0000-000000000002"), // KEY-002 → EmployeeUserId
                IsArchived          = false,
                ApprovedAt          = null,
                RejectedAt          = new DateTime(2025, 2, 8, 0, 0, 0, DateTimeKind.Utc),
                FulfilledAt         = null,
                ReturnedAt          = null,
                CreatedAt           = new DateTime(2025, 2, 6, 0, 0, 0, DateTimeKind.Utc),
                UpdatedAt           = new DateTime(2025, 2, 8, 0, 0, 0, DateTimeKind.Utc)
            },

            // ── Fulfilled ─────────────────────────────────────────────────────────
            new CheckoutRequest  // Employee checked out LAP-001
            {
                Id                  = new Guid("b4444444-0000-0000-0000-000000000001"),
                RequestType         = CheckoutRequestType.Checkout,
                RequestedByUserId   = EmployeeUserId,
                AssetCategory       = AssetCategory.Laptop,
                Reason              = "Primary laptop for daily work",
                Status              = CheckoutRequestStatus.Fulfilled,
                ReviewedByUserId    = ManagerUserId,
                AssignedAssetId     = new Guid("a1111111-0000-0000-0000-000000000001"), // LAP-001 → EmployeeUserId
                IsArchived          = false,
                ApprovedAt          = new DateTime(2025, 1, 9, 0, 0, 0, DateTimeKind.Utc),
                RejectedAt          = null,
                FulfilledAt         = new DateTime(2025, 1, 10, 0, 0, 0, DateTimeKind.Utc),
                ReturnedAt          = null,
                CreatedAt           = new DateTime(2025, 1, 8, 0, 0, 0, DateTimeKind.Utc),
                UpdatedAt           = new DateTime(2025, 1, 10, 0, 0, 0, DateTimeKind.Utc)
            },

            // ── Returned ──────────────────────────────────────────────────────────
            new CheckoutRequest  // Employee returned a phone (historical, phone is back in pool)
            {
                Id                  = new Guid("b5555555-0000-0000-0000-000000000001"),
                RequestType         = CheckoutRequestType.Return,
                RequestedByUserId   = EmployeeUserId,
                AssetCategory       = AssetCategory.Phone,
                Reason              = "Temporary phone for project sprint, returning now",
                Status              = CheckoutRequestStatus.Returned,
                ReviewedByUserId    = ManagerUserId,
                AssignedAssetId     = new Guid("a3333333-0000-0000-0000-000000000001"), // PHN-001 (now Available again)
                IsArchived          = false,
                ApprovedAt          = null,
                RejectedAt          = null,
                FulfilledAt         = null,
                ReturnedAt          = new DateTime(2025, 1, 20, 0, 0, 0, DateTimeKind.Utc),
                CreatedAt           = new DateTime(2025, 1, 15, 0, 0, 0, DateTimeKind.Utc),
                UpdatedAt           = new DateTime(2025, 1, 20, 0, 0, 0, DateTimeKind.Utc)
            },

            // ── Cancelled Checkout ────────────────────────────────────────────────
            new CheckoutRequest
            {
                Id                  = new Guid("b6666666-0000-0000-0000-000000000001"),
                RequestType         = CheckoutRequestType.Checkout,
                RequestedByUserId   = EmployeeUserId,
                AssetCategory       = AssetCategory.Headset,
                Reason              = "Requested headset for calls, no longer needed",
                Status              = CheckoutRequestStatus.Cancelled,
                ReviewedByUserId    = null,
                AssignedAssetId     = null,
                IsArchived          = false,
                ApprovedAt          = null,
                RejectedAt          = null,
                FulfilledAt         = null,
                ReturnedAt          = null,
                CreatedAt           = new DateTime(2025, 2, 7, 0, 0, 0, DateTimeKind.Utc),
                UpdatedAt           = new DateTime(2025, 2, 9, 0, 0, 0, DateTimeKind.Utc)
            },

            // ── Cancelled Return ──────────────────────────────────────────────────
            new CheckoutRequest  // Manager changed mind about returning MON-001
            {
                Id                  = new Guid("b6666666-0000-0000-0000-000000000002"),
                RequestType         = CheckoutRequestType.Return,
                RequestedByUserId   = ManagerUserId,
                AssetCategory       = AssetCategory.Monitor,
                Reason              = "Requested to return monitor, changed mind",
                Status              = CheckoutRequestStatus.Cancelled,
                ReviewedByUserId    = null,
                AssignedAssetId     = new Guid("a2222222-0000-0000-0000-000000000001"), // MON-001 → ManagerUserId
                IsArchived          = false,
                ApprovedAt          = null,
                RejectedAt          = null,
                FulfilledAt         = null,
                ReturnedAt          = null,
                CreatedAt           = new DateTime(2025, 2, 8, 0, 0, 0, DateTimeKind.Utc),
                UpdatedAt           = new DateTime(2025, 2, 10, 0, 0, 0, DateTimeKind.Utc)
            }
        };
    }
}