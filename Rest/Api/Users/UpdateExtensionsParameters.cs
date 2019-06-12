namespace
TwitchNet.Rest.Api.Users
{
    public class
    UpdateExtensionsParameters
    {
        /// <summary>
        /// The active extension data to update.
        /// </summary>
        [Body("data")]
        public ActiveExtensionsData data { get; set; }

        public
        UpdateExtensionsParameters()
        {
            data = new ActiveExtensionsData();
        }
    }
}
