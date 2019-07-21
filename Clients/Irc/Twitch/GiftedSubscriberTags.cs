// project namespaces
using TwitchNet.Debugger;
using TwitchNet.Utilities;

namespace
TwitchNet.Clients.Irc.Twitch
{
    public class
    GiftedSubscriberTags : SubscriberTags, ITags
    {
        /// <summary>
        /// The id of the user who was gifted the subscription. 
        /// </summary>
        [ValidateTag("msg-param-recipient-id")]
        public string msg_param_recipient_id            { get; protected set; }

        /// <summary>
        /// The login of the user who was gifted the subscription.
        /// </summary>
        [ValidateTag("msg-param-recipient-user-name")]
        public string msg_param_recipient_user_name     { get; protected set; }

        /// <summary>
        /// The display name of the user who was gifted the subscription.
        /// </summary>
        [ValidateTag("msg-param-recipient-display-name")]
        public string msg_param_recipient_display_name  { get; protected set; }

        /// <summary>
        /// Creates a new instance of the <see cref="GiftedSubscriberTags"/> class.
        /// </summary>
        /// <param name="message">The IRC message to parse.</param>
        public GiftedSubscriberTags(in IrcMessage message) : base(message)
        {
            if (!exist)
            {
                return;
            }

            msg_param_recipient_id              = TagsUtil.ToString(message, "msg-param-recipient-id");
            msg_param_recipient_user_name       = TagsUtil.ToString(message, "msg-param-recipient-user-name");
            msg_param_recipient_display_name    = TagsUtil.ToString(message, "msg-param-recipient-display-name");
        }
    }
}