﻿// standard namespaces
using System.Runtime.Serialization;

// imported namespaces
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace
TwitchNet
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum
    UserType
    {
        /// <summary>
        /// The user is a normal user.
        /// </summary>
        [EnumMember(Value = "")]
        None       = 0,

        /// <summary>
        /// The user is a channel moderator.
        /// This is only applicanle in chat messages and will never be part of a valid API response.
        /// </summary>
        [EnumMember(Value = "mod")]
        Mod         = 1,

        /// <summary>
        /// The user is a Twitch global mod.
        /// </summary>
        [EnumMember(Value = "global_mod")]
        GlobalMod   = 2,

        /// <summary>
        /// The user is a Twitch staff member.
        /// </summary>
        [EnumMember(Value = "staff")]
        Staff       = 3,

        /// <summary>
        /// The user is a Twitch admin.
        /// </summary>
        [EnumMember(Value = "admin")]
        Admin       = 4,
    }
}