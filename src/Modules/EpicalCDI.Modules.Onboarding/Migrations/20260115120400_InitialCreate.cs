using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace EpicalCDI.Modules.Onboarding.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "onboarding");

            migrationBuilder.CreateTable(
                name: "CodeMappings",
                schema: "onboarding",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    HospitalId = table.Column<int>(type: "integer", nullable: false),
                    SourceSystem = table.Column<string>(type: "text", nullable: false),
                    SourceCode = table.Column<string>(type: "text", nullable: false),
                    NormalizedCode = table.Column<string>(type: "text", nullable: false),
                    CodeSystem = table.Column<int>(type: "integer", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CodeMappings", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Credentials",
                schema: "onboarding",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    HospitalId = table.Column<int>(type: "integer", nullable: false),
                    CredentialType = table.Column<int>(type: "integer", nullable: false),
                    SecretReference = table.Column<string>(type: "text", nullable: false),
                    RotationDateUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Credentials", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DataScopes",
                schema: "onboarding",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    HospitalId = table.Column<int>(type: "integer", nullable: false),
                    DataDomain = table.Column<int>(type: "integer", nullable: false),
                    Enabled = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DataScopes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Hospitals",
                schema: "onboarding",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false),
                    HospitalCode = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    TimeZone = table.Column<string>(type: "text", nullable: false),
                    ExternalSystemType = table.Column<int>(type: "integer", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    CreatedUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Hospitals", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ImportSettings",
                schema: "onboarding",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    HospitalId = table.Column<int>(type: "integer", nullable: false),
                    UpdateStrategy = table.Column<int>(type: "integer", nullable: false),
                    AllowDeletes = table.Column<bool>(type: "boolean", nullable: false),
                    DefaultLookbackDays = table.Column<int>(type: "integer", nullable: false),
                    MaxBatchSize = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ImportSettings", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "IntegrationEndpoints",
                schema: "onboarding",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    HospitalId = table.Column<int>(type: "integer", nullable: false),
                    IntegrationType = table.Column<int>(type: "integer", nullable: false),
                    EndpointUrl = table.Column<string>(type: "text", nullable: false),
                    Protocol = table.Column<int>(type: "integer", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IntegrationEndpoints", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Hospitals_HospitalCode",
                schema: "onboarding",
                table: "Hospitals",
                column: "HospitalCode",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CodeMappings",
                schema: "onboarding");

            migrationBuilder.DropTable(
                name: "Credentials",
                schema: "onboarding");

            migrationBuilder.DropTable(
                name: "DataScopes",
                schema: "onboarding");

            migrationBuilder.DropTable(
                name: "Hospitals",
                schema: "onboarding");

            migrationBuilder.DropTable(
                name: "ImportSettings",
                schema: "onboarding");

            migrationBuilder.DropTable(
                name: "IntegrationEndpoints",
                schema: "onboarding");
        }
    }
}
