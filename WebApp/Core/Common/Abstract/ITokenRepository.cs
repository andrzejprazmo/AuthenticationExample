using WebApp.Domain.Entities;

namespace WebApp.Core.Common.Abstract;

public interface ITokenRepository
{
    Task<string> CreateToken(AccountEntity account);
    Task<Guid> CreateRefreshToken(int userId);
    Task<RefreshTokenEntity?> GetRefreshToken(Guid tokenId);
}
