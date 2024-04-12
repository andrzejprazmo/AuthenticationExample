namespace WebApp.Domain.Entities;

public class AccountEntity
{
    public int Id { get; set; }
    public required string Login { get; set; }
    public string Password { get; set; } = string.Empty;
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
}
