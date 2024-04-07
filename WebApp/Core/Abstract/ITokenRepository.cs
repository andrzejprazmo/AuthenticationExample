using WebApp.Domain.Entities;

namespace WebApp.Core.Abstract;

public interface ITokenRepository
{
    Task<string> CreateToken(AccountEntity account);
    Task<Guid> CreateRefreshToken(int userId);
}
