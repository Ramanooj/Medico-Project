using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MedicoAPI.Migrations
{
    /// <inheritdoc />
    public partial class updatedfinal : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MedicalHistory_Doctor_DoctorId",
                table: "MedicalHistory");

            migrationBuilder.DropIndex(
                name: "IX_MedicalHistory_DoctorId",
                table: "MedicalHistory");

            migrationBuilder.DropColumn(
                name: "DoctorId",
                table: "MedicalHistory");

            migrationBuilder.RenameColumn(
                name: "gender",
                table: "Patient",
                newName: "Gender");

            migrationBuilder.RenameColumn(
                name: "Created",
                table: "MedicalHistory",
                newName: "LastUpdated");

            migrationBuilder.RenameColumn(
                name: "DoctorName",
                table: "Doctor",
                newName: "LastName");

            migrationBuilder.AddColumn<string>(
                name: "PatientAssessmentAssessmentId",
                table: "MedicalHistory",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                table: "Doctor",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "IsVerified",
                table: "Doctor",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "PatientAssessment",
                columns: table => new
                {
                    AssessmentId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    PatientId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    DoctorId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    AssessmentDescription = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PatientAssessment", x => x.AssessmentId);
                    table.ForeignKey(
                        name: "FK_PatientAssessment_Doctor_DoctorId",
                        column: x => x.DoctorId,
                        principalTable: "Doctor",
                        principalColumn: "DoctorId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PatientAssessment_Patient_PatientId",
                        column: x => x.PatientId,
                        principalTable: "Patient",
                        principalColumn: "PatientId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MedicalHistory_PatientAssessmentAssessmentId",
                table: "MedicalHistory",
                column: "PatientAssessmentAssessmentId");

            migrationBuilder.CreateIndex(
                name: "IX_PatientAssessment_DoctorId",
                table: "PatientAssessment",
                column: "DoctorId");

            migrationBuilder.CreateIndex(
                name: "IX_PatientAssessment_PatientId",
                table: "PatientAssessment",
                column: "PatientId");

            migrationBuilder.AddForeignKey(
                name: "FK_MedicalHistory_PatientAssessment_PatientAssessmentAssessmentId",
                table: "MedicalHistory",
                column: "PatientAssessmentAssessmentId",
                principalTable: "PatientAssessment",
                principalColumn: "AssessmentId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MedicalHistory_PatientAssessment_PatientAssessmentAssessmentId",
                table: "MedicalHistory");

            migrationBuilder.DropTable(
                name: "PatientAssessment");

            migrationBuilder.DropIndex(
                name: "IX_MedicalHistory_PatientAssessmentAssessmentId",
                table: "MedicalHistory");

            migrationBuilder.DropColumn(
                name: "PatientAssessmentAssessmentId",
                table: "MedicalHistory");

            migrationBuilder.DropColumn(
                name: "FirstName",
                table: "Doctor");

            migrationBuilder.DropColumn(
                name: "IsVerified",
                table: "Doctor");

            migrationBuilder.RenameColumn(
                name: "Gender",
                table: "Patient",
                newName: "gender");

            migrationBuilder.RenameColumn(
                name: "LastUpdated",
                table: "MedicalHistory",
                newName: "Created");

            migrationBuilder.RenameColumn(
                name: "LastName",
                table: "Doctor",
                newName: "DoctorName");

            migrationBuilder.AddColumn<string>(
                name: "DoctorId",
                table: "MedicalHistory",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_MedicalHistory_DoctorId",
                table: "MedicalHistory",
                column: "DoctorId");

            migrationBuilder.AddForeignKey(
                name: "FK_MedicalHistory_Doctor_DoctorId",
                table: "MedicalHistory",
                column: "DoctorId",
                principalTable: "Doctor",
                principalColumn: "DoctorId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
