using System;
using Microsoft.EntityFrameworkCore.Migrations;
using MySql.EntityFrameworkCore.Metadata;

#nullable disable

namespace MediTrack.MedicalAppointmentService.API.Migrations
{
    /// <inheritdoc />
    public partial class InitialAppointmentSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "appointments",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    patient_id = table.Column<int>(type: "int", nullable: false),
                    type = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    appointment_date = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    location = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true),
                    notes = table.Column<string>(type: "varchar(400)", maxLength: 400, nullable: true),
                    status = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    updated_at = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_appointments", x => x.id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "appointment_requirements",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    appointment_id = table.Column<int>(type: "int", nullable: false),
                    description = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_appointment_requirements", x => x.id);
                    table.ForeignKey(
                        name: "FK_appointment_requirements_appointments_appointment_id",
                        column: x => x.appointment_id,
                        principalTable: "appointments",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "clinical_exams",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    patient_id = table.Column<int>(type: "int", nullable: false),
                    appointment_id = table.Column<int>(type: "int", nullable: true),
                    exam_type = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    scheduled_date = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    pickup_date = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    status = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_clinical_exams", x => x.id);
                    table.ForeignKey(
                        name: "FK_clinical_exams_appointments_appointment_id",
                        column: x => x.appointment_id,
                        principalTable: "appointments",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_appointment_requirements_appointment_id",
                table: "appointment_requirements",
                column: "appointment_id");

            migrationBuilder.CreateIndex(
                name: "IX_clinical_exams_appointment_id",
                table: "clinical_exams",
                column: "appointment_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "appointment_requirements");

            migrationBuilder.DropTable(
                name: "clinical_exams");

            migrationBuilder.DropTable(
                name: "appointments");
        }
    }
}
