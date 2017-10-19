//standard namespaces
using System.Collections.Generic;

// project namespaces
using TwitchNet.Interfaces.Helpers.Paging;

// project namespaces
using TwitchNet.Enums.Api.Streams;

namespace TwitchNet.Helpers.Paging.Streams
{
    //TODO: Test to see if these paging parameters function properly
    public class
    StreamsQueryParameters : ITwitchQueryParameters
    {
        #region Fields

        private QueryComparable<ushort>     _first          = new QueryComparable<ushort>(1, 100, 20);

        private QueryParameter              _after          = new QueryParameter();

        private QueryList                   _community_ids  = new QueryList();
        private QueryList                   _game_ids       = new QueryList();
        private QueryList                   _user_ids       = new QueryList();
        private QueryList                   _user_logins    = new QueryList();

        private QueryEnum<StreamLanguage>   _language       = new QueryEnum<StreamLanguage>();
        private QueryEnum<StreamType>       _type           = new QueryEnum<StreamType>();

        #endregion

        #region Properties

        /// <summary>
        /// Maximum number of objects to return.
        /// Minimum: 1;
        /// Maximum: 100.
        /// Default: 20.
        /// </summary>
        [QueryParameter("first")]
        public ushort first
        {
            get
            {
                return _first.value;
            }
            set
            {
                _first.value = value;
            }
        }

        /// <summary>
        /// The cursor that tells the server where to start fetching the next set of results, in a multi-page response.
        /// </summary>
        [QueryParameter("after", false)]
        public string after
        {
            get
            {
                return _after.value;
            }
            set
            {
                _after.value = value;
            }
        }

        /// <summary>
        /// Returns streams that are part of part of the specified communities.
        /// Maximum: 100 id's.
        /// If more than 100 id's are specified, only the first 100 will be added.
        /// </summary>
        [QueryParameter("community_id")]
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
        /// Returns streams that are playing the specified game.
        /// Maximum: 100 id's.
        /// If more than 100 id's are specified, only the first 100 will be added.
        /// </summary>
        [QueryParameter("game_id")]
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
        /// Returns streams that have the selected stream langages. Bitfield enum.
        /// </summary>
        [QueryParameter("language")]
        public StreamLanguage? language
        {
            get
            {
                return _language.value;
            }
            set
            {
                _language.value = value;
            }
        }

        /// <summary>
        /// Returns streams that match the selected stream types. Bitfield enum.
        /// </summary>
        [QueryParameter("type")]
        public StreamType? type
        {
            get
            {
                return _type.value;
            }
            set
            {
                _type.value = value;
            }
        }

        /// <summary>
        /// Returns streams broadcast by one or more specified user id's.
        /// Maximum: 100 id's.
        /// If more than 100 id's are specified, only the first 100 will be added.
        /// </summary>
        [QueryParameter("user_id")]
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
        /// Returns streams broadcast by one or more specified user login names
        /// Maximum: 100 names.
        /// If more than 100 names are specified, only the first 100 will be added.
        /// </summary>
        [QueryParameter("user_login")]
        public List<string> user_login
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

        #endregion

        #region Contstructor

        public StreamsQueryParameters()
        {

        }

        #endregion
    }
}
