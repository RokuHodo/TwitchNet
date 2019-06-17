// standard namespaces
using System.Runtime.Serialization;

// project namespaces
using TwitchNet.Helpers.Json;

// imported .dll's
using Newtonsoft.Json;

namespace
TwitchNet
{
    [JsonConverter(typeof(EnumConverter))]
    public enum
    SubscriptionTier
    {
        /// <summary>
        /// Unsupported subscription plan.
        /// </summary>
        [EnumMember(Value = "")]
        Other    = 0,

        /// <summary>
        /// Free Twitch Prime subscription.
        /// Equivelent <see cref="Tier1"/>
        /// </summary>
        [EnumMember(Value = "Prime")]
        Prime   = 1,

        /// <summary>
        /// $4.99 subsctiption plan.
        /// </summary>
        [EnumMember(Value = "1000")]
        Tier1   = 1000,

        /// <summary>
        /// $9.99 subsctiption plan.
        /// </summary>
        [EnumMember(Value = "2000")]
        Tier2   = 2000,

        /// <summary>
        /// $24.99 subsctiption plan.
        /// </summary>
        [EnumMember(Value = "3000")]
        Tier3   = 3000
    }
}
