namespace
TwitchNet.Rest.Api.Clips
{
    //TODO: Update query parameters.
    public class
    ClipCreationQueryParameters
    {
        /// <summary>
        /// The user ID from which the clip will be made.
        /// </summary>
        [QueryParameter("broadcaster_id")]
        public string broadcaster_id { get; set; }

        /// <summary>
        /// Creates a new blank instance of the <see cref="ClipCreationQueryParameters"/> class.
        /// </summary>
        public ClipCreationQueryParameters()
        {

        }
    }
}
