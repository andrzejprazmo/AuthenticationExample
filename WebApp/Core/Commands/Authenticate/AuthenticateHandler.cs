using FluentValidation;
using FluentValidation.Results;
using MassTransit;
using MediatR;
using WebApp.Core.Common;
using WebApp.Core.Common.Abstract;
using WebApp.Core.Common.Const;
using WebApp.Core.Common.Extensions;
using WebApp.Core.Common.Helpers;
using WebApp.Core.Common.Response;
using WebApp.Domain.Notifications;

namespace WebApp.Core.Commands.Authenticate;

public record AuthenticateRequest(string Login, string Password) : IRequest<Result<TokenDto>>;

public class AuthenticateHandler : IRequestHandler<AuthenticateRequest, Result<TokenDto>>
{
    private readonly IValidator<AuthenticateRequest> _validator;
    private readonly IAccountRepository _accountRepository;
    private readonly ITokenRepository _tokenRepository;
    private readonly IHttpContextAccessor _contextAccessor;
    private readonly ILogger<AuthenticateHandler> _logger;
    private readonly IBus _bus;

    public AuthenticateHandler(IValidator<AuthenticateRequest> validator, IAccountRepository accountRepository, ITokenRepository tokenRepository, IHttpContextAccessor contextAccessor, ILogger<AuthenticateHandler> logger, IBus bus)
    {
        _validator = validator ?? throw new ArgumentNullException(nameof(validator));
        _accountRepository = accountRepository ?? throw new ArgumentNullException(nameof(accountRepository));
        _tokenRepository = tokenRepository ?? throw new ArgumentNullException(nameof(tokenRepository));
        _contextAccessor = contextAccessor ?? throw new ArgumentNullException(nameof(contextAccessor));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _bus = bus ?? throw new ArgumentNullException(nameof(bus));
    }

    public async Task<Result<TokenDto>> Handle(AuthenticateRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var validationResult = _validator.Validate(request);
            if (validationResult.IsValid)
            {
                //_ = Task.Run(async () => 
                //{
                //    _logger.LogInformation("Executing long-term query for user {Login}", request.Login);
                //    var data = await _accountRepository.ExecuteLongTermQuery(cancellationToken);
                //    _logger.LogInformation("Completed long-term query for user {Login}", request.Login);
                //});
                var account = await _accountRepository.GetAccountByLogin(request.Login, cancellationToken);
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
                _logger.LogInformation("Executing long-term query for user {Login}", request.Login);
                var data = await _accountRepository.ExecuteLongTermQuery(cancellationToken);
                _logger.LogInformation("Completed long-term query for user {Login}", request.Login);
                await PublishAuthEvent(request, cancellationToken);
                _logger.LogWarning("Failed to authenticate user with login {Login}", request.Login);
            }
            return new Result<TokenDto>([ErrorCodes.GetValidationFailure(nameof(AuthenticateRequest.Login), ErrorCodes.AUTHENTICATE_BAD_USER_OR_PASSWORD)]);

        }
        //catch (TaskCanceledException)
        //{
        //    _logger.LogWarning("Authentication request for user {Login} was cancelled", request.Login);
        //    return new Result<TokenDto>([ErrorCodes.GetValidationFailure(nameof(AuthenticateRequest.Login), ErrorCodes.AUTHENTICATE_BAD_USER_OR_PASSWORD)]);
        //}
        catch (Exception e)
        {
            _logger.LogError(e, "An error occurred while authenticating user with login {Login}", request.Login);
            throw;
        }
    }

    private async Task PublishAuthEvent(AuthenticateRequest request, CancellationToken cancellationToken)
    {
        try
        {
            await Task.Run(async () =>
            {
                await _bus.Publish(new UserAuthenticatedEvent
                {
                    Login = request.Login,
                    Timestamp = DateTime.UtcNow
                }, cancellationToken);
            });
        }
        catch (TaskCanceledException)
        {
            _logger.LogWarning("Authentication request for user {Login} was cancelled", request.Login);
        }
        catch (OperationCanceledException)
        {
            _logger.LogWarning("Authentication request for user {Login} was cancelled", request.Login);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "An error occurred while publishing authentication event for user with login {Login}", request.Login);
        }
    }
}
