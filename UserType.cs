// imported namespaces
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace
TwitchNet
{
    // TODO: Add custom converter for the deserializer.
    [JsonConverter(typeof(StringEnumConverter))]
    public enum
    UserType
    {
        /// <summary>
        /// The user is a normal user.
        /// </summary>
        None       = 0,

        /// <summary>
        /// The user is a channel moderator.
        /// This is only applicanle in chat messages and will never be part of a valid API response.
        /// </summary>
        Mod         = 1,

        /// <summary>
        /// The user is a Twitch global mod.
        /// </summary>
        GlobalMod   = 2,

        /// <summary>
        /// The user is a Twitch staff member.
        /// </summary>
        Staff       = 3,

        /// <summary>
        /// The user is a Twitch admin.
        /// </summary>
        Admin       = 4,
    }
}
