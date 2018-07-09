//standard namespaces
using System.Collections.Generic;

// project namespaces
using TwitchNet.Helpers;

namespace
TwitchNet.Rest.Api.Streams
{
    public class
    StreamsQueryParameters : HelixQueryParameters, IHelixQueryParameters
    {
        private ClampedList<string> _community_ids  = new ClampedList<string>();
        private ClampedList<string> _game_ids       = new ClampedList<string>();
        private ClampedList<string> _user_ids       = new ClampedList<string>();
        private ClampedList<string> _user_logins    = new ClampedList<string>();

        /// <summary>
        /// A list of communities on Twitch.
        /// Maximum: 100 id's.
        /// If more than 100 id's are specified, only the first 100 will be added.
        /// </summary>
        [QueryParameter("community_id", typeof(SeparateQueryFormatter))]
        public List<string> community_ids
        {
            get
            {
                return _community_ids.values;
            }
            set
            {
                _community_ids.values = value;
            }
        }

        /// <summary>
        /// A list of game ID's.
        /// Maximum: 100 id's.
        /// If more than 100 id's are specified, only the first 100 will be added.
        /// </summary>
        [QueryParameter("game_id", typeof(SeparateQueryFormatter))]
        public List<string> game_ids
        {
            get
            {
                return _game_ids.values;
            }
            set
            {
                _game_ids.values = value;
            }
        }

        /// <summary>
        /// The language of the stream.
        /// This is the language selected at the home page, not the language found in the Twitch dashboard.
        /// Bitfield enum.
        /// </summary>
        [QueryParameter("language", typeof(SeparateQueryFormatter))]
        public StreamLanguage? language { get; set; }

        /// <summary>
        /// The stream type.
        /// Bitfield enum.
        /// </summary>
        [QueryParameter("type", typeof(SeparateQueryFormatter))]
        public StreamType? type { get; set; }

        /// <summary>
        /// A list of user ID's.
        /// Maximum: 100 id's.
        /// If more than 100 id's are specified, only the first 100 will be added.
        /// </summary>
        [QueryParameter("user_id", typeof(SeparateQueryFormatter))]
        public List<string> user_ids
        {
            get
            {
                return _user_ids.values;
            }
            set
            {
                _user_ids.values = value;
            }
        }

        /// <summary>
        /// A list of user login names.
        /// Maximum: 100 names.
        /// If more than 100 names are specified, only the first 100 will be added.
        /// </summary>
        [QueryParameter("user_login", typeof(SeparateQueryFormatter))]
        public List<string> user_logins
        {
            get
            {
                return _user_logins.values;
            }
            set
            {
                _user_logins.values = value;
            }
        }

        /// <summary>
        /// Creates a new blank instance of the <see cref="StreamsQueryParameters"/> class.
        /// </summary>
        public StreamsQueryParameters()
        {

        }
    }
}
