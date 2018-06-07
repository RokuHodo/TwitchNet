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
    WhisperTags : ITags
    {
        /// <summary>
        /// Whether or not tags are atached to the message.
        /// </summary>
        public bool     exist        { get; protected set; }

        /// <summary>
        /// Whether or not the sender has Twitch turbo.
        /// </summary>
        [ValidateTag("turbo")]
        public bool     turbo           { get; protected set; }

        /// <summary>
        /// <para>The display name of the sender.</para>
        /// <para>This is empty if it was never set by the sender.</para>
        /// </summary>
        [ValidateTag("display-name")]
        public string   display_name    { get; protected set; }

        /// <summary>
        /// The user id of the sender.
        /// </summary>
        [ValidateTag("user-id")]
        public string   user_id         { get; protected set; }

        /// <summary>
        /// The id of the message.
        /// </summary>
        [ValidateTag("message-id")]
        public string   message_id      { get; protected set; }

        /// <summary>
        /// The sender id and recipient id.
        /// </summary>
        [ValidateTag("thread-id")]
        public string   thread_id       { get; protected set; }

        /// <summary>
        /// The id of the recipient.
        /// </summary>
        [ValidateTag("recipient-id")]
        public string   recipient_id    { get; protected set; }

        /// <summary>
        /// <para>The sender's user type</para>
        /// <para>Set to <see cref="UserType.None"/> if the sender has no elevated privileges.</para>
        /// </summary>
        [ValidateTag("user-type")]
        public UserType user_type       { get; protected set; }

        /// <summary>
        /// <para>The color of the sender's display name.</para>
        /// <para>The color is <see cref="Color.Empty"/> if it was never set by the sender.</para>
        /// </summary>
        [ValidateTag("color")]
        public Color    color           { get; protected set; }

        /// <summary>
        /// <para>The chat badges that the sender has, if any.</para>
        /// <para>The array is empty if the sender has no chat badges.</para>
        /// </summary>
        [ValidateTag("badges")]
        public Badge[]  badges          { get; protected set; }

        /// <summary>
        /// <para>The emotes the sender used in the message, if any.</para>
        /// <para>The array is empty if the sender did not use any emotes.</para>
        /// </summary>
        [ValidateTag("emotes")]
        public Emote[]  emotes          { get; protected set; }

        /// <summary>
        /// Creates a new instance of the <see cref="WhisperTags"/> class.
        /// </summary>
        /// <param name="message">The IRC message to parse.</param>
        public WhisperTags(in IrcMessage message)
        {
            exist = message.tags.IsValid();
            if (!exist)
            {
                return;
            }

            turbo           = TagsUtil.ToBool(message.tags, "turbo");

            display_name    = TagsUtil.ToString(message.tags, "display-name");
            user_id         = TagsUtil.ToString(message.tags, "user-id");
            message_id      = TagsUtil.ToString(message.tags, "message-id");
            thread_id       = TagsUtil.ToString(message.tags, "thread-id");
            recipient_id    = thread_id.TextAfter('_');

            user_type       = TagsUtil.ToUserType(message.tags, "user-type");

            color           = TagsUtil.FromtHtml(message.tags, "color");

            badges          = TagsUtil.ToBadges(message.tags, "badges");
            emotes          = TagsUtil.ToEmotes(message.tags, "emotes");
        }
    }
}