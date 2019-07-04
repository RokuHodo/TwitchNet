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
        /// <para>The number of results to be returned.</para>
        /// <para>
        /// Min:        1,
        /// Max         100,
        /// Default:    10.
        /// </para>
        /// </summary>
        [QueryParameter("count")]
        public virtual int? count { get; set; }

        /// <summary>
        /// <para>The time period over which data is aggregated according to started_at.</para>
        /// <para>If set to all, started_at is ignored.</para>
        /// </summary>
        [QueryParameter("period")]
        public virtual BitsLeaderboardPeriod? period { get; set; }

        // TODO: Make an alternate converter that assumes you're supplying PST? Right now this auto-converts time into your local time zone.

        /// <summary>
        /// <para>
        /// The start date/time for the returned leaderboard data.
        /// The date/time is converted and passed as local time and is then converted to PST by Twitch.
        /// </para>
        /// <para>
        /// If specified, the returned data is aggregated over the specified period.
        /// If period is set to <see cref="BitsLeaderboardPeriod.All"/>, started_at is ignored.
        /// </para>
        /// </summary>
        [QueryParameter("started_at", typeof(RFC3339QueryConverter))]
        public virtual DateTime? started_at { get; set; }

        /// <summary>
        /// <para>The ID of the user who paid for the bits.</para>
        /// <para>
        /// If specified and count is greater than 1, the returned data includes additional users who cheered more/less than the specified user.
        /// If not specified, the top users are returned.
        /// </para>
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
        /// The bits leaderboard data is aggregated starting on 00:00:00 of the day started_at resolves to.
        /// </summary>
        [EnumMember(Value = "day")]
        Day,

        /// <summary>
        /// <para>The bits leaderboard data is aggregated over the week that started_at resolves to.</para>
        /// <para>This range starts from 00:00:00 on the Monday of the resolved week to 00:00:00 on the following Monday.</para>
        /// </summary>
        [EnumMember(Value = "week")]
        Week,

        /// <summary>
        /// <para>The bits leaderboard data is aggregated over the month that started_at resolves to</para>
        /// <para>This range starts from 00:00:00 on the first day of the resolved month to 00:00:00 on the first day on the following month.</para>
        /// </summary>
        [EnumMember(Value = "month")]
        Month,

        /// <summary>
        /// <para>The bits leaderboard data is aggregated over the year that started_at resolves to.</para>
        /// <para>This range starts from 00:00:00 on the first day of the resolved year to 00:00:00 on the first day on the following year.</para>
        /// </summary>
        [EnumMember(Value = "year")]
        Year
    }

    public class
    BitsLeaderboardData<data_page> : Data<data_page>
    {
        /// <summary>
        /// The date/time range that the leaderboard data was taken over.
        /// </summary>
        [JsonProperty("date_range")]
        public DateRange date_ange { get; protected set; }

        /// <summary>
        /// <para>The number of results (users) returned.</para>
        /// <para>This is the lower of the specified count or number of entries in the leaderboard.</para>
        /// </summary>
        [JsonProperty("total")]
        public uint total { get; protected set; }
    }

    public class
    BitsUser
    {
        /// <summary>
        /// The ID of the user.
        /// </summary>
        [JsonProperty("user_id")]
        public string user_id { get; protected set; }

        /// <summary>
        /// The login of the user.
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
