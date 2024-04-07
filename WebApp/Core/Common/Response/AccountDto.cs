namespace WebApp.Core.Common.Response
{
    public class AccountDto
    {
        public int Id { get; set; }
        public required string Login { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
    }
}
