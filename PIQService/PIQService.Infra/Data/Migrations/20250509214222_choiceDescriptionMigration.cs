using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PIQService.Infra.Data.Migrations
{
    /// <inheritdoc />
    public partial class choiceDescriptionMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "behavior_form_id",
                table: "assessment_templates",
                type: "char(36)",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                collation: "ascii_general_ci");

            migrationBuilder.AddColumn<string>(
                name: "description",
                table: "assessment_choices",
                type: "varchar(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_assessment_templates_behavior_form_id",
                table: "assessment_templates",
                column: "behavior_form_id");

            migrationBuilder.AddForeignKey(
                name: "FK_assessment_templates_assessment_forms_behavior_form_id",
                table: "assessment_templates",
                column: "behavior_form_id",
                principalTable: "assessment_forms",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_assessment_templates_assessment_forms_behavior_form_id",
                table: "assessment_templates");

            migrationBuilder.DropIndex(
                name: "IX_assessment_templates_behavior_form_id",
                table: "assessment_templates");

            migrationBuilder.DropColumn(
                name: "behavior_form_id",
                table: "assessment_templates");

            migrationBuilder.DropColumn(
                name: "description",
                table: "assessment_choices");
        }
    }
}
