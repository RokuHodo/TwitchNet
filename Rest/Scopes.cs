// standard namespaces
using System;
using System.Runtime.Serialization;

// project namespaces
using TwitchNet.Helpers.Json;

// imported .dll's
using Newtonsoft.Json;

namespace
TwitchNet.Rest
{
    [Flags]
    [JsonConverter(typeof(EnumConverter))]
    public enum
    Scopes
    {
        /// <summary>
        /// Unsupported scope.
        /// </summary>
        [EnumMember(Value = "")]
        Other = 0,

        /// <summary>
        /// <para>analytics:read:extensions</para>
        /// <para>Allows extension analytic data to be obtained.</para>
        /// </summary>
        [EnumMember(Value = "analytics:read:extensions")]
        AnalyticsReadExtensions = 1 << 0,

        /// <summary>
        /// <para>analytics:read:games</para>
        /// <para>Allows game analytic data to be obtained.</para>
        /// </summary>
        [EnumMember(Value = "analytics:read:games")]
        AnalyticsReadGames      = 1 << 1,

        /// <summary>
        /// <para>bits:read</para>
        /// <para>Allows bits information to be obtained.</para>
        /// </summary>
        [EnumMember(Value = "bits:read")]
        BitsRead                = 1 << 2,

        /// <summary>
        /// <para>clips:edit</para>
        /// <para>Allows for clips to be edited.</para>
        /// </summary>
        [EnumMember(Value = "clips:edit")]
        ClipsEdit               = 1<< 3,

        /// <summary>
        /// <para>user:edit</para>
        /// <para>Allows the user's information to be changed.</para>
        /// </summary>
        [EnumMember(Value = "user:edit")]
        UserEdit                = 1 << 4,

        /// <summary>
        /// <para>user:edit:broadcast</para>
        /// <para>Allows the channel's broadcast configuration to be changes, including extensions.</para>
        /// </summary>
        [EnumMember(Value = "user:edit:broadcast")]
        UserEditBroadcast       = 1 << 5,

        /// <summary>
        /// <para>user:edit:broadcast</para>
        /// <para>Allows the channel's broadcast configuration to be changes, including extensions.</para>
        /// </summary>
        [EnumMember(Value = "user:read:broadcast")]
        UserReadBroadcast       = 1 << 6,

        /// <summary>
        /// <para>user:read:email</para>
        /// <para>Allows for the user's email address to be obtained.</para>
        /// </summary>
        [EnumMember(Value = "user:read:email")]
        UserReadEmail           = 1 << 7
    }
}
