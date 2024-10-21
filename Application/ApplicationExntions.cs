using Application.Helpers.JwtConfingrations;
using Application.Services.Imp;
using Application.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Application;

public static class ApplicationExntions
{
    public static void ConfigureApplication(this IServiceCollection services)
    {
        services.AddDistributedMemoryCache();
        services.AddAutoMapper(Assembly.GetExecutingAssembly());
        services.AddTransient<IJwtUtils, JwtUtils>();
        services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        services.AddTransient<IUserService, UserService>();
    }
}
