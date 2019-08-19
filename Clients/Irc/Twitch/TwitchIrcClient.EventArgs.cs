﻿// standard namespaces
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
    // -------------------------------------------------------------------------------------------------------------
    //
    // - This file contains all IRC messages that are specifc to Twitch.     
    // - Any message that is natively supported and documented in the RFC 1459 Specification can be found in it's own file, IrcClient.EventArgs.
    //   RFC 1459 Spec: https://tools.ietf.org/html/rfc1459.html
    //
    // - However, there are messages in IrcClient.EventArgs that nay contain Twitch specific parsing. These members are separated 
    //   clearly from the native IRC spec members witin each data structure.
    //
    // -------------------------------------------------------------------------------------------------------------

    #region Command: WHISPER - Updated

    public class
    WhisperEventArgs : IrcMessageEventArgs
    {
        /// <summary>
        /// Whether or not IRC tags were sent with the message.
        /// </summary>
        public bool tags_exist { get; private set; }

        /// <summary>
        /// The tags attached to the message.
        /// </summary>
        [ValidateMember(Check.TagsMissing)]
        [ValidateMember(Check.TagsExtra)]
        public WhisperTags tags { get; protected set; }

        /// <summary>
        /// The nick of the IRC user who sent the whisper.
        /// </summary>
        [ValidateMember(Check.IsValid)]
        public string sender { get; protected set; }

        /// <summary>
        /// The nick of IRC user who received the whisper.
        /// </summary>
        [ValidateMember(Check.IsValid)]
        public string recipient { get; protected set; }

        /// <summary>
        /// The body of the message.
        /// </summary>
        [ValidateMember(Check.IsValid)]
        public string body { get; protected set; }        

        public
        WhisperEventArgs(in IrcMessage message) : base(message)
        {
            tags_exist = message.tags_exist;
            tags = new WhisperTags(message);

            sender = message.server_or_nick;

            if (message.parameters.Length > 0)
            {
                recipient = message.parameters[0];
            }

            body = message.trailing;            
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
            if (!message.tags_exist)
            {
                return;
            }

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

    #region Command: CLEARCHAT - Updated

    public class
    ClearChatEventArgs : ChatRoomSupportedMessageEventArgs
    {
        /// <summary>
        /// Whether or not IRC tags were sent with the message.
        /// </summary>
        public bool tags_exist { get; private set; }

        /// <summary>
        /// The tags attached to the message.
        /// </summary>
        [ValidateMember(Check.TagsMissing)]
        public ClearChatTags tags { get; protected set; }

        /// <summary>
        /// The IRC channel where the chat was cleared or where the user was timed out or banned.</para>
        /// </summary>
        [ValidateMember(Check.IsValid)]
        public string channel { get; protected set; }

        /// <summary>
        /// <para>The nick of user who was timed out or banned.</para>
        /// <para>Set to an empty string if the entire chat was cleared.</para>
        /// </summary>
        [ValidateMember(Check.IsValid)]
        public string user { get; protected set; }        

        public
        ClearChatEventArgs(in IrcMessage message) : base(message)
        {
            tags_exist = message.tags_exist;
            tags = new ClearChatTags(message);

            if (message.parameters.Length > 0)
            {
                channel = message.parameters[0];
            }

            user = message.trailing;
        }
    }

    public class
    ClearChatTags
    {
        /// <summary>
        /// <para>The length of the ban, in seconds.</para>
        /// <para>Set to <see cref="TimeSpan.Zero"/> if the ban is permanent or the entire chat was cleared.</para>
        /// </summary>
        [IrcTag("ban-duration")]
        public TimeSpan ban_duration { get; protected set; }

        /// <summary>
        /// <para>The reason for the time out or ban.</para>
        /// <para>Set to an empty string if the no reason was provided or the entire chat was cleared.</para>
        /// </summary>
        [IrcTag("ban-reason")]
        public string ban_reason { get; protected set; }

        /// <summary>
        /// The ID of the room whose chat was cleared or a where the user got timed out/banned.
        /// </summary>
        [IrcTag("room-id")]
        public string room_id { get; protected set; }

        /// <summary>
        /// <para>The ID of the user who got timed out or banned.</para>
        /// <para>Set to an empty string if the entire chat was cleared.</para>
        /// </summary>
        [IrcTag("target-user-id")]
        public string target_user_id { get; protected set; }

        /// <summary>
        /// When the IRC message was sent.
        /// </summary>
        [IrcTag("tmi-sent-ts")]
        public DateTime tmi_sent_ts { get; protected set; }

        public ClearChatTags(in IrcMessage message)
        {
            if (!message.tags_exist)
            {
                return;
            }

            ban_duration    = TwitchIrcUtil.Tags.ToTimeSpanFromSeconds(message, "ban-duration");
            ban_reason      = TwitchIrcUtil.Tags.ToString(message, "ban-reason").Replace("\\s", " ");
            room_id         = TwitchIrcUtil.Tags.ToString(message, "room-id");
            target_user_id  = TwitchIrcUtil.Tags.ToString(message, "target-user-id");
            tmi_sent_ts     = TwitchIrcUtil.Tags.FromUnixEpochMilliseconds(message, "tmi-sent-ts");
        }
    }

    #endregion

    #region Command: GLOBALUSERSTATE 

    public class
    GlobalUserStateEventArgs : IrcMessageEventArgs
    {
        /// <summary>
        /// Whether or not IRC tags were sent with the message.
        /// </summary>
        public bool tags_exist { get; private set; }

        /// <summary>
        /// <para>The tags attached to the message, if any.</para>
        /// </summary>
        [ValidateMember(Check.TagsMissing)]
        public GlobalUserStateTags tags { get; protected set; }

        public GlobalUserStateEventArgs(in IrcMessage message) : base(message)
        {
            tags_exist = message.tags_exist;
            tags = new GlobalUserStateTags(message);
        }
    }

    public class
    GlobalUserStateTags
    {
        /// <summary>
        /// The id of the user.
        /// </summary>
        [IrcTag("user-id")]
        public string user_id { get; protected set; }

        /// <summary>
        /// <para>The display name of the user.</para>
        /// <para>This is empty if it was never set by the user.</para>
        /// </summary>
        [IrcTag("display-name")]
        public string display_name { get; protected set; }

        /// <summary>
        /// The emote sets that are available for the user to use.
        /// </summary>
        [IrcTag("emote-sets")]
        public string[] emote_sets { get; protected set; }

        /// <summary>
        /// <para>The user's type</para>
        /// <para>Set to <see cref="UserType.None"/> if the user has no elevated privileges.</para>
        /// </summary>
        [IrcTag("user-type")]
        public UserType user_type { get; protected set; }

        /// <summary>
        /// <para>The color of the user's display name.</para>
        /// <para>The color is <see cref="Color.Empty"/> if it was never set by the user.</para>
        /// </summary>
        [IrcTag("color")]
        public Color color { get; protected set; }

        /// <summary>
        /// <para>The chat badges that the user has, if any.</para>
        /// <para>The array is empty if the user has no chat badges.</para>
        /// </summary>
        [IrcTag("badges")]
        public Badge[] badges { get; protected set; }

        /// <summary>
        /// Creates a new instance of the <see cref="GlobalUserStateTags"/> class.
        /// </summary>
        /// <param name="message">The IRC message to parse.</param>
        public GlobalUserStateTags(in IrcMessage message)
        {
            if (!message.tags_exist)
            {
                return;
            }

            user_id = TwitchIrcUtil.Tags.ToString(message, "user-id");
            display_name = TwitchIrcUtil.Tags.ToString(message, "display-name");
            emote_sets = TwitchIrcUtil.Tags.ToStringArray(message, "emote-sets", ',');

            user_type = TwitchIrcUtil.Tags.ToUserType(message, "user-type");

            color = TwitchIrcUtil.Tags.FromtHtml(message, "color");

            badges = TwitchIrcUtil.Tags.ToBadges(message, "badges");
        }
    }

    #endregion

    #region Command: ROOMSTATE

    public class
    RoomStateEventArgs : ChatRoomSupportedMessageEventArgs
    {
        /// <summary>
        /// Whether or not IRC tags were sent with the message.
        /// </summary>
        public bool tags_exist { get; private set; }

        /// <summary>
        /// <para>The tags attached to the message, if any.</para>
        /// </summary>
        [ValidateMember(Check.TagsMissing)]
        public RoomStateTags tags_stream_chat { get; protected set; }

        /// <summary>
        /// <para>The tags attached to the message, if any.</para>
        /// </summary>
        [ValidateMember(Check.TagsMissing)]
        public ChatRoomRoomStateTags tags_chat_room { get; protected set; }

        /// <summary>
        /// The channel whose state has changed and/or the client has joined.
        /// Always valid.
        /// </summary>
        [ValidateMember(Check.IsValid)]
        public string channel { get; protected set; }

        public RoomStateEventArgs(in IrcMessage message) : base(message)
        {
            tags_exist = message.tags_exist;
            if(source == MessageSource.StreamChat)
            {
                tags_stream_chat = new RoomStateTags(message);
            }
            else
            {
                tags_chat_room = new ChatRoomRoomStateTags(message);
            }

            if (message.parameters.Length > 0)
            {
                channel = message.parameters[0];
            }
        }
    }

    public class
    RoomStateTags
    {
        /// <summary>
        /// Whether or not the room is in emote only mode.
        /// </summary>
        [IrcTag("emote-only")]
        public bool emote_only { get; protected set; }

        /// <summary>
        /// Whether or r9k mode is enabled.
        /// When enabled, messages 9 characters or longer must be unique from other messages.
        /// </summary>
        [IrcTag("r9k")]
        public bool r9k { get; protected set; }

        /// <summary>
        /// Whether or not rituals are enabled.
        /// </summary>
        [IrcTag("rituals")]
        public bool rituals { get; protected set; }

        /// <summary>
        /// Whether or not the room is in sub only mode.
        /// When enabled, only subs can sent chat messages.
        /// </summary>
        [IrcTag("subs-only")]
        public bool subs_only { get; protected set; }

        /// <summary>
        /// <para>How long, in minutes, the user only needs to be following.</para>
        /// <para>
        /// Set to 0 when no duration is required and a user only needs to be following.
        /// Set to -1 when follower only mode is disabled.
        /// </para>
        /// </summary>
        [IrcTag("followers-only")]
        public int followers_only { get; protected set; }

        /// <summary>
        /// <para>How frequently, in seconds, non-elevated users can send messages.</para>
        /// <para>Set to 0 if slow mode is disabled.</para>
        /// </summary>
        [IrcTag("slow")]
        public uint slow { get; protected set; }

        /// <summary>
        /// The id of the room whose state has changed and/or the client has joined.
        /// </summary>
        [IrcTag("room-id")]
        public string room_id { get; protected set; }

        /// <summary>
        /// <para>
        /// The language the chat is restricted to.
        /// This is the language selected in the Twitch dashboard or in the video information editor, not the language selected at the home page.
        /// </para>
        /// <para>Set to <see cref="BroadcasterLanguage.None"/> if the room is not language restricted.</para>
        /// </summary>
        [IrcTag("broadcaster-lang")]
        public BroadcasterLanguage broadcaster_lang { get; protected set; }

        /// <summary>
        /// <para>
        /// Bitfield enum. Contains the room state(s) that changed.
        /// Check this to see which room state fields are valid.
        /// </para>
        /// <para>Set to <see cref="RoomStateType.None"/> if no room states have changed. This should never be the case.</para>
        /// </summary>
        public RoomStateType changed_states { get; protected set; }

        public RoomStateTags(in IrcMessage message)
        {
            if (!message.tags_exist)
            {
                return;
            }

            if (TwitchIrcUtil.Tags.IsTagValid(message, "emote-only"))
            {
                emote_only = TwitchIrcUtil.Tags.ToBool(message, "emote-only");
                changed_states |= RoomStateType.EmoteOnly;
            }

            if (TwitchIrcUtil.Tags.IsTagValid(message, "r9k"))
            {
                r9k = TwitchIrcUtil.Tags.ToBool(message, "r9k");
                changed_states |= RoomStateType.R9K;
            }

            if (TwitchIrcUtil.Tags.IsTagValid(message, "rituals"))
            {
                rituals = TwitchIrcUtil.Tags.ToBool(message, "rituals");
                changed_states |= RoomStateType.Rituals;
            }

            if (TwitchIrcUtil.Tags.IsTagValid(message, "subs-only"))
            {
                subs_only = TwitchIrcUtil.Tags.ToBool(message, "subs-only");
                changed_states |= RoomStateType.SubsOnly;
            }

            if (TwitchIrcUtil.Tags.IsTagValid(message, "followers-only"))
            {
                followers_only = TwitchIrcUtil.Tags.ToInt32(message, "followers-only");
                changed_states |= RoomStateType.FollowersOnly;
            }

            if (TwitchIrcUtil.Tags.IsTagValid(message, "slow"))
            {
                slow = TwitchIrcUtil.Tags.ToUInt32(message, "slow");
                changed_states |= RoomStateType.Slow;
            }

            if (TwitchIrcUtil.Tags.IsTagValid(message, "broadcaster-lang"))
            {
                broadcaster_lang = TwitchIrcUtil.Tags.ToBroadcasterLanguage(message, "broadcaster-lang");
                changed_states |= RoomStateType.BroadcasterLang;
            }

            room_id = TwitchIrcUtil.Tags.ToString(message, "room-id");
        }
    }

    public class
    ChatRoomRoomStateTags
    {
        /// <summary>
        /// Whether or not the room is in emote only mode.
        /// </summary>
        [IrcTag("emote-only")]
        public bool emote_only { get; protected set; }

        /// <summary>
        /// Whether or r9k mode is enabled.
        /// When enabled, messages 9 characters or longer must be unique from other messages.
        /// </summary>
        [IrcTag("r9k")]
        public bool r9k { get; protected set; }

        /// <summary>
        /// <para>How frequently, in seconds, non-elevated users can send messages.</para>
        /// <para>Set to 0 if slow mode is disabled.</para>
        /// </summary>
        [IrcTag("slow")]
        public uint slow { get; protected set; }

        /// <summary>
        /// The id of the room whose state has changed and/or the client has joined.
        /// </summary>
        [IrcTag("room-id")]
        public string room_id { get; protected set; }

        /// <summary>
        /// <para>
        /// Bitfield enum. Contains the room state(s) that changed.
        /// Check this to see which room state fields are valid.
        /// </para>
        /// <para>Set to <see cref="RoomStateType.None"/> if no room states have changed. This should never be the case.</para>
        /// </summary>
        public RoomStateType changed_states { get; protected set; }

        public ChatRoomRoomStateTags(in IrcMessage message)
        {
            if (!message.tags_exist)
            {
                return;
            }

            if (TwitchIrcUtil.Tags.IsTagValid(message, "emote-only"))
            {
                emote_only = TwitchIrcUtil.Tags.ToBool(message, "emote-only");
                changed_states |= RoomStateType.EmoteOnly;
            }

            if (TwitchIrcUtil.Tags.IsTagValid(message, "r9k"))
            {
                r9k = TwitchIrcUtil.Tags.ToBool(message, "r9k");
                changed_states |= RoomStateType.R9K;
            }

            if (TwitchIrcUtil.Tags.IsTagValid(message, "slow"))
            {
                slow = TwitchIrcUtil.Tags.ToUInt32(message, "slow");
                changed_states |= RoomStateType.Slow;
            }

            room_id = TwitchIrcUtil.Tags.ToString(message, "room-id");
        }
    }

    #endregion

    #region Command: USERNOTICE

    public class
    UserNoticeEventArgs : IrcMessageEventArgs
    {
        /// <summary>
        /// Whether or not IRC tags were sent with the message.
        /// </summary>
        public bool tags_exist { get; private set; }

        /// <summary>
        /// <para>The tags attached to the message, if any.</para>
        /// </summary>
        [ValidateMember(Check.TagsMissing)]
        public UserNoticeTags tags { get; protected set; }        

        /// <summary>
        /// The channel that the user notice was sent in.
        /// Always valid.
        /// </summary>
        [ValidateMember(Check.IsValid)]
        public string channel { get; protected set; }

        /// <summary>
        /// <para>The messsage sent by the user.</para>
        /// <para>This is empty if the user did not send a message.</para>
        /// </summary>
        [ValidateMember(Check.IsValid)]
        public string body { get; protected set; }

        public UserNoticeEventArgs(in IrcMessage message) : base(message)
        {
            tags_exist = message.tags_exist;
            tags = new UserNoticeTags(message);

            if (message.parameters.Length > 0)
            {
                channel = message.parameters[0];
            }

            body = message.trailing;

        }
    }

    public class
    UserNoticeTags
    {
        /// <summary>
        /// <para>The chat badges that the user has, if any.</para>
        /// <para>The array is empty if the user has no chat badges.</para>
        /// </summary>
        [IrcTag("badges")]
        public Badge[] badges { get; protected set; }

        [IrcTag("badge-info")]
        public BadgeInfo[] badge_info { get; protected set; }

        /// <summary>
        /// <para>The color of the user's display name.</para>
        /// <para>The color is <see cref="Color.Empty"/> if it was never set by the sender.</para>
        /// </summary>
        [IrcTag("color")]
        public Color color { get; protected set; }

        /// <summary>
        /// <para>The display name of the user.</para>
        /// <para>This is empty if it was never set by the user.</para>
        /// </summary>
        [IrcTag("display-name")]
        public string display_name { get; protected set; }

        /// <summary>
        /// <para>The emotes the user used in the message, if any.</para>
        /// <para>The array is empty if the user did not use any emotes.</para>
        /// </summary>
        [IrcTag("emotes")]
        public Emote[] emotes { get; protected set; }

        /// <summary>
        /// The unique message id.
        /// </summary>
        [IrcTag("id")]
        public string id { get; protected set; }

        /// <summary>
        /// The login name of the user.
        /// </summary>
        [IrcTag("login")]
        public string login { get; protected set; }

        /// <summary>
        /// Whether or not the user is a moderator.
        /// </summary>
        [IrcTag("mod")]
        public bool mod { get; protected set; }

        /// <summary>
        /// <para>The type of the user notice.</para>
        /// <para>Set to <see cref="UserNoticeType.Other"/> if the tag could not be parsed.</para>
        /// </summary>
        [IrcTag("msg-id")]
        public UserNoticeType msg_id { get; protected set; }

        /// <summary>
        /// The id of the channel the user notice was sent in.
        /// </summary>
        [IrcTag("room-id")]
        public string room_id { get; protected set; }

        /// <summary>
        /// Whether or not the user is subscribed to the channel.
        /// </summary>
        [IrcTag("subscriber")]
        public bool subscriber { get; protected set; }        

        /// <summary>
        /// The message printed in chat along with its notice.
        /// </summary>
        [IrcTag("system-msg")]
        public string system_msg { get; protected set; }

        /// <summary>
        /// The time the user notice was sent.
        /// </summary>
        [IrcTag("tmi-sent-ts")]
        public DateTime tmi_sent_ts { get; protected set; }

        /// <summary>
        /// Whether or not the user has Twitch turbo.
        /// </summary>
        [IrcTag("turbo")]
        public bool turbo { get; protected set; }                        

        /// <summary>
        /// The id of the user who triggered the user notice.
        /// </summary>
        [IrcTag("user-id")]
        public string user_id { get; protected set; }        

        /// <summary>
        /// <para>The type of the user.</para>
        /// <para>Set to <see cref="UserType.None"/> if the user has no elevated privileges.</para>
        /// </summary>
        [IrcTag("user-type")]
        public UserType user_type { get; protected set; }                             

        public UserNoticeTags(in IrcMessage message)
        {
            if (!message.tags_exist)
            {
                return;
            }

            // Universal tags
            badges = TwitchIrcUtil.Tags.ToBadges(message, "badges");
            badge_info = TwitchIrcUtil.Tags.ToBadgeInfo(message, "badge-info");
            color = TwitchIrcUtil.Tags.FromtHtml(message, "color");
            display_name = TwitchIrcUtil.Tags.ToString(message, "display-name");
            emotes = TwitchIrcUtil.Tags.ToEmotes(message, "emotes");
            id = TwitchIrcUtil.Tags.ToString(message, "id");
            login = TwitchIrcUtil.Tags.ToString(message, "login");
            mod = TwitchIrcUtil.Tags.ToBool(message, "mod");
            msg_id = TwitchIrcUtil.Tags.ToUserNoticeType(message, "msg-id");
            room_id = TwitchIrcUtil.Tags.ToString(message, "room-id");
            subscriber = TwitchIrcUtil.Tags.ToBool(message, "subscriber");
            system_msg = TwitchIrcUtil.Tags.ToString(message, "system-msg").Replace("\\s", " ");
            tmi_sent_ts = TwitchIrcUtil.Tags.FromUnixEpochMilliseconds(message, "tmi-sent-ts");
            turbo = TwitchIrcUtil.Tags.ToBool(message, "turbo");
            user_id = TwitchIrcUtil.Tags.ToString(message, "user-id");
            user_type = TwitchIrcUtil.Tags.ToUserType(message, "user-type");

            /*

            // sub, resub tags
            msg_param_cumulative_months
            msg_param_should_share_streak
            msg_param_streak_months

            // subgift, anonsubgift tags
            msg_param_months = TwitchIrcUtil.Tags.ToUInt16(message, "msg-param-months");
            msg_param_recipient_display_name
            msg_param_recipient_id
            msg_param_recipient_user_name

            // giftpaidupgrade tags
            msg_param_sender_login
            msg_param_sender_name

            // giftpaidupgrade, anongiftpaidupgrade tags
            msg_param_promo_gift_total
            msg_param_promo_name

            // sub, resub, subgift, anonsubgift tags
            msg_param_sub_plan
            msg_param_sub_plan_name

            // raid tags
            msg_param_displayName
            msg_param_login
            msg_param_viewerCount

            // ritual tags
            msg_param_ritual_name

            // bitsbadgetier tags
            msg_param_threshold

            */
        }
    }

    public enum
    UserNoticeType
    {
        /// <summary>
        /// Unsupported user notice type.
        /// </summary>
        [EnumMember(Value = "")]
        Other = 0,

        /// <summary>
        /// A user subscribed.
        /// </summary>
        [EnumMember(Value = "sub")]
        Sub,

        /// <summary>
        /// A user resubscribed.
        /// </summary>
        [EnumMember(Value = "resub")]
        Resub,

        /// <summary>
        /// A user gifted a sub to another user.
        /// </summary>
        [EnumMember(Value = "giftsub")]
        GiftSub,

        /// <summary>
        /// A user is rading another user.
        /// </summary>
        [EnumMember(Value = "raid")]
        Raid,

        /// <summary>
        /// A ritual has occured.
        /// </summary>
        [EnumMember(Value = "ritual")]
        Ritual
    }

    #endregion

    #region Command: USERSTATE

    public class
    UserStateEventArgs : ChatRoomSupportedMessageEventArgs
    {
        /// <summary>
        /// Whether or not IRC tags were sent with the message.
        /// </summary>
        public bool tags_exist { get; private set; }

        /// <summary>
        /// <para>The tags attached to the message, if any.</para>
        /// <para>Check the <code>exist</code> property to determine if tags were attached to the message.</para>
        /// </summary>
        [ValidateMember(Check.TagsMissing)]
        public UserStateTags tags_stream_chat { get; protected set; }

        /// <summary>
        /// <para>The tags attached to the message, if any.</para>
        /// <para>Check the <code>exist</code> property to determine if tags were attached to the message.</para>
        /// </summary>
        [ValidateMember(Check.TagsMissing)]
        public ChatRoomUserStateTags tags_chat_room { get; protected set; }

        /// <summary>
        /// The channel that the user has joined or sent sent a message in.
        /// </summary>
        [ValidateMember(Check.IsValid)]
        public string channel { get; protected set; }        

        public UserStateEventArgs(in IrcMessage message) : base(message)
        {
            if (source == MessageSource.StreamChat)
            {
                tags_stream_chat = new UserStateTags(message);
            }
            else
            {
                tags_chat_room = new ChatRoomUserStateTags(message);
            }

            if (message.parameters.Length > 0)
            {
                channel = message.parameters[0];
            }            
        }
    }

    public class
    UserStateTags
    {
        /// <summary>
        /// Whether or not the user is a moderator.
        /// </summary>
        [IrcTag("mod")]
        public bool mod { get; protected set; }

        /// <summary>
        /// <para>The display name of the user.</para>
        /// <para>This is empty if it was never set by the user.</para>
        /// </summary>
        [IrcTag("display-name")]
        public string display_name { get; protected set; }

        /// <summary>
        /// The emote sets that are available for the user to use.
        /// </summary>
        [IrcTag("emote-sets")]
        public string[] emote_sets { get; protected set; }

        /// <summary>
        /// <para>The user's type</para>
        /// <para>Set to <see cref="UserType.None"/> if the user has no elevated privileges.</para>
        /// </summary>
        [IrcTag("user-type")]
        public UserType user_type { get; protected set; }

        /// <summary>
        /// <para>The color of the user's display name.</para>
        /// <para>The color is <see cref="Color.Empty"/> if it was never set by the user.</para>
        /// </summary>
        [IrcTag("color")]
        public Color color { get; protected set; }

        /// <summary>
        /// Whether or not the user is subscribed to the channel.
        /// </summary>
        [IrcTag("subscriber")]
        public bool subscriber { get; protected set; }

        /// <summary>
        /// <para>The chat badges that the user has, if any.</para>
        /// <para>The array is empty if the user has no chat badges.</para>
        /// </summary>
        [IrcTag("badges")]
        public Badge[] badges { get; protected set; }

        public UserStateTags(in IrcMessage message)
        {
            if (!message.tags_exist)
            {
                return;
            }

            mod = TwitchIrcUtil.Tags.ToBool(message, "mod");

            display_name = TwitchIrcUtil.Tags.ToString(message, "display-name");
            emote_sets = TwitchIrcUtil.Tags.ToStringArray(message, "emote-sets", ',');

            user_type = TwitchIrcUtil.Tags.ToUserType(message, "user-type");

            color = TwitchIrcUtil.Tags.FromtHtml(message, "color");

            subscriber = TwitchIrcUtil.Tags.ToBool(message, "subscriber");

            badges = TwitchIrcUtil.Tags.ToBadges(message, "badges");
        }
    }

    public class
    ChatRoomUserStateTags
    {
        /// <summary>
        /// Whether or not the user is a moderator.
        /// </summary>
        [IrcTag("mod")]
        public bool mod { get; protected set; }

        /// <summary>
        /// <para>The display name of the user.</para>
        /// <para>This is empty if it was never set by the user.</para>
        /// </summary>
        [IrcTag("display-name")]
        public string display_name { get; protected set; }

        /// <summary>
        /// The emote sets that are available for the user to use.
        /// </summary>
        [IrcTag("emote-sets")]
        public string[] emote_sets { get; protected set; }

        /// <summary>
        /// <para>The user's type</para>
        /// <para>Set to <see cref="UserType.None"/> if the user has no elevated privileges.</para>
        /// </summary>
        [IrcTag("user-type")]
        public UserType user_type { get; protected set; }

        /// <summary>
        /// <para>The color of the user's display name.</para>
        /// <para>The color is <see cref="Color.Empty"/> if it was never set by the user.</para>
        /// </summary>
        [IrcTag("color")]
        public Color color { get; protected set; }

        public ChatRoomUserStateTags(in IrcMessage message)
        {
            if (!message.tags_exist)
            {
                return;
            }

            mod = TwitchIrcUtil.Tags.ToBool(message, "mod");

            display_name = TwitchIrcUtil.Tags.ToString(message, "display-name");
            emote_sets = TwitchIrcUtil.Tags.ToStringArray(message, "emote-sets", ',');

            user_type = TwitchIrcUtil.Tags.ToUserType(message, "user-type");

            color = TwitchIrcUtil.Tags.FromtHtml(message, "color");
        }
    }

    #endregion

    #region Command: HOSTTARGET

    public class
    HostTargetEventArgs : IrcMessageEventArgs
    {
        /// <summary>
        /// <para>The number of viewers watching hosting channel.</para>
        /// <para>Set to -1 if this value was not inlcuded in the message.</para>
        /// </summary>
        [ValidateMember(Check.IsNotEqualTo, -1)]
        public int viewer_count { get; protected set; }

        /// <summary>
        /// <para>The user that is being hosted.</para>
        /// <para>This is empty if the hosting channel stops hosting.</para>
        /// </summary>
        [ValidateMember(Check.IsValid)]
        public string target_user { get; protected set; }

        /// <summary>
        /// The channel that started or stopped hosting.
        /// </summary>
        [ValidateMember(Check.IsValid)]
        public string hosting_channel { get; protected set; }

        /// <summary>
        /// <para>Whether the hosting channel started or stopped hosting a channel.</para>
        /// <para>Set to <see cref="HostTargetType.None"/> if the message could not be parsed.</para>
        /// </summary>
        [ValidateMember(Check.IsNotEqualTo, HostTargetType.None)]
        public HostTargetType target_type { get; protected set; }

        public HostTargetEventArgs(in IrcMessage message) : base(message)
        {
            target_type = HostTargetType.None;

            if (message.parameters.Length == 0)
            {
                return;
            }

            hosting_channel = message.parameters[0];

            if (!message.trailing.IsValid())
            {
                return;
            }

            if (message.trailing[0] == '-')
            {
                target_type = HostTargetType.Stop;

                target_user = string.Empty;
            }
            else
            {
                target_type = HostTargetType.Start;

                target_user = message.trailing.TextBefore(' ');
                if (!target_user.IsValid())
                {
                    target_user = message.trailing;
                }
            }

            viewer_count = Int32.TryParse(message.trailing.TextAfter(' '), out int _viewer_count) ? _viewer_count : -1;
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