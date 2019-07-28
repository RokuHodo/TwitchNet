// standard namespaces
using System.Drawing;

// project namespaces
using TwitchNet.Debugger;
using TwitchNet.Extensions;
using TwitchNet.Utilities;

namespace
TwitchNet.Clients.Irc.Twitch
{
    public class
    WhisperTags
    {
        /// <summary>
        /// Whether or not the sender has Twitch turbo.
        /// </summary>
        [IrcTag("turbo")]
        public bool     turbo           { get; protected set; }

        /// <summary>
        /// <para>The display name of the sender.</para>
        /// <para>This is empty if it was never set by the sender.</para>
        /// </summary>
        [IrcTag("display-name")]
        public string   display_name    { get; protected set; }

        /// <summary>
        /// The user id of the sender.
        /// </summary>
        [IrcTag("user-id")]
        public string   user_id         { get; protected set; }

        /// <summary>
        /// The id of the message.
        /// </summary>
        [IrcTag("message-id")]
        public string   message_id      { get; protected set; }

        /// <summary>
        /// The sender id and recipient id.
        /// </summary>
        [IrcTag("thread-id")]
        public string   thread_id       { get; protected set; }

        /// <summary>
        /// The id of the recipient.
        /// </summary>
        [IrcTag("recipient-id")]
        public string   recipient_id    { get; protected set; }

        /// <summary>
        /// <para>The sender's user type</para>
        /// <para>Set to <see cref="UserType.None"/> if the sender has no elevated privileges.</para>
        /// </summary>
        [IrcTag("user-type")]
        public UserType user_type       { get; protected set; }

        /// <summary>
        /// <para>The color of the sender's display name.</para>
        /// <para>The color is <see cref="Color.Empty"/> if it was never set by the sender.</para>
        /// </summary>
        [IrcTag("color")]
        public Color    color           { get; protected set; }

        /// <summary>
        /// <para>The chat badges that the sender has, if any.</para>
        /// <para>The array is empty if the sender has no chat badges.</para>
        /// </summary>
        [IrcTag("badges")]
        public Badge[]  badges          { get; protected set; }

        /// <summary>
        /// <para>The emotes the sender used in the message, if any.</para>
        /// <para>The array is empty if the sender did not use any emotes.</para>
        /// </summary>
        [IrcTag("emotes")]
        public Emote[]  emotes          { get; protected set; }

        /// <summary>
        /// Creates a new instance of the <see cref="WhisperTags"/> class.
        /// </summary>
        /// <param name="message">The IRC message to parse.</param>
        public WhisperTags(in IrcMessage message)
        {
            turbo           = TwitchIrcUtil.Tags.ToBool(message, "turbo");

            display_name    = TwitchIrcUtil.Tags.ToString(message, "display-name");
            user_id         = TwitchIrcUtil.Tags.ToString(message, "user-id");
            message_id      = TwitchIrcUtil.Tags.ToString(message, "message-id");
            thread_id       = TwitchIrcUtil.Tags.ToString(message, "thread-id");
            recipient_id    = thread_id.TextAfter('_');

            user_type       = TwitchIrcUtil.Tags.ToUserType(message, "user-type");

            color           = TwitchIrcUtil.Tags.FromtHtml(message, "color");

            badges          = TwitchIrcUtil.Tags.ToBadges(message, "badges");
            emotes          = TwitchIrcUtil.Tags.ToEmotes(message, "emotes");
        }
    }
}