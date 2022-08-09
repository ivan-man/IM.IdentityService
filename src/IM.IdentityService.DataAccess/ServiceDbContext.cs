﻿using IM.IdentityService.Domain.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
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

    public ServiceDbContext(DbContextOptions<ServiceDbContext> options)
        : base(options)
    {
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
            .HasIndex(q => q.NormalizedUserName);

        modelBuilder.Entity<ApplicationUser>()
            .HasIndex(q => q.NormalizedEmail);

        modelBuilder.Entity<ApplicationUsing>()
            .HasKey(q => new { q.ApplicationUserId, q.ApplicationId });

        modelBuilder.Entity<ApplicationUsing>()
            .HasIndex(q => new { q.ApplicationUserId });
        
        modelBuilder.Entity<ApplicationUsing>()
            .HasIndex(q => new { q.ApplicationId });
    }

    private static ILoggerFactory? GetDbLoggerFactory()
    {
        IServiceCollection serviceCollection = new ServiceCollection();
        serviceCollection.AddLogging(builder =>
            builder.AddFilter(DbLoggerCategory.Database.Command.Name, LogLevel.Debug));

        return serviceCollection.BuildServiceProvider().GetService<ILoggerFactory>();
    }
}
