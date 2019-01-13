namespace
TwitchNet.Rest.Api.Users
{
    public class
    UpdateExtensionsParameters
    {
        /// <summary>
        /// The active extensions to update.
        /// </summary>
        [Body]
        public virtual ActiveExtensions extensions { get; set; }

        public
        UpdateExtensionsParameters()
        {
            extensions = new ActiveExtensions();
        }

        public
        UpdateExtensionsParameters(ActiveExtensions extensions)
        {
            this.extensions = extensions;
        }
    }
}
