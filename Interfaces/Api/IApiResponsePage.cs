using TwitchNet.Models.Api;

namespace
TwitchNet.Interfaces.Api
{
    public interface
    IApiResponsePage<type> : IApiResponse
    where type : class, new()
    {
        /// <summary>
        /// Contains the deserialized result from the Twitch API.
        /// </summary>
        ApiDataPage<type> result { get; }
    }
}
