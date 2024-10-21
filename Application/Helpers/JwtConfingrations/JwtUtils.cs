using Application.Model.Common;
using Domain.Entites;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Application.Helpers.JwtConfingrations;
public class JwtUtils : IJwtUtils
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly UserManager<AppUser> _userManager;
    private readonly IOptions<JWTSetting> _JWTSetting;

    public JwtUtils(UserManager<AppUser> userManager, IOptions<JWTSetting> jWTSetting, IHttpContextAccessor httpContextAccessor)
    {
        _userManager = userManager;
        _JWTSetting = jWTSetting;
        _httpContextAccessor = httpContextAccessor;
    }
    public async Task<JwtSecurityToken> CreateJwtToken(AppUser user)
    {
        var userClaims = await _userManager.GetClaimsAsync(user);

        var roleClaims = new List<Claim>();

        try
        {
            var claims1 = new[]
      {
                new Claim("Id", user.Id.ToString()) ,
                new Claim("FullName", user.FullName.ToString()) ,
                new Claim("UserName", user.UserName.ToString()) ,
                new Claim("TypeAccountValue", user.TypeAccountValue.ToString()) ,
                new Claim("TypeAccountText", user.TypeAccountText.ToString()) ,
                new Claim("EmployeeId", user.EmployeeId.ToString()) ,
        }
      .Union(userClaims)
      .Union(roleClaims);

            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_JWTSetting.Value.Key ?? ""));
            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);
            var jwtSecurityToken = new JwtSecurityToken(
            issuer: _JWTSetting.Value.Issuer,
            audience: _JWTSetting.Value.Audience,
            notBefore: DateTime.UtcNow,
            claims: claims1,
            expires: DateTime.UtcNow.AddDays(_JWTSetting.Value.DurationInDays),
            signingCredentials: signingCredentials);

            return jwtSecurityToken;
        }
        catch (Exception e)
        {

            throw;
        }





    }
    public string ReadTokenClaim(TokenClaimType type)
    {
        var nameCliam = Enum.GetName(typeof(TokenClaimType), type) ?? "Id";
        return _httpContextAccessor.HttpContext.User.FindFirst(nameCliam)?.Value;
    }
}
