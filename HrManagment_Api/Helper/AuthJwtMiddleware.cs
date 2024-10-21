using Application.Helpers;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Text;

namespace HrManagment_Api.Helper;

public class AuthJwtMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IConfiguration _configuration;

    public AuthJwtMiddleware(RequestDelegate next, IConfiguration configuration)
    {
        _next = next;
        _configuration = configuration;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            var schema = context.Request.Path;

            if (schema.Value.ToString().ToLower() != "/Login".ToLower()&&
                schema.Value.ToString().ToLower() != "/Register".ToLower())

            {
                var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
                if (token != null && ValidateToken(context, token))
                {
                    await _next(context);
                    //await ReadFormDataFiles(context);
                }
                else
                {
                    context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    await context.Response.CompleteAsync();
                }
            }
            else
            {
                await _next(context);

            }
        }
        catch (Exception ex) { }

    }
    public bool ValidateToken(HttpContext context, string token)
    {
        TokenValidationParameters validationParameters = new()
        {
            ValidIssuer = _configuration["JWTSetting:Issuer"],
            ValidAudience = _configuration["JWTSetting:Audience"],
            ValidateLifetime = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWTSetting:Key"] ?? ""))
        };
        try
        {
            var handler = new JwtSecurityTokenHandler();
            handler.ValidateToken(token, validationParameters, out SecurityToken validatedToken);
            var jwtToken = (JwtSecurityToken)validatedToken;

            context.Items["Id"] = jwtToken.Claims.First(x => x.Type == TokenClaimType.Id.ToString()).Value;
            context.Items["FullName"] = jwtToken.Claims.First(x => x.Type == TokenClaimType.FullName.ToString()).Value;
            context.Items["UserName"] = jwtToken.Claims.First(x => x.Type == TokenClaimType.UserName.ToString()).Value;

            return true;
        }
        catch (SecurityTokenInvalidAudienceException)
        {
            return false;
        }
        catch
        {
            return false;
        }
    }


}
