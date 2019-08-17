// standard namespaces
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.Serialization;

// project namespaces
using TwitchNet.Debugger;
using TwitchNet.Extensions;
using TwitchNet.Clients.Irc.Twitch;
using TwitchNet.Utilities;

namespace
TwitchNet.Clients.Irc
{
    // -------------------------------------------------------------------------------------------------------------
    //
    // - This file contains all IRC messages that are natively supported and documented in the RFC 1459 Specification.
    // - RFC 1459 Spec: https://tools.ietf.org/html/rfc1459.html
    // - Any message that is specific to Twitch can be found in its own file, TwitchIrcClient.EventArgs.
    //
    // - However, these messages do contain extra parsed members that pertain only to Twitch. These members are separated 
    //   clearly from the native IRC spec members witin each data structure.
    //
    // -------------------------------------------------------------------------------------------------------------

    public class
    DataEventArgs : EventArgs
    {
        /// <summary>
        /// The byte data.
        /// </summary>
        [ValidateMember(Check.IsValid)]
        public byte[] data { get; protected set; }

        /// <summary>
        /// The UTF-8 encoded data.
        /// </summary>
        [ValidateMember(Check.IsValid)]
        public string message { get; protected set; }

        public DataEventArgs(byte[] data, string message)
        {
            this.data = data;

            this.message = message;
        }
    }

    public class
    IrcMessageEventArgs : EventArgs
    {
        /// <summary>
        /// The parsed IRC message.
        /// </summary>
        public IrcMessage irc_message { get; protected set; }

        public IrcMessageEventArgs(in IrcMessage message)
        {
            irc_message = message;
        }
    }

    public class
    NumericReplyEventArgs : EventArgs
    {
        /// <summary>
        /// The parsed IRC message.
        /// </summary>
        [ValidateMember(Check.IsNotNullOrDefault)]
        public IrcMessage irc_message { get; protected set; }

        /// <summary>
        /// The IRC client nick.
        /// </summary>
        [ValidateMember(Check.IsValid)]
        public string client { get; protected set; }

        public NumericReplyEventArgs(in IrcMessage message)
        {
            irc_message = message;

            if (message.parameters.Length > 0)
            {
                client = message.parameters[0];
            }
        }
    }

    public class
    NamReplyEventArgs : ChatRoomSupportedMessageEventArgs
    {
        /// <summary>
        /// The parsed IRC message.
        /// </summary>
        public IrcMessage irc_message { get; protected set; }

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
        /// Whether or not the IRC channel is public.
        /// The channel is public if the status is equal to '='.
        /// </summary>
        [ValidateMember(Check.IsNotNull)]
        public bool is_public { get; protected set; }

        /// <summary>
        /// Whether or not the IRC channel is secret.
        /// The channel is secret if the status is equal to '@'.
        /// </summary>
        [ValidateMember(Check.IsNotNull)]
        public bool is_secret { get; protected set; }

        /// <summary>
        /// Whether or not the IRC channel is private.
        /// The channel is private if the status is equal to '*'.
        /// </summary>
        [ValidateMember(Check.IsNotNull)]
        public bool is_private { get; protected set; }

        public NamReplyEventArgs(in IrcMessage message) : base(message, 2)
        {
            irc_message = message;

            // Native IRC aprsing
            if (message.parameters.Length > 2)
            {
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
            }

            names = message.trailing.Split(' ');
        }
    }

    public class
    EndOfNamesEventArgs : ChatRoomSupportedMessageEventArgs
    {
        /// <summary>
        /// The parsed IRC message.
        /// </summary>
        public IrcMessage irc_message { get; protected set; }

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
            irc_message = message;

            this.names = names[channel].ToArray();
            
            if (message.parameters.Length > 1)
            {
                client = message.parameters[0];
                channel = message.parameters[1];
            }
        }
    }

    public class
    MotdEventArgs : EventArgs
    {
        /// <summary>
        /// The parsed IRC message.
        /// </summary>
        public IrcMessage irc_message { get; protected set; }

        /// <summary>
        /// The IRC client nick.
        /// </summary>
        [ValidateMember(Check.IsValid)]
        public string client { get; protected set; }

        /// <summary>
        /// The IRC server's message of the day.
        /// </summary>
        [ValidateMember(Check.IsValid)]
        public string motd { get; protected set; }

        public MotdEventArgs(in IrcMessage message)
        {
            irc_message = message;

            if (message.parameters.Length > 0)
            {
                client = message.parameters[0];
            }

            motd = message.trailing;
        }
    }

    public class
    UnknownCommandEventArgs : EventArgs
    {
        /// <summary>
        /// The parsed IRC message.
        /// </summary>
        public IrcMessage irc_message { get; protected set; }

        /// <summary>
        /// The IRC client nick.
        /// </summary>
        [ValidateMember(Check.IsValid)]
        public string client { get; protected set; }

        /// <summary>
        /// The unsupported IRC command.
        /// </summary>
        [ValidateMember(Check.IsValid)]
        public string command { get; protected set; }

        /// <summary>
        /// The error description.
        /// </summary>
        [ValidateMember(Check.IsValid)]
        public string description { get; protected set; }

        public UnknownCommandEventArgs(in IrcMessage message)
        {
            irc_message = message;


            if (message.parameters.Length > 1)
            {
                client = message.parameters[0];
                command = message.parameters[1];
            }

            description = message.trailing;
        }
    }

    public class
    JoinEventArgs : ChatRoomSupportedMessageEventArgs
    {
        /// <summary>
        /// The parsed IRC message.
        /// </summary>
        public IrcMessage irc_message { get; protected set; }

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
            irc_message = message;

            nick = message.server_or_nick;

            if (message.parameters.Length > 0)
            {
                channel = message.parameters[0];
            }
		}
    }

    public class
    PartEventArgs : ChatRoomSupportedMessageEventArgs
    {
        /// <summary>
        /// The parsed IRC message.
        /// </summary>
        public IrcMessage irc_message { get; protected set; }

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
            irc_message = message;

            nick = message.server_or_nick;

            if (message.parameters.Length > 0)
            {
                channel = message.parameters[0];
            }
        }		
    }

    public class
    ChannelModeEventArgs : EventArgs
    {
        /// <summary>
        /// The parsed IRC message.
        /// </summary>
        public IrcMessage irc_message { get; protected set; }

        /// <summary>
        /// Denotes the whether the mode was added '+', or removed '-'.
        /// </summary>
        [ValidateMember(Check.IsNotNullOrDefault)]
        public char modifier { get; protected set; }

        /// <summary>
        /// The change that occured to either the channel or the user.
        /// </summary>
        [ValidateMember(Check.IsNotNullOrDefault)]
        public char mode { get; protected set; }

        /// <summary>
        /// A combination of the 'modifier' and the 'mode'.
        /// The complete change that occured.
        /// </summary>
        [ValidateMember(Check.IsValid)]
        public string mode_set { get; protected set; }

        /// <summary>
        /// The IRC channel whose mode was changed.
        /// </summary>
        [ValidateMember(Check.IsValid)]
        public string channel { get; protected set; }

        // TODO: Change to an array since this could be none or all 3 arguments,

        /// <summary>
        /// Arguments, if any, associated with the mode change.
        /// These inckude a ban mask, limit, and/or an IRC user.
        /// </summary>
        [ValidateMember(Check.IsValid)]
        public string arguments { get; protected set; }

        public ChannelModeEventArgs(in IrcMessage message)
        {
            irc_message = message;

            if (message.parameters.Length > 2)
            {
                channel = message.parameters[0];

                // This assumes only one argument after the mode set.
                // This is fine for Twitch, but change this to an array because it *could* be up to 3 parameters after the mode set.
                arguments = message.parameters[2];

                mode_set = message.parameters[1];
                if (message.parameters[1].Length > 1)
                {
                    modifier = message.parameters[1][0];
                    mode = message.parameters[1][1];
                }
            }
        }
    }

    public class
    ChannelOperatorEventArgs : EventArgs
    {
        /// <summary>
        /// The parsed IRC message.
        /// </summary>
        public IrcMessage irc_message { get; protected set; }

        /// <summary>
        /// Whether or not the user is an operator in the IRC channel.
        /// </summary>
        [ValidateMember(Check.IsNotNull)]
        public bool is_operator { get; protected set; }

        /// <summary>
        /// The user nick that gained or lost operator status.
        /// </summary>
        [ValidateMember(Check.IsValid)]
        public string user { get; protected set; }

        /// <summary>
        /// The IRC channel where the user's operator status changed.
        /// </summary>
        [ValidateMember(Check.IsValid)]
        public string channel { get; protected set; }

        public ChannelOperatorEventArgs(ChannelModeEventArgs args)
        {
            irc_message = args.irc_message;

            is_operator = args.modifier == '+' ? true : false;
            user = args.arguments;
            channel = args.channel;
        }
    }

    public class
    UserModeEventArgs : EventArgs
    {
        /// <summary>
        /// The parsed IRC message.
        /// </summary>
        public IrcMessage irc_message { get; protected set; }

        /// <summary>
        /// The name of the user whose mode was changed.
        /// </summary>
        [ValidateMember(Check.IsValid)]
        public string nick { get; protected set; }

        /// <summary>
        /// Denotes the whether the mode was added '+', or removed '-'.
        /// </summary>
        [ValidateMember(Check.IsNotNullOrDefault)]
        public char modifier { get; protected set; }

        /// <summary>
        /// The change that occured to either the channel or the user.
        /// </summary>
        [ValidateMember(Check.IsNotNullOrDefault)]
        public char mode { get; protected set; }

        /// <summary>
        /// A combination of the 'modifier' and the 'mode_set'.
        /// The complete change that occured.
        /// </summary>
        [ValidateMember(Check.IsValid)]
        public string mode_set { get; protected set; }

        public UserModeEventArgs(in IrcMessage message)
        {
            irc_message = message;

            if (message.parameters.Length > 1)
            {
                nick = message.parameters[0];

                mode_set = message.parameters[1];
                if (message.parameters[1].Length > 1)
                {
                    modifier = message.parameters[1][0];
                    mode = message.parameters[1][1];
                }
            }
        }
    }

    public class
    PrivmsgEventArgs : ChatRoomSupportedMessageEventArgs
    {
        /// <summary>
        /// The parsed IRC message.
        /// </summary>
        public IrcMessage irc_message { get; protected set; }

        // Native RFC 1459 propperties

        /// <summary>
        /// Whether or not IRC tags were sent with the message.
        /// </summary>
        public bool tags_exist { get; private set; }

        /// <summary>
        /// The nick of the client who sent the message.
        /// </summary>
        [ValidateMember(Check.IsValid)]
        public string nick { get; protected set; }

        /// <summary>
        /// The IRC user who sent the message.
        /// </summary>
        [ValidateMember(Check.IsValid)]
        public string user { get; protected set; }

        /// <summary>
        /// The IRC host name.
        /// </summary>
        [ValidateMember(Check.IsValid)]
        public string host { get; protected set; }

        /// <summary>
        /// The IRC channel the message was sent in.
        /// </summary>
        [ValidateMember(Check.IsValid)]
        public string channel { get; protected set; }

        /// <summary>
        /// The body of the message.
        /// </summary>
        [ValidateMember(Check.IsValid)]
        public string body { get; protected set; }

        // Twitch specific propertties

        /// <summary>
        /// Whether or not the message started with '\u0001ACTION', i.e., the '/me' command was used with the message.
        /// </summary>
        public bool action { get; protected set; }

        /// <summary>
        /// <para>The converted IRC tags attached to the message.</para>
        /// <para>Set to null if the message source was not from a chat room.</para>
        /// </summary>
        [ValidateMember]
        [ValidateMember(Check.TagsMissing)]
        // [ValidateMember(Check.TagsExtra)]
        public ChatRoomPrivmsgTags tags_chat_room { get; protected set; }

        /// <summary>
        /// <para>The converted IRC tags attached to the message.</para>
        /// <para>Set to null if the message source was not from a stream chat.</para>
        /// </summary>
        [ValidateMember]
        [ValidateMember(Check.TagsMissing)]
        // [ValidateMember(Check.TagsExtra)]
        public StreamChatPrivmsgTags tags_stream_chat { get; protected set; }

        public PrivmsgEventArgs(in IrcMessage message) : base(message)
        {
            irc_message = message;

            // RFC 1459 parsing
            tags_exist = irc_message.tags_exist;

            nick = message.server_or_nick;
            user = message.user;
            host = message.host;

            channel = message.parameters[0];

            body = message.trailing;            

            // Twitch specific parsing
            action = body.StartsWith(TwitchIrcUtil.ACTION_PREFIX) ? true : false;
            if (action)
            {
                body = body.TextBetween(TwitchIrcUtil.ACTION_PREFIX, TwitchIrcUtil.ACTION_SUFFIX).Trim();
            }

            if (tags_exist)
            {
                if (source == MessageSource.ChatRoom)
                {
                    tags_chat_room = new ChatRoomPrivmsgTags(irc_message);
                }
                else if (source == MessageSource.StreamChat)
                {
                    tags_stream_chat = new StreamChatPrivmsgTags(irc_message);
                }
            }
        }
    }

    public class
    ChatRoomPrivmsgTags
    {
        /// <summary>
        /// Whether or not the sender is a moderator.
        /// </summary>
        [IrcTag("mod")]
        public bool mod { get; protected set; }

        /// <summary>
        /// Whether or not the sender is subscribed to the channel.
        /// </summary>
        [Obsolete("This tag is obsolte and can be deleted at any time. Use the 'badges' tag to look for this information instraad")]
        [IrcTag("subscriber")]
        public bool subscriber { get; protected set; }

        /// <summary>
        /// Whether or not the sender has Twitch turbo.
        /// </summary>
        [Obsolete("This tag has been deprecated and can be deleted at any time. Use the 'badges' tag to look for this information instead")]
        [IrcTag("turbo")]
        public bool turbo { get; protected set; }

        /// <summary>
        /// Whether or not the body of the message only contains emotes.
        /// </summary>
        [IrcTag("emote-only")]
        public bool emote_only { get; protected set; }

        /// <summary>
        /// The unique message ID.
        /// </summary>
        [IrcTag("id")]
        public string id { get; protected set; }

        /// <summary>
        /// <para>The display name of the sender.</para>
        /// <para>Set to an empty string if the sender never explicitly set their display name.</para>
        /// </summary>
        [IrcTag("display-name")]
        public string display_name { get; protected set; }

        /// <summary>
        /// The user ID of the sender.
        /// </summary>
        [IrcTag("user-id")]
        public string user_id { get; protected set; }

        /// <summary>
        /// The user ID of the channel the message was sent in.
        /// </summary>
        [IrcTag("room-id")]
        public string room_id { get; protected set; }

        /// <summary>
        /// <para>The sender's user type</para>
        /// <para>Set to <see cref="UserType.None"/> if the sender has no elevated privileges.</para>
        /// </summary>
        [Obsolete("This tag has been deprecated and can be deleted at any time. Use the 'badges' tag to look for this information instead")]
        [IrcTag("user-type")]
        public UserType user_type { get; protected set; }

        /// <summary>
        /// <para>The color of the sender's display name.</para>
        /// <para>Set to <see cref="Color.Empty"/> if the sender never explicitly set their display name color.</para>
        /// </summary>
        [IrcTag("color")]
        public Color color { get; protected set; }

        /// <summary>
        /// The time the message was sent.
        /// </summary>
        [IrcTag("tmi-sent-ts")]
        public DateTime tmi_sent_ts { get; protected set; }

        /// <summary>
        /// <para>The chat badges that the sender has, if any.</para>
        /// <para>Set to an empty array if the sender has no chat badges.</para>
        /// </summary>
        [ValidateMember]
        [IrcTag("badges")]
        public Badge[] badges { get; protected set; }

        /// <summary>
        /// <para>
        /// Detailed information on badge tenure.
        /// Currently, this only returns information for the subscriber badge.
        /// </para>
        /// <para>Set to an empty array if the sender has no chat badges.</para>
        /// </summary>
        [ValidateMember]
        [IrcTag("badge-info")]
        public BadgeInfo[] badge_info { get; protected set; }

        /// <summary>
        /// <para>The emotes the sender used in the message, if any.</para>
        /// <para>Set to an empty array if the sender did not use any emotes in the message.</para>
        /// </summary>
        [ValidateMember]
        [IrcTag("emotes")]
        public Emote[] emotes { get; protected set; }

        public ChatRoomPrivmsgTags(IrcMessage message)
        {
            mod = TwitchIrcUtil.Tags.ToBool(message, "mod");
            subscriber = TwitchIrcUtil.Tags.ToBool(message, "subscriber");
            turbo = TwitchIrcUtil.Tags.ToBool(message, "turbo");
            emote_only = TwitchIrcUtil.Tags.ToBool(message, "emote-only");

            id = TwitchIrcUtil.Tags.ToString(message, "id");
            display_name = TwitchIrcUtil.Tags.ToString(message, "display-name");
            user_id = TwitchIrcUtil.Tags.ToString(message, "user-id");
            room_id = TwitchIrcUtil.Tags.ToString(message, "room-id");

            user_type = TwitchIrcUtil.Tags.ToUserType(message, "user-type");

            color = TwitchIrcUtil.Tags.FromtHtml(message, "color");
            tmi_sent_ts = TwitchIrcUtil.Tags.FromUnixEpochMilliseconds(message, "tmi-sent-ts");

            badges = TwitchIrcUtil.Tags.ToBadges(message, "badges");
            badge_info = TwitchIrcUtil.Tags.ToBadgeInfo(message, "badge-info");
            emotes = TwitchIrcUtil.Tags.ToEmotes(message, "emotes");
        }
    }

    public class
    StreamChatPrivmsgTags
    {
        /// <summary>
        /// <para>The amount of bits the sender cheered, if any.</para>
        /// <para>Set to 0 if the sender did not cheer.</para>
        /// </summary>
        [IrcTag("bits")]
        public uint bits { get; protected set; }

        /// <summary>
        /// Whether or not the sender is a moderator.
        /// </summary>
        [IrcTag("mod")]
        public bool mod { get; protected set; }

        /// <summary>
        /// Whether or not the sender is subscribed to the channel.
        /// </summary>
        [Obsolete("This tag is obsolte and can be deleted at any time. Use the 'badges' tag to look for this information instraad")]
        [IrcTag("subscriber")]
        public bool subscriber { get; protected set; }

        /// <summary>
        /// Whether or not the sender has Twitch turbo.
        /// </summary>
        [Obsolete("This tag has been deprecated and can be deleted at any time. Use the 'badges' tag to look for this information instead")]
        [IrcTag("turbo")]
        public bool turbo { get; protected set; }

        /// <summary>
        /// Whether or not the body of the message only contains emotes.
        /// </summary>
        [IrcTag("emote-only")]
        public bool emote_only { get; protected set; }

        /// <summary>
        /// The unique message ID.
        /// </summary>
        [IrcTag("id")]
        public string id { get; protected set; }

        /// <summary>
        /// <para>The display name of the sender.</para>
        /// <para>Set to an empty string if the sender never explicitly set their display name.</para>
        /// </summary>
        [IrcTag("display-name")]
        public string display_name { get; protected set; }

        /// <summary>
        /// The user ID of the sender.
        /// </summary>
        [IrcTag("user-id")]
        public string user_id { get; protected set; }

        /// <summary>
        /// The user ID of the channel the message was sent in.
        /// </summary>
        [IrcTag("room-id")]
        public string room_id { get; protected set; }

        /// <summary>
        /// <para>The sender's user type</para>
        /// <para>Set to <see cref="UserType.None"/> if the sender has no elevated privileges.</para>
        /// </summary>
        [Obsolete("This tag has been deprecated and can be deleted at any time. Use the 'badges' tag to look for this information instead")]
        [IrcTag("user-type")]
        public UserType user_type { get; protected set; }

        /// <summary>
        /// <para>The color of the sender's display name.</para>
        /// <para>Set to <see cref="Color.Empty"/> if the sender never explicitly set their display name color.</para>
        /// </summary>
        [IrcTag("color")]
        public Color color { get; protected set; }

        /// <summary>
        /// The time the message was sent.
        /// </summary>
        [IrcTag("tmi-sent-ts")]
        public DateTime tmi_sent_ts { get; protected set; }

        /// <summary>
        /// <para>The chat badges that the sender has, if any.</para>
        /// <para>Set to an empty array if the sender has no chat badges.</para>
        /// </summary>
        [ValidateMember]
        [IrcTag("badges")]
        public Badge[] badges { get; protected set; }

        /// <summary>
        /// <para>
        /// Detailed information on badge tenure.
        /// Currently, this only returns information for the subscriber badge.
        /// </para>
        /// <para>Set to an empty array if the sender has no chat badges.</para>
        /// </summary>
        [ValidateMember]
        [IrcTag("badge-info")]
        public BadgeInfo[] badge_info { get; protected set; }

        /// <summary>
        /// <para>The emotes the sender used in the message, if any.</para>
        /// <para>Set to an empty array if the sender did not use any emotes in the message.</para>
        /// </summary>
        [ValidateMember]
        [IrcTag("emotes")]
        public Emote[] emotes { get; protected set; }

        public StreamChatPrivmsgTags(IrcMessage message)
        {
            bits = TwitchIrcUtil.Tags.ToUInt32(message, "bits");

            mod = TwitchIrcUtil.Tags.ToBool(message, "mod");
            subscriber = TwitchIrcUtil.Tags.ToBool(message, "subscriber");
            turbo = TwitchIrcUtil.Tags.ToBool(message, "turbo");
            emote_only = TwitchIrcUtil.Tags.ToBool(message, "emote-only");

            id = TwitchIrcUtil.Tags.ToString(message, "id");
            display_name = TwitchIrcUtil.Tags.ToString(message, "display-name");
            user_id = TwitchIrcUtil.Tags.ToString(message, "user-id");
            room_id = TwitchIrcUtil.Tags.ToString(message, "room-id");

            user_type = TwitchIrcUtil.Tags.ToUserType(message, "user-type");

            color = TwitchIrcUtil.Tags.FromtHtml(message, "color");
            tmi_sent_ts = TwitchIrcUtil.Tags.FromUnixEpochMilliseconds(message, "tmi-sent-ts");

            badges = TwitchIrcUtil.Tags.ToBadges(message, "badges");
            badge_info = TwitchIrcUtil.Tags.ToBadgeInfo(message, "badge-info");
            emotes = TwitchIrcUtil.Tags.ToEmotes(message, "emotes");
        }
    }

    public class
    Badge
    {
        /// <summary>
        /// <para>The badge verison.</para>
        /// <para>Set to -1 when <see cref="type"/> is equal to <see cref="BadgeType.Other"/>.</para>
        /// </summary>
        [ValidateMember(Check.IsNotEqualTo, -1)]
        public int version { get; protected set; }

        /// <summary>
        /// <para>The badge type.</para>
        /// <para>Set to <see cref="BadgeType.Other"/> if no supported badge type is found.</para>
        /// </summary>
        [ValidateMember(Check.IsNotEqualTo, BadgeType.Other)]
        public BadgeType type { get; protected set; }

        public Badge(string pair)
        {
            char separator = '/';

            if (!Int32.TryParse(pair.TextAfter(separator), out int _version))
            {
                _version = -1;
            }
            version = _version;

            EnumUtil.TryParse(pair.TextBefore('/'), out BadgeType _type);
            type = _type;
        }
    }

    public class
    BadgeInfo
    {
        /// <summary>
        /// <para>How many months the user has been subscribed.</para>
        /// <para>Set to -1 when <see cref="type"/> is equal to <see cref="BadgeType.Other"/>.</para>
        /// </summary>
        [ValidateMember(Check.IsNotEqualTo, -1)]
        public int tenure { get; protected set; }

        /// <summary>
        /// <para>
        /// The badge type.
        /// Currently, only <see cref="BadgeType.Subscriber"/> is the only valid value.
        /// </para>
        /// <para>Set to <see cref="BadgeType.Other"/> if no supported badge type is found.</para>
        /// </summary>
        [ValidateMember(Check.IsNotEqualTo, BadgeType.Other)]
        public BadgeType type { get; protected set; }

        public BadgeInfo(string pair)
        {
            char separator = '/';

            if (!Int32.TryParse(pair.TextAfter(separator), out int _tenure))
            {
                _tenure = -1;
            }
            tenure = _tenure;

            EnumUtil.TryParse(pair.TextBefore('/'), out BadgeType _type);
            type = _type;
        }
    }

    public enum
    BadgeType
    {
        #region Default badge

        /// <summary>
        /// Unsupported or unknown badge type.
        /// </summary>
        [EnumMember(Value = "")]
        Other = 0,

        #endregion  

        #region Normal badges

        /// <summary>
        /// Admin badge.
        /// </summary>
        [EnumMember(Value = "admin")]
        Admin,

        /// <summary>
        /// Bits badge.
        /// </summary>
        [EnumMember(Value = "bits")]
        Bits,

        /// <summary>
        /// Bits charity badge.
        /// </summary>
        [EnumMember(Value = "bits-charity")]
        BitsCharity,

        /// <summary>
        /// Broadcaster badge.
        /// </summary>
        [EnumMember(Value = "broadcaster")]
        Broadcaster,

        /// <summary>
        /// Global mod badge.
        /// </summary>
        [EnumMember(Value = "global_mod")]
        GlobalMod,

        /// <summary>
        /// Moderator badge.
        /// </summary>
        [EnumMember(Value = "moderator")]
        Moderator,

        /// <summary>
        /// Subscriber badge.
        /// </summary>
        [EnumMember(Value = "subscriber")]
        Subscriber,

        /// <summary>
        /// Staff badge.
        /// </summary>
        [EnumMember(Value = "staff")]
        Staff,

        /// <summary>
        /// Trwitch prime badge.
        /// </summary>
        [EnumMember(Value = "premium")]
        Premium,

        /// <summary>
        /// Twitch turbo badge.
        /// </summary>
        [EnumMember(Value = "turbo")]
        Turbo,

        /// <summary>
        /// Twitch partner badge.
        /// </summary>
        [EnumMember(Value = "partner")]
        Partner,

        #endregion

        #region Other badges

        /// <summary>
        /// Twitch partner badge.
        /// </summary>
        [EnumMember(Value = "sub-gifter")]
        SubGifter,

        /// <summary>
        /// Clip champ badge.
        /// </summary>
        [EnumMember(Value = "clip-champ")]
        ClipChamp,

        /// <summary>
        /// Clip champ badge.
        /// </summary>
        [EnumMember(Value = "twitchcon2017")]
        Twitchcon2017,

        /// <summary>
        /// Clip champ badge.
        /// </summary>
        [EnumMember(Value = "overwatch-league-insider_1")]
        OverwatchLeagueInsider,

        #endregion        
    }

    public class
    Emote
    {
        /// <summary>
        /// The emote ID.
        /// </summary>
        [ValidateMember(Check.IsValid)]
        public string id { get; protected set; }

        /// <summary>
        /// The character index range(s) in the message where the emote was used.
        /// </summary>
        [ValidateMember(Check.IsValid)]
        public EmoteRange[] ranges { get; protected set; }

        public Emote(string pair)
        {
            if (pair.IsValid())
            {
                id = pair.TextBefore(':');

                List<EmoteRange> ranges_list = new List<EmoteRange>();

                string[] _ranges = pair.TextAfter(':').Split(',');
                foreach (string _range in _ranges)
                {
                    EmoteRange range = new EmoteRange(_range);
                    ranges_list.Add(range);
                }

                ranges = ranges_list.ToArray();
            }
        }
    }

    public class
    EmoteRange
    {
        /// <summary>
        /// <para>The index in the message where the first emote character is located.</para>
        /// <para>Set to -1 if the index could not be parsed.</para>
        /// </summary>
        [ValidateMember(Check.IsNotEqualTo, -1)]
        public int index_start { get; protected set; }

        /// <summary>
        /// <para>The index in the message where the last emote character is located.</para>
        /// <para>Set to -1 if the index could not be parsed.</para>
        /// </summary>
        [ValidateMember(Check.IsNotEqualTo, -1)]
        public int index_end { get; protected set; }

        public EmoteRange(string range_pair)
        {
            if (!Int32.TryParse(range_pair.TextBefore('-'), out int _index_start))
            {
                _index_start = -1;
            }
            index_start = _index_start;

            if (!Int32.TryParse(range_pair.TextAfter('-'), out int _index_end))
            {
                _index_end = -1;
            }
            index_end = _index_end;
        }
    }

    public class
    ChatRoomSupportedMessageEventArgs : EventArgs
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

        public ChatRoomSupportedMessageEventArgs(in IrcMessage message, uint channel_index = 0)
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