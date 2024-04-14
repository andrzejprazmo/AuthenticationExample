using MediatR;
using WebApp.Core.Common.Response;
using WebApp.Core.Common;
using WebApp.Core.Common.Const;
using WebApp.Core.Common.Abstract;
using WebApp.Core.Common.Extensions;

namespace WebApp.Core.Commands.RefreshToken;

public record RefreshTokenRequest(string Token) : IRequest<Result<TokenDto>>;
public class RefreshTokenHandler : IRequestHandler<RefreshTokenRequest, Result<TokenDto>>
{
    private readonly ITokenRepository _tokenRepository;
    private readonly IAccountRepository _accountRepository;
    private readonly IHttpContextAccessor _contextAccessor;

    public RefreshTokenHandler(ITokenRepository tokenRepository, IAccountRepository accountRepository, IHttpContextAccessor contextAccessor)
    {
        _tokenRepository = tokenRepository ?? throw new ArgumentNullException(nameof(tokenRepository));
        _accountRepository = accountRepository ?? throw new ArgumentNullException(nameof(accountRepository));
        _contextAccessor = contextAccessor ?? throw new ArgumentNullException(nameof(contextAccessor));
    }

    public async Task<Result<TokenDto>> Handle(RefreshTokenRequest request, CancellationToken cancellationToken)
    {
        var refreshToken = _contextAccessor.GetRefreshToken();
        if (refreshToken.HasValue)
        {
            var refreshTokenEntity = await _tokenRepository.GetRefreshToken(refreshToken.Value);
            if (refreshTokenEntity != null && refreshTokenEntity!.Expires > DateTime.UtcNow)
            {
                var account = await _accountRepository.GetAccountById(refreshTokenEntity.UserId);
                if (account != null)
                {
                    var token = await _tokenRepository.CreateToken(account);
                    var newRefreshToken = await _tokenRepository.CreateRefreshToken(account.Id);
                    _contextAccessor.SetRefreshToken(newRefreshToken);
                    return new TokenDto
                    {
                        Token = token,
                    };

                }
            }
        }
        return new Result<TokenDto>([ErrorCodes.GetValidationFailure("RefreshToken", ErrorCodes.AUTHENTICATE_REFRESHTOKEN_EXPIRED)]);
    }
}
