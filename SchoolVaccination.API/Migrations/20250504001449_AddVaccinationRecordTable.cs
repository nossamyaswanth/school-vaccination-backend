using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SchoolVaccination.API.Migrations
{
    /// <inheritdoc />
    public partial class AddVaccinationRecordTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "VaccinationRecords",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StudentId = table.Column<int>(type: "int", nullable: false),
                    DriveId = table.Column<int>(type: "int", nullable: false),
                    IsVaccinated = table.Column<bool>(type: "bit", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VaccinationRecords", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VaccinationRecords_Students_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Students",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_VaccinationRecords_VaccinationDrives_DriveId",
                        column: x => x.DriveId,
                        principalTable: "VaccinationDrives",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_VaccinationRecords_DriveId",
                table: "VaccinationRecords",
                column: "DriveId");

            migrationBuilder.CreateIndex(
                name: "IX_VaccinationRecords_StudentId",
                table: "VaccinationRecords",
                column: "StudentId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "VaccinationRecords");
        }
    }
}
