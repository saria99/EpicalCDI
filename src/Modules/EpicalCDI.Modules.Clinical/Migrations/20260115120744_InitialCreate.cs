using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace EpicalCDI.Modules.Clinical.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "clinical");

            migrationBuilder.CreateTable(
                name: "Encounters",
                schema: "clinical",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    HospitalId = table.Column<int>(type: "integer", nullable: false),
                    ExternalEncounterId = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    PatientExternalId = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    AdmitDateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DischargeDateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    LastUpdatedUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Encounters", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Medications",
                schema: "clinical",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    HospitalId = table.Column<int>(type: "integer", nullable: false),
                    EncounterId = table.Column<long>(type: "bigint", nullable: false),
                    MedicationName = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    StartTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    EndTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    HashChecksum = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    LastUpdatedUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Medications", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Medications_Encounters_EncounterId",
                        column: x => x.EncounterId,
                        principalSchema: "clinical",
                        principalTable: "Encounters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Observations",
                schema: "clinical",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    HospitalId = table.Column<int>(type: "integer", nullable: false),
                    EncounterId = table.Column<long>(type: "bigint", nullable: false),
                    Code = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Value = table.Column<string>(type: "text", nullable: false),
                    Unit = table.Column<string>(type: "text", nullable: true),
                    ObservationTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    HashChecksum = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    LastUpdatedUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Observations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Observations_Encounters_EncounterId",
                        column: x => x.EncounterId,
                        principalSchema: "clinical",
                        principalTable: "Encounters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Encounters_HospitalId_ExternalEncounterId",
                schema: "clinical",
                table: "Encounters",
                columns: new[] { "HospitalId", "ExternalEncounterId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Medications_EncounterId",
                schema: "clinical",
                table: "Medications",
                column: "EncounterId");

            migrationBuilder.CreateIndex(
                name: "IX_Medications_HospitalId_EncounterId_MedicationName_StartTime",
                schema: "clinical",
                table: "Medications",
                columns: new[] { "HospitalId", "EncounterId", "MedicationName", "StartTime" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Observations_EncounterId",
                schema: "clinical",
                table: "Observations",
                column: "EncounterId");

            migrationBuilder.CreateIndex(
                name: "IX_Observations_HospitalId_EncounterId_Code_ObservationTime",
                schema: "clinical",
                table: "Observations",
                columns: new[] { "HospitalId", "EncounterId", "Code", "ObservationTime" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Medications",
                schema: "clinical");

            migrationBuilder.DropTable(
                name: "Observations",
                schema: "clinical");

            migrationBuilder.DropTable(
                name: "Encounters",
                schema: "clinical");
        }
    }
}
