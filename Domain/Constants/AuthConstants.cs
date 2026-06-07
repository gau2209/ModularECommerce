using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Constants
{
    public static class AuthConstants
    {
        public static class Roles
        {
            public const string Admin = "Admin";
            public const string Manager = "Manager";
            public const string Staff = "Staff";
            public const string Customer = "Customer";
        }

        public static class Permissions
        {
            public const string ProductView = "Product.View";
            public const string ProductCreate = "Product.Create";
            public const string ProductUpdate = "Product.Update";
            public const string ProductDelete = "Product.Delete";

            public const string OrderView = "Order.View";
            public const string OrderCreate = "Order.Create";
            public const string OrderUpdateStatus = "Order.UpdateStatus";
            public const string OrderCancel = "Order.Cancel";
            public const string OrderApprove = "Order.Approve";

            public const string InventoryView = "Inventory.View";
            public const string InventoryImport = "Inventory.Import";
            public const string InventoryExport = "Inventory.Export";
            public const string InventoryAdjust = "Inventory.Adjust";

            public const string ReportViewRevenue = "Report.ViewRevenue";
            public const string ReportViewInventory = "Report.ViewInventory";

            public const string UserView = "User.View";
            public const string UserCreate = "User.Create";
            public const string UserUpdate = "User.Update";
            public const string UserAssignRole = "User.AssignRole";
        }
    }
}
