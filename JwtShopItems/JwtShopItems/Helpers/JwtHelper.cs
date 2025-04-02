using Microsoft.IdentityModel.Tokens;
using MusicAPI.Models.Authentication;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MusicAPI.Helpers
{
    public static class JwtHelper
    {
        public static IEnumerable<Claim> GetClaims(this UserToken userAccounts, Guid Id)
        {
            IEnumerable<Claim> claims = new Claim[] 
            {
                new Claim("Id", userAccounts.Id.ToString()),
                new Claim(ClaimTypes.Name, userAccounts.UserName),
                new Claim(ClaimTypes.Email, userAccounts.EmailId),
                new Claim(ClaimTypes.NameIdentifier, Id.ToString()),
                new Claim(ClaimTypes.Expiration, DateTime.UtcNow.AddDays(1).ToString("MMM ddd dd yyyy HH:mm:ss tt"))
            };

            return claims;
        }

        public static IEnumerable<Claim> GetClaims(this UserToken userAccounts, out Guid Id)
        {
            Id = Guid.NewGuid();
            return GetClaims(userAccounts, Id);
        }

        public static UserToken GenerateTokenKey(UserToken token, JwtSettings jwtSettings)
        {
            ArgumentNullException.ThrowIfNull(token);
            ArgumentNullException.ThrowIfNull(jwtSettings);

            DateTime expireTime = DateTime.UtcNow.AddDays(1);
            byte[] key = Encoding.ASCII.GetBytes(jwtSettings.IssuerSigningKey);

            SigningCredentials signingCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256);
            JwtSecurityToken securityToken = new JwtSecurityToken(issuer: jwtSettings.ValidIssuer,
                                                                  audience: jwtSettings.ValidAudience,
                                                                  claims: GetClaims(token, out Guid Id),
                                                                  notBefore: new DateTimeOffset(DateTime.Now).DateTime,
                                                                  expires: new DateTimeOffset(expireTime).DateTime,
                                                                  signingCredentials: signingCredentials);
            return new UserToken()
            {
                Validaty = expireTime.TimeOfDay,
                Token = new JwtSecurityTokenHandler().WriteToken(securityToken),
                UserName = token.UserName,
                Id = token.Id,
                GuidId = Id
            };
        }
    }
}
