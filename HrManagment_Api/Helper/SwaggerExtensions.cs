using Microsoft.OpenApi.Models;

namespace HrManagment_Api.Helper;

public static class SwaggerExtensions
{
    public static void ConfigureSwagger(this IServiceCollection services)
    {

        services.AddSwaggerGen(a =>
        {
            a.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "Hr_Managment",
                Version = "v1"
            });
            a.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
            {
                Name = "Authorization",
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Description = "Input the JWT like: Bearer {your token}",
            });
            a.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    Array.Empty<string>()
                }
            });
        });
    }
}
