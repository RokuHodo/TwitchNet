namespace
TwitchNet.Rest.Api.Tags
{
    public class
    StreamsTagsParameters
    {
        // TODO: Make overloads for methods that use this parameter class that takes a broadcaster_id directly.

        /// <summary>
        /// A user ID who is live to get the stream tags for.
        /// </summary>
        [QueryParameter("broadcaster_id")]
        public virtual string broadcaster_id { get; set; }
    }
}
