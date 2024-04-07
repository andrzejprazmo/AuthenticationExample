using FluentValidation.Results;
using System.Diagnostics.Contracts;

namespace WebApp.Core.Common
{
    public class Result<T>
    {
        public IEnumerable<ValidationFailure> Errors { get; set; }
        public bool Succeeded => !Errors.Any();

        public T? Data { get; set; }

        public Result(T data)
        {
            Data = data;
            Errors = Enumerable.Empty<ValidationFailure>();
        }
        public Result(IEnumerable<ValidationFailure> errors)
        {
            Errors = errors;
        }

        public R Match<R>(Func<T, R> success, Func<IEnumerable<ValidationFailure>, R> failure)
        {
            if (Succeeded)
            {
                return success(Data!);
            }
            return failure(Errors);
        }

        [Pure]
        public static implicit operator Result<T>(T value) => new Result<T>(value);

    }
}
