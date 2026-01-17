using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SecureFlow.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class MenuEntity_IsActive_Added : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                schema: "auth",
                table: "Menus",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsActive",
                schema: "auth",
                table: "Menus");
        }
    }
}
