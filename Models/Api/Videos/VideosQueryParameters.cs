// standard namespaces
using System.Collections.Generic;

// project namespaces
using TwitchNet.Helpers;
using TwitchNet.Enums.Api.Videos;

// project namespaces
using TwitchNet.Interfaces.Api;

namespace
TwitchNet.Models.Api.Videos
{
    public class
    VideosQueryParameters : QueryParametersPage, IQueryParametersPage
    {
        #region Fields

        private string              _before;
        private string              _user_id;
        private string              _game_id;

        private ClampedList<string> _ids = new ClampedList<string>();

        private VideoLanguage?      _language;
        private VideoPeriod?        _period;
        private VideoSort?          _sort;
        private VideoType?          _type;

        #endregion

        #region Properties

        /// <summary>
        /// The cursor that tells the server where to start fetching the next set of results, in a multi-page response.
        /// </summary>
        [QueryParameter("before", false)]
        public string before
        {
            get
            {
                return _before;
            }
            set
            {
                _before = value;
            }
        }

        /// <summary>
        /// The ID of a user.
        /// Only one or more video ID, one user ID, or one game ID can be provided with each request.
        /// </summary>
        [QueryParameter("user_id")]
        public string user_id
        {
            get
            {
                return _user_id;
            }
            set
            {
                _user_id = value;
            }
        }

        /// <summary>
        /// The ID of a game.
        /// Only one or more video ID, one user ID, or one game ID can be provided with each request.
        /// </summary>
        [QueryParameter("game_id")]
        public string game_id
        {
            get
            {
                return _game_id;
            }
            set
            {
                _game_id = value;
            }
        }

        /// <summary>
        /// <para>
        /// A list of video ID's.
        /// Only one or more video ID, one user ID, or one game ID can be provided with each request.
        /// No other query parameters may be provided if a video ID's are provided.
        /// </para>        
        /// <para>
        /// Maximum: 100 id's.
        /// If more than 100 id's are specified, only the first 100 will be added.
        /// </para>
        /// </summary>
        [QueryParameter("id")]
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
        /// The language of the video.
        /// This is the language selected in the Twitch dashboard or in the video information editor, not the language selected at the home page.
        /// </summary>
        [QueryParameter("language")]
        public VideoLanguage? language
        {
            get
            {
                return _language;
            }
            set
            {
                _language = value;
            }
        }

        /// <summary>
        /// The period when the video was created.
        /// </summary>
        [QueryParameter("period")]
        public VideoPeriod? period
        {
            get
            {
                return _period;
            }
            set
            {
                _period = value;
            }
        }

        /// <summary>
        /// The soprt order of the videos.
        /// </summary>
        [QueryParameter("sort")]
        public VideoSort? sort
        {
            get
            {
                return _sort;
            }
            set
            {
                _sort = value;
            }
        }

        /// <summary>
        /// The type of the video.
        /// </summary>
        [QueryParameter("type")]
        public VideoType? type
        {
            get
            {
                return _type;
            }
            set
            {
                _type = value;
            }
        }

        #endregion

        #region Contstructors

        public VideosQueryParameters()
        {

        }

        #endregion
    }
}