using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PIQService.Infra.Data.Migrations
{
    /// <inheritdoc />
    public partial class assessmentMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "assessment_criteria",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    name = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    description = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_assessment_criteria", x => x.id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "assessment_forms",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    type = table.Column<sbyte>(type: "tinyint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_assessment_forms", x => x.id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "assessment_questions",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    question_text = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    weight = table.Column<float>(type: "float", nullable: false),
                    form_id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    criteria_id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    order = table.Column<sbyte>(type: "tinyint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_assessment_questions", x => x.id);
                    table.ForeignKey(
                        name: "FK_assessment_questions_assessment_criteria_criteria_id",
                        column: x => x.criteria_id,
                        principalTable: "assessment_criteria",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_assessment_questions_assessment_forms_form_id",
                        column: x => x.form_id,
                        principalTable: "assessment_forms",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "assessment_templates",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    name = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    circle_form_id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_assessment_templates", x => x.id);
                    table.ForeignKey(
                        name: "FK_assessment_templates_assessment_forms_circle_form_id",
                        column: x => x.circle_form_id,
                        principalTable: "assessment_forms",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "forms_criteria",
                columns: table => new
                {
                    CriteriaListId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    FormDboId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_forms_criteria", x => new { x.CriteriaListId, x.FormDboId });
                    table.ForeignKey(
                        name: "FK_forms_criteria_assessment_criteria_CriteriaListId",
                        column: x => x.CriteriaListId,
                        principalTable: "assessment_criteria",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_forms_criteria_assessment_forms_FormDboId",
                        column: x => x.FormDboId,
                        principalTable: "assessment_forms",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "assessment_choices",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    question_id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    text = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    value = table.Column<short>(type: "smallint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_assessment_choices", x => x.id);
                    table.ForeignKey(
                        name: "FK_assessment_choices_assessment_questions_question_id",
                        column: x => x.question_id,
                        principalTable: "assessment_questions",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "assessments",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    name = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    template_id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    start_date = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    end_date = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_assessments", x => x.id);
                    table.ForeignKey(
                        name: "FK_assessments_assessment_templates_template_id",
                        column: x => x.template_id,
                        principalTable: "assessment_templates",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "assessment_marks",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    assessor_id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    assessed_id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    session_id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_assessment_marks", x => x.id);
                    table.ForeignKey(
                        name: "FK_assessment_marks_assessments_session_id",
                        column: x => x.session_id,
                        principalTable: "assessments",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_assessment_marks_users_assessed_id",
                        column: x => x.assessed_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_assessment_marks_users_assessor_id",
                        column: x => x.assessor_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

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

            migrationBuilder.CreateTable(
                name: "marks_choices",
                columns: table => new
                {
                    AssessmentMarkDboId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    ChoicesId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_marks_choices", x => new { x.AssessmentMarkDboId, x.ChoicesId });
                    table.ForeignKey(
                        name: "FK_marks_choices_assessment_choices_ChoicesId",
                        column: x => x.ChoicesId,
                        principalTable: "assessment_choices",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_marks_choices_assessment_marks_AssessmentMarkDboId",
                        column: x => x.AssessmentMarkDboId,
                        principalTable: "assessment_marks",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_assessment_choices_question_id",
                table: "assessment_choices",
                column: "question_id");

            migrationBuilder.CreateIndex(
                name: "IX_assessment_marks_assessed_id",
                table: "assessment_marks",
                column: "assessed_id");

            migrationBuilder.CreateIndex(
                name: "IX_assessment_marks_assessor_id",
                table: "assessment_marks",
                column: "assessor_id");

            migrationBuilder.CreateIndex(
                name: "IX_assessment_marks_session_id",
                table: "assessment_marks",
                column: "session_id");

            migrationBuilder.CreateIndex(
                name: "IX_assessment_questions_criteria_id",
                table: "assessment_questions",
                column: "criteria_id");

            migrationBuilder.CreateIndex(
                name: "IX_assessment_questions_form_id",
                table: "assessment_questions",
                column: "form_id");

            migrationBuilder.CreateIndex(
                name: "IX_assessment_templates_circle_form_id",
                table: "assessment_templates",
                column: "circle_form_id");

            migrationBuilder.CreateIndex(
                name: "IX_assessments_template_id",
                table: "assessments",
                column: "template_id");

            migrationBuilder.CreateIndex(
                name: "IX_assessments_teams_TeamsId",
                table: "assessments_teams",
                column: "TeamsId");

            migrationBuilder.CreateIndex(
                name: "IX_forms_criteria_FormDboId",
                table: "forms_criteria",
                column: "FormDboId");

            migrationBuilder.CreateIndex(
                name: "IX_marks_choices_ChoicesId",
                table: "marks_choices",
                column: "ChoicesId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "assessments_teams");

            migrationBuilder.DropTable(
                name: "forms_criteria");

            migrationBuilder.DropTable(
                name: "marks_choices");

            migrationBuilder.DropTable(
                name: "assessment_choices");

            migrationBuilder.DropTable(
                name: "assessment_marks");

            migrationBuilder.DropTable(
                name: "assessment_questions");

            migrationBuilder.DropTable(
                name: "assessments");

            migrationBuilder.DropTable(
                name: "assessment_criteria");

            migrationBuilder.DropTable(
                name: "assessment_templates");

            migrationBuilder.DropTable(
                name: "assessment_forms");
        }
    }
}
