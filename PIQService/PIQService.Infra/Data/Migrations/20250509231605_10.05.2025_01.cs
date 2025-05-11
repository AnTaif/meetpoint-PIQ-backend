using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PIQService.Infra.Data.Migrations
{
    /// <inheritdoc />
    public partial class _10052025_01 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "assessments_teams");

            migrationBuilder.AddColumn<Guid>(
                name: "team_id",
                table: "assessments",
                type: "char(36)",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                collation: "ascii_general_ci");

            migrationBuilder.CreateIndex(
                name: "IX_assessments_team_id",
                table: "assessments",
                column: "team_id");

            migrationBuilder.AddForeignKey(
                name: "FK_assessments_teams_team_id",
                table: "assessments",
                column: "team_id",
                principalTable: "teams",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_assessments_teams_team_id",
                table: "assessments");

            migrationBuilder.DropIndex(
                name: "IX_assessments_team_id",
                table: "assessments");

            migrationBuilder.DropColumn(
                name: "team_id",
                table: "assessments");

            migrationBuilder.CreateTable(
                name: "assessments_teams",
                columns: table => new
                {
                    AssessmentDboId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    TeamsId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_assessments_teams", x => new { x.AssessmentDboId, x.TeamsId });
                    table.ForeignKey(
                        name: "FK_assessments_teams_assessments_AssessmentDboId",
                        column: x => x.AssessmentDboId,
                        principalTable: "assessments",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_assessments_teams_teams_TeamsId",
                        column: x => x.TeamsId,
                        principalTable: "teams",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_assessments_teams_TeamsId",
                table: "assessments_teams",
                column: "TeamsId");
        }
    }
}
