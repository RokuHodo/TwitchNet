namespace
TwitchNet.Rest.Api.Clips
{
    public class
    ClipQueryParameters
    {
        /// <summary>
        /// The ID of the clip being queried.
        /// </summary>
        [QueryParameter("id")]
        public string id { get; set; }

        /// <summary>
        /// Creates a new blank insteance of the <see cref="ClipQueryParameters"/> class.
        /// </summary>
        public ClipQueryParameters()
        {

        }
    }
}
