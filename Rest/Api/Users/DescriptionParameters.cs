namespace
TwitchNet.Rest.Api.Users
{
    public class
    DescriptionParameters
    {
        /// <summary>
        /// The text to set the user's description to.
        /// </summary>
        [QueryParameter("description")]
        public virtual string description { get; set; }
    }
}
