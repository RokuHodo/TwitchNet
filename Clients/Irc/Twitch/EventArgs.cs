// standard namespaces
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.Serialization;

// project namespaces
using TwitchNet.Debugger;
using TwitchNet.Extensions;
using TwitchNet.Utilities;

namespace
TwitchNet.Clients.Irc.Twitch
{
    #region ChatPrivmsgEventArgs

    public class
    ChatPrivmsgEventArgs : IrcMessageEventArgs
    {
        /// <summary>
        /// Where the message was sent from.
        /// </summary>
        public MessageSource source { get; private set; }

        /// <summary>
        /// Whether or not the message started with '\u0001ACTION', i.e., the '/me' command was used with the message.
        /// </summary>
        public bool action { get; protected set; }

        /// <summary>
        /// <para>The nick of the IRC user who sent the message.</para>
        /// <para>The sender is equivalent to their login name.</para>
        /// </summary>
        [ValidateMember(Check.IsValid)]
        public string sender { get; protected set; }

        /// <summary>
        /// The IRC channel the message was sent in.
        /// <para>
        /// If the message was sent in a stream chat, the channel is equivalent to the streamer's login name prefixed with '#'.
        /// If the message was sent in a chat room, the channel contains the room's uuid and the ID of the user who owns the room.
        /// </para>
        /// </summary>
        [ValidateMember(Check.IsValid)]
        public string channel { get; protected set; }

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

        /// <summary>
        /// The body of the message.
        /// Any instance of '\u0001ACTION' or '\u0001' is automatically removed if the '/me' command was used.
        /// </summary>
        [ValidateMember(Check.IsValid)]
        public string body { get; protected set; }        

        /// <summary>
        /// Whether or not IRC tags were sent with the message.
        /// </summary>
        public bool tags_exist { get; private set; }

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

        public ChatPrivmsgEventArgs(PrivmsgEventArgs args) : base(args.irc_message)
        {
            source = TwitchIrcUtil.GetMessageSource(args.channel);

            action = args.body.StartsWith(TwitchIrcUtil.ACTION_PREFIX) ? true : false;            

            sender = args.nick;
            channel = args.channel;
            channel_user_id = string.Empty;
            channel_uuid = string.Empty;

            body = args.body;
            if (action)
            {
                body = body.TextBetween(TwitchIrcUtil.ACTION_PREFIX, TwitchIrcUtil.ACTION_SUFFIX).Trim();
            }

            tags_exist = args.irc_message.tags_exist;

            if (source == MessageSource.ChatRoom)
            {
                Tuple<string, string> temp = TwitchIrcUtil.ParseChatRoomChannel(args.channel);
                channel_user_id = temp.Item1;
                channel_uuid = temp.Item2;

                tags_chat_room = new ChatRoomPrivmsgTags(tags_exist, args);
            }
            else if (source == MessageSource.StreamChat)
            {
                tags_stream_chat = new StreamChatPrivmsgTags(tags_exist, args);
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

        public ChatRoomPrivmsgTags(bool tags_exist, PrivmsgEventArgs args)
        {
            if (!tags_exist)
            {
                return;
            }

            mod = TwitchIrcUtil.Tags.ToBool(args.irc_message, "mod");
            subscriber = TwitchIrcUtil.Tags.ToBool(args.irc_message, "subscriber");
            turbo = TwitchIrcUtil.Tags.ToBool(args.irc_message, "turbo");
            emote_only = TwitchIrcUtil.Tags.ToBool(args.irc_message, "emote-only");

            id = TwitchIrcUtil.Tags.ToString(args.irc_message, "id");
            display_name = TwitchIrcUtil.Tags.ToString(args.irc_message, "display-name");
            user_id = TwitchIrcUtil.Tags.ToString(args.irc_message, "user-id");
            room_id = TwitchIrcUtil.Tags.ToString(args.irc_message, "room-id");

            user_type = TwitchIrcUtil.Tags.ToUserType(args.irc_message, "user-type");

            color = TwitchIrcUtil.Tags.FromtHtml(args.irc_message, "color");
            tmi_sent_ts = TwitchIrcUtil.Tags.FromUnixEpochMilliseconds(args.irc_message, "tmi-sent-ts");

            badges = TwitchIrcUtil.Tags.ToBadges(args.irc_message, "badges");
            badge_info = TwitchIrcUtil.Tags.ToBadgeInfo(args.irc_message, "badge-info");
            emotes = TwitchIrcUtil.Tags.ToEmotes(args.irc_message, "emotes");
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

        public StreamChatPrivmsgTags(bool tags_exist, PrivmsgEventArgs args)
        {
            if (!tags_exist)
            {
                return;
            }

            bits = TwitchIrcUtil.Tags.ToUInt32(args.irc_message, "bits");

            mod = TwitchIrcUtil.Tags.ToBool(args.irc_message, "mod");
            subscriber = TwitchIrcUtil.Tags.ToBool(args.irc_message, "subscriber");
            turbo = TwitchIrcUtil.Tags.ToBool(args.irc_message, "turbo");
            emote_only = TwitchIrcUtil.Tags.ToBool(args.irc_message, "emote-only");

            id = TwitchIrcUtil.Tags.ToString(args.irc_message, "id");
            display_name = TwitchIrcUtil.Tags.ToString(args.irc_message, "display-name");
            user_id = TwitchIrcUtil.Tags.ToString(args.irc_message, "user-id");
            room_id = TwitchIrcUtil.Tags.ToString(args.irc_message, "room-id");

            user_type = TwitchIrcUtil.Tags.ToUserType(args.irc_message, "user-type");

            color = TwitchIrcUtil.Tags.FromtHtml(args.irc_message, "color");
            tmi_sent_ts = TwitchIrcUtil.Tags.FromUnixEpochMilliseconds(args.irc_message, "tmi-sent-ts");

            badges = TwitchIrcUtil.Tags.ToBadges(args.irc_message, "badges");
            badge_info = TwitchIrcUtil.Tags.ToBadgeInfo(args.irc_message, "badge-info");
            emotes = TwitchIrcUtil.Tags.ToEmotes(args.irc_message, "emotes");
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

    #endregion

    #region WhisperEventArgs

    public class
    WhisperEventArgs : IrcMessageEventArgs
    {
        /// <summary>
        /// <para>The nick of the IRC user who sent the whisper.</para>
        /// <para>The sender is equivalent to their login name.</para>
        /// </summary>
        [ValidateMember(Check.IsValid)]
        public string sender { get; protected set; }

        /// <summary>
        /// <para>The nick of user who received the whisper.</para>
        /// <para>The recipient is equivalent to their login name.</para>
        /// </summary>
        [ValidateMember(Check.IsValid)]
        public string recipient { get; protected set; }

        /// <summary>
        /// The body of the message.
        /// </summary>
        [ValidateMember(Check.IsValid)]
        public string body { get; protected set; }

        /// <summary>
        /// <para>The tags attached to the whisper, if any.</para>
        /// <para>Check the <code>exist</code> property to determine if tags were attached to the message.</para>
        /// </summary>
        [ValidateMember(Check.TagsMissing)]
        [ValidateMember(Check.TagsExtra)]
        public WhisperTags tags { get; protected set; }

        /// <summary>
        /// Creates a new instance of the <see cref="WhisperEventArgs"/> class.
        /// </summary>
        /// <param name="message">The IRC message to parse.</param>
        public WhisperEventArgs(in IrcMessage message) : base(message)
        {
            sender = message.server_or_nick;

            if (message.parameters.IsValid())
            {
                recipient = message.parameters[0];
            }

            body = message.trailing;

            tags = new WhisperTags(message);
        }
    }

    public class
    WhisperTags
    {
        /// <summary>
        /// Whether or not the sender has Twitch turbo.
        /// </summary>
        [Obsolete("This tag has been deprecated and can be deleted at any time. Use the 'badges' tag to look for this information instead")]
        [IrcTag("turbo")]
        public bool turbo { get; protected set; }

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
        /// The ID of the message.
        /// </summary>
        [IrcTag("message-id")]
        public string message_id { get; protected set; }

        /// <summary>
        /// The user ID of the sender and the user ID of the recipient concatenated with an underscore.
        /// </summary>
        [IrcTag("thread-id")]
        public string thread_id { get; protected set; }

        /// <summary>
        /// The user ID of the recipient.
        /// </summary>
        public string recipient_id { get; protected set; }

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
        /// <para>The chat badges that the sender has, if any.</para>
        /// <para>Set to an empty array if the sender has no chat badges.</para>
        /// </summary>
        [ValidateMember]
        [IrcTag("badges")]
        public Badge[] badges { get; protected set; }

        /// <summary>
        /// <para>The emotes the sender used in the message, if any.</para>
        /// <para>Set to an empty array if the sender did not use any emotes in the message.</para>
        /// </summary>
        [ValidateMember]
        [IrcTag("emotes")]
        public Emote[] emotes { get; protected set; }

        /// <summary>
        /// Creates a new instance of the <see cref="WhisperTags"/> class.
        /// </summary>
        /// <param name="message">The IRC message to parse.</param>
        public WhisperTags(in IrcMessage message)
        {
            turbo = TwitchIrcUtil.Tags.ToBool(message, "turbo");

            display_name = TwitchIrcUtil.Tags.ToString(message, "display-name");
            user_id = TwitchIrcUtil.Tags.ToString(message, "user-id");
            message_id = TwitchIrcUtil.Tags.ToString(message, "message-id");
            thread_id = TwitchIrcUtil.Tags.ToString(message, "thread-id");
            recipient_id = thread_id.TextAfter('_');

            user_type = TwitchIrcUtil.Tags.ToUserType(message, "user-type");

            color = TwitchIrcUtil.Tags.FromtHtml(message, "color");

            badges = TwitchIrcUtil.Tags.ToBadges(message, "badges");
            emotes = TwitchIrcUtil.Tags.ToEmotes(message, "emotes");
        }
    }

    #endregion

    #region Helpers

    public enum
    MessageSource
    {
        /// <summary>
        /// The message source could not be determined.
        /// The message was likely corrupt and was not able to be parsed.
        /// </summary>
        Other = 0,

        /// <summary>
        /// The message was sent in a stream chat.
        /// </summary>
        StreamChat,

        /// <summary>
        /// The message was sent in a chat room.
        /// </summary>
        ChatRoom
    }    

    #endregion
}