using System;

using TwitchNet.Helpers;

namespace
TwitchNet.Rest.Api.Bits
{
    public class
    BitsLeaderboardQueryParameters
    {
        private ClampedNumber<uint> _count = new  ClampedNumber<uint>(1, 100, 10);

        /// <summary>
        /// <para>The number of results to be returned.</para>
        /// <para>
        /// Min:        1,
        /// Max         100,
        /// Default:    10.
        /// </para>
        /// </summary>
        [QueryParameter("count")]
        public uint count
        {
            get
            {
                return _count.value;
            }
            set
            {
                _count.value = value;
            }
        }


        /// <summary>
        /// <para>The time period over which data is aggregated according to started_at.</para>
        /// <para>If set to all, started_at is ignored.</para>
        /// </summary>
        [QueryParameter("period")]
        public BitsLeaderboardPeriod    period      { get; set; }

        /// <summary>
        /// <para>The start date/time for the returned leaderboard data.</para>
        /// <para>
        /// If specified, the returned data is aggregated over the specified period.
        /// If period is set to <see cref="BitsLeaderboardPeriod.All"/>, started_at is ignored.
        /// </para>
        /// </summary>
        [QueryParameter("started_at", typeof(RFC3339QueryFormatter))]
        public DateTime?                started_at  { get; set; }

        /// <summary>
        /// <para>The ID of the user who paid for the bits.</para>
        /// <para>
        /// If specified and count is greater than 1, the returned data includes additional users who cheered more/less than the specified user.
        /// If not specified, the top users are returned.
        /// </para>
        /// </summary>
        [QueryParameter("user_id")]
        public string                   user_id     { get; set; }

        /// <summary>
        /// Creates a new blank instance of the <see cref="BitsLeaderboardQueryParameters"/> class.
        /// </summary>
        public BitsLeaderboardQueryParameters()
        {

        }
    }
}
