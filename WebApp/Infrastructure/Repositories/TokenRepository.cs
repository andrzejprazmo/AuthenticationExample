using Dapper;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Mail;
using System.Security.Claims;
using System.Text;
using WebApp.Core.Common.Abstract;
using WebApp.Domain.Configuration;
using WebApp.Domain.Entities;
using WebApp.Infrastructure.Providers;
using WebApp.Infrastructure.Records;

namespace WebApp.Infrastructure.Repositories;

public class TokenRepository : ITokenRepository
{
    private readonly IConfiguration _configuration;
    private readonly IDatabaseConnectionProvider _connectionProvider;


    public TokenRepository(IConfiguration configuration, IDatabaseConnectionProvider connectionProvider)
    {
        _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        _connectionProvider = connectionProvider ?? throw new ArgumentNullException(nameof(connectionProvider));
    }

    public async Task<Guid> CreateRefreshToken(int userId)
    {
        var jwtConfiguration = _configuration.GetSection("Jwt").Get<JwtSettings>() ?? throw new Exception("Cannot find JWT configuration");
        var refreshToken = Guid.NewGuid();
        string sql = @"DELETE FROM tokens WHERE account_id=@UserId;
                INSERT INTO tokens (account_id, token, expires) VALUES (@UserId, @Token, @Expires)";
        using var connection = _connectionProvider.GetConnection();
        await connection.ExecuteAsync(sql, new
        {
            UserId = userId,
            Token = refreshToken.ToString(),
            Expires = DateTime.UtcNow.AddMinutes(jwtConfiguration.RefreshTokenExpire).Ticks,
        });
        return refreshToken;
    }

    public async Task<RefreshTokenEntity?> GetRefreshToken(Guid tokenId)
    {
        string sql = @"SELECT account_id, expires FROM tokens WHERE token = @TokenId";
        using var connection = _connectionProvider.GetConnection();
        var result = await connection.QuerySingleOrDefaultAsync<RefreshTokenRecord>(sql, new { TokenId = tokenId.ToString() });
        if (result != null)
        {
            return new RefreshTokenEntity
            {
                UserId = (int)result.account_id,
                Expires = new DateTime(result.expires),
            };
        }
        return null;
    }

    public async Task<string> CreateToken(AccountEntity account)
    {
        var jwtConfiguration = _configuration.GetSection("Jwt").Get<JwtSettings>() ?? throw new Exception("Cannot find JWT configuration");

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Expires = DateTime.UtcNow.AddMinutes(jwtConfiguration.TokenExpire),
            Issuer = jwtConfiguration.Issuer,
            Audience = jwtConfiguration.Audience,
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtConfiguration.Key)), SecurityAlgorithms.HmacSha512Signature),
            Subject = new ClaimsIdentity(new[]
            {
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Sub, account.Login),
                new Claim(JwtRegisteredClaimNames.NameId, account.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Name, account.FirstName),
                new Claim(JwtRegisteredClaimNames.FamilyName, account.LastName),
            })
        };
        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return await Task.FromResult(tokenHandler.WriteToken(token));
    }
}
