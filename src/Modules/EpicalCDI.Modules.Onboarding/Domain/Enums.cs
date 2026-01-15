namespace EpicalCDI.Modules.Onboarding.Domain;

public enum ExternalSystemType { Epic, Cerner, Meditech, Other }
public enum HospitalStatus { Onboarding, Active, Suspended }
public enum IntegrationType { FHIR, HL7, File }
public enum ProtocolType { HTTPS, SFTP, TCP }
public enum CredentialType { OAuth2, APIKey, Certificate, SFTP }
public enum DataDomain { Encounters, Observations, Labs, Medications, Cultures }
public enum CodeSystemType { LOINC, RxNorm, Internal }
public enum UpdateStrategy { Upsert, Versioned }
