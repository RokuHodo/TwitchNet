//standard namespaces
using System.Collections.Generic;

// project namespaces
using TwitchNet.Helpers;

namespace
TwitchNet.Rest.Api.Streams
{
    public class
    StreamsParameters : PagingParameters, IPagingParameters
    {
        /// <summary>
        /// A list of communities on Twitch.
        /// Maximum: 100 id's.
        /// If more than 100 id's are specified, only the first 100 will be added.
        /// </summary>
        [QueryParameter("community_id", typeof(SeparateQueryConverter))]
        public virtual List<string> community_ids { get; set; }

        /// <summary>
        /// A list of game ID's.
        /// Maximum: 100 id's.
        /// If more than 100 id's are specified, only the first 100 will be added.
        /// </summary>
        [QueryParameter("game_id", typeof(SeparateQueryConverter))]
        public virtual List<string> game_ids { get; set; }

        /// <summary>
        /// The language of the stream.
        /// This is the language selected at the home page, not the language found in the Twitch dashboard.
        /// Bitfield enum.
        /// </summary>
        [QueryParameter("language", typeof(SeparateQueryConverter))]
        public virtual StreamLanguage?  language    { get; set; }

        /// <summary>
        /// A list of user ID's.
        /// Maximum: 100 id's.
        /// If more than 100 id's are specified, only the first 100 will be added.
        /// </summary>
        [QueryParameter("user_id", typeof(SeparateQueryConverter))]
        public virtual List<string> user_ids { get; set; }

        /// <summary>
        /// A list of user login names.
        /// Maximum: 100 names.
        /// If more than 100 names are specified, only the first 100 will be added.
        /// </summary>
        [QueryParameter("user_login", typeof(SeparateQueryConverter))]
        public virtual List<string> user_logins { get; set; }

        /// <summary>
        /// The cursor that tells the server where to start fetching the next set of results, in a multi-page response.
        /// </summary>
        [QueryParameter("before")]
        public string before { get; set; }

        public StreamsParameters()
        {
            community_ids   = new List<string>();
            game_ids        = new List<string>();
            user_ids        = new List<string>();
            user_logins     = new List<string>();
        }
    }
}
