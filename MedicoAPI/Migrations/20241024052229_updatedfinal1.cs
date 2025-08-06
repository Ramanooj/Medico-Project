using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MedicoAPI.Migrations
{
    /// <inheritdoc />
    public partial class updatedfinal1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MedicalHistory_PatientAssessment_PatientAssessmentAssessmentId",
                table: "MedicalHistory");

            migrationBuilder.DropForeignKey(
                name: "FK_MedicalHistory_Patient_PatientId",
                table: "MedicalHistory");

            migrationBuilder.DropForeignKey(
                name: "FK_PatientAssessment_Doctor_DoctorId",
                table: "PatientAssessment");

            migrationBuilder.DropForeignKey(
                name: "FK_PatientAssessment_Patient_PatientId",
                table: "PatientAssessment");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PatientAssessment",
                table: "PatientAssessment");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MedicalHistory",
                table: "MedicalHistory");

            migrationBuilder.DropIndex(
                name: "IX_MedicalHistory_PatientAssessmentAssessmentId",
                table: "MedicalHistory");

            migrationBuilder.DropIndex(
                name: "IX_MedicalHistory_PatientId",
                table: "MedicalHistory");

            migrationBuilder.DropColumn(
                name: "PatientAssessmentAssessmentId",
                table: "MedicalHistory");

            migrationBuilder.RenameTable(
                name: "PatientAssessment",
                newName: "Assessments");

            migrationBuilder.RenameTable(
                name: "MedicalHistory",
                newName: "MedicalHistories");

            migrationBuilder.RenameIndex(
                name: "IX_PatientAssessment_PatientId",
                table: "Assessments",
                newName: "IX_Assessments_PatientId");

            migrationBuilder.RenameIndex(
                name: "IX_PatientAssessment_DoctorId",
                table: "Assessments",
                newName: "IX_Assessments_DoctorId");

            migrationBuilder.AlterColumn<string>(
                name: "PatientId",
                table: "MedicalHistories",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<string>(
                name: "PatientAssessmentId",
                table: "MedicalHistories",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Assessments",
                table: "Assessments",
                column: "AssessmentId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MedicalHistories",
                table: "MedicalHistories",
                column: "MedicalHistoryId");

            migrationBuilder.CreateIndex(
                name: "IX_MedicalHistories_PatientAssessmentId",
                table: "MedicalHistories",
                column: "PatientAssessmentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Assessments_Doctor_DoctorId",
                table: "Assessments",
                column: "DoctorId",
                principalTable: "Doctor",
                principalColumn: "DoctorId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Assessments_Patient_PatientId",
                table: "Assessments",
                column: "PatientId",
                principalTable: "Patient",
                principalColumn: "PatientId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MedicalHistories_Assessments_PatientAssessmentId",
                table: "MedicalHistories",
                column: "PatientAssessmentId",
                principalTable: "Assessments",
                principalColumn: "AssessmentId",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Assessments_Doctor_DoctorId",
                table: "Assessments");

            migrationBuilder.DropForeignKey(
                name: "FK_Assessments_Patient_PatientId",
                table: "Assessments");

            migrationBuilder.DropForeignKey(
                name: "FK_MedicalHistories_Assessments_PatientAssessmentId",
                table: "MedicalHistories");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MedicalHistories",
                table: "MedicalHistories");

            migrationBuilder.DropIndex(
                name: "IX_MedicalHistories_PatientAssessmentId",
                table: "MedicalHistories");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Assessments",
                table: "Assessments");

            migrationBuilder.DropColumn(
                name: "PatientAssessmentId",
                table: "MedicalHistories");

            migrationBuilder.RenameTable(
                name: "MedicalHistories",
                newName: "MedicalHistory");

            migrationBuilder.RenameTable(
                name: "Assessments",
                newName: "PatientAssessment");

            migrationBuilder.RenameIndex(
                name: "IX_Assessments_PatientId",
                table: "PatientAssessment",
                newName: "IX_PatientAssessment_PatientId");

            migrationBuilder.RenameIndex(
                name: "IX_Assessments_DoctorId",
                table: "PatientAssessment",
                newName: "IX_PatientAssessment_DoctorId");

            migrationBuilder.AlterColumn<string>(
                name: "PatientId",
                table: "MedicalHistory",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(
                name: "PatientAssessmentAssessmentId",
                table: "MedicalHistory",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_MedicalHistory",
                table: "MedicalHistory",
                column: "MedicalHistoryId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PatientAssessment",
                table: "PatientAssessment",
                column: "AssessmentId");

            migrationBuilder.CreateIndex(
                name: "IX_MedicalHistory_PatientAssessmentAssessmentId",
                table: "MedicalHistory",
                column: "PatientAssessmentAssessmentId");

            migrationBuilder.CreateIndex(
                name: "IX_MedicalHistory_PatientId",
                table: "MedicalHistory",
                column: "PatientId");

            migrationBuilder.AddForeignKey(
                name: "FK_MedicalHistory_PatientAssessment_PatientAssessmentAssessmentId",
                table: "MedicalHistory",
                column: "PatientAssessmentAssessmentId",
                principalTable: "PatientAssessment",
                principalColumn: "AssessmentId");

            migrationBuilder.AddForeignKey(
                name: "FK_MedicalHistory_Patient_PatientId",
                table: "MedicalHistory",
                column: "PatientId",
                principalTable: "Patient",
                principalColumn: "PatientId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PatientAssessment_Doctor_DoctorId",
                table: "PatientAssessment",
                column: "DoctorId",
                principalTable: "Doctor",
                principalColumn: "DoctorId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PatientAssessment_Patient_PatientId",
                table: "PatientAssessment",
                column: "PatientId",
                principalTable: "Patient",
                principalColumn: "PatientId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
