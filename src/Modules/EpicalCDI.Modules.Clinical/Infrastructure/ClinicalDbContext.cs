using Microsoft.EntityFrameworkCore;
using EpicalCDI.Shared.Abstractions;
using EpicalCDI.Shared.Domain;
using EpicalCDI.Modules.Clinical.Domain;

namespace EpicalCDI.Modules.Clinical.Infrastructure;

public class ClinicalDbContext : DbContext
{
    public DbSet<Encounter> Encounters => Set<Encounter>();
    public DbSet<Observation> Observations => Set<Observation>();
    public DbSet<Medication> Medications => Set<Medication>();

    public ClinicalDbContext(DbContextOptions<ClinicalDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.HasDefaultSchema("clinical");

        // Helper to configure common ITenantEntity properties
        void ConfigureTenantEntity<T>(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<T> builder) where T : class, ITenantEntity
        {
            builder.Property(x => x.HospitalId)
                   .HasConversion(id => id.Value, value => new HospitalId(value))
                   .IsRequired();
        }

        modelBuilder.Entity<Encounter>(builder =>
        {
            builder.HasKey(x => x.Id);
            ConfigureTenantEntity(builder);
            
            builder.Property(x => x.ExternalEncounterId).IsRequired().HasMaxLength(100);
            builder.Property(x => x.PatientExternalId).IsRequired().HasMaxLength(100);

            // Natural Key: HospitalId + ExternalEncounterId
            builder.HasIndex(x => new { x.HospitalId, x.ExternalEncounterId }).IsUnique();

            builder.HasMany(e => e.Observations)
                   .WithOne()
                   .HasForeignKey(o => o.EncounterId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(e => e.Medications)
                   .WithOne()
                   .HasForeignKey(m => m.EncounterId)
                   .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Observation>(builder =>
        {
            builder.HasKey(x => x.Id);
            ConfigureTenantEntity(builder);

            builder.Property(x => x.Code).IsRequired().HasMaxLength(50);
            builder.Property(x => x.HashChecksum).IsRequired().HasMaxLength(64); // SHA256 hex string length

            // Natural Key: HospitalId + EncounterId + Code + ObservationTime
            builder.HasIndex(x => new { x.HospitalId, x.EncounterId, x.Code, x.ObservationTime }).IsUnique();
        });

        modelBuilder.Entity<Medication>(builder =>
        {
            builder.HasKey(x => x.Id);
            ConfigureTenantEntity(builder);

            builder.Property(x => x.MedicationName).IsRequired().HasMaxLength(200);
            builder.Property(x => x.HashChecksum).IsRequired().HasMaxLength(64);

            // Natural Key: HospitalId + EncounterId + MedicationName + StartTime
            builder.HasIndex(x => new { x.HospitalId, x.EncounterId, x.MedicationName, x.StartTime }).IsUnique();
        });
    }
}
