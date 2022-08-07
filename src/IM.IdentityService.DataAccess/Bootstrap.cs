using IM.IdentityService.DataAccess.Repositories;
using IM.IdentityService.DataAccess.Repositories.Implementations;
using IM.IdentityService.Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace IM.IdentityService.DataAccess;

public static class Bootstrap
{
    public static IServiceCollection AddDataAccess(
        this IServiceCollection services, IConfiguration configuration, string connectionStringName = "default")
    {
        services
            .AddDbContext<ServiceDbContext>((sp, options) =>
                options.UseNpgsql(configuration.GetConnectionString(connectionStringName)));

        services.AddScoped<DbContext, ServiceDbContext>();
        
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
        
        services.AddIdentity<ApplicationUser, ApplicationUserRole>(options =>
            {
                options.User.RequireUniqueEmail = true;
                // options.Password.RequireDigit = false;
                // options.Password.RequireLowercase = false;
                // options.Password.RequireNonAlphanumeric = false;
                // options.Password.RequireUppercase = false;
                // options.SignIn.RequireConfirmedPhoneNumber = true;
            })
            .AddEntityFrameworkStores<ServiceDbContext>()
            .AddDefaultTokenProviders();
        
        return services
            .AddScoped<IUsersRepository, UsersRepository>();
    }
}
