namespace TwitchNet.Enums.Utilities
{
    internal enum
    Authentication
    {
        /// <summary>
        /// Specifies that an OAuth token is being used for authentication.
        /// </summary>
        Authorization   = 0,

        /// <summary>
        /// Specifies that a Client Id is being used for authentication.
        /// </summary>
        Client_ID       = 1,

        /// <summary>
        /// Specifies that a OAuth token and Client Id are being used for authentication.
        /// </summary>
        Both = 2
    }
}