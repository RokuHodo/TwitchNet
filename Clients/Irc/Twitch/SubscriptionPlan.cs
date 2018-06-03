namespace
TwitchNet.Clients.Irc.Twitch
{
    public enum
    SubscriptionPlan
    {
        /// <summary>
        /// Unsupported subscription plan.
        /// </summary>
        Other    = 0,

        /// <summary>
        /// Free Twitch Prime subscription.
        /// Equivelent <see cref="Tier1"/>
        /// </summary>
        Prime   = 1,

        /// <summary>
        /// $4.99 subsctiption plan.
        /// </summary>
        Tier1   = 1000,

        /// <summary>
        /// $9.99 subsctiption plan.
        /// </summary>
        Tier2   = 2000,

        /// <summary>
        /// $24.99 subsctiption plan.
        /// </summary>
        Tier3   = 3000
    }
}
