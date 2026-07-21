using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Rumos.Acd.DataDemo.Migrations
{
    /// <inheritdoc />
    public partial class RemovedNickName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Nickname",
                table: "Users");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Nickname",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
