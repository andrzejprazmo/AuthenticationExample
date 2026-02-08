using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Microsoft.AspNetCore.Identity;
using WebApp.Core.Commands.Authenticate;
using WebApp.Core.Common;
using WebApp.Core.Common.Abstract;
using WebApp.Core.Common.Const;
using WebApp.Core.Common.Extensions;
using WebApp.Core.Common.Helpers;
using WebApp.Core.Common.Response;
using WebApp.Domain.Entities;

namespace WebApp.Core.Commands.Login.Abstract;

public abstract class LoginHandler<TRequest>: IRequestHandler<TRequest, Result<TokenDto>>
    where TRequest : IRequest<Result<TokenDto>>
{
    private readonly IValidator<AuthenticateRequest> _validator;
    private readonly IAccountRepository _accountRepository;
    private readonly ITokenRepository _tokenRepository;
    private readonly IHttpContextAccessor _contextAccessor;

    protected LoginHandler(IValidator<AuthenticateRequest> validator, IAccountRepository accountRepository, ITokenRepository tokenRepository, IHttpContextAccessor contextAccessor)
    {
        _validator = validator ?? throw new ArgumentNullException(nameof(validator));
        _accountRepository = accountRepository ?? throw new ArgumentNullException(nameof(accountRepository));
        _tokenRepository = tokenRepository ?? throw new ArgumentNullException(nameof(tokenRepository));
        _contextAccessor = contextAccessor ?? throw new ArgumentNullException(nameof(contextAccessor));
    }

    public async Task<Result<TokenDto>> Handle(TRequest request, CancellationToken ct)
    {
        var validationResult = _validator.Validate(request as AuthenticateRequest);
        if (validationResult.IsValid)
        {
            var data = GetRequest(request);
            var account = await _accountRepository.GetAccountByLogin(data.Login);
            if (account is not null)
            {
                var password = PasswordHelper.EncryptSSHA512(account.Login, data.Password);
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

    protected abstract AuthenticateRequest GetRequest(TRequest request);
    protected abstract Task<bool> ValidatePasswordAsync(AccountEntity user, string password);
}

// Request dla plain text
public record LoginWithPlainTextRequest(string Username, string Password)
    : IRequest<Result<TokenDto>>;

public class PlainTextAuthHandler
    : LoginHandler<LoginWithPlainTextRequest>
{
    public PlainTextAuthHandler(IValidator<AuthenticateRequest> validator, IAccountRepository accountRepository, ITokenRepository tokenRepository, IHttpContextAccessor contextAccessor) : base(validator, accountRepository, tokenRepository, contextAccessor)
    {
    }

    protected override AuthenticateRequest GetRequest(LoginWithPlainTextRequest request)
    {
        return new AuthenticateRequest(request.Username, request.Password);
    }

    protected override Task<bool> ValidatePasswordAsync(AccountEntity user, string password)
    {
        throw new NotImplementedException();
    }
}

// Request dla SHA512
public record LoginWithSha512Request(string Username, string HashedPassword)
    : IRequest<Result<TokenDto>>;

public class Sha512AuthHandler
    : LoginHandler<LoginWithSha512Request>
{
    public Sha512AuthHandler(IValidator<AuthenticateRequest> validator, IAccountRepository accountRepository, ITokenRepository tokenRepository, IHttpContextAccessor contextAccessor) : base(validator, accountRepository, tokenRepository, contextAccessor)
    {
    }

    protected override AuthenticateRequest GetRequest(LoginWithSha512Request request)
    {
        throw new NotImplementedException();
    }

    protected override Task<bool> ValidatePasswordAsync(AccountEntity user, string password)
    {
        throw new NotImplementedException();
    }
}
