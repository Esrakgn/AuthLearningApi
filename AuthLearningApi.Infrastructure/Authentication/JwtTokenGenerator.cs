using AuthLearningApi.Application.DTOs.Auth;
using AuthLearningApi.Domain.Entities;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AuthLearningApi.Infrastructure.Authentication;

public class JwtTokenGenerator
{
    private readonly JwtSettings _jwtSettings;

    public JwtTokenGenerator(IOptions<JwtSettings> jwtSettings)
    {
        _jwtSettings = jwtSettings.Value;
    }

    public AuthResponse CreateToken(AppUser user)
    {
        var expiration = DateTime.UtcNow.AddMinutes(_jwtSettings.ExpirationInMinutes);

        //token içine koyduğumuz kullanıcı bilgileri
        var claims = new List<Claim>
        {
             new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
             new Claim(ClaimTypes.Name, user.FullName),
             new Claim(ClaimTypes.Email, user.Email),
             new Claim(ClaimTypes.Role, user.Role)
             //kullanıcının rolünü token içine koyuyor
        };

        //tokenı üretirken bu key kullanılır,tokenı kontrol ederken de aynı key ile doğrulanır
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        //HmacSha256 = secret key + hash algoritması ile imzalama yöntemi

        var token = new JwtSecurityToken(
            issuer: _jwtSettings.Issuer,
            audience: _jwtSettings.Audience,
            claims: claims,
            expires: expiration,
            signingCredentials: credentials);

        var tokenHandler = new JwtSecurityTokenHandler();
        var jwtToken = tokenHandler.WriteToken(token);

        return new AuthResponse
        {
            Token = jwtToken,
            Expiration = expiration
        };
    }
}
