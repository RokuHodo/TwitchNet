namespace
TwitchNet.Rest.Api.Clips
{
    public class
    ExtensionAnalyticsQueryParameters
    {
        /// <summary>
        /// <para>The ID of an extension.</para>
        /// <para>
        /// If specified, the only CSV URL returned will be for that extension ID.
        /// Otherwise, CSV URL's will be returned for all extensions associated with the authenticated user.
        /// </para>
        /// </summary>
        [QueryParameter("extension_id")]
        public string extension_id { get; set; }

        /// <summary>
        /// Creates a new blank instance of the <see cref="ExtensionAnalyticsQueryParameters"/> class.
        /// </summary>
        public ExtensionAnalyticsQueryParameters()
        {

        }
    }
}
