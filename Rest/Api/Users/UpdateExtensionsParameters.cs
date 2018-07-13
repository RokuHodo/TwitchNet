namespace
TwitchNet.Rest.Api.Users
{
    public class
    UpdateExtensionsParameters
    {
        /// <summary>
        /// The ID of the user whose installed active extensions will be returned.
        /// </summary>
        [RequestBody]
        public virtual ActiveExtensionsData data { get; set; }
    }
}
