using Microsoft.EntityFrameworkCore;
using EpicalCDI.Shared.Abstractions;
using EpicalCDI.Shared.Domain;
using EpicalCDI.Modules.Onboarding.Domain; // Add namespace for domain entities

namespace EpicalCDI.Modules.Onboarding.Infrastructure;

public class OnboardingDbContext : DbContext
{
    public DbSet<Hospital> Hospitals => Set<Hospital>();
    public DbSet<HospitalIntegrationEndpoint> IntegrationEndpoints => Set<HospitalIntegrationEndpoint>();
    public DbSet<HospitalCredential> Credentials => Set<HospitalCredential>();
    public DbSet<HospitalDataScope> DataScopes => Set<HospitalDataScope>();
    public DbSet<HospitalCodeMapping> CodeMappings => Set<HospitalCodeMapping>();
    public DbSet<HospitalImportSetting> ImportSettings => Set<HospitalImportSetting>();

    public OnboardingDbContext(DbContextOptions<OnboardingDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.HasDefaultSchema("onboarding");

        modelBuilder.Entity<Hospital>(builder =>
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasConversion(id => id.Value, value => new HospitalId(value));
            builder.Property(x => x.HospitalCode).IsRequired().HasMaxLength(50);
            builder.HasIndex(x => x.HospitalCode).IsUnique();
        });

        // Helper to configure common ITenantEntity properties
        void ConfigureTenantEntity<T>(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<T> builder) where T : class, ITenantEntity
        {
            builder.Property(x => x.HospitalId)
                   .HasConversion(id => id.Value, value => new HospitalId(value))
                   .IsRequired();
        }

        modelBuilder.Entity<HospitalIntegrationEndpoint>(builder =>
        {
            builder.HasKey(x => x.Id);
            ConfigureTenantEntity(builder);
        });

        modelBuilder.Entity<HospitalCredential>(builder =>
        {
            builder.HasKey(x => x.Id);
            ConfigureTenantEntity(builder);
        });

        modelBuilder.Entity<HospitalDataScope>(builder =>
        {
            builder.HasKey(x => x.Id);
            ConfigureTenantEntity(builder);
        });

        modelBuilder.Entity<HospitalCodeMapping>(builder =>
        {
            builder.HasKey(x => x.Id);
            ConfigureTenantEntity(builder);
        });

        modelBuilder.Entity<HospitalImportSetting>(builder =>
        {
            builder.HasKey(x => x.Id);
            ConfigureTenantEntity(builder);
        });
    }
}
