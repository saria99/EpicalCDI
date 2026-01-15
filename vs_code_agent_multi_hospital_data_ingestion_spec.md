# VS Code Agent Project Specification

## Project Name
Multi-Hospital Clinical Data Ingestion Platform (.NET / C#)

---

## Objective
Build a **.NET (C#) service** whose sole responsibility is to:

- Import clinical data from multiple hospitals
- Persist the data to a database
- Allow re-importing the same data safely (idempotent)
- Update records only when source data changes
- Ensure **strict isolation between hospitals (tenants)**

❗ Clinical interpretation, scoring, or decision logic is **explicitly out of scope**.

---

## Core Constraints

- Multiple hospitals may send identical identifiers
- Each hospital must be treated in isolation
- Imports must be repeatable without duplication
- Source systems are authoritative
- System must support replay and recovery

---

## Tenancy Model

- **Hospital = Tenant**
- Tenant-per-row strategy
- All tables include `HospitalId`
- All natural keys are scoped by `HospitalId`
- No cross-hospital queries or updates

---

## Hospital Onboarding Configuration Tables

### Hospitals
Root tenant table.

- HospitalId (PK)
- HospitalCode (unique)
- Name
- TimeZone
- ExternalSystemType (Epic | Cerner | Meditech | Other)
- Status (Onboarding | Active | Suspended)
- CreatedUtc
- UpdatedUtc

---

### HospitalIntegrationEndpoints
Defines how data is received.

- EndpointId (PK)
- HospitalId (FK)
- IntegrationType (FHIR | HL7 | File)
- EndpointUrl
- Protocol (HTTPS | SFTP | TCP)
- IsActive

---

### HospitalCredentials
Stores secure references only.

- CredentialId (PK)
- HospitalId (FK)
- CredentialType (OAuth2 | APIKey | Certificate | SFTP)
- SecretReference
- RotationDateUtc
- IsActive

---

### HospitalDataScopes
Controls which domains a hospital sends.

- ScopeId (PK)
- HospitalId (FK)
- DataDomain (Encounters | Observations | Labs | Medications | Cultures)
- Enabled

---

### HospitalCodeMappings
Hospital-specific code normalization.

- MappingId (PK)
- HospitalId (FK)
- SourceSystem
- SourceCode
- NormalizedCode
- CodeSystem (LOINC | RxNorm | Internal)
- IsActive

---

### HospitalImportSettings
Per-hospital ingestion behavior.

- SettingId (PK)
- HospitalId (FK)
- UpdateStrategy (Upsert | Versioned)
- AllowDeletes (bool)
- DefaultLookbackDays
- MaxBatchSize

---

## Core Clinical Data Tables (Simplified)

### Encounters
- EncounterId (PK)
- HospitalId (FK)
- ExternalEncounterId
- PatientExternalId
- AdmitDateTime
- DischargeDateTime
- LastUpdatedUtc

---

### Observations
- ObservationId (PK)
- HospitalId (FK)
- EncounterId (FK)
- Code
- Value
- Unit
- ObservationTime
- HashChecksum
- LastUpdatedUtc

---

### Medications
- MedicationId (PK)
- HospitalId (FK)
- EncounterId (FK)
- MedicationName
- StartTime
- EndTime
- HashChecksum
- LastUpdatedUtc

---

## Idempotent Import Rules

### Natural Keys (Scoped by Hospital)

- Observation: HospitalId + EncounterId + Code + ObservationTime
- Medication: HospitalId + EncounterId + MedicationName + StartTime
- Encounter: HospitalId + ExternalEncounterId

---

### Upsert Algorithm

```
FOR each incoming record
  Compute hash of relevant fields
  IF natural key does not exist
    INSERT
  ELSE IF stored hash != incoming hash
    UPDATE record
  ELSE
    SKIP (no-op)
```

---

## Ingestion Workflow

1. Receive payload
2. Resolve HospitalId from credentials or endpoint
3. Validate hospital status and configuration
4. Map payload to internal DTOs
5. Apply hospital-scoped upsert logic
6. Persist data
7. Log insert/update/skip outcome

---

## Architecture (Logical)

- ASP.NET Core API
- Integration Layer (FHIR / HL7 / File)
- Mapping Layer
- Persistence Layer (EF Core preferred)
- Import Audit & Logging

---

## Suggested .NET Solution Structure

```
/src
  /Api
  /Integration
  /Domain
  /Persistence
  /Configuration
/tests
```

---

## Non-Functional Requirements

- HIPAA compliant
- Encryption in transit and at rest
- Role-based access
- Full auditability
- Tenant-safe queries only

---

## Instructions for VS Code Agent

Use this document to:
- Generate EF Core entities and migrations
- Enforce HospitalId on all entities
- Implement idempotent upsert services
- Scaffold ingestion pipelines
- Add global tenant query filters

Do NOT implement clinical logic, scoring, or analytics.

---

## End of Scope
This system strictly ingests and persists hospital data. All interpretation occurs downstream.

---

# eClinicalWorks Testing Project (POC)

## Goal
Set up an xUnit testing project in C# that uses synthetic eClinicalWorks-like (FHIR R4) data for deterministic, offline testing.

## Constraints
- Use .NET 10
- Use xUnit
- Use HL7 FHIR R4 SDK
- Use synthetic data only
- All tests must run offline
- Project must be compatible with VS Code
- Follow clean architecture and test best practices

## Phase 1 – Solution & Project Setup
1. Integrate into existing EpicalCDI.slnx
2. Create an xUnit project named Ecw.Tests
3. Add the project to the solution
4. Restore all NuGet packages
5. Verify tests are discoverable in VS Code

### Packages
- xunit
- xunit.runner.visualstudio
- Microsoft.NET.Test.Sdk
- FluentAssertions
- Hl7.Fhir.R4
- Bogus

## Phase 2 – Folder Structure
Inside `Ecw.Tests`:
- `Data/FhirBundles`
- `Fixtures`
- `Helpers`

## Phase 3 – Synthetic FHIR Test Data
1. Add 3–5 static FHIR R4 Bundle JSON files under `Data/FhirBundles`
2. Each bundle must include Patient, Encounter, Observation
3. Ensure deterministic, valid FHIR JSON

## Phase 4 – Shared Test Fixture
1. Create `Fixtures/FhirFixture.cs`
2. Load one FHIR Bundle JSON file from disk
3. Parse it using `FhirJsonParser`
4. Expose `Bundle` as a public property
5. Load data once per test run

## Phase 5 – Unit Tests
**Patient Tests:**
- Name exists
- DOB present
- Gender set

**Encounter Tests:**
- Status valid
- References patient

**Observation Tests:**
- Code present
- Value present
- Effective date present

## Phase 6 – Negative & Edge Case Tests
- Empty bundle
- Missing patient
- Invalid observation value
- Malformed JSON throws exception

## Phase 7 – Optional Mock API
- Minimal ASP.NET Core API
- Serve FHIR bundles from disk
- Integration tests with WebApplicationFactory

## Phase 8 – Test Execution
Ensure tests run with: `dotnet test`

