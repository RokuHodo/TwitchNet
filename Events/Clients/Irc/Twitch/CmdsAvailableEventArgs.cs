// standard namespaces
using System.Collections.Generic;

// project namespaces
using TwitchNet.Enums.Clients.Irc.Twitch;
using TwitchNet.Extensions;
using TwitchNet.Utilities;

namespace
TwitchNet.Events.Clients.Irc.Twitch
{
    public class
    CmdsAvailableEventArgs : IrcMessageEventArgs
    {        
        /// <summary>
        /// The channel that the NOTICE was sent to.
        /// </summary>
        public string           channel     { get; protected set; }

        /// <summary>
        /// The  commands that can be used in chat.
        /// </summary>
        public ChatCommand[]    commands    { get; protected set; }

        public CmdsAvailableEventArgs(NoticeEventArgs args) : base(args.irc_message)
        {
            channel = args.channel;

            string _commands = args.body.TextAfter(':').Trim(' ');
            string[] array = _commands.StringToArray<string>(' ', System.StringSplitOptions.RemoveEmptyEntries);

            List<ChatCommand> list = new List<ChatCommand>();
            foreach(string element in array)
            {
                list.Add(EnumCacheUtil.ToChatCommand(element));
            }
            commands = list.ToArray();
        }
    }
}