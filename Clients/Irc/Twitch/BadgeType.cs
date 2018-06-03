namespace
TwitchNet.Clients.Irc.Twitch
{
    public enum
    BadgeType
    {
        #region Default badge

        /// <summary>
        /// No badge.
        /// </summary>
        None        = 0,

        #endregion  

        #region Normal badges

        /// <summary>
        /// Admin badge.
        /// </summary>
        Admin,

        /// <summary>
        /// Bits badge.
        /// </summary>
        Bits,

        /// <summary>
        /// Broadcaster badge.
        /// </summary>
        Broadcaster,

        /// <summary>
        /// Global mod badge.
        /// </summary>
        GlobalMod,

        /// <summary>
        /// Moderator badge.
        /// </summary>
        Moderator,

        /// <summary>
        /// Subscriber badge.
        /// </summary>
        Subscriber,

        /// <summary>
        /// Staff badge.
        /// </summary>
        Staff,

        /// <summary>
        /// Trwitch prime badge.
        /// </summary>
        Premium,

        /// <summary>
        /// Twitch turbo badge.
        /// </summary>
        Turbo,

        /// <summary>
        /// Twitch partner badge.
        /// </summary>
        Partner,

        #endregion

        #region Other badges

        /// <summary>
        /// Twitch partner badge.
        /// </summary>
        SubGifter,

        /// <summary>
        /// Clip champ badge.
        /// </summary>
        ClipChamp,

        /// <summary>
        /// Clip champ badge.
        /// </summary>
        Twitchcon2017,

        /// <summary>
        /// Clip champ badge.
        /// </summary>
        OverwatchLeagueInsider,

        #endregion        
    }
}
