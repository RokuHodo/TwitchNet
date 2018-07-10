// imported .dll's
using Newtonsoft.Json;

namespace
TwitchNet.Rest.Api.Bits
{
    public class
    BitsLeaderboardData<data_page> : Data<data_page>
    {
        /// <summary>
        /// The date/time range that the leaderboard data was taken over.
        /// </summary>
        [JsonProperty("date_range")]
        public DateRange    date_ange   { get; protected set; }

        /// <summary>
        /// <para>The number of results (users) returned.</para>
        /// <para>This is the lower of the specified count or number of entries in the leaderboard.</para>
        /// </summary>
        [JsonProperty("total")]
        public uint         total       { get; protected set; }
    }
}
