// project namespaces
using TwitchNet.Interfaces.Clients.Irc;
using TwitchNet.Utilities;

namespace
TwitchNet.Models.Clients.Irc.Twitch
{
    public class
    GiftedSubscriberTags : SubscriberTags, ITags
    {
        /// <summary>
        /// The id of the user who was gifted the subscription. 
        /// </summary>
        public string msg_param_recipient_id            { get; protected set; }

        /// <summary>
        /// The login of the user who was gifted the subscription.
        /// </summary>
        public string msg_param_recipient_user_name     { get; protected set; }

        /// <summary>
        /// The display name of the user who was gifted the subscription.
        /// </summary>
        public string msg_param_recipient_display_name  { get; protected set; }

        public GiftedSubscriberTags(IrcMessage message) : base(message)
        {
            if (!is_valid)
            {
                return;
            }

            msg_param_recipient_id              = TagsUtil.ToString(message.tags, "msg-param-recipient-id");
            msg_param_recipient_user_name       = TagsUtil.ToString(message.tags, "msg-param-recipient-user-name");
            msg_param_recipient_display_name    = TagsUtil.ToString(message.tags, "msg-param-recipient-display-name");
        }
    }
}