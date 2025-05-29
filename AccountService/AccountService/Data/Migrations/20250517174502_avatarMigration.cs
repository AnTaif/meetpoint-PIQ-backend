using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace AccountService.Data.Migrations
{
    /// <inheritdoc />
    public partial class avatarMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AvatarUrl",
                table: "AspNetUsers",
                type: "varchar(255)",
                maxLength: 255,
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { new Guid("29743c2e-6bd9-4542-be0f-01db917f1cca"), "e86ce623-0d69-45ff-9d6e-4d33871918d7", "Tutor", "TUTOR" },
                    { new Guid("4bee3b12-62fe-42f5-b931-56c16ebe0652"), "ff3540b5-dea9-49e5-b57e-dd0823b68d35", "Admin", "ADMIN" },
                    { new Guid("bbb876dd-6358-47c5-bd26-27a6b5d484e0"), "27a02563-1f0a-4ca6-8cf8-62d354e0a98e", "Member", "MEMBER" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("29743c2e-6bd9-4542-be0f-01db917f1cca"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("4bee3b12-62fe-42f5-b931-56c16ebe0652"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("bbb876dd-6358-47c5-bd26-27a6b5d484e0"));

            migrationBuilder.DropColumn(
                name: "AvatarUrl",
                table: "AspNetUsers");
        }
    }
}
