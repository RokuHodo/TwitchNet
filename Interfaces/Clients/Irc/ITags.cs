namespace TwitchNet.Interfaces.Clients.Irc
{
    public interface
    ITags
    {
        /// <summary>
        /// Whether or not tags are atached to the message.
        /// </summary>
        bool is_valid { get; }
    }
}
