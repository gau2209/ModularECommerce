using Domain.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Seed
{
    public static class AuthSeedData
    {
        public static readonly Guid AdminRoleID = Guid.Parse("11111111-1111-1111-1111-111111111111");
        public static readonly Guid ManagerRoleID = Guid.Parse("22222222-2222-2222-2222-222222222222");
        public static readonly Guid StaffRoleID = Guid.Parse("33333333-3333-3333-3333-333333333333");
        public static readonly Guid CustomerRoleID = Guid.Parse("44444444-4444-4444-4444-444444444444");

        public static readonly DateTime SeedCreatedAt = new(2026, 1, 1, 0, 0, 0, DateTimeKind.Local);
        public const string SeedCreatedBy = "SYSTEM";

        public static readonly IReadOnlyList<object> Roles =
        [
            new
        {
            ID = AdminRoleID,
            Name = AuthConstants.Roles.Admin,
            Description = "System administrator with full access.",
            IsSystemRole = true,
            CreatedAt = SeedCreatedAt,
            CreatedBy = SeedCreatedBy,
            IsDeleted = false
        },
        new
        {
            ID = ManagerRoleID,
            Name = AuthConstants.Roles.Manager,
            Description = "Manager role for product, order, inventory and report operations.",
            IsSystemRole = true,
            CreatedAt = SeedCreatedAt,
            CreatedBy = SeedCreatedBy,
            IsDeleted = false
        },
        new
        {
            ID = StaffRoleID,
            Name = AuthConstants.Roles.Staff,
            Description = "Staff role for basic back-office operations.",
            IsSystemRole = true,
            CreatedAt = SeedCreatedAt,
            CreatedBy = SeedCreatedBy,
            IsDeleted = false
        },
        new
        {
            ID = CustomerRoleID,
            Name = AuthConstants.Roles.Customer,
            Description = "Default customer role.",
            IsSystemRole = true,
            CreatedAt = SeedCreatedAt,
            CreatedBy = SeedCreatedBy,
            IsDeleted = false
        }
        ];

        public static readonly IReadOnlyList<PermissionSeedItem> Permissions =
        [
        new(Guid.Parse("10000000-0000-0000-0000-000000000001"), AuthConstants.Permissions.ProductView, "Product"),
        new(Guid.Parse("10000000-0000-0000-0000-000000000002"), AuthConstants.Permissions.ProductCreate, "Product"),
        new(Guid.Parse("10000000-0000-0000-0000-000000000003"), AuthConstants.Permissions.ProductUpdate, "Product"),
        new(Guid.Parse("10000000-0000-0000-0000-000000000004"), AuthConstants.Permissions.ProductDelete, "Product"),

        new(Guid.Parse("20000000-0000-0000-0000-000000000001"), AuthConstants.Permissions.OrderView, "Order"),
        new(Guid.Parse("20000000-0000-0000-0000-000000000002"), AuthConstants.Permissions.OrderCreate, "Order"),
        new(Guid.Parse("20000000-0000-0000-0000-000000000003"), AuthConstants.Permissions.OrderUpdateStatus, "Order"),
        new(Guid.Parse("20000000-0000-0000-0000-000000000004"), AuthConstants.Permissions.OrderCancel, "Order"),
        new(Guid.Parse("20000000-0000-0000-0000-000000000005"), AuthConstants.Permissions.OrderApprove, "Order"),

        new(Guid.Parse("30000000-0000-0000-0000-000000000001"), AuthConstants.Permissions.InventoryView, "Inventory"),
        new(Guid.Parse("30000000-0000-0000-0000-000000000002"), AuthConstants.Permissions.InventoryImport, "Inventory"),
        new(Guid.Parse("30000000-0000-0000-0000-000000000003"), AuthConstants.Permissions.InventoryExport, "Inventory"),
        new(Guid.Parse("30000000-0000-0000-0000-000000000004"), AuthConstants.Permissions.InventoryAdjust, "Inventory"),

        new(Guid.Parse("40000000-0000-0000-0000-000000000001"), AuthConstants.Permissions.ReportViewRevenue, "Report"),
        new(Guid.Parse("40000000-0000-0000-0000-000000000002"), AuthConstants.Permissions.ReportViewInventory, "Report"),

        new(Guid.Parse("50000000-0000-0000-0000-000000000001"), AuthConstants.Permissions.UserView, "User"),
        new(Guid.Parse("50000000-0000-0000-0000-000000000002"), AuthConstants.Permissions.UserCreate, "User"),
        new(Guid.Parse("50000000-0000-0000-0000-000000000003"), AuthConstants.Permissions.UserUpdate, "User"),
        new(Guid.Parse("50000000-0000-0000-0000-000000000004"), AuthConstants.Permissions.UserAssignRole, "User")
        ];

        public static IEnumerable<object> GetPermissionSeedData ()
        {
            return Permissions.Select(x => new
            {
                ID = x.ID,
                Name = x.Name,
                GroupName = x.GroupName,
                Description = $"{x.Name} permission.",
                CreatedAt = SeedCreatedAt,
                CreatedBy = SeedCreatedBy,
                IsDeleted = false
            });
        }

        public static IEnumerable<object> GetRolePermissionSeedData ()
        {
            var adminPermissions = Permissions.Select(x => new
            {
                RoleID = AdminRoleID,
                PermissionID = x.ID
            });

            var managerPermissions = Permissions
                .Where(x =>
                    x.GroupName is "Product" or "Order" or "Inventory" or "Report")
                .Select(x => new
                {
                    RoleID = ManagerRoleID,
                    PermissionID = x.ID
                });

            var staffPermissions = Permissions
                .Where(x =>
                    x.Name == AuthConstants.Permissions.ProductView ||
                    x.Name == AuthConstants.Permissions.OrderView ||
                    x.Name == AuthConstants.Permissions.InventoryView)
                .Select(x => new
                {
                    RoleID = StaffRoleID,
                    PermissionID = x.ID
                });

            var customerPermissions = Permissions
                .Where(x =>
                    x.Name == AuthConstants.Permissions.OrderCreate ||
                    x.Name == AuthConstants.Permissions.OrderView ||
                    x.Name == AuthConstants.Permissions.OrderCancel)
                .Select(x => new
                {
                    RoleID = CustomerRoleID,
                    PermissionID = x.ID
                });

            return adminPermissions
                .Concat(managerPermissions)
                .Concat(staffPermissions)
                .Concat(customerPermissions);
        }
    }

    public sealed record PermissionSeedItem (Guid ID, string Name, string GroupName);
}
