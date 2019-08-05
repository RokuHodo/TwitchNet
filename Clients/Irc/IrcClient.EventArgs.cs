// standard namespaces
using System.Collections.Generic;

// project namespaces
using TwitchNet.Debugger;
using TwitchNet.Extensions;
using TwitchNet.Clients.Irc.Twitch;

namespace
TwitchNet.Clients.Irc
{
    public partial class
    NamReplyEventArgs : ChatRoomMessageEventArgs
    {
        /// <summary>
        /// The character that specifies if the IRC channel is public, secret, or private.
        /// </summary>
        [ValidateMember(Check.IsNotNullOrDefault)]
        public char status { get; protected set; }

        /// <summary>
        /// The IRC client nick.
        /// </summary>
        [ValidateMember(Check.IsValid)]
        public string client { get; protected set; }

        /// <summary>
        /// The IRC channel that the clients have joined.
        /// </summary>
        [ValidateMember(Check.IsValid)]
        public string channel { get; protected set; }

        /// <summary>
        /// A partial or complete list of client nicks that have joined the IRC channel.
        /// </summary>
        [ValidateMember(Check.IsValid)]
        public string[] names { get; protected set; }

        /// <summary>
        /// <para>Whether or not the IRC channel is public.</para>
        /// <para>The channel is public if the status is equal to '='.</para>
        /// </summary>
        [ValidateMember(Check.IsNotNull)]
        public bool is_public { get; protected set; }

        /// <summary>
        /// <para>Whether or not the IRC channel is secret.</para>
        /// <para>The channel is secret if the status is equal to '@'.</para>
        /// </summary>
        [ValidateMember(Check.IsNotNull)]
        public bool is_secret { get; protected set; }

        /// <summary>
        /// <para>Whether or not the IRC channel is private.</para>
        /// <para>The channel is private if the status is equal to '*'.</para>
        /// </summary>
        [ValidateMember(Check.IsNotNull)]
        public bool is_private { get; protected set; }

        public NamReplyEventArgs(in IrcMessage message) : base(message, 2)
        {
            // Native IRC aprsing
            if (!message.parameters.IsValid() || message.parameters.Length < 3)
            {
                return;
            }

            client = message.parameters[0];
            status = message.parameters[1][0];
            if (status == '=')
            {
                is_public = true;
            }
            else if (status == '@')
            {
                is_secret = true;
            }
            else if (status == '*')
            {
                is_private = true;
            }

            channel = message.parameters[2];

            names = message.trailing.Split(' ');
        }
    }

    public class
    EndOfNamesEventArgs : ChatRoomMessageEventArgs
    {
        /// <summary>
        /// The IRC client nick.
        /// </summary>
        [ValidateMember(Check.IsValid)]
        public string client { get; protected set; }

        /// <summary>
        /// The IRC channel that the clients have joined.
        /// </summary>
        [ValidateMember(Check.IsValid)]
        public string channel { get; protected set; }

        /// <summary>
        /// The complete list of client nicks that have joined the IRC channel.
        /// </summary>
        [ValidateMember(Check.IsValid)]
        public string[] names { get; protected set; }

        public EndOfNamesEventArgs(in IrcMessage message, Dictionary<string, List<string>> names) : base(message, 1)
        {
            if (!message.parameters.IsValid() || message.parameters.Length < 2)
            {
                return;
            }

            client = message.parameters[0];
            channel = message.parameters[1];

            this.names = names[channel].ToArray();
        }
    }

    public class
    JoinEventArgs : ChatRoomMessageEventArgs
    {
        /// <summary>
        /// The nick of the client who joined the channel.
        /// </summary>
        [ValidateMember(Check.IsValid)]
        public string nick { get; protected set; }

        /// <summary>
        /// The IRC channel the client has joined.
        /// </summary>
        [ValidateMember(Check.IsValid)]
        public string channel { get; protected set; }

        public JoinEventArgs(in IrcMessage message) : base(message)
        {
            nick = message.server_or_nick;

            if (!message.parameters.IsValid())
            {
                return;
            }

            channel = message.parameters[0];
		}
    }

    public class
    PartEventArgs : ChatRoomMessageEventArgs
    {
        /// <summary>
        /// The nick of the client who left the channel.
        /// </summary>
        [ValidateMember(Check.IsValid)]
        public string nick { get; protected set; }

        /// <summary>
        /// The IRC channel the client has left.
        /// </summary>
        [ValidateMember(Check.IsValid)]
        public string channel { get; protected set; }

        public PartEventArgs(in IrcMessage message) : base(message)
        {
            nick = message.server_or_nick;

            if (!message.parameters.IsValid())
            {
                return;
            }

            channel = message.parameters[0];
        }		
    }

    public class
    ChatRoomMessageEventArgs : IrcMessageEventArgs
    {
        /// <summary>
        /// Where the message was sent from.
        /// </summary>
        public MessageSource source { get; private set; }

        /// <summary>
        /// <para>The ID of the user who owns the chat room.</para>
        /// <para>Set to an empty string if the message source was not from a chat room.</para>
        /// </summary>
        [ValidateMember(Check.IsValid)]
        public string channel_user_id { get; protected set; }

        /// <summary>
        /// <para>The UUID of the chat room.</para>
        /// <para>Set to an empty string if the message source was not from a chat room.</para>
        /// </summary>
        [ValidateMember(Check.RegexIsMatch, TwitchIrcUtil.REGEX_PATTERN_UUID)]
        public string channel_uuid { get; protected set; }

        public ChatRoomMessageEventArgs(in IrcMessage message, uint channel_index = 0) : base(message)
        {
            source = TwitchIrcUtil.GetMessageSource(message.parameters[channel_index]);

            channel_user_id = string.Empty;
            channel_uuid = string.Empty;
            if (source == MessageSource.ChatRoom)
            {
                channel_user_id = message.parameters[channel_index].TextBetween(':', ':');

                int index = message.parameters[channel_index].LastIndexOf(':');
                if (index != -1)
                {
                    channel_uuid = message.parameters[channel_index].TextAfter(':', index);
                }
            }
        }
    }
}
