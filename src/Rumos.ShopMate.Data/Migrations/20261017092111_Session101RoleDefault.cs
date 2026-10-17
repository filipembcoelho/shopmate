using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Rumos.ShopMate.Data.Migrations
{
    /// <inheritdoc />
    public partial class Session101RoleDefault : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Role",
                table: "Accounts",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "User",
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20);

            // Repair only the blank roles introduced by the earlier migration.
            // Existing User/Admin assignments and every account ID stay intact.
            migrationBuilder.Sql("UPDATE [Accounts] SET [Role] = N'User' WHERE [Role] = N'';");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Role",
                table: "Accounts",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20,
                oldDefaultValue: "User");
        }
    }
}
