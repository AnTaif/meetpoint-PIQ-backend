using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PIQService.Infra.Data.Migrations
{
    /// <inheritdoc />
    public partial class assessmentMigration2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "use_behavior_assessment",
                table: "assessments",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "use_circle_assessment",
                table: "assessments",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "use_behavior_assessment",
                table: "assessments");

            migrationBuilder.DropColumn(
                name: "use_circle_assessment",
                table: "assessments");
        }
    }
}
