namespace Messages;
public class Message
{
    public string Greeting { get;set; }
    public string MachineName { get;set;}
    public DateTimeOffset CreatedAt { get; set;}
    public Message(string greeting) {
        this.Greeting = greeting;
        this.MachineName = Environment.MachineName;
        this.CreatedAt = DateTimeOffset.Now;
    }
    public override string ToString() {
        return $"Greeting {Greeting} from {MachineName} at {CreatedAt}";
    }
}
