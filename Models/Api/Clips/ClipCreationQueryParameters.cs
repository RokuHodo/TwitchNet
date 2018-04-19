namespace
TwitchNet.Models.Api.Clips
{
    public class
    ClipCreationQueryParameters
    {
        //TODO: Update query parameters.
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
