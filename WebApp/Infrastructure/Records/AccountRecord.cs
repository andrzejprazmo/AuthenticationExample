namespace WebApp.Infrastructure.Records;

internal class AccountRecord
{
    public long id { get; set; }
    public string login { get; set; } = string.Empty;
    public string password { get; set; } = string.Empty;
    public string first_name { get; set; } = string.Empty;
    public string last_name { get; set; } = string.Empty;
}
