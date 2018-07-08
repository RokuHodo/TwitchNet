namespace
TwitchNet.Rest.Api.Clips
{
    public class
    ClipCreationQueryParameters
    {
        /// <summary>
        /// The user ID from which the clip will be made.
        /// </summary>
        [QueryParameter("broadcaster_id")]
        public string broadcaster_id { get; set; }

        /// <summary>
        /// Whether or not a delay is added before the clip is created.
        /// </summary>
        [QueryParameter("has_delay")]
        public bool has_delay { get; set; }

        /// <summary>
        /// Creates a new blank instance of the <see cref="ClipCreationQueryParameters"/> class.
        /// </summary>
        public ClipCreationQueryParameters()
        {

        }
    }
}
