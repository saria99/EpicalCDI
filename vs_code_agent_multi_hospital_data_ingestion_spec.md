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

