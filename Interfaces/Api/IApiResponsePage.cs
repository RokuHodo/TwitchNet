using TwitchNet.Models.Api;

namespace
TwitchNet.Interfaces.Api
{
    public interface
    IApiResponsePage<data_type> : IApiResponse
    where data_type : class, new()
    {
        /// <summary>
        /// Contains the deserialized result from the Twitch API.
        /// </summary>
        ApiDataPage<data_type> result { get; }
    }
}
