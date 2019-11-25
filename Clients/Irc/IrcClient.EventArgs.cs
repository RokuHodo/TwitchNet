// standard namespaces
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.Serialization;

// project namespaces
using TwitchNet.Debugger;
using TwitchNet.Extensions;
using TwitchNet.Clients.Irc;
using TwitchNet.Utilities;

namespace
TwitchNet.Clients.Irc
{
    // -------------------------------------------------------------------------------------------------------------
    //
    // - This file contains all IRC messages that are natively supported and documented in the RFC 1459 Specification.
    //   RFC 1459 Spec: https://tools.ietf.org/html/rfc1459.html
    // - Any message that is specific to Twitch can be found in its own file, TwitchIrcClient.EventArgs.
    //
    // - However, these messages do contain extra parsed members that pertain only to Twitch. These members are separated 
    //   clearly from the native IRC spec members witin each data structure.
    //
    // -------------------------------------------------------------------------------------------------------------

    #region Command: 353 (NamReply)

    public class
    NamReplyEventArgs : IrcMessageEventArgs
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

        public NamReplyEventArgs(in IrcMessage message) : base(message)
        {
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

    #endregion

    #region Command: 366 (EndOfNames)

    public class
    EndOfNamesEventArgs : IrcMessageEventArgs
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

        public EndOfNamesEventArgs(in IrcMessage message, Dictionary<string, List<string>> names) : base(message)
        {
            if (message.parameters.Length > 1)
            {
                client = message.parameters[0];
                channel = message.parameters[1];
            }

            this.names = names[channel].ToArray();
        }
    }

    #endregion

    #region Command: 372 (Motd)

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

    #endregion

    #region Command: 421 (UnknownCommand)

    public class
    UnknownCommandEventArgs : IrcMessageEventArgs
    {
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

        public UnknownCommandEventArgs(in IrcMessage message) : base(message)
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

    #endregion

    #region Command: JOIN

    public class
    JoinEventArgs : IrcMessageEventArgs
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
            irc_message = message;

            nick = message.server_or_nick;

            if (message.parameters.Length > 0)
            {
                channel = message.parameters[0];
            }
		}
    }

    #endregion

    #region Command: PART

    public class
    PartEventArgs : IrcMessageEventArgs
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
            irc_message = message;

            nick = message.server_or_nick;

            if (message.parameters.Length > 0)
            {
                channel = message.parameters[0];
            }
        }		
    }

    #endregion

    #region Command: MODE

    public class
    ChannelModeEventArgs : IrcMessageEventArgs
    {
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

        public ChannelModeEventArgs(in IrcMessage message) : base(message)
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
    ChannelOperatorEventArgs : IrcMessageEventArgs
    {
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

        public ChannelOperatorEventArgs(ChannelModeEventArgs args) : base(args.irc_message)
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

    #endregion

    #region Command: PRIVMSG

    public class
    PrivmsgEventArgs : IrcMessageEventArgs
    {
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
        /// <para>Set to null if no tags were sent with the message.</para>
        /// </summary>
        [ValidateMember]
        [ValidateMember(Check.TagsMissing)]
        // [ValidateMember(Check.TagsExtra)]
        public PrivmsgTags tags { get; protected set; }

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
                tags = new PrivmsgTags(irc_message.tags);
            }
        }
    }

    public class
    PrivmsgTags
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

        public PrivmsgTags(in IrcTags tags)
        {
            bits            = TwitchIrcUtil.Tags.ToUInt32(tags, "bits");

            mod             = TwitchIrcUtil.Tags.ToBool(tags, "mod");
            subscriber      = TwitchIrcUtil.Tags.ToBool(tags, "subscriber");
            turbo           = TwitchIrcUtil.Tags.ToBool(tags, "turbo");
            emote_only      = TwitchIrcUtil.Tags.ToBool(tags, "emote-only");

            id              = TwitchIrcUtil.Tags.ToString(tags, "id");
            display_name    = TwitchIrcUtil.Tags.ToString(tags, "display-name");
            user_id         = TwitchIrcUtil.Tags.ToString(tags, "user-id");
            room_id         = TwitchIrcUtil.Tags.ToString(tags, "room-id");

            user_type       = TwitchIrcUtil.Tags.ToEnum<UserType>(tags, "user-type");

            color           = TwitchIrcUtil.Tags.FromtHtml(tags, "color");
            tmi_sent_ts     = TwitchIrcUtil.Tags.FromUnixEpochMilliseconds(tags, "tmi-sent-ts");

            badges          = TwitchIrcUtil.Tags.ToBadges(tags, "badges");
            badge_info      = TwitchIrcUtil.Tags.ToBadgeInfo(tags, "badge-info");
            emotes          = TwitchIrcUtil.Tags.ToEmotes(tags, "emotes");
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
        /// </summary>\
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

    #endregion

    #region Command: NOTICE

    public class
    NoticeEventArgs : IrcMessageEventArgs
    {
        /// <summary>
        /// Whether or not IRC tags were sent with the message.
        /// </summary>
        public bool tags_exist { get; private set; }

        /// <summary>
        /// <para>The converted IRC tags attached to the message.</para>
        /// <para>Set to null if no tags were sent with the message.</para>
        /// </summary>
        [ValidateMember(Check.TagsMissing)]
        public NoticeTags tags { get; protected set; }

        /// <summary>
        /// <para>The channel notice was sent in.</para>
        /// </summary>
        [ValidateMember(Check.IsValid)]
        public string channel { get; protected set; }

        /// <summary>
        /// The notice message sent by the server.
        /// </summary>
        [ValidateMember(Check.IsValid)]
        public string body { get; protected set; }

        /// <summary>
        /// Creates a new instance of the <see cref="NoticeEventArgs"/> class.
        /// </summary>
        /// <param name="message">The IRC message to parse.</param>
        public NoticeEventArgs(in IrcMessage message) : base(message)
        {
            tags_exist = message.tags_exist;
            if (tags_exist)
            {
                tags = new NoticeTags(message.tags);
            }

            if (message.parameters.Length > 0)
            {
                channel = message.parameters[0];
            }

            body = message.trailing;
        }
    }

    public class
    NoticeTags
    {
        /// <summary>
        /// The id that describes the notice from the server.
        /// <para>Set to <see cref="UserNoticeType.Other"/> if the id could not be parsed.</para>
        /// </summary>
        [IrcTag("msg-id")]
        public NoticeType msg_id { get; protected set; }

        public
        NoticeTags(in IrcTags tags)
        {
            msg_id = TwitchIrcUtil.Tags.ToEnum<NoticeType>(tags, "msg-id");
        }
    }

    public enum
    NoticeType
    {
        /// <summary>
        /// Unknown or unsupported msg-id.
        /// </summary>
        [EnumMember(Value = "")]
        Other = 0,

        // --- \\
        /// <summary>
        /// {user} is already banned in this channel.
        /// </summary>
        [EnumMember(Value = "already_banned")]
        AlreadyBanned, 

        /// <summary>
        /// This room is not in emote-only mode.
        /// </summary>
        [EnumMember(Value = "already_emote_only_off")]
        AlreadyEmoteOnlyOff,

        /// <summary>
        /// This room is already in emote-only mode.
        /// </summary>
        [EnumMember(Value = "already_emote_only_on")]
        AlreadyEmoteOnlyOn,

        /// <summary>
        /// This room is not in r9k mode.
        /// </summary>
        [EnumMember(Value = "already_r9k_off")]
        AlreadyR9kOff,

        /// <summary>
        /// This room is already in r9k mode.
        /// </summary>
        [EnumMember(Value = "already_r9k_on")]
        AlreadyR9kOn,

        /// <summary>
        /// This room is not in subscribers-only mode.
        /// </summary>
        [EnumMember(Value = "already_subs_off")]
        AlreadySubsOff,

        /// <summary>
        /// This room is already in subscribers-only mode.
        /// </summary>
        [EnumMember(Value = "already_subs_on")]
        AlreadySubsOn,

        // --- \\
        /// <summary> - INC
        /// You cannot ban admin {user}. Please email support@twitch.tv if an admin is being abusive.
        /// </summary>
        [EnumMember(Value = "bad_ban_admin")]
        BadBanAdmin,

        // --- \\
        /// <summary>
        /// You cannot ban anonymous users.
        /// </summary>
        [EnumMember(Value = "bad_ban_anon")]
        BadBanAnon,

        // --- \\
        /// <summary>
        /// You cannot ban the broadcaster.
        /// </summary>
        [EnumMember(Value = "bad_ban_broadcaster")]
        BadBanBroadcaster,

        // --- \\
        /// <summary>
        /// You cannot ban global moderator {user}. Please email support@twitch.tv if a global moderator is being abusive.
        /// </summary>
        [EnumMember(Value = "bad_ban_global_mod")]
        BadBanGlobalMod,

        // --- \\
        /// <summary>
        /// You cannot ban moderator {user} unless you are the owner of this channel.
        /// </summary>
        [EnumMember(Value = "bad_ban_mod")]
        BadBanMod,

        // --- \\
        /// <summary>
        /// You cannot ban yourself.
        /// </summary>
        [EnumMember(Value = "bad_ban_self")]
        BadBanSelf,

        // --- \\
        /// <summary>
        /// You cannot ban staff {user}. Please email support@twitch.tv if a staff member is being abusive.
        /// </summary>
        [EnumMember(Value = "bad_ban_staff")]
        BadBanStaff,

        /// <summary>
        /// Failed to start commercial.
        /// </summary>
        [EnumMember(Value = "bad_commercial_error")]
        BadCommercialError,

        /// <summary>
        /// You cannot delete the broadcaster's messages.
        /// </summary>
        [EnumMember(Value = "bad_delete_message_broadcaster")]
        BadDeleteMessageBroadcaster,

        /// <summary>
        /// You cannot delete messages from another moderator {user}.
        /// </summary>
        [EnumMember(Value = "bad_delete_message_mod")]
        BadDeleteMessageMod,

        /// <summary>
        /// There was a problem hosting {channel}. Please try again in a minute.
        /// </summary>
        [EnumMember(Value = "bad_host_error")]
        BadHostError,

        /// <summary>
        /// This channel is already hosting {channel}.
        /// </summary>
        [EnumMember(Value = "bad_host_hosting")]
        BadHostHosting,

        /// <summary>
        /// Host target cannot be changed more than {number} times every half hour.
        /// </summary>
        [EnumMember(Value = "bad_host_rate_exceeded")]
        BadHostRateExceeded,

        /// <summary>
        /// This channel is unable to be hosted.
        /// </summary>
        [EnumMember(Value = "bad_host_rejected")]
        BadHostRejected,

        /// <summary>
        /// A channel cannot host itself.
        /// </summary>
        [EnumMember(Value = "bad_host_self")]
        BadHostSelf,

        /// <summary>
        /// Sorry, /marker is not available through this client.
        /// </summary>
        [EnumMember(Value = "bad_marker_client")]
        BadMarkerClient,

        /// <summary>
        /// {user} is banned in this channel. You must unban this user before granting mod status.
        /// </summary>
        [EnumMember(Value = "bad_mod_banned")]
        BadModBanned,

        /// <summary>
        /// {user} is already a moderator of this channel.
        /// </summary>
        [EnumMember(Value = "bad_mod_mod")]
        BadModMod,

        /// <summary>
        /// You cannot set slow delay to more than {number} seconds.
        /// </summary>
        [EnumMember(Value = "bad_slow_duration")]
        BadSlowDuration,

        /// <summary>
        /// You cannot timeout admin {user}. Please email support@twitch.tv if an admin is being abusive.
        /// </summary>
        [EnumMember(Value = "bad_timeout_admin")]
        BadTimeoutAdmin,

        /// <summary>
        /// You cannot timeout anonymous users.
        /// </summary>
        [EnumMember(Value = "bad_timeout_anon")]
        BadTimeoutAnon,

        /// <summary>
        /// You cannot timeout the broadcaster.
        /// </summary>
        [EnumMember(Value = "bad_timeout_broadcaster")]
        BadTimeoutBroadcaster,

        /// <summary>
        /// You cannot time a user out for more than {seconds}.
        /// </summary>
        [EnumMember(Value = "bad_timeout_duration")]
        BadTimeoutDuration,

        /// <summary>
        /// You cannot timeout global moderator {user}. Please email support@twitch.tv if a global moderator is being abusive.
        /// </summary>
        [EnumMember(Value = "bad_timeout_global_mod")]
        BadTimeoutGlobalMod,

        /// <summary>
        /// You cannot timeout moderator {user} unless you are the owner of this channel.
        /// </summary>
        [EnumMember(Value = "bad_timeout_mod")]
        BadTimeoutMod,

        /// <summary>
        /// You cannot timeout yourself.
        /// </summary>
        [EnumMember(Value = "bad_timeout_self")]
        BadTimeoutSelf,

        /// <summary>
        /// You cannot timeout staff {user}. Please email support@twitch.tv if a staff member is being abusive.
        /// </summary>
        [EnumMember(Value = "bad_timeout_staff")]
        BadTimeoutStaff,

        // -- \\
        /// <summary>
        /// {user} is not banned from this channel.
        /// </summary>
        [EnumMember(Value = "bad_unban_no_ban")]
        BadUnbanNoBan,

        /// <summary>
        /// There was a problem exiting host mode. Please try again in a minute.
        /// </summary>
        [EnumMember(Value = "bad_unhost_error")]
        BadUnhostError,

        /// <summary>
        /// {user} is not a moderator of this channel.
        /// </summary>
        [EnumMember(Value = "bad_unmod_mod")]
        BadUnmodMod,

        // --- \\
        /// <summary>
        /// {user} is now banned from this channel.
        /// </summary>
        [EnumMember(Value = "ban_success")]
        BanSuccess,

        /// <summary>
        /// Commands available to you in this room (use /help {command} for details): {list of commands}
        /// </summary>
        [EnumMember(Value = "cmds_available")]
        CmdsAvailable,

        /// <summary>
        /// Your color has been changed.
        /// </summary>
        [EnumMember(Value = "color_changed")]
        ColorChanged,

        /// <summary>
        /// Initiating {number} second commercial break. Please keep in mind that your stream is still live and not everyone will get a commercial.
        /// </summary>
        [EnumMember(Value = "commercial_success")]
        CommercialSuccess,

        /// <summary>
        /// The message from {user} is now deleted.
        /// </summary>
        [EnumMember(Value = "delete_message_success")]
        DeleteMessageSuccess,

        /// <summary>
        /// This room is no longer in emote-only mode.
        /// </summary>
        [EnumMember(Value = "emote_only_off")]
        EmoteOnlyOff,

        /// <summary>
        /// This room is now in emote-only mode.
        /// </summary>
        [EnumMember(Value = "emote_only_on")]
        EmoteOnlyOn,

        /// <summary>
        /// This room is no longer in followers-only mode.
        /// </summary>
        [EnumMember(Value = "followers_off")]
        FollowersOff,

        /// <summary>
        /// This room is now in {duration} followers-only mode.
        /// </summary>
        [EnumMember(Value = "followers_on")]
        FollowersOn,

        /// <summary>
        /// This room is now in followers-only mode.
        /// </summary>
        [EnumMember(Value = "followers_onzero")]
        FollowersOnzero,

        /// <summary>
        /// Exited host mode.
        /// </summary>
        [EnumMember(Value = "host_off")]
        HostOff,

        /// <summary>
        /// Now hosting {channel}.
        /// </summary>
        [EnumMember(Value = "host_on")]
        HostOn,

        /// <summary>
        /// {user} is now hosting you.
        /// </summary>
        [EnumMember(Value = "host_success")]
        HostSuccess,

        /// <summary>
        /// {user} is now hosting you for up to {number} viewers.
        /// </summary>
        [EnumMember(Value = "host_success_viewers")]
        HostSuccessViewers,

        /// <summary>
        /// {channel} has gone offline. Exiting host mode.
        /// </summary>
        [EnumMember(Value = "host_target_went_offline")]
        HostTargetWentOffline,

        /// <summary>
        /// {number} host commands remaining this half hour.
        /// </summary>
        [EnumMember(Value = "hosts_remaining")]
        HostsRemaining,

        /// <summary>
        /// Invalid username: {user}
        /// </summary>
        [EnumMember(Value = "invalid_user")]
        InvalidUser,

        /// <summary>
        /// You have added {user} as a moderator of this channel.
        /// </summary>
        [EnumMember(Value = "mod_success")]
        ModSuccess,

        /// <summary>
        /// You are permanently banned from talking in {channel}.
        /// </summary>
        [EnumMember(Value = "msg_banned")]
        MsgBanned,

        /// <summary>
        /// Your message was not sent because it contained too many unprocessable characters. If you believe this is an error, please rephrase and try again.
        /// </summary>
        [EnumMember(Value = "msg_bad_characters")]
        MsgBadCharacters,

        /// <summary>
        /// Your message was not sent because your account is not in good standing in this channel.
        /// </summary>
        [EnumMember(Value = "msg_channel_blocked")]
        MsgChannelBlocked,

        /// <summary>
        /// This channel has been suspended.
        /// </summary>
        [EnumMember(Value = "msg_channel_suspended")]
        MsgChannelSuspended,

        /// <summary>
        /// Your message was not sent because it is identical to the previous one you sent, less than 30 seconds ago.
        /// </summary>
        [EnumMember(Value = "msg_duplicate")]
        MsgDuplicate,

        /// <summary>
        /// This room is in emote only mode. You can find your currently available emoticons using the smiley in the chat text area.
        /// </summary>
        [EnumMember(Value = "msg_emoteonly")]
        MsgEmoteonly,

        /// <summary>
        /// You must Facebook Connect to send messages to this channel. You can Facebook Connect in your Twitch settings under the connections tab.
        /// </summary>
        [EnumMember(Value = "msg_facebook")]
        MsgFacebook,

        /// <summary>
        /// This room is in {duration} followers-only mode. Follow {channel} to join the community!
        /// </summary>
        [EnumMember(Value = "msg_followersonly")]
        MsgFollowersonly,

        /// <summary>
        /// This room is in {duration1} followers-only mode. You have been following for {duration2}. Continue following to chat!
        /// </summary>
        [EnumMember(Value = "msg_followersonly_followed")]
        MsgFollowersonlyFollowed,

        /// <summary>
        /// This room is in followers-only mode. Follow {channel} to join the community!
        /// </summary>
        [EnumMember(Value = "msg_followersonly_zero")]
        MsgFollowersonlyZero,

        /// <summary>
        /// This room is in r9k mode and the message you attempted to send is not unique.
        /// </summary>
        [EnumMember(Value = "msg_r9k")]
        MsgR9k,

        /// <summary>
        /// Your message was not sent because you are sending messages too quickly.
        /// </summary>
        [EnumMember(Value = "msg_ratelimit")]
        MsgRatelimit,

        /// <summary>
        /// Hey! Your message is being checked by mods and has not been sent.
        /// </summary>
        [EnumMember(Value = "msg_rejected")]
        MsgRejected,

        /// <summary>
        /// Your message wasn't posted due to conflicts with the channel's moderation settings.
        /// </summary>
        [EnumMember(Value = "msg_rejected_mandatory")]
        MsgRejectedMandatory,

        /// <summary>
        /// The room was not found.
        /// </summary>
        [EnumMember(Value = "msg_room_not_found")]
        MsgRoomNotFound,

        /// <summary>
        /// This room is in slow mode and you are sending messages too quickly. You will be able to talk again in {number} seconds.
        /// </summary>
        [EnumMember(Value = "msg_slowmode")]
        MsgSlowmode,

        /// <summary>
        /// This room is in subscribers only mode. To talk, purchase a channel subscription at https://www.twitch.tv/products/{broadcaster login name}/ticket?ref=subscriber_only_mode_chat.
        /// </summary>
        [EnumMember(Value = "msg_subsonly")]
        MsgSubsOnly,

        /// <summary>
        /// Your account has been suspended.
        /// </summary>
        [EnumMember(Value = "msg_suspended")]
        MsgSuspended,

        /// <summary>
        /// You are banned from talking in {channel} for {number} more seconds.
        /// </summary>
        [EnumMember(Value = "msg_timedout")]
        MsgTimedOut,

        /// <summary>
        /// This room requires a verified email address to chat. Please verify your email at https://www.twitch.tv/settings/profile.
        /// </summary>
        [EnumMember(Value = "msg_verified_email")]
        MsgVerifiedEmail,

        /// <summary>
        /// No help available.
        /// </summary>
        [EnumMember(Value = "no_help")]
        NoHelp,

        /// <summary>
        /// There are no moderators of this channel.
        /// </summary>
        [EnumMember(Value = "no_mods")]
        NoMods,

        /// <summary>
        /// No channel is currently being hosted.
        /// </summary>
        [EnumMember(Value = "not_hosting")]
        NotHosting,

        /// <summary>
        /// You don’t have permission to perform that action.
        /// </summary>
        [EnumMember(Value = "no_permission")]
        NoPermission,

        /// <summary>
        /// This room is no longer in r9k mode.
        /// </summary>
        [EnumMember(Value = "r9k_off")]
        R9kOff,

        /// <summary>
        /// This room is now in r9k mode.
        /// </summary>
        [EnumMember(Value = "r9k_on")]
        R9kOn,

        /// <summary>
        /// You already have a raid in progress.
        /// </summary>
        [EnumMember(Value = "raid_error_already_raiding")]
        RaidErrorAlreadyRaiding,

        /// <summary>
        /// You cannot raid this channel.
        /// </summary>
        [EnumMember(Value = "raid_error_forbidden")]
        RaidErrorForbidden,

        /// <summary>
        /// A channel cannot raid itself.
        /// </summary>
        [EnumMember(Value = "raid_error_self")]
        RaidErrorSelf,

        /// <summary>
        /// Sorry, you have more viewers than the maximum currently supported by raids right now.
        /// </summary>
        [EnumMember(Value = "raid_error_too_many_viewers")]
        RaidErrorTooManyViewers,

        /// <summary>
        /// There was a problem raiding {channel}. Please try again in a minute.
        /// </summary>
        [EnumMember(Value = "raid_error_unexpected")]
        RaidErrorUnexpected,

        /// <summary>
        /// This channel is intended for mature audiences.
        /// </summary>
        [EnumMember(Value = "raid_notice_mature")]
        RaidNoticeMature,

        /// <summary>
        /// This channel has follower or subscriber only chat.
        /// </summary>
        [EnumMember(Value = "raid_notice_restricted_chat")]
        RaidNoticeRestrictedChat,

        /// <summary>
        /// The moderators of this channel are: {list of users}
        /// </summary>
        [EnumMember(Value = "room_mods")]
        RoomMods,

        /// <summary>
        /// This room is no longer in slow mode.
        /// </summary>
        [EnumMember(Value = "slow_off")]
        SlowOff,

        /// <summary>
        /// This room is now in slow mode. You may send messages every {number} seconds.
        /// </summary>
        [EnumMember(Value = "slow_on")]
        SlowOn,

        /// <summary>
        /// This room is no longer in subscribers-only mode.
        /// </summary>
        [EnumMember(Value = "subs_off")]
        SubsOff,

        /// <summary>
        /// This room is now in subscribers-only mode.
        /// </summary>
        [EnumMember(Value = "subs_on")]
        SubsOn,

        /// <summary>
        /// {user} is not timed out from this channel.
        /// </summary>
        [EnumMember(Value = "timeout_no_timeout")]
        TimeoutNoTimeout,

        /// <summary>
        /// {user} has been timed out for {duration} seconds.
        /// </summary>
        [EnumMember(Value = "timeout_success")]
        TimeoutSuccess,

        /// <summary>
        /// The community has closed channel {channel} due to Terms of Service violations.
        /// </summary>
        [EnumMember(Value = "tos_ban")]
        TosBan,

        /// <summary>
        /// Only turbo users can specify an arbitrary hex color. Use one of the following instead: {list of colors}.
        /// </summary>
        [EnumMember(Value = "turbo_only_color")]
        TurboOnlyColor,

        /// <summary>
        /// {user} is no longer banned from this channel.
        /// </summary>
        [EnumMember(Value = "unban_success")]
        UnbanSuccess,

        /// <summary>
        /// You have removed {user} as a moderator of this channel.
        /// </summary>
        [EnumMember(Value = "unmod_success")]
        UnmodSuccess,

        /// <summary>
        /// You do not have an active raid.
        /// </summary>
        [EnumMember(Value = "unraid_error_no_active_raid")]
        UnraidErrorNoActiveRaid,

        /// <summary>
        /// There was a problem stopping the raid. Please try again in a minute.
        /// </summary>
        [EnumMember(Value = "unraid_error_unexpected")]
        UnraidErrorUnexpected,

        /// <summary>
        /// The raid has been cancelled.
        /// </summary>
        [EnumMember(Value = "unraid_success")]
        UnraidSuccess,

        /// <summary>
        /// Unrecognized command: {command}
        /// </summary>
        [EnumMember(Value = "unrecognized_cmd")]
        UnrecognizedCmd,

        /// <summary>
        /// {user} is permanently banned. Use "/unban" to remove a ban.
        /// </summary>
        [EnumMember(Value = "untimeout_banned")]
        UntimeoutBanned,

        /// <summary>
        /// {user} is no longer timed out in this channel.
        /// </summary>
        [EnumMember(Value = "untimeout_success")]
        UntimeoutSuccess,

        /// <summary>
        /// Usage: “/ban {username} [reason]” - Permanently prevent a user from chatting. Reason is optional and will be shown to the target and other moderators. Use “/unban” to remove a ban.
        /// </summary>
        [EnumMember(Value = "usage_ban")]
        UsageBan,

        /// <summary>
        /// Usage: “/clear” - Clear chat history for all users in this room.
        /// </summary>
        [EnumMember(Value = "usage_clear")]
        UsageClear,

        /// <summary>
        /// Usage: “/color” {color} - Change your username color. Color must be in hex (#000000) or one of the following: Blue, BlueViolet, CadetBlue, Chocolate, Coral, DodgerBlue, Firebrick, GoldenRod, Green, HotPink, OrangeRed, Red, SeaGreen, SpringGreen, YellowGreen.
        /// </summary>
        [EnumMember(Value = "usage_color")]
        UsageColor,

        /// <summary>
        /// Usage: “/commercial [length]” - Triggers a commercial. Length (optional) must be a positive number of seconds.
        /// </summary>
        [EnumMember(Value = "usage_commercial")]
        UsageCommercial,

        /// <summary>
        /// Usage: “/disconnect” - Reconnects to chat.
        /// </summary>
        [EnumMember(Value = "usage_disconnect")]
        UsageDisconnect,

        /// <summary>
        /// Usage: /emoteonlyoff” - Disables emote-only mode.
        /// </summary>
        [EnumMember(Value = "usage_emote_only_off")]
        UsageEmoteOnlyOff,

        /// <summary>
        /// Usage: “/emoteonly” - Enables emote-only mode (only emoticons may be used in chat). Use /emoteonlyoff to disable.
        /// </summary>
        [EnumMember(Value = "usage_emote_only_on")]
        UsageEmoteOnlyOn,

        /// <summary>
        /// Usage: /followersoff” - Disables followers-only mode.
        /// </summary>
        [EnumMember(Value = "usage_followers_off")]
        UsageFollowersOff,

        /// <summary>
        /// Usage: “/followers - Enables followers-only mode (only users who have followed for “duration” may chat). Examples: “30m”, “1 week”, “5 days 12 hours”. Must be less than 3 months.
        /// </summary>
        [EnumMember(Value = "usage_followers_on")]
        UsageFollowersOn,

        /// <summary>
        /// Usage: “/help” - Lists the commands available to you in this room.
        /// </summary>
        [EnumMember(Value = "usage_help")]
        UsageHelp,

        /// <summary>
        /// Usage: “/host {channel}” - Host another channel. Use “/unhost” to unset host mode.
        /// </summary>
        [EnumMember(Value = "usage_host")]
        UsageHost,

        /// <summary>
        /// Usage: “/marker {optional comment}” - Adds a stream marker (with an optional comment, max 140 characters) at the current timestamp. You can use markers in the Highlighter for easier editing.
        /// </summary>
        [EnumMember(Value = "usage_marker")]
        UsageMarker,

        /// <summary>
        /// Usage: “/me {message}” - Send an “emote” message in the third person.
        /// </summary>
        [EnumMember(Value = "usage_me")]
        UsageMe,

        /// <summary>
        /// Usage: “/mod {username}” - Grant mod status to a user. Use “/mods” to list the moderators of this channel.
        /// </summary>
        [EnumMember(Value = "usage_mod")]
        UsageMod,

        /// <summary>
        /// Usage: “/mods” - Lists the moderators of this channel.
        /// </summary>
        [EnumMember(Value = "usage_mods")]
        UsageMods,

        /// <summary>
        /// Usage: “/r9kbetaoff” - Disables r9k mode.
        /// </summary>
        [EnumMember(Value = "usage_r9k_off")]
        UsageR9kOff,

        /// <summary>
        /// Usage: “/r9kbeta” - Enables r9k mode. Use “/r9kbetaoff“ to disable.
        /// </summary>
        [EnumMember(Value = "usage_r9k_on")]
        UsageR9kOn,

        /// <summary>
        /// Usage: “/raid {channel}” - Raid another channel. Use “/unraid” to cancel the Raid.
        /// </summary>
        [EnumMember(Value = "usage_raid")]
        UsageRaid,

        /// <summary>
        /// Usage: “/slowoff” - Disables slow mode.
        /// </summary>
        [EnumMember(Value = "usage_slow_off")]
        UsageSlowOff,

        /// <summary>
        /// Usage: “/slow” [duration] - Enables slow mode (limit how often users may send messages). Duration (optional, default={number}) must be a positive integer number of seconds. Use “/slowoff” to disable.
        /// </summary>
        [EnumMember(Value = "usage_slow_on")]
        UsageSlowOn,

        /// <summary>
        /// Usage: “/subscribersoff” - Disables subscribers-only mode.
        /// </summary>
        [EnumMember(Value = "usage_subs_off")]
        UsageSubsOff,

        /// <summary>
        /// Usage: “/subscribers” - Enables subscribers-only mode (only subscribers may chat in this channel). Use “/subscribersoff” to disable.
        /// </summary>
        [EnumMember(Value = "usage_subs_on")]
        UsageSubsOn,

        /// <summary>
        /// Usage: “/timeout {username} [duration][time unit] [reason]" - Temporarily prevent a user from chatting. Duration (optional, default=10 minutes) must be a positive integer; time unit (optional, default=s) must be one of s, m, h, d, w; maximum duration is 2 weeks. Combinations like 1d2h are also allowed. Reason is optional and will be shown to the target user and other moderators. Use “untimeout” to remove a timeout.
        /// </summary>
        [EnumMember(Value = "usage_timeout")]
        UsageTimeout,

        /// <summary>
        /// Usage: “/unban {username}” - Removes a ban on a user.
        /// </summary>
        [EnumMember(Value = "usage_unban")]
        UsageUnban,

        /// <summary>
        /// Usage: “/unhost” - Stop hosting another channel.
        /// </summary>
        [EnumMember(Value = "usage_unhost")]
        UsageUnhost,

        /// <summary>
        /// Usage: “/unmod {username}” - Revoke mod status from a user. Use “/mods” to list the moderators of this channel.
        /// </summary>
        [EnumMember(Value = "usage_unmod")]
        UsageUnmod,

        /// <summary>
        /// Usage: “/unraid” - Cancel the Raid.
        /// </summary>
        [EnumMember(Value = "usage_unraid")]
        UsageUnraid,

        /// <summary>
        /// Usage: “/raid {username}” - Removes a timeout on a user.
        /// </summary>
        [EnumMember(Value = "usage_untimeout")]
        UsageUntimeout,

        /// <summary>
        /// You have been banned from sending whispers.
        /// </summary>
        [EnumMember(Value = "whisper_banned")]
        WhisperBanned,

        /// <summary>
        /// That user has been banned from receiving whispers.
        /// </summary>
        [EnumMember(Value = "whisper_banned_recipient")]
        WhisperBannedRecipient,

        /// <summary>
        /// Usage: {login} {message}
        /// </summary>
        [EnumMember(Value = "whisper_invalid_args")]
        WhisperInvalidArgs,

        /// <summary>
        /// No user matching that login.
        /// </summary>
        [EnumMember(Value = "whisper_invalid_login")]
        WhisperInvalidLogin,

        /// <summary>
        /// You cannot whisper to yourself.
        /// </summary>
        [EnumMember(Value = "whisper_invalid_self")]
        WhisperInvalidSelf,

        /// <summary>
        /// You are sending whispers too fast. Try again in a minute.
        /// </summary>
        [EnumMember(Value = "whisper_limit_per_min")]
        WhisperLimitPerMin,

        /// <summary>
        /// You are sending whispers too fast. Try again in a second.
        /// </summary>
        [EnumMember(Value = "whisper_limit_per_sec")]
        WhisperLimitPerSec,

        /// <summary>
        /// Your settings prevent you from sending this whisper.
        /// </summary>
        [EnumMember(Value = "whisper_restricted")]
        WhisperRestricted,

        /// <summary>
        /// That user's settings prevent them from receiving this whisper.
        /// </summary>
        [EnumMember(Value = "whisper_restricted_recipient")]
        WhisperRestrictedRecipient
    }

    /*
    public class
    BanFailedEventArgs : IrcMessageEventArgs
    {
        /// <summary>
        /// Why the ban failed.
        /// </summary>
        [ValidateMember(Check.IsValid)]
        public NoticeType reason { get; protected set; }

        /// <summary>
        /// The channel that the NOTICE was sent to.
        /// </summary>
        [ValidateMember(Check.IsValid)]
        public string channel { get; protected set; }

        /// <summary>
        /// The nick of the user who was attempted and failed to be banned.
        /// Set to an empty string if no user nick was available.
        /// </summary>
        [ValidateMember(Check.IsValid)]
        public string user_nick { get; protected set; }

        /// <summary>
        /// The notice message sent by the server.
        /// </summary>
        [ValidateMember(Check.IsValid)]
        public string body { get; protected set; }

        public
        BanFailedEventArgs(NoticeEventArgs args) : base(args.irc_message)
        {
            reason = args.tags.msg_id;
            channel = args.channel;
            body = args.body;

            user_nick = string.Empty;
            switch (args.tags.msg_id)
            {
                case NoticeType.AlreadyBanned:
                {
                    user_nick = args.body.TextBefore(' '); 
                }
                break;

                case NoticeType.BadBanAdmin:
                {
                    user_nick = args.body.TextBetween("admin ", ".");
                }
                break;

                case NoticeType.BadBanGlobalMod:
                case NoticeType.BadBanMod:
                {
                    user_nick = args.body.TextBetween("moderator ", " ").TrimEnd('.');
                }
                break;

                case NoticeType.BadBanStaff:
                {
                    user_nick = args.body.TextBetween("staff ", ".");
                }
                break;

                case NoticeType.BadBanBroadcaster:
                {
                    user_nick = channel.TextAfter('#');
                }
                break;
            }
        }
    }

    public class
    BanSuccessEventArgs : IrcMessageEventArgs
    {
        /// <summary>
        /// The channel that the NOTICE was sent to.
        /// </summary>
        [ValidateMember(Check.IsValid)]
        public string channel { get; protected set; }

        /// <summary>
        /// The nick of the user who was banned.
        /// </summary>
        [ValidateMember(Check.IsValid)]
        public string user_nick { get; protected set; }

        /// <summary>
        /// The notice message sent by the server.
        /// </summary>
        [ValidateMember(Check.IsValid)]
        public string body { get; protected set; }

        public
        BanSuccessEventArgs(NoticeEventArgs args) : base(args.irc_message)
        {
            channel = args.channel;
            body = args.body;

            user_nick = body.TextBefore(' ');
        }
    }

    public class
    UnbanFailedEventArgs : IrcMessageEventArgs
    {
        /// <summary>
        /// Why the ban failed.
        /// </summary>
        [ValidateMember(Check.IsValid)]
        public NoticeType reason { get; protected set; }

        /// <summary>
        /// The channel that the NOTICE was sent to.
        /// </summary>
        [ValidateMember(Check.IsValid)]
        public string channel { get; protected set; }

        /// <summary>
        /// The nick of the user who was attempted and failed to be unbanned.
        /// </summary>
        [ValidateMember(Check.IsValid)]
        public string user_nick { get; protected set; }

        /// <summary>
        /// The notice message sent by the server.
        /// </summary>
        [ValidateMember(Check.IsValid)]
        public string body { get; protected set; }

        public
        UnbanFailedEventArgs(NoticeEventArgs args) : base(args.irc_message)
        {
            reason = args.tags.msg_id;

            channel = args.channel;
            body = args.body;

            user_nick = body.TextBefore(' ');
        }
    }

    public class
    UnbanSuccessEventArgs : IrcMessageEventArgs
    {
        /// <summary>
        /// The channel that the NOTICE was sent to.
        /// </summary>
        [ValidateMember(Check.IsValid)]
        public string channel { get; protected set; }

        /// <summary>
        /// The nick of the user who was attempted and failed to be unbanned.
        /// </summary>
        [ValidateMember(Check.IsValid)]
        public string user_nick { get; protected set; }

        /// <summary>
        /// The notice message sent by the server.
        /// </summary>
        [ValidateMember(Check.IsValid)]
        public string body { get; protected set; }

        public
        UnbanSuccessEventArgs(NoticeEventArgs args) : base(args.irc_message)
        {
            channel = args.channel;
            body = args.body;

            user_nick = body.TextBefore(' ');
        }
    }

    public class
    ModsEventArgs : IrcMessageEventArgs
    {
        /// <summary>
        /// The channel that the NOTICE was sent to.
        /// </summary>
        [ValidateMember(Check.IsValid)]
        public string channel { get; protected set; }

        /// <summary>
        /// The room's moderators.
        /// Set to an empty array if there are no assigned moderators.
        /// </summary>
        [ValidateMember(Check.IsValid)]
        public string[] user_nicks { get; protected set; }

        public
        ModsEventArgs(NoticeEventArgs args) : base(args.irc_message)
        {
            channel = args.channel;

            string nicks = args.body.TextAfter(':').Trim(' ');
            user_nicks = nicks.Split(new string[] { ", " }, StringSplitOptions.RemoveEmptyEntries);
        }
    }

    public class
    BadHostHostingEventArgs : IrcMessageEventArgs
    {
        /// <summary>
        /// The channel that the NOTICE was sent to.
        /// </summary>
        [ValidateMember(Check.IsValid)]
        public string channel { get; protected set; }

        /// <summary>
        /// The user that was attempted to be hosted, but is already being hosted.
        /// </summary>
        [ValidateMember(Check.IsValid)]
        public string user_nick { get; protected set; }

        public
        BadHostHostingEventArgs(NoticeEventArgs args) : base(args.irc_message)
        {
            channel = args.channel;

            int index = args.body.LastIndexOf(' ');
            if (index != 1)
            {
                user_nick = args.body.TextBetween(' ', '.', index);
            }
        }
    }

    public class
    BadHostRateExceededEventArgs : IrcMessageEventArgs
    {
        /// <summary>
        /// The channel that the NOTICE was sent to.
        /// </summary>
        [ValidateMember(Check.IsValid)]
        public string channel { get; protected set; }

        /// <summary>
        /// The maximum number of users that can be hosted in half an hour.
        /// Set to -1 if the value could not be parsed.
        /// </summary>
        [ValidateMember(Check.IsNotEqualTo, -1)]
        public int host_rate_limit { get; protected set; }

        public
        BadHostRateExceededEventArgs(NoticeEventArgs args) : base(args.irc_message)
        {
            channel = args.channel;

            host_rate_limit = Int32.TryParse(args.body.TextBetween("than ", " times"), out int _host_rate_limit) ? _host_rate_limit : -1;
        }
    }

    public class
    BadModModEventArgs : IrcMessageEventArgs
    {
        /// <summary>
        /// The channel that the NOTICE was sent to.
        /// </summary>
        [ValidateMember(Check.IsValid)]
        public string channel { get; protected set; }

        /// <summary>
        /// The user who was attempted to be modded but is modded.
        /// </summary>
        [ValidateMember(Check.IsValid)]
        public string user_nick { get; protected set; }

        public
        BadModModEventArgs(NoticeEventArgs args) : base(args.irc_message)
        {
            channel = args.channel;

            user_nick = args.body.TextBefore(' ');
        }
    }

    public class
    BadUnmodModEventArgs : IrcMessageEventArgs
    {
        /// <summary>
        /// The channel that the NOTICE was sent to.
        /// </summary>
        [ValidateMember(Check.IsValid)]
        public string channel { get; protected set; }

        /// <summary>
        /// The user who was attempted to be unmodded but is not modded.
        /// </summary>
        [ValidateMember(Check.IsValid)]
        public string user_nick { get; protected set; }

        public
        BadUnmodModEventArgs(NoticeEventArgs args) : base(args.irc_message)
        {
            channel = args.channel;

            user_nick = args.body.TextBefore(' ');
        }
    }

    public class
    CmdsAvailableEventArgs : IrcMessageEventArgs
    {
        /// <summary>
        /// The channel that the NOTICE was sent to.
        /// </summary>
        [ValidateMember(Check.IsValid)]
        public string channel { get; protected set; }

        /// <summary>
        /// The commands that can be used in chat.
        /// </summary>
        [ValidateMember(Check.IsValid)]
        public ChatCommand[] commands { get; protected set; }

        /// <summary>
        /// Creates a new instance of the <see cref="CmdsAvailableEventArgs"/> class.
        /// </summary>
        /// <param name="args">The event arguments to parse.</param>
        public CmdsAvailableEventArgs(NoticeEventArgs args) : base(args.irc_message)
        {
            channel = args.channel;

            string _commands = args.body.TextAfter(':').Trim(' ');
            string[] array = _commands.Split(' ');

            List<ChatCommand> list = new List<ChatCommand>();
            foreach (string element in array)
            {
                EnumUtil.TryParse(element, out ChatCommand command);
                list.Add(command);
            }
            commands = list.ToArray();
        }
    }

    public class
    HostsRemainingEventArgs : IrcMessageEventArgs
    {
        /// <summary>
        /// The channel that the NOTICE was sent to.
        /// </summary>
        [ValidateMember(Check.IsValid)]
        public string channel { get; protected set; }

        /// <summary>
        /// The remaining number of hosts that can be used until it resets.
        /// Set to -1 if the value could not be parsed.
        /// </summary>
        [ValidateMember(Check.IsNotEqualTo, -1)]
        public int remaining { get; protected set; }

        /// <summary>
        /// Creates a new instance of the <see cref="HostsRemainingEventArgs"/> class.
        /// </summary>
        /// <param name="args">The event arguments to parse.</param>
        public HostsRemainingEventArgs(NoticeEventArgs args) : base(args.irc_message)
        {
            channel = args.channel;

            remaining = Int32.TryParse(args.body.TextBefore(' '), out int _hosts_remaining) ? _hosts_remaining : -1;
        }
    }

    public class
    InvalidUserEventArgs : IrcMessageEventArgs
    {
        /// <summary>
        /// The channel that the NOTICE was sent to.
        /// </summary>
        [ValidateMember(Check.IsValid)]
        public string channel { get; protected set; }

        /// <summary>
        /// The invalid user nick
        /// </summary>
        [ValidateMember(Check.IsValid)]
        public string user_nick { get; protected set; }

        /// <summary>
        /// Creates a new instance of the <see cref="InvalidUserEventArgs"/> class.
        /// </summary>
        /// <param name="args">The event arguments to parse.</param>
        public InvalidUserEventArgs(NoticeEventArgs args) : base(args.irc_message)
        {
            channel = args.channel;

            user_nick = args.body.TextAfter(':').TrimStart(' ');
        }
    }
    */
    #endregion

    #region Helpers

    public class
    DataEventArgs : EventArgs
    {
        /// <summary>
        /// The byte data.
        /// </summary>
        [ValidateMember(Check.IsValid)]
        public byte[] data { get; protected set; }

        /// <summary>
        /// The encoded data.
        /// </summary>
        [ValidateMember(Check.IsValid)]
        public string message { get; protected set; }

        /*
        /// <summary>
        /// <para>A unique string manually provided to identify individual messages sent to the IRC server.</para>
        /// <para>
        /// Set to an empty string if it is data received from the IRC server as this is a library specific feature.
        /// Set to the unique string provided when sending a message to the IRC server.
        /// Set to an empty string otherwise.
        /// </para>
        /// </summary>
        public string nonce { get; protected set; }
        */

        public
        DataEventArgs(byte[] data, string message)
        {
            this.data       = data;
            this.message    = message;
            // this.nonce      = nonce;
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
    NumericReplyEventArgs : IrcMessageEventArgs
    {
        /// <summary>
        /// The IRC client nick.
        /// </summary>
        [ValidateMember(Check.IsValid)]
        public string client { get; protected set; }

        public
        NumericReplyEventArgs(in IrcMessage message) : base(message)
        {
            if (message.parameters.Length > 0)
            {
                client = message.parameters[0];
            }
        }
    }

    #endregion
}