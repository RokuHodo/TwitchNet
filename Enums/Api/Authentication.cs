namespace TwitchNet.Enums.Api
{
    internal enum Authentication
    {
        /// <summary>
        /// Specifies that an OAuth token is being used for authentication.
        /// </summary>
        Authorization   = 0,

        /// <summary>
        /// Specifies that a Client Id is being used for authentication.
        /// </summary>
        Client_ID       = 1
    }
}