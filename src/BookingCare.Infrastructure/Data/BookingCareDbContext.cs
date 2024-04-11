using BookingCare.Domain.Base;
using BookingCare.Domain.Entities;
using BookingCare.Infrastructure.Data.Configurations;
using BookingCare.Infrastructure.Data.SeedData;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BookingCare.Infrastructure.Data;

public class BookingCareDbContext : IdentityDbContext<AppUser, IdentityRole<Guid>, Guid>
{
    public BookingCareDbContext(DbContextOptions<BookingCareDbContext> options) : base(options)
    {
    }

    #region DbSet
    public DbSet<Province>? Provinces { get; set; }
    public DbSet<District>? Districts { get; set; }
    public DbSet<Ward>? Wards { get; set; }
    public DbSet<Language>? Languages { get; set; }
    public DbSet<TextContent>? TextContents { get; set; }
    public DbSet<Translation>? Translations { get; set; }
    public DbSet<DoctorInfo>? DoctorInfos { get; set; }
    public DbSet<Clinic>? Clinics { get; set; }
    public DbSet<Speciality>? Specialities { get; set; }
    public DbSet<Booking>? Bookings { get; set; }
    public DbSet<Schedule>? Schedules { get; set; }
    public DbSet<Contact>? Contacts { get; set; }
    #endregion

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var insertedEntries = this.ChangeTracker.Entries()
                               .Where(x => x.State == EntityState.Added)
                               .Select(x => x.Entity);

        foreach (var insertedEntry in insertedEntries)
        {
            var auditableEntity = insertedEntry as IAuditable;
            //If the inserted object is an Auditable. 
            if (auditableEntity != null)
            {
                auditableEntity.CreatedAt = DateTimeOffset.UtcNow;
            }
        }

        var modifiedEntries = this.ChangeTracker.Entries()
                   .Where(x => x.State == EntityState.Modified)
                   .Select(x => x.Entity);

        foreach (var modifiedEntry in modifiedEntries)
        {
            //If the inserted object is an Auditable. 
            var auditableEntity = modifiedEntry as IAuditable;
            if (auditableEntity != null)
            {
                auditableEntity.UpdatedAt = DateTimeOffset.UtcNow;
            }
        }

        return base.SaveChangesAsync(cancellationToken);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfiguration(new ProvinceConfiguration());
        modelBuilder.ApplyConfiguration(new DistrictConfiguration());
        modelBuilder.ApplyConfiguration(new WardConfiguration());
        modelBuilder.ApplyConfiguration(new LanguageConfiguration());
        modelBuilder.ApplyConfiguration(new TextContentConfiguration());
        modelBuilder.ApplyConfiguration(new TranslationConfiguration());
        modelBuilder.ApplyConfiguration(new AppUserConfiguration());
        modelBuilder.ApplyConfiguration(new DoctorInfoConfiguration());
        modelBuilder.ApplyConfiguration(new ClinicConfiguration());
        modelBuilder.ApplyConfiguration(new SpecialityConfiguration());
        modelBuilder.ApplyConfiguration(new BookingConfiguration());
        modelBuilder.ApplyConfiguration(new ScheduleConfiguration());
        modelBuilder.ApplyConfiguration(new ContactConfiguration());

        modelBuilder.Entity<IdentityRole<Guid>>().ToTable("AppRoles");
        modelBuilder.Entity<IdentityUserClaim<Guid>>().ToTable("AppUserClaims");
        modelBuilder.Entity<IdentityUserRole<Guid>>().ToTable("AppUserRoles").HasKey(x => new { x.UserId, x.RoleId });
        modelBuilder.Entity<IdentityUserLogin<Guid>>().ToTable("AppUserLogins").HasKey(x => x.UserId);
        modelBuilder.Entity<IdentityRoleClaim<Guid>>().ToTable("AppRoleClaims");
        modelBuilder.Entity<IdentityUserToken<Guid>>().ToTable("AppUserTokens").HasKey(x => x.UserId);

        modelBuilder.Seed();
    }
}