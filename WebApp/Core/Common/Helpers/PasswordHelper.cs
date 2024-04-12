using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace WebApp.Core.Common.Helpers;

public static class PasswordHelper
{
    public static string EncryptSSHA512(string login, string password)
    {
        using var sha512 = SHA512.Create();
        var bytes = Encoding.UTF8.GetBytes($"{password}.{login.ToLower()}");
        var hash = sha512.ComputeHash(bytes);
        var salt = Encoding.UTF8.GetBytes($".{login.ToLower()}");
        hash = hash.Concat(salt).ToArray();
        return "{SSHA512}" + Convert.ToBase64String(hash);
    }

    public static bool IsStrong(string password)
    {
        var regex = new Regex(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*[0-9])(?=.*[!@#\$%\^&\*]).{8,}$");
        return regex.IsMatch(password);
    }
}
