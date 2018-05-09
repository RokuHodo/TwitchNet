// project namespaces
using TwitchNet.Enums.Clients.Irc.Twitch;
using TwitchNet.Interfaces.Clients.Irc;
using TwitchNet.Utilities;

namespace
TwitchNet.Models.Clients.Irc.Twitch
{
    public class
    SubscriberTags : UserNoticeTags, ITags
    {
        /// <summary>
        /// <para>How many months the recipient has been subscribed to the channel.</para>
        /// <para>Set to 0 if the recipient has subscribed for the first time.</para>
        /// </summary>
        [Tag("msg-param-months")]
        public ushort           msg_param_months        { get; protected set; }

        /// <summary>
        /// The display name of the subscription plan.
        /// </summary>
        [Tag("msg-param-sub-plan-name")]
        public string           msg_param_sub_plan_name { get; protected set; }

        /// <summary>
        /// <para>The subscription tier.</para>
        /// <para>Set to <see cref="SubscriptionPlan.None"/> if the plan could not be parsed.</para>
        /// </summary>
        [Tag("msg-param-sub-plan")]
        public SubscriptionPlan msg_param_sub_plan      { get; protected set; }

        public SubscriberTags(IrcMessage message) : base(message)
        {
            if (!is_valid)
            {
                return;
            }

            msg_param_months        = TagsUtil.ToUInt16(message.tags, "msg-param-months");
            msg_param_sub_plan_name = TagsUtil.ToString(message.tags, "msg-param-sub-plan-name").Replace("\\s", " ");
            msg_param_sub_plan      = TagsUtil.ToSubscriptionPlan(message.tags, "msg-param-sub-plan");
        }
    }
}