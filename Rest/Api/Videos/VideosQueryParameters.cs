// standard namespaces
using System.Collections.Generic;

// project namespaces
using TwitchNet.Helpers;

namespace
TwitchNet.Rest.Api.Videos
{
    public class
    VideosQueryParameters : HelixQueryParameters, IHelixQueryParameters
    {
        private ClampedList<string> _ids = new ClampedList<string>();

        /// <summary>
        /// The cursor that tells the server where to start fetching the next set of results, in a multi-page response.
        /// </summary>
        [QueryParameter("before")]
        public string               before      { get; set; }

        /// <summary>
        /// The ID of a user.
        /// Only one or more video ID, one user ID, or one game ID can be provided with each request.
        /// </summary>
        [QueryParameter("user_id")]
        public string               user_id     { get; set; }

        /// <summary>
        /// The ID of a game.
        /// Only one or more video ID, one user ID, or one game ID can be provided with each request.
        /// </summary>
        [QueryParameter("game_id")]
        public string               game_id     { get; set; }

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
        [QueryParameter("id", typeof(SeparateQueryFormatter))]
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
        public BroadcasterLanguage? language    { get; set; }

        /// <summary>
        /// The period when the video was created.
        /// </summary>
        [QueryParameter("period")]
        public VideoPeriod?         period      { get; set; }

        /// <summary>
        /// The soprt order of the videos.
        /// </summary>
        [QueryParameter("sort")]
        public VideoSort?           sort        { get; set; }

        /// <summary>
        /// The type of the video.
        /// Bitfield enum.
        /// </summary>
        [QueryParameter("type", typeof(SeparateQueryFormatter))]
        public VideoType?           type        { get; set; }

        /// <summary>
        /// Creates a new blank instance of the <see cref="VideosQueryParameters"/> class.
        /// </summary>
        public VideosQueryParameters()
        {

        }
    }
}