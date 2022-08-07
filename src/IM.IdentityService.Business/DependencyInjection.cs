using FluentValidation;
using IM.IdentityService.Business.InternalServices.Tokens;
using IM.IdentityService.Business.InternalServices.Users;
using IM.IdentityService.DataAccess;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace IM.IdentityService.Business;

public static class DependencyInjection
{
    public static IServiceCollection AddBusiness(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services
            .AddMediatR(typeof(Bootstrap).Assembly)
            // .AddTransient(typeof(IPipelineBehavior<,>))
            .AddValidatorsFromAssembly(typeof(Bootstrap).Assembly)
            .AddScoped<ICurrentUserService, CurrentUserService>()
            .AddScoped<IJwtTokenGenerator, JwtTokenGenerator>()
            .AddDataAccess(configuration);

        return services;
    }
}
