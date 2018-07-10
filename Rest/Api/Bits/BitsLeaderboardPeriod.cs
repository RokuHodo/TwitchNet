// standard namespaces
using System.Runtime.Serialization;

namespace
TwitchNet.Rest.Api.Bits
{
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
}
