namespace
TwitchNet.Rest.Api.Users
{
    public class
    UpdateExtensionsParameters
    {
        /// <summary>
        /// The ID of the user whose installed active extensions will be returned.
        /// </summary>
        [Body]
        public virtual ActiveExtensionsData extensions { get; set; }

        public
        UpdateExtensionsParameters()
        {
            extensions = new ActiveExtensionsData();
        }

        public
        UpdateExtensionsParameters(ActiveExtensionsData data)
        {
            this.extensions = data;
        }
    }
}
