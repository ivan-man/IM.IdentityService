using IM.Common.Models.Domain;
using IM.IdentityService.Domain.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace IM.IdentityService.DataAccess;

public sealed class ServiceDbContext : IdentityDbContext<ApplicationUser, ApplicationUserRole, Guid>
{
    public DbContext AppDbContext => this;
    public DbSet<ApplicationUser> Users { get; set; }
    public DbSet<RefreshToken> RefreshTokens { get; set; }
    public DbSet<Totp> Totp { get; set; }
    public DbSet<Application> Applications { get; set; }
    public DbSet<ApplicationUsing> ApplicationUsings { get; set; }

    public ServiceDbContext(DbContextOptions<ServiceDbContext> options)
        : base(options)
    {
        ChangeTracker.StateChanged += OnEntityStateChanged;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseLoggerFactory(GetDbLoggerFactory()).EnableSensitiveDataLogging();
        optionsBuilder.EnableDetailedErrors();
        
        base.OnConfiguring(optionsBuilder);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ServiceDbContext).Assembly);
        modelBuilder.HasPostgresExtension("uuid-ossp");
        modelBuilder.HasDefaultSchema("public");
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<RefreshToken>()
            .HasOne<ApplicationUser>();

        modelBuilder.Entity<ApplicationUser>()
            .HasIndex(q => q.NormalizedUserName)
            .IsUnique();

        modelBuilder.Entity<ApplicationUser>()
            .HasIndex(q => q.NormalizedEmail)
            .IsUnique();

        modelBuilder.Entity<ApplicationUser>()
            .HasIndex(q => q.PhoneNumber)
            .IsUnique();

        modelBuilder.Entity<ApplicationUser>()
            .Property(b => b.Created)
            .HasDefaultValueSql("CURRENT_TIMESTAMP")
            .ValueGeneratedOnAdd();

        modelBuilder.Entity<ApplicationUser>()
            .Property(b => b.Updated)
            .HasDefaultValueSql("CURRENT_TIMESTAMP")
            .ValueGeneratedOnUpdate();

        modelBuilder.Entity<ApplicationUsing>()
            .HasKey(q => new { q.ApplicationUserId, q.ApplicationId });

        modelBuilder.Entity<ApplicationUsing>()
            .HasIndex(q => q.ApplicationUserId);

        modelBuilder.Entity<ApplicationUsing>()
            .HasIndex(q => q.ApplicationId);

        modelBuilder.Entity<Application>()
            .HasIndex(q => q.AppKey);
    }
    
    private void OnEntityStateChanged(object? sender, EntityStateChangedEventArgs e)
    {
        if (e.Entry.Entity is not IBaseEntity entity)
            return;

        switch (e.Entry.State)
        {
            case EntityState.Modified:
                entity.Updated = DateTime.UtcNow;
                break;
            case EntityState.Added:
                entity.Created = DateTime.UtcNow;
                break;
        }
        
    }

     

    private static ILoggerFactory? GetDbLoggerFactory()
    {
        IServiceCollection serviceCollection = new ServiceCollection();
        serviceCollection.AddLogging(builder =>
            builder.AddFilter(DbLoggerCategory.Database.Command.Name, LogLevel.Debug));

        return serviceCollection.BuildServiceProvider().GetService<ILoggerFactory>();
    }
}
