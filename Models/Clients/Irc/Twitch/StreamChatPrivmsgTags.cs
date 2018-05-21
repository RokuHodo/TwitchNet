// project namespaces
using TwitchNet.Debugger;
using TwitchNet.Enums.Debugger;
using TwitchNet.Events.Clients.Irc;
using TwitchNet.Interfaces.Clients.Irc.Twitch;
using TwitchNet.Utilities;

namespace
TwitchNet.Models.Clients.Irc.Twitch
{
    public class
    StreamChatPrivmsgTags : ChatRoomPrivmsgTags, ISharedPrivmsgTags
    {
        /// <summary>
        /// <para>The amount of bits the sender cheered, if any.</para>
        /// <para>Set to 0 if the sender did not cheer.</para>
        /// </summary>
        [ValidateTag("bits", Check.IsNotEqualTo, 0u)]
        public uint bits { get; protected set; }

        /// <summary>
        /// Creates a new instance of the <see cref="StreamChatPrivmsgTags"/> class.
        /// </summary>
        /// <param name="args">The event arguments to parse.</param>
        public StreamChatPrivmsgTags(PrivmsgEventArgs args) : base(args)
        {
            if (!is_valid)
            {
                return;
            }

            bits = TagsUtil.ToUInt32(args.tags, "bits");
        }
    }
}