namespace TwitchNet.Helpers.Json
{
    public interface
    ISerializer
    {
        string content_type { get; set; }

        string Serialize(object obj);
    }
}
