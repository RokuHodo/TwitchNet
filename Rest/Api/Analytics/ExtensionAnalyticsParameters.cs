namespace
TwitchNet.Rest.Api.Clips
{
    public class
    ExtensionAnalyticsParameters
    {
        /// <summary>
        /// <para>The ID of an extension.</para>
        /// <para>
        /// If specified, the only CSV URL returned will be for that extension ID.
        /// Otherwise, CSV URL's will be returned for all extensions associated with the authenticated user.
        /// </para>
        /// </summary>
        [QueryParameter("extension_id")]
        public virtual string extension_id { get; set; }
    }
}
