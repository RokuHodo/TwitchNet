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
        public virtual ActiveExtensions extensions { get; set; }

        public
        UpdateExtensionsParameters()
        {
            extensions = new ActiveExtensions();
        }

        public
        UpdateExtensionsParameters(ActiveExtensions data)
        {
            this.extensions = data;
        }
    }
}
