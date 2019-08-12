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