using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace SecureFlow.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Update_User_Role_UserRole_Schema_Properties : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserRoles_Users_UserId",
                table: "UserRoles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Users",
                table: "Users");

            migrationBuilder.EnsureSchema(
                name: "business");

            migrationBuilder.EnsureSchema(
                name: "auth");

            migrationBuilder.RenameTable(
                name: "Users",
                newName: "Users",
                newSchema: "auth");

            migrationBuilder.RenameTable(
                name: "UserRoles",
                newName: "UserRoles",
                newSchema: "auth");

            migrationBuilder.RenameTable(
                name: "Roles",
                newName: "Roles",
                newSchema: "auth");

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                schema: "auth",
                table: "Users",
                type: "integer",
                nullable: false,
                defaultValue: 0)
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddColumn<int>(
                name: "CreatedBy",
                schema: "auth",
                table: "Users",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedOn",
                schema: "auth",
                table: "Users",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "DeletedBy",
                schema: "auth",
                table: "Users",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedOn",
                schema: "auth",
                table: "Users",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                schema: "auth",
                table: "Users",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "LastModifiedBy",
                schema: "auth",
                table: "Users",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModifiedOn",
                schema: "auth",
                table: "Users",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastName",
                schema: "auth",
                table: "Users",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ProImgEvidenceId",
                schema: "auth",
                table: "Users",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UserType",
                schema: "auth",
                table: "Users",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "RoleId",
                schema: "auth",
                table: "UserRoles",
                type: "integer",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                schema: "auth",
                table: "UserRoles",
                type: "integer",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AddColumn<int>(
                name: "CreatedBy",
                schema: "auth",
                table: "UserRoles",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedOn",
                schema: "auth",
                table: "UserRoles",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "DeletedBy",
                schema: "auth",
                table: "UserRoles",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedOn",
                schema: "auth",
                table: "UserRoles",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Id",
                schema: "auth",
                table: "UserRoles",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "LastModifiedBy",
                schema: "auth",
                table: "UserRoles",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModifiedOn",
                schema: "auth",
                table: "UserRoles",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                schema: "auth",
                table: "Roles",
                type: "integer",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddColumn<int>(
                name: "CreatedBy",
                schema: "auth",
                table: "Roles",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedOn",
                schema: "auth",
                table: "Roles",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "DeletedBy",
                schema: "auth",
                table: "Roles",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedOn",
                schema: "auth",
                table: "Roles",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "LastModifiedBy",
                schema: "auth",
                table: "Roles",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModifiedOn",
                schema: "auth",
                table: "Roles",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Users",
                schema: "auth",
                table: "Users",
                column: "UserId");

            migrationBuilder.CreateTable(
                name: "Evidence",
                schema: "business",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    FileName = table.Column<string>(type: "text", nullable: false),
                    FileExtension = table.Column<string>(type: "text", nullable: false),
                    FileSizeInBytes = table.Column<long>(type: "bigint", nullable: false),
                    ContentType = table.Column<string>(type: "text", nullable: false),
                    StorageKey = table.Column<string>(type: "text", nullable: false),
                    StorageBucket = table.Column<string>(type: "text", nullable: false),
                    FileUrl = table.Column<string>(type: "text", nullable: true),
                    UploadedByUserId = table.Column<int>(type: "integer", nullable: false),
                    RelatedEntityType = table.Column<string>(type: "text", nullable: true),
                    RelatedEntityId = table.Column<int>(type: "integer", nullable: true),
                    Description = table.Column<string>(type: "text", nullable: true),
                    CreatedBy = table.Column<int>(type: "integer", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LastModifiedBy = table.Column<int>(type: "integer", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeletedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeletedBy = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Evidence", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Users_Id",
                schema: "auth",
                table: "Users",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_ProImgEvidenceId",
                schema: "auth",
                table: "Users",
                column: "ProImgEvidenceId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserRoles_Users_UserId",
                schema: "auth",
                table: "UserRoles",
                column: "UserId",
                principalSchema: "auth",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Evidence_ProImgEvidenceId",
                schema: "auth",
                table: "Users",
                column: "ProImgEvidenceId",
                principalSchema: "business",
                principalTable: "Evidence",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserRoles_Users_UserId",
                schema: "auth",
                table: "UserRoles");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_Evidence_ProImgEvidenceId",
                schema: "auth",
                table: "Users");

            migrationBuilder.DropTable(
                name: "Evidence",
                schema: "business");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Users",
                schema: "auth",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_Id",
                schema: "auth",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_ProImgEvidenceId",
                schema: "auth",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "UserId",
                schema: "auth",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                schema: "auth",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "CreatedOn",
                schema: "auth",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                schema: "auth",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "DeletedOn",
                schema: "auth",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "FirstName",
                schema: "auth",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "LastModifiedBy",
                schema: "auth",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "LastModifiedOn",
                schema: "auth",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "LastName",
                schema: "auth",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "ProImgEvidenceId",
                schema: "auth",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "UserType",
                schema: "auth",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                schema: "auth",
                table: "UserRoles");

            migrationBuilder.DropColumn(
                name: "CreatedOn",
                schema: "auth",
                table: "UserRoles");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                schema: "auth",
                table: "UserRoles");

            migrationBuilder.DropColumn(
                name: "DeletedOn",
                schema: "auth",
                table: "UserRoles");

            migrationBuilder.DropColumn(
                name: "Id",
                schema: "auth",
                table: "UserRoles");

            migrationBuilder.DropColumn(
                name: "LastModifiedBy",
                schema: "auth",
                table: "UserRoles");

            migrationBuilder.DropColumn(
                name: "LastModifiedOn",
                schema: "auth",
                table: "UserRoles");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                schema: "auth",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "CreatedOn",
                schema: "auth",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                schema: "auth",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "DeletedOn",
                schema: "auth",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "LastModifiedBy",
                schema: "auth",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "LastModifiedOn",
                schema: "auth",
                table: "Roles");

            migrationBuilder.RenameTable(
                name: "Users",
                schema: "auth",
                newName: "Users");

            migrationBuilder.RenameTable(
                name: "UserRoles",
                schema: "auth",
                newName: "UserRoles");

            migrationBuilder.RenameTable(
                name: "Roles",
                schema: "auth",
                newName: "Roles");

            migrationBuilder.AlterColumn<Guid>(
                name: "RoleId",
                table: "UserRoles",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<Guid>(
                name: "UserId",
                table: "UserRoles",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "Roles",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Users",
                table: "Users",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserRoles_Users_UserId",
                table: "UserRoles",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
