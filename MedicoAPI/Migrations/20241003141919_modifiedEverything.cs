using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MedicoAPI.Migrations
{
    /// <inheritdoc />
    public partial class modifiedEverything : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PatientAddress",
                table: "Patient",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ClinicAddress",
                table: "Doctor",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Specialty",
                table: "Doctor",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PatientAddress",
                table: "Patient");

            migrationBuilder.DropColumn(
                name: "ClinicAddress",
                table: "Doctor");

            migrationBuilder.DropColumn(
                name: "Specialty",
                table: "Doctor");
        }
    }
}
