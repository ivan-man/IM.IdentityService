using IM.IdentityService.Domain.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace IM.IdentityService.DataAccess;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationUserRole, int>
{
    public DbContext AppDbContext => this;
    public DbSet<ApplicationUser> Users { get; set; }
    public DbSet<RefreshToken> RefreshTokens { get; set; }
    public DbSet<Totp> Totp { get; set; }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseLoggerFactory(GetDbLoggerFactory()).EnableSensitiveDataLogging();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
        modelBuilder.HasDefaultSchema("public");
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.Entity<ApplicationUser>()
            .HasIndex(q => q.NormalizedUserName);
        
        modelBuilder.Entity<ApplicationUser>()
            .HasIndex(q => q.NormalizedEmail);
    }

    private static ILoggerFactory GetDbLoggerFactory()
    {
        IServiceCollection serviceCollection = new ServiceCollection();
        serviceCollection.AddLogging(builder =>
            builder.AddFilter(DbLoggerCategory.Database.Command.Name, LogLevel.Debug));

        return serviceCollection.BuildServiceProvider().GetService<ILoggerFactory>();
    }
}
