namespace WebApp;

public class Contact
{
    // Mandatory properties
    public string Name { get; set; }
    public string Surname { get; set; }
    public string PhoneNumber { get; set; }
    // Optional properties
    public string Email { get; set; }
    public string Address { get; set; }
    public string Notes { get; set; }
}
public interface ILinkedinRepository
{
    Task<string> GetName();
}
public interface IContactBuilder
{
    IContactBuilder WithName(string name);
    IContactBuilder WithSurname(string surname);
    IContactBuilder WithPhone(string phoneNumber);
    IContactBuilder WithEmail(string email);
    IContactBuilder WithAddress(string address);
    IContactBuilder WithNotes(string notes);
    Task<Contact> Build();
}
public class LinkedRepository : ILinkedinRepository
{
    public Task<string> GetName()
    {
        return Task.FromResult("John");
    }
}
public class ContactBuilder : IContactBuilder
{
    private readonly ILinkedinRepository _linkedinRepository;
    private readonly Contact contact;
    private Queue<Func<Task>> builderQueue;
    public ContactBuilder(ILinkedinRepository linkedinRepository)
    {
        this.contact = new Contact();
        this.builderQueue = new Queue<Func<Task>>();
        _linkedinRepository = linkedinRepository;
    }
    public async Task<Contact> Build()
    {
        while (builderQueue.Count > 0)
        {
            var builderAction = builderQueue.Dequeue();
            await builderAction();
        }
        return this.contact;
    }

    public IContactBuilder WithAddress(string address)
    {
        return this;
    }

    public IContactBuilder WithEmail(string email)
    {
        return this;
    }

    public IContactBuilder WithName(string name)
    {
        builderQueue.Enqueue(() => Task.Run(async () =>
        {
            var name = await this._linkedinRepository.GetName();
            contact.Name = name;
        }));
        return this;
    }

    public IContactBuilder WithNotes(string notes)
    {
        return this;
    }

    public IContactBuilder WithPhone(string phoneNumber)
    {
        return this;
    }

    public IContactBuilder WithSurname(string surname)
    {
        return this;
    }
}

public class ContactBuilderConsumer
{
    public async Task<Contact> Consume()
    {
        var contact = await new ContactBuilder(new LinkedRepository())
            .WithName("John")
            .WithSurname("Doe")
            .WithPhone("123-456-7890")
            .WithEmail("")
            .Build();
        return contact;
    }
}
