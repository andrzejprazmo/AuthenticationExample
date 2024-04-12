using FluentValidation.Results;

namespace WebApp.Core.Common.Const;

public static class ErrorCodes
{
    public const string AUTHENTICATE_BAD_USER_OR_PASSWORD = "AUTHENTICATE_BAD_USER_OR_PASSWORD";
    public const string AUTHENTICATE_REFRESHTOKEN_EXPIRED = "AUTHENTICATE_REFRESHTOKEN_EXPIRED";

    public const string ACCOUNT_LOGIN_EXISTS = "ACCOUNT_LOGIN_EXISTS";
    public const string ACCOUNT_PASSWORD_IS_WEAK = "ACCOUNT_PASSWORD_IS_WEAK";
    public const string ACCOUNT_CANNOT_DELETE_LAST = "ACCOUNT_CANNOT_DELETE_LAST";

    public static ValidationFailure GetValidationFailure(string propertyName, string errorCode)
        => new ValidationFailure
        {
            PropertyName = propertyName,
            ErrorCode = errorCode,
        };
}
