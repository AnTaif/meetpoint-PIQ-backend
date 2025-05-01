using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PIQService.Infra.Data.Migrations
{
    /// <inheritdoc />
    public partial class templateIdMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "name",
                table: "events",
                type: "varchar(30)",
                maxLength: 30,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<Guid>(
                name: "assessment_template_id",
                table: "events",
                type: "char(36)",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                collation: "ascii_general_ci");

            migrationBuilder.CreateIndex(
                name: "IX_events_assessment_template_id",
                table: "events",
                column: "assessment_template_id");

            migrationBuilder.AddForeignKey(
                name: "FK_events_assessment_templates_assessment_template_id",
                table: "events",
                column: "assessment_template_id",
                principalTable: "assessment_templates",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_events_assessment_templates_assessment_template_id",
                table: "events");

            migrationBuilder.DropIndex(
                name: "IX_events_assessment_template_id",
                table: "events");

            migrationBuilder.DropColumn(
                name: "assessment_template_id",
                table: "events");

            migrationBuilder.AlterColumn<string>(
                name: "name",
                table: "events",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(30)",
                oldMaxLength: 30)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");
        }
    }
}
