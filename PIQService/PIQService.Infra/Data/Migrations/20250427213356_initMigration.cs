using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PIQService.Infra.Data.Migrations
{
    /// <inheritdoc />
    public partial class initMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "events",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    name = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    start_date = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    end_date = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_events", x => x.id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "directions",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    event_id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    name = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_directions", x => x.id);
                    table.ForeignKey(
                        name: "FK_directions_events_event_id",
                        column: x => x.event_id,
                        principalTable: "events",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "projects",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    direction_id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    name = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_projects", x => x.id);
                    table.ForeignKey(
                        name: "FK_projects_directions_direction_id",
                        column: x => x.direction_id,
                        principalTable: "directions",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "teams",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    project_id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    tutor_id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    name = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_teams", x => x.id);
                    table.ForeignKey(
                        name: "FK_teams_projects_project_id",
                        column: x => x.project_id,
                        principalTable: "projects",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    team_id = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_users", x => x.id);
                    table.ForeignKey(
                        name: "FK_users_teams_team_id",
                        column: x => x.team_id,
                        principalTable: "teams",
                        principalColumn: "id");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_directions_event_id",
                table: "directions",
                column: "event_id");

            migrationBuilder.CreateIndex(
                name: "IX_projects_direction_id",
                table: "projects",
                column: "direction_id");

            migrationBuilder.CreateIndex(
                name: "IX_teams_project_id",
                table: "teams",
                column: "project_id");

            migrationBuilder.CreateIndex(
                name: "IX_teams_tutor_id",
                table: "teams",
                column: "tutor_id");

            migrationBuilder.CreateIndex(
                name: "IX_users_team_id",
                table: "users",
                column: "team_id");

            migrationBuilder.AddForeignKey(
                name: "FK_teams_users_tutor_id",
                table: "teams",
                column: "tutor_id",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_directions_events_event_id",
                table: "directions");

            migrationBuilder.DropForeignKey(
                name: "FK_projects_directions_direction_id",
                table: "projects");

            migrationBuilder.DropForeignKey(
                name: "FK_teams_projects_project_id",
                table: "teams");

            migrationBuilder.DropForeignKey(
                name: "FK_teams_users_tutor_id",
                table: "teams");

            migrationBuilder.DropTable(
                name: "events");

            migrationBuilder.DropTable(
                name: "directions");

            migrationBuilder.DropTable(
                name: "projects");

            migrationBuilder.DropTable(
                name: "users");

            migrationBuilder.DropTable(
                name: "teams");
        }
    }
}
