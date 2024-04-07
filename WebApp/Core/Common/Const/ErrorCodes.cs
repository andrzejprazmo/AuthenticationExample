using FluentValidation.Results;

namespace WebApp.Core.Common.Const;

public static class ErrorCodes
{
    public const string AUTHENTICATE_BAD_USER_OR_PASSWORD = "AUTHENTICATE_BAD_USER_OR_PASSWORD";

    public static ValidationFailure GetValidationFailure(string propertyName, string errorCode)
        => new ValidationFailure
        {
            PropertyName = propertyName,
            ErrorCode = errorCode,
        };
}
