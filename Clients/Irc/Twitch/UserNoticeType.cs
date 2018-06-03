namespace
TwitchNet.Clients.Irc.Twitch
{
    public enum
    UserNoticeType
    {
        /// <summary>
        /// Unsupported user notice type.
        /// </summary>
        Other    = 0,

        /// <summary>
        /// A user subscribed.
        /// </summary>
        Sub     = 1,

        /// <summary>
        /// A user resubscribed.
        /// </summary>
        Resub   = 2,

        /// <summary>
        /// A user gifted a sub to another user.
        /// </summary>
        GiftSub = 3,

        /// <summary>
        /// A user is rading another user.
        /// </summary>
        Raid    = 4,

        /// <summary>
        /// A ritual has occured.
        /// </summary>
        Ritual  = 5
    }
}
