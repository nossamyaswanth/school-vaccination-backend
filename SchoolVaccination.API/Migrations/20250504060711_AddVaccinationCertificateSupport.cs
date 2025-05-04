using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SchoolVaccination.API.Migrations
{
    /// <inheritdoc />
    public partial class AddVaccinationCertificateSupport : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CertificateFileName",
                table: "Students",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CertificateFileName",
                table: "Students");
        }
    }
}
