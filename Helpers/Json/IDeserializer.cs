namespace TwitchNet.Helpers.Json
{
    public interface
    IDeserializer
    {
        result_type Deserialize<result_type>(string content);
    }
}
