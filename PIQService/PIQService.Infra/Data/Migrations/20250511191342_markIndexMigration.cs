using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PIQService.Infra.Data.Migrations
{
    /// <inheritdoc />
    public partial class markIndexMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameIndex(
                name: "IX_assessment_marks_session_id",
                table: "assessment_marks",
                newName: "IX_assessmentId");

            migrationBuilder.CreateIndex(
                name: "IX_assessmentId_assessorId_assessedId",
                table: "assessment_marks",
                columns: new[] { "session_id", "assessor_id", "assessed_id" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_assessmentId_assessorId_assessedId",
                table: "assessment_marks");

            migrationBuilder.RenameIndex(
                name: "IX_assessmentId",
                table: "assessment_marks",
                newName: "IX_assessment_marks_session_id");
        }
    }
}
