namespace
TwitchNet.Rest.Api.Clips
{
    public class
    ClipParameters
    {
        /// <summary>
        /// The ID of the clip being queried.
        /// </summary>
        [QueryParameter("id")]
        public virtual string id { get; set; }
    }
}
