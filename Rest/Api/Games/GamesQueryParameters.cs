// standard namespaces
using System.Collections.Generic;

// project namespaces
using TwitchNet.Helpers;

namespace
TwitchNet.Rest.Api.Games
{
    public class
    GamesQueryParameters
    {
        private ClampedList<string> _ids    = new ClampedList<string>();
        private ClampedList<string> _names  = new ClampedList<string>();

        /// <summary>
        /// <para>A list of game id's to get information about.</para>
        /// <para>
        /// Maximum: 100 id's.
        /// If more than 100 id's are specified, only the first 100 will be added.
        /// </para>
        /// </summary>
        [QueryParameter("id", false)]
        public List<string> ids
        {
            get
            {
                return _ids.values;
            }
            set
            {
                _ids.values = value;
            }
        }

        /// <summary>
        /// <para>
        /// A list of game names to get information about.
        /// The game name must be an exact match and can not be used as a query search.
        /// </para>
        /// <para>
        /// Maximum: 100 names.
        /// If more than 100 names are specified, only the first 100 will be added.
        /// </para>
        /// </summary>
        [QueryParameter("name")]
        public List<string> names
        {
            get
            {
                return _names.values;
            }
            set
            {
                _names.values = value;
            }
        }

        /// <summary>
        /// Creates a new blank instance of the <see cref="GamesQueryParameters"/> class.
        /// </summary>
        public GamesQueryParameters()
        {

        }
    }
}
