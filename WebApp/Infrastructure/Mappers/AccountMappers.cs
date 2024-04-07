using System.Security.Principal;
using WebApp.Domain.Entities;
using WebApp.Infrastructure.Records;

namespace WebApp.Infrastructure.Mappers;

public static class AccountMappers
{
    internal static AccountEntity? ToAccountEntity(this AccountRecord data)
    {
        if (data is not null) 
        {
            return new AccountEntity
            {
                Id = (int)data.id,
                Login = data.login,
                Password = data.password,
                FirstName = data.first_name,
                LastName = data.last_name,
            };
        }
        return null;
    }
}
