namespace
TwitchNet.Rest.Api.Clips
{
    public class
    ClipCreationParameters
    {
        /// <summary>
        /// The user ID from which the clip will be made.
        /// </summary>
        [QueryParameter("broadcaster_id")]
        public virtual string   broadcaster_id  { get; set; }

        /// <summary>
        /// Whether or not a delay is added before the clip is created.
        /// </summary>
        [QueryParameter("has_delay")]
        public virtual bool     has_delay       { get; set; }
    }
}
