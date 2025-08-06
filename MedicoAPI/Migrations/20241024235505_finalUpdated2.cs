using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MedicoAPI.Migrations
{
    /// <inheritdoc />
    public partial class finalUpdated2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MedicalHistories_Assessments_PatientAssessmentId",
                table: "MedicalHistories");

            migrationBuilder.DropIndex(
                name: "IX_MedicalHistories_PatientAssessmentId",
                table: "MedicalHistories");

            migrationBuilder.DropColumn(
                name: "PatientAssessmentId",
                table: "MedicalHistories");

            migrationBuilder.AddColumn<string>(
                name: "DoctorId",
                table: "MedicalHistories",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "Assessments",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DoctorId",
                table: "MedicalHistories");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "Assessments");

            migrationBuilder.AddColumn<string>(
                name: "PatientAssessmentId",
                table: "MedicalHistories",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_MedicalHistories_PatientAssessmentId",
                table: "MedicalHistories",
                column: "PatientAssessmentId");

            migrationBuilder.AddForeignKey(
                name: "FK_MedicalHistories_Assessments_PatientAssessmentId",
                table: "MedicalHistories",
                column: "PatientAssessmentId",
                principalTable: "Assessments",
                principalColumn: "AssessmentId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
