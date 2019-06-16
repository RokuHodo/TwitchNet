namespace
TwitchNet.Rest.Api.Tags
{
    public class
    StreamsTagsParameters
    {
        /// <summary>
        /// The user ID of a broadcaster.
        /// </summary>
        [QueryParameter("broadcaster_id")]
        public virtual string broadcaster_id { get; set; }
    }
}
