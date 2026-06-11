using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Persistence.Migrations
{
    /// <inheritdoc />
    public partial class InitCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWSEQUENTIALID()"),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Slug = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    ParentID = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Categories_Categories_ParentID",
                        column: x => x.ParentID,
                        principalTable: "Categories",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Permissions",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWSEQUENTIALID()"),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    GroupName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Permissions", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWSEQUENTIALID()"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    IsSystemRole = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWSEQUENTIALID()"),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    FullName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "RolePermissions",
                columns: table => new
                {
                    RoleID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PermissionID = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RolePermissions", x => new { x.RoleID, x.PermissionID });
                    table.ForeignKey(
                        name: "FK_RolePermissions_Permissions_PermissionID",
                        column: x => x.PermissionID,
                        principalTable: "Permissions",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RolePermissions_Roles_RoleID",
                        column: x => x.RoleID,
                        principalTable: "Roles",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RefreshTokens",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWSEQUENTIALID()"),
                    UserID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Token = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    ExpiresAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RevokedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedByIp = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    RevokedByIp = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ReplacedByToken = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    ReasonRevoked = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RefreshTokens", x => x.ID);
                    table.ForeignKey(
                        name: "FK_RefreshTokens_Users_UserID",
                        column: x => x.UserID,
                        principalTable: "Users",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserRoles",
                columns: table => new
                {
                    UserID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RoleID = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRoles", x => new { x.UserID, x.RoleID });
                    table.ForeignKey(
                        name: "FK_UserRoles_Roles_RoleID",
                        column: x => x.RoleID,
                        principalTable: "Roles",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserRoles_Users_UserID",
                        column: x => x.UserID,
                        principalTable: "Users",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Permissions",
                columns: new[] { "ID", "CreatedAt", "CreatedBy", "DeletedAt", "Description", "GroupName", "Name", "UpdatedAt", "UpdatedBy" },
                values: new object[,]
                {
                    { new Guid("10000000-0000-0000-0000-000000000001"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Local), "SYSTEM", null, "Product.View permission.", "Product", "Product.View", null, null },
                    { new Guid("10000000-0000-0000-0000-000000000002"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Local), "SYSTEM", null, "Product.Create permission.", "Product", "Product.Create", null, null },
                    { new Guid("10000000-0000-0000-0000-000000000003"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Local), "SYSTEM", null, "Product.Update permission.", "Product", "Product.Update", null, null },
                    { new Guid("10000000-0000-0000-0000-000000000004"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Local), "SYSTEM", null, "Product.Delete permission.", "Product", "Product.Delete", null, null },
                    { new Guid("20000000-0000-0000-0000-000000000001"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Local), "SYSTEM", null, "Order.View permission.", "Order", "Order.View", null, null },
                    { new Guid("20000000-0000-0000-0000-000000000002"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Local), "SYSTEM", null, "Order.Create permission.", "Order", "Order.Create", null, null },
                    { new Guid("20000000-0000-0000-0000-000000000003"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Local), "SYSTEM", null, "Order.UpdateStatus permission.", "Order", "Order.UpdateStatus", null, null },
                    { new Guid("20000000-0000-0000-0000-000000000004"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Local), "SYSTEM", null, "Order.Cancel permission.", "Order", "Order.Cancel", null, null },
                    { new Guid("20000000-0000-0000-0000-000000000005"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Local), "SYSTEM", null, "Order.Approve permission.", "Order", "Order.Approve", null, null },
                    { new Guid("30000000-0000-0000-0000-000000000001"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Local), "SYSTEM", null, "Inventory.View permission.", "Inventory", "Inventory.View", null, null },
                    { new Guid("30000000-0000-0000-0000-000000000002"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Local), "SYSTEM", null, "Inventory.Import permission.", "Inventory", "Inventory.Import", null, null },
                    { new Guid("30000000-0000-0000-0000-000000000003"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Local), "SYSTEM", null, "Inventory.Export permission.", "Inventory", "Inventory.Export", null, null },
                    { new Guid("30000000-0000-0000-0000-000000000004"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Local), "SYSTEM", null, "Inventory.Adjust permission.", "Inventory", "Inventory.Adjust", null, null },
                    { new Guid("40000000-0000-0000-0000-000000000001"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Local), "SYSTEM", null, "Report.ViewRevenue permission.", "Report", "Report.ViewRevenue", null, null },
                    { new Guid("40000000-0000-0000-0000-000000000002"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Local), "SYSTEM", null, "Report.ViewInventory permission.", "Report", "Report.ViewInventory", null, null },
                    { new Guid("50000000-0000-0000-0000-000000000001"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Local), "SYSTEM", null, "User.View permission.", "User", "User.View", null, null },
                    { new Guid("50000000-0000-0000-0000-000000000002"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Local), "SYSTEM", null, "User.Create permission.", "User", "User.Create", null, null },
                    { new Guid("50000000-0000-0000-0000-000000000003"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Local), "SYSTEM", null, "User.Update permission.", "User", "User.Update", null, null },
                    { new Guid("50000000-0000-0000-0000-000000000004"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Local), "SYSTEM", null, "User.AssignRole permission.", "User", "User.AssignRole", null, null }
                });

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "ID", "CreatedAt", "CreatedBy", "DeletedAt", "Description", "IsSystemRole", "Name", "UpdatedAt", "UpdatedBy" },
                values: new object[,]
                {
                    { new Guid("11111111-1111-1111-1111-111111111111"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Local), "SYSTEM", null, "System administrator with full access.", true, "Admin", null, null },
                    { new Guid("22222222-2222-2222-2222-222222222222"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Local), "SYSTEM", null, "Manager role for product, order, inventory and report operations.", true, "Manager", null, null },
                    { new Guid("33333333-3333-3333-3333-333333333333"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Local), "SYSTEM", null, "Staff role for basic back-office operations.", true, "Staff", null, null },
                    { new Guid("44444444-4444-4444-4444-444444444444"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Local), "SYSTEM", null, "Default customer role.", true, "Customer", null, null }
                });

            migrationBuilder.InsertData(
                table: "RolePermissions",
                columns: new[] { "PermissionID", "RoleID" },
                values: new object[,]
                {
                    { new Guid("10000000-0000-0000-0000-000000000001"), new Guid("11111111-1111-1111-1111-111111111111") },
                    { new Guid("10000000-0000-0000-0000-000000000002"), new Guid("11111111-1111-1111-1111-111111111111") },
                    { new Guid("10000000-0000-0000-0000-000000000003"), new Guid("11111111-1111-1111-1111-111111111111") },
                    { new Guid("10000000-0000-0000-0000-000000000004"), new Guid("11111111-1111-1111-1111-111111111111") },
                    { new Guid("20000000-0000-0000-0000-000000000001"), new Guid("11111111-1111-1111-1111-111111111111") },
                    { new Guid("20000000-0000-0000-0000-000000000002"), new Guid("11111111-1111-1111-1111-111111111111") },
                    { new Guid("20000000-0000-0000-0000-000000000003"), new Guid("11111111-1111-1111-1111-111111111111") },
                    { new Guid("20000000-0000-0000-0000-000000000004"), new Guid("11111111-1111-1111-1111-111111111111") },
                    { new Guid("20000000-0000-0000-0000-000000000005"), new Guid("11111111-1111-1111-1111-111111111111") },
                    { new Guid("30000000-0000-0000-0000-000000000001"), new Guid("11111111-1111-1111-1111-111111111111") },
                    { new Guid("30000000-0000-0000-0000-000000000002"), new Guid("11111111-1111-1111-1111-111111111111") },
                    { new Guid("30000000-0000-0000-0000-000000000003"), new Guid("11111111-1111-1111-1111-111111111111") },
                    { new Guid("30000000-0000-0000-0000-000000000004"), new Guid("11111111-1111-1111-1111-111111111111") },
                    { new Guid("40000000-0000-0000-0000-000000000001"), new Guid("11111111-1111-1111-1111-111111111111") },
                    { new Guid("40000000-0000-0000-0000-000000000002"), new Guid("11111111-1111-1111-1111-111111111111") },
                    { new Guid("50000000-0000-0000-0000-000000000001"), new Guid("11111111-1111-1111-1111-111111111111") },
                    { new Guid("50000000-0000-0000-0000-000000000002"), new Guid("11111111-1111-1111-1111-111111111111") },
                    { new Guid("50000000-0000-0000-0000-000000000003"), new Guid("11111111-1111-1111-1111-111111111111") },
                    { new Guid("50000000-0000-0000-0000-000000000004"), new Guid("11111111-1111-1111-1111-111111111111") },
                    { new Guid("10000000-0000-0000-0000-000000000001"), new Guid("22222222-2222-2222-2222-222222222222") },
                    { new Guid("10000000-0000-0000-0000-000000000002"), new Guid("22222222-2222-2222-2222-222222222222") },
                    { new Guid("10000000-0000-0000-0000-000000000003"), new Guid("22222222-2222-2222-2222-222222222222") },
                    { new Guid("10000000-0000-0000-0000-000000000004"), new Guid("22222222-2222-2222-2222-222222222222") },
                    { new Guid("20000000-0000-0000-0000-000000000001"), new Guid("22222222-2222-2222-2222-222222222222") },
                    { new Guid("20000000-0000-0000-0000-000000000002"), new Guid("22222222-2222-2222-2222-222222222222") },
                    { new Guid("20000000-0000-0000-0000-000000000003"), new Guid("22222222-2222-2222-2222-222222222222") },
                    { new Guid("20000000-0000-0000-0000-000000000004"), new Guid("22222222-2222-2222-2222-222222222222") },
                    { new Guid("20000000-0000-0000-0000-000000000005"), new Guid("22222222-2222-2222-2222-222222222222") },
                    { new Guid("30000000-0000-0000-0000-000000000001"), new Guid("22222222-2222-2222-2222-222222222222") },
                    { new Guid("30000000-0000-0000-0000-000000000002"), new Guid("22222222-2222-2222-2222-222222222222") },
                    { new Guid("30000000-0000-0000-0000-000000000003"), new Guid("22222222-2222-2222-2222-222222222222") },
                    { new Guid("30000000-0000-0000-0000-000000000004"), new Guid("22222222-2222-2222-2222-222222222222") },
                    { new Guid("40000000-0000-0000-0000-000000000001"), new Guid("22222222-2222-2222-2222-222222222222") },
                    { new Guid("40000000-0000-0000-0000-000000000002"), new Guid("22222222-2222-2222-2222-222222222222") },
                    { new Guid("10000000-0000-0000-0000-000000000001"), new Guid("33333333-3333-3333-3333-333333333333") },
                    { new Guid("20000000-0000-0000-0000-000000000001"), new Guid("33333333-3333-3333-3333-333333333333") },
                    { new Guid("30000000-0000-0000-0000-000000000001"), new Guid("33333333-3333-3333-3333-333333333333") },
                    { new Guid("20000000-0000-0000-0000-000000000001"), new Guid("44444444-4444-4444-4444-444444444444") },
                    { new Guid("20000000-0000-0000-0000-000000000002"), new Guid("44444444-4444-4444-4444-444444444444") },
                    { new Guid("20000000-0000-0000-0000-000000000004"), new Guid("44444444-4444-4444-4444-444444444444") }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Categories_Name",
                table: "Categories",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_Categories_ParentID",
                table: "Categories",
                column: "ParentID");

            migrationBuilder.CreateIndex(
                name: "IX_Categories_Slug",
                table: "Categories",
                column: "Slug",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Permissions_GroupName",
                table: "Permissions",
                column: "GroupName");

            migrationBuilder.CreateIndex(
                name: "IX_Permissions_Name",
                table: "Permissions",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RefreshTokens_Token",
                table: "RefreshTokens",
                column: "Token",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RefreshTokens_UserID",
                table: "RefreshTokens",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_RolePermissions_PermissionID",
                table: "RolePermissions",
                column: "PermissionID");

            migrationBuilder.CreateIndex(
                name: "IX_Roles_Name",
                table: "Roles",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserRoles_RoleID",
                table: "UserRoles",
                column: "RoleID");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                table: "Users",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_UserName",
                table: "Users",
                column: "UserName",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropTable(
                name: "RefreshTokens");

            migrationBuilder.DropTable(
                name: "RolePermissions");

            migrationBuilder.DropTable(
                name: "UserRoles");

            migrationBuilder.DropTable(
                name: "Permissions");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
