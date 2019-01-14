// standard namespaces
using System.Collections.Generic;

namespace
TwitchNet.Rest.Api.Videos
{
    public class
    VideosParameters : PagingParameters, IPagingParameters
    {
        /// <summary>
        /// The cursor that tells the server where to start fetching the next set of results, in a multi-page response.
        /// </summary>
        [QueryParameter("before")]
        public virtual string               before      { get; set; }

        /// <summary>
        /// <para>The ID of a user.</para>
        /// <para>Only one or more video ID, one user ID, or one game ID can be provided with each request.</para>
        /// </summary>
        [QueryParameter("user_id")]
        public virtual string               user_id     { get; set; }

        /// <summary>
        /// The ID of a game.
        /// <para>Only one or more video ID, one user ID, or one game ID can be provided with each request.</para>
        /// </summary>
        [QueryParameter("game_id")]
        public virtual string               game_id     { get; set; }

        /// <summary>
        /// <para>
        /// A list of video ID's, up to 100.
        /// All elements that are null, empty, or contain only whitespace are filtered out and all duplicate elements are removed before calculating the final count.
        /// All other optional parameters are ignored if video ID's are provited.
        /// </para>
        /// <para>Only one or more video ID, one user ID, or one game ID can be provided with each request.</para>       
        /// </summary>
        [QueryParameter("id", typeof(SeparateQueryConverter))]
        public virtual List<string>         ids         { get; set; }

        /// <summary>
        /// The language of the video.
        /// This is the language selected in the Twitch dashboard or in the video information editor, not the language selected at the home page.
        /// </summary>
        [QueryParameter("language")]
        public virtual BroadcasterLanguage? language    { get; set; }

        /// <summary>
        /// The period when the video was created.
        /// </summary>
        [QueryParameter("period")]
        public virtual VideoPeriod?         period      { get; set; }

        /// <summary>
        /// The soprt order of the videos.
        /// </summary>
        [QueryParameter("sort")]
        public virtual VideoSort?           sort        { get; set; }

        /// <summary>
        /// The type of the video.
        /// </summary>
        [QueryParameter("type")]
        public virtual VideoType?           type        { get; set; }

        public VideosParameters()
        {
            ids = new List<string>();
        }
    }
}