using MediatR;
using WebApp.Core.Common.Response;
using WebApp.Core.Common;
using WebApp.Core.Abstract;
using WebApp.Core.Common.Const;

namespace WebApp.Core.Queries.RefreshToken
{
    public record RefreshTokenRequest(string Token, Guid RefreshToken) : IRequest<Result<TokenDto>>;
    public class RefreshTokenHandler : IRequestHandler<RefreshTokenRequest, Result<TokenDto>>
    {
        private readonly ITokenRepository _tokenRepository;
        private readonly IAccountRepository _accountRepository;

        public RefreshTokenHandler(ITokenRepository tokenRepository, IAccountRepository accountRepository)
        {
            _tokenRepository = tokenRepository ?? throw new ArgumentNullException(nameof(tokenRepository));
            _accountRepository = accountRepository ?? throw new ArgumentNullException(nameof(accountRepository));
        }

        public async Task<Result<TokenDto>> Handle(RefreshTokenRequest request, CancellationToken cancellationToken)
        {
            var refreshToken = await _tokenRepository.GetRefreshToken(request.RefreshToken);
            if (refreshToken != null && refreshToken!.Expires > DateTime.UtcNow)
            {
                var account = await _accountRepository.GetAccountById(refreshToken.UserId);
                if (account != null)
                {
                    var token = await _tokenRepository.CreateToken(account);
                    var newRefreshToken = await _tokenRepository.CreateRefreshToken(account.Id);
                    return new TokenDto
                    {
                        RefreshToken = newRefreshToken,
                        Token = token,
                    };

                }
            }
            return new Result<TokenDto>([ErrorCodes.GetValidationFailure(nameof(RefreshTokenRequest.RefreshToken), ErrorCodes.AUTHENTICATE_REFRESHTOKEN_EXPIRED)]);
        }
    }
}
