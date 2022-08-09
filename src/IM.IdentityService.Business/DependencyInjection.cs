using FluentValidation;
using IM.Common.MediatR;
using IM.IdentityService.Business.Configuration;
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
            .Configure<JwtConfig>(e => configuration.GetSection("jwtConfig").Bind(e))
            .AddImMediatr(typeof(DependencyInjection))
            .AddScoped<ICurrentUserService, CurrentUserService>()
            .AddScoped<IJwtTokenGenerator, JwtTokenGenerator>()
            .AddDataAccess(configuration);

        return services;
    }
}
