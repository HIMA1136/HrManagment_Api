using Domain.Entites;
using System.IdentityModel.Tokens.Jwt;

namespace Application.Helpers.JwtConfingrations;
public interface IJwtUtils
{
    Task<JwtSecurityToken> CreateJwtToken(AppUser user);
    string ReadTokenClaim(TokenClaimType type);
}
