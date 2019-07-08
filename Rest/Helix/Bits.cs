// standard namespaces
using System;
using System.Runtime.Serialization;

// imported .dll's
using Newtonsoft.Json;

namespace
TwitchNet.Rest.Helix
{
    #region /bits/leaderboard

    public class
    BitsLeaderboardParameters
    {
        /// <summary>
        /// <para>
        /// The number of results to be returned.
        /// The value is clamped between the minimum and the maximum values.
        /// </para>
        /// <para>
        /// Min:        1,
        /// Max         100,
        /// Default:    10.
        /// </para>
        /// </summary>
        [QueryParameter("count")]
        public virtual int? count { get; set; }

        /// <summary>
        /// <para>The time period over which data is aggregated using started_at as the starting point.</para>
        /// <para>If set to All, started_at is ignored.</para>
        /// </summary>
        [QueryParameter("period")]
        public virtual BitsLeaderboardPeriod? period { get; set; }

        /// <summary>
        /// <para>
        /// The earliest date that the leaderboard data will be.
        /// The date is assumed to be local time and is then converted to PST by Twitch.
        /// </para>
        /// <para>
        /// If provided, the returned data is aggregated over the specified period.
        /// If the period is set to <see cref="BitsLeaderboardPeriod.All"/>, started_at is ignored.
        /// </para>
        /// </summary>
        [QueryParameter("started_at", typeof(RFC3339QueryConverter))]
        public virtual DateTime? started_at { get; set; }

        /// <summary>
        /// <para>The ID of the user who paid for the bits.</para>
        /// <para>If the user ID is provided and the count is greater than 1, the returned data includes additional users who cheered more and less than the specified user.</para>
        /// </summary>
        [QueryParameter("user_id")]
        public virtual string user_id { get; set; }
    }

    public enum
    BitsLeaderboardPeriod
    {
        /// <summary>
        /// The bits leaderboard data is aggregated over the lifetime of the broadcaster's channel.
        /// </summary>
        [EnumMember(Value = "all")]
        All = 0,

        /// <summary>
        /// The bits leaderboard data is aggregated starting on 12:00 AM PST of the day started_at resolves to.
        /// </summary>
        [EnumMember(Value = "day")]
        Day,

        /// <summary>
        /// <para>The bits leaderboard data is aggregated over the week that started_at resolves to.</para>
        /// <para>This range starts from 12:00 AM PST on the Monday of the resolved week to 12:00 AM PST on the following Monday.</para>
        /// </summary>
        [EnumMember(Value = "week")]
        Week,

        /// <summary>
        /// <para>The bits leaderboard data is aggregated over the month that started_at resolves to</para>
        /// <para>This range starts from 12:00 AM PST on the first day of the resolved month to 12:00 AM PST on the first day on the following month.</para>
        /// </summary>
        [EnumMember(Value = "month")]
        Month,

        /// <summary>
        /// <para>The bits leaderboard data is aggregated over the year that started_at resolves to.</para>
        /// <para>This range starts from 12:00 AM PST on the first day of the reoslved year to 12:00 AM PST on the first day on the following year.</para>
        /// </summary>
        [EnumMember(Value = "year")]
        Year
    }

    public class
    BitsLeaderboardData<data_page> : Data<data_page>
    {
        /// <summary>
        /// The time periord range that the leaderboard data covers.
        /// </summary>
        [JsonProperty("date_range")]
        public DateRange date_ange { get; protected set; }

        /// <summary>
        /// The number of results (users) that cheered during the time period.
        /// This is the lower of the specified count or number of entries in the leaderboard.
        /// </summary>
        [JsonProperty("total")]
        public uint total { get; protected set; }
    }

    public class
    BitsUser
    {
        /// <summary>
        /// The ID of the user who cheered.
        /// </summary>
        [JsonProperty("user_id")]
        public string user_id { get; protected set; }

        /// <summary>
        /// The display name of the user.
        /// </summary>
        [JsonProperty("user_name")]
        public string user_name { get; protected set; }

        /// <summary>
        /// The rank of the user in the bits leaderboard.
        /// </summary>
        [JsonProperty("rank")]
        public uint rank { get; protected set; }

        /// <summary>
        /// How many bits the user has cheered.
        /// </summary>
        [JsonProperty("score")]
        public uint score { get; protected set; }
    }

    #endregion
}
