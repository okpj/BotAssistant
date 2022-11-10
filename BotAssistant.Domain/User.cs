namespace BotAssistant.Domain;

public class User : IEntity<long>
{
    public long Id { get; set; }

    public string? UserName { get; set; }

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

}