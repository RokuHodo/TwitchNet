// standard namespaces
using System;
using System.Drawing;

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
        /// The user who sent the message.
        /// </summary>
        [ValidateMember(Check.IsValid)]
        public string sender { get; protected set; }

        /// <summary>
        /// <para>The channel the message was sent in.</para>
        /// <para>
        /// If the message was sent in a chat room, this is set to the full IRC channel, i.e., the #chatroom signifier, channel ID, and UUID.
        /// If the message was sent in a stream chat, the channel is just the name of the channel the message was sent in.
        /// </para>
        /// </summary>
        [ValidateMember(Check.IsValid)]
        public string channel { get; protected set; }

        /// <summary>
        /// <para>The ID of the user who the chat room belongs to.</para>
        /// <para>Set to null if the message was not sent in a chat room.</para>
        /// </summary>
        //[ValidateMember(Check.IsValid)]
        public string channel_user_id { get; protected set; }

        /// <summary>
        /// <para>The unique UUID of the chat room.</para>
        /// <para>Set to null if the message was not sent in a chat room.</para>
        /// </summary>
        //[ValidateMember(Check.RegexIsMatch, RegexPatternUtil.UUID)]
        public string channel_uuid { get; protected set; }

        /// <summary>
        /// The body of the message.
        /// </summary>
        [ValidateMember(Check.IsValid)]
        public string body { get; protected set; }

        /// <summary>
        /// Where the message was sent from.
        /// </summary>
        public MessageSource source { get; private set; }

        /// <summary>
        /// Whether or not IRC tags were sent with the message.
        /// </summary>
        public bool tags_exist { get; private set; }

        /// <summary>
        /// <para>The converted IRC tags attached to the message.</para>
        /// <para>Set to null if the message was not sent in a chat room.</para>
        /// </summary>
        //[ValidateMember(Check.Tags)]
        public ChatRoomPrivmsgTags tags_chat_room { get; protected set; }

        /// <summary>
        /// <para>The converted IRC tags attached to the message.</para>
        /// <para>Set to null if the message was not sent in a stream chat.</para>
        /// </summary>
        [ValidateMember(Check.Tags)]
        public StreamChatPrivmsgTags tags_stream_chat { get; protected set; }

        /// <summary>
        /// Creates a new instance of the <see cref="ChatPrivmsgEventArgs"/> class.
        /// </summary>
        /// <param name="args">The event arguments to parse.</param>
        public ChatPrivmsgEventArgs(PrivmsgEventArgs args) : base(args.irc_message)
        {
            sender = args.nick;
            body = args.body;

            tags_exist = args.irc_message.tags.IsValid();

            source = Helpers.GetMessageSource(args.channel);

            if (source == MessageSource.ChatRoom)
            {
                channel = args.channel;
                channel_user_id = channel.TextBetween(':', ':');

                int index = channel.LastIndexOf(':');
                if (index != -1)
                {
                    channel_uuid = channel.TextAfter(':', index);
                }

                tags_chat_room = new ChatRoomPrivmsgTags(tags_exist, args);
            }
            else if (source == MessageSource.StreamChat)
            {
                channel = args.channel;

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
        [ValidateTag("mod")]
        public bool mod { get; protected set; }

        /// <summary>
        /// Whether or not the sender is subscribed to the channel.
        /// </summary>
        [ValidateTag("subscriber")]
        public bool subscriber { get; protected set; }

        /// <summary>
        /// Whether or not the sender has Twitch turbo.
        /// </summary>
        [ValidateTag("turbo")]
        public bool turbo { get; protected set; }

        /// <summary>
        /// Whether or not the body of the message only contains emotes.
        /// </summary>
        [ValidateTag("emote-only")]
        public bool emote_only { get; protected set; }

        /// <summary>
        /// The unique message id.
        /// </summary>
        [ValidateTag("id")]
        public string id { get; protected set; }

        /// <summary>
        /// <para>The display name of the sender.</para>
        /// <para>This is empty if it was never set by the sender.</para>
        /// </summary>
        [ValidateTag("display-name")]
        public string display_name { get; protected set; }

        /// <summary>
        /// The user id of the sender.
        /// </summary>
        [ValidateTag("user-id")]
        public string user_id { get; protected set; }

        /// <summary>
        /// The user id of the channel the message was sent in.
        /// </summary>
        [ValidateTag("room-id")]
        public string room_id { get; protected set; }

        /// <summary>
        /// <para>The sender's user type</para>
        /// <para>Set to <see cref="UserType.None"/> if the sender has no elevated privileges.</para>
        /// </summary>
        [ValidateTag("user-type")]
        public UserType user_type { get; protected set; }

        /// <summary>
        /// <para>The color of the sender's display name.</para>
        /// <para>The color is <see cref="Color.Empty"/> if it was never set by the sender.</para>
        /// </summary>
        [ValidateTag("color")]
        public Color color { get; protected set; }

        /// <summary>
        /// The time the message was sent.
        /// </summary>
        [ValidateTag("tmi-sent-ts")]
        public DateTime tmi_sent_ts { get; protected set; }

        /// <summary>
        /// <para>The chat badges that the sender has, if any.</para>
        /// <para>The array is empty if the sender has no chat badges.</para>
        /// </summary>
        [ValidateTag("badges")]
        public Badge[] badges { get; protected set; }

        /// <summary>
        /// <para>The emotes the sender used in the message, if any.</para>
        /// <para>The array is empty if the sender did not use any emotes.</para>
        /// </summary>
        [ValidateTag("emotes")]
        public Emote[] emotes { get; protected set; }

        /// <summary>
        /// Creates a new instance of the <see cref="ChatRoomPrivmsgTags"/> class.
        /// </summary>
        /// <param name="args">The event arguments to parse.</param>
        public ChatRoomPrivmsgTags(bool tags_exist, PrivmsgEventArgs args)
        {
            if (!tags_exist)
            {
                return;
            }

            mod = TagsUtil.ToBool(args.irc_message, "mod");
            subscriber = TagsUtil.ToBool(args.irc_message, "subscriber");
            turbo = TagsUtil.ToBool(args.irc_message, "turbo");
            emote_only = TagsUtil.ToBool(args.irc_message, "emote-only");

            id = TagsUtil.ToString(args.irc_message, "id");
            display_name = TagsUtil.ToString(args.irc_message, "display-name");
            user_id = TagsUtil.ToString(args.irc_message, "user-id");
            room_id = TagsUtil.ToString(args.irc_message, "room-id");

            user_type = TagsUtil.ToUserType(args.irc_message, "user-type");

            color = TagsUtil.FromtHtml(args.irc_message, "color");
            tmi_sent_ts = TagsUtil.FromUnixEpochMilliseconds(args.irc_message, "tmi-sent-ts");

            badges = TagsUtil.ToBadges(args.irc_message, "badges");
            emotes = TagsUtil.ToEmotes(args.irc_message, "emotes");
        }
    }

    public class
    StreamChatPrivmsgTags
    {
        /// <summary>
        /// <para>The amount of bits the sender cheered, if any.</para>
        /// <para>Set to 0 if the sender did not cheer.</para>
        /// </summary>
        [ValidateTag("bits", Check.IsNotEqualTo, 0u)]
        public uint bits { get; protected set; }

        /// <summary>
        /// Whether or not the sender is a moderator.
        /// </summary>
        [ValidateTag("mod")]
        public bool mod { get; protected set; }

        /// <summary>
        /// Whether or not the sender is subscribed to the channel.
        /// </summary>
        [ValidateTag("subscriber")]
        public bool subscriber { get; protected set; }

        /// <summary>
        /// Whether or not the sender has Twitch turbo.
        /// </summary>
        [ValidateTag("turbo")]
        public bool turbo { get; protected set; }

        /// <summary>
        /// Whether or not the body of the message only contains emotes.
        /// </summary>
        [ValidateTag("emote-only")]
        public bool emote_only { get; protected set; }

        /// <summary>
        /// The unique message id.
        /// </summary>
        [ValidateTag("id")]
        public string id { get; protected set; }

        /// <summary>
        /// <para>The display name of the sender.</para>
        /// <para>This is empty if it was never set by the sender.</para>
        /// </summary>
        [ValidateTag("display-name")]
        public string display_name { get; protected set; }

        /// <summary>
        /// The user id of the sender.
        /// </summary>
        [ValidateTag("user-id")]
        public string user_id { get; protected set; }

        /// <summary>
        /// The user id of the channel the message was sent in.
        /// </summary>
        [ValidateTag("room-id")]
        public string room_id { get; protected set; }

        /// <summary>
        /// <para>The sender's user type</para>
        /// <para>Set to <see cref="UserType.None"/> if the sender has no elevated privileges.</para>
        /// </summary>
        [ValidateTag("user-type")]
        public UserType user_type { get; protected set; }

        /// <summary>
        /// <para>The color of the sender's display name.</para>
        /// <para>The color is <see cref="Color.Empty"/> if it was never set by the sender.</para>
        /// </summary>
        [ValidateTag("color")]
        public Color color { get; protected set; }

        /// <summary>
        /// The time the message was sent.
        /// </summary>
        [ValidateTag("tmi-sent-ts")]
        public DateTime tmi_sent_ts { get; protected set; }

        /// <summary>
        /// <para>The chat badges that the sender has, if any.</para>
        /// <para>The array is empty if the sender has no chat badges.</para>
        /// </summary>
        [ValidateTag("badges")]
        public Badge[] badges { get; protected set; }

        /// <summary>
        /// <para>The emotes the sender used in the message, if any.</para>
        /// <para>The array is empty if the sender did not use any emotes.</para>
        /// </summary>
        [ValidateTag("emotes")]
        public Emote[] emotes { get; protected set; }

        /// <summary>
        /// Creates a new instance of the <see cref="ChatRoomPrivmsgTags"/> class.
        /// </summary>
        /// <param name="args">The event arguments to parse.</param>
        public StreamChatPrivmsgTags(bool tags_exist, PrivmsgEventArgs args)
        {
            if (!tags_exist)
            {
                return;
            }

            bits = TagsUtil.ToUInt32(args.irc_message, "bits");

            mod = TagsUtil.ToBool(args.irc_message, "mod");
            subscriber = TagsUtil.ToBool(args.irc_message, "subscriber");
            turbo = TagsUtil.ToBool(args.irc_message, "turbo");
            emote_only = TagsUtil.ToBool(args.irc_message, "emote-only");

            id = TagsUtil.ToString(args.irc_message, "id");
            display_name = TagsUtil.ToString(args.irc_message, "display-name");
            user_id = TagsUtil.ToString(args.irc_message, "user-id");
            room_id = TagsUtil.ToString(args.irc_message, "room-id");

            user_type = TagsUtil.ToUserType(args.irc_message, "user-type");

            color = TagsUtil.FromtHtml(args.irc_message, "color");
            tmi_sent_ts = TagsUtil.FromUnixEpochMilliseconds(args.irc_message, "tmi-sent-ts");

            badges = TagsUtil.ToBadges(args.irc_message, "badges");
            emotes = TagsUtil.ToEmotes(args.irc_message, "emotes");
        }
    }

    #endregion

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

    static class
    Helpers
    {
        public static MessageSource
        GetMessageSource(string channel)
        {
            if (channel.IsNull())
            {
                return MessageSource.Other;
            }

            return channel.TextBefore(':') == "#chatrooms" ? MessageSource.ChatRoom : MessageSource.StreamChat;
        }
    }    
}