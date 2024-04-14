using FluentValidation;
using FluentValidation.Results;
using MediatR;
using WebApp.Core.Common;
using WebApp.Core.Common.Abstract;
using WebApp.Core.Common.Const;
using WebApp.Core.Common.Extensions;
using WebApp.Core.Common.Helpers;
using WebApp.Core.Common.Response;

namespace WebApp.Core.Commands.Authenticate;

public record AuthenticateRequest(string Login, string Password) : IRequest<Result<TokenDto>>;

public class AuthenticateHandler : IRequestHandler<AuthenticateRequest, Result<TokenDto>>
{
    private readonly IValidator<AuthenticateRequest> _validator;
    private readonly IAccountRepository _accountRepository;
    private readonly ITokenRepository _tokenRepository;
    private readonly IHttpContextAccessor _contextAccessor;

    public AuthenticateHandler(IValidator<AuthenticateRequest> validator, IAccountRepository accountRepository, ITokenRepository tokenRepository, IHttpContextAccessor contextAccessor)
    {
        _validator = validator ?? throw new ArgumentNullException(nameof(validator));
        _accountRepository = accountRepository ?? throw new ArgumentNullException(nameof(accountRepository));
        _tokenRepository = tokenRepository ?? throw new ArgumentNullException(nameof(tokenRepository));
        _contextAccessor = contextAccessor ?? throw new ArgumentNullException(nameof(contextAccessor));
    }

    public async Task<Result<TokenDto>> Handle(AuthenticateRequest request, CancellationToken cancellationToken)
    {
        var validationResult = _validator.Validate(request);
        if (validationResult.IsValid)
        {
            var account = await _accountRepository.GetAccountByLogin(request.Login);
            if (account is not null)
            {
                var password = PasswordHelper.EncryptSSHA512(account.Login, request.Password);
                if (string.Equals(password, account.Password, StringComparison.Ordinal))
                {
                    var token = await _tokenRepository.CreateToken(account);
                    var refreshToken = await _tokenRepository.CreateRefreshToken(account.Id);
                    _contextAccessor.SetRefreshToken(refreshToken);
                    return new TokenDto
                    {
                        Token = token,
                    };
                }
            }
        }
        return new Result<TokenDto>([ErrorCodes.GetValidationFailure(nameof(AuthenticateRequest.Login), ErrorCodes.AUTHENTICATE_BAD_USER_OR_PASSWORD)]);
    }
}
