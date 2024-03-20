using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Lab2.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Projects",
                columns: table => new
                {
                    ProjectId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Projects", x => x.ProjectId);
                });

			migrationBuilder.CreateTable(
				name: "ProjectTasks",
				columns: table => new
				{
					ProjectTaskId = table.Column<int>(type: "int", nullable: false)
						.Annotation("SqlServer:Identity", "1, 1"),
					Title = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
					Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
					ProjectId = table.Column<int>(type: "int", nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_ProjectTasks", x => x.ProjectTaskId);
					table.ForeignKey(
						name: "FK_ProjectTasks_Projects_ProjectId",
						column: x => x.ProjectId,
						principalTable: "Projects",
						principalColumn: "ProjectId",
						onDelete: ReferentialAction.Cascade);
				});

			migrationBuilder.CreateIndex(
				name: "IX_ProjectTasks_ProjectId",
				table: "ProjectTasks",
				column: "ProjectId");

			migrationBuilder.CreateTable(
				name: "ProjectComments",
				columns: table => new
				{
					ProjectCommentId = table.Column<int>(type: "int", nullable: false)
						.Annotation("SqlServer:Identity", "1, 1"),
					Content = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
					CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
					ProjectId = table.Column<int>(type: "int", nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_ProjectComments", x => x.ProjectCommentId);
					table.ForeignKey(
						name: "FK_ProjectComments_Projects_ProjectId",
						column: x => x.ProjectId,
						principalTable: "Projects",
						principalColumn: "ProjectId",
						onDelete: ReferentialAction.Cascade);
				});

			migrationBuilder.CreateIndex(
				name: "IX_ProjectComments_ProjectId",
				table: "ProjectComments",
				column: "ProjectId");
		}

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Projects");

			migrationBuilder.DropTable(
				name: "ProjectTasks");

			migrationBuilder.DropTable(
			   name: "ProjectComments");
		}
    }
}
