// project namespaces
using TwitchNet.Debugger;
using TwitchNet.Utilities;

namespace
TwitchNet.Clients.Irc.Twitch
{
    public class
    SubscriberTags : UserNoticeTags, ITags
    {
        /// <summary>
        /// <para>How many months the recipient has been subscribed to the channel.</para>
        /// <para>Set to 0 if the recipient has subscribed for the first time.</para>
        /// </summary>
        [ValidateTag("msg-param-months")]
        public ushort           msg_param_months        { get; protected set; }

        /// <summary>
        /// The display name of the subscription plan.
        /// </summary>
        [ValidateTag("msg-param-sub-plan-name")]
        public string           msg_param_sub_plan_name { get; protected set; }

        /// <summary>
        /// <para>The subscription tier.</para>
        /// <para>Set to <see cref="SubscriptionTier.Other"/> if the plan could not be parsed.</para>
        /// </summary>
        [ValidateTag("msg-param-sub-plan")]
        public SubscriptionTier msg_param_sub_plan      { get; protected set; }

        /// <summary>
        /// Creates a new instance of the <see cref="SubscriberTags"/> class.
        /// </summary>
        /// <param name="message">The IRC message to parse.</param>
        public SubscriberTags(in IrcMessage message) : base(message)
        {
            if (!exist)
            {
                return;
            }

            msg_param_months        = TagsUtil.ToUInt16(message, "msg-param-months");
            msg_param_sub_plan_name = TagsUtil.ToString(message, "msg-param-sub-plan-name").Replace("\\s", " ");
            msg_param_sub_plan      = TagsUtil.ToSubscriptionPlan(message, "msg-param-sub-plan");
        }
    }
}