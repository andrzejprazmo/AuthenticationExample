using System.ComponentModel.DataAnnotations.Schema;

namespace WebApp.Domain.Entities;

public class AccountEntity
{
    public int Id { get; set; }
    public string Login { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
}
