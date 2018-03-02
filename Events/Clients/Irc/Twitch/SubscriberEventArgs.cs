// project namespaces
using TwitchNet.Enums.Clients.Irc.Twitch;
using TwitchNet.Extensions;
using TwitchNet.Utilities;

namespace
TwitchNet.Events.Clients.Irc.Twitch
{
    public class
    SubscriberEventArgs : UserNoticeEventArgs
    {
        /// <summary>
        /// <para>How many months the user has been subscribed to the channel.</para>
        /// <para>Set to 0 if the user has subscribed for the first time.</para>
        /// </summary>
        public ushort           msg_param_months        { get; protected set; }

        /// <summary>
        /// The display name of the subscription plan.
        /// </summary>
        public string           msg_param_sub_plan_name { get; protected set; }

        /// <summary>
        /// <para>The subscription tier.</para>
        /// <para>Set to <see cref="SubscriptionPlan.None"/> if the plan could not be parsed.</para>
        /// </summary>
        public SubscriptionPlan msg_param_sub_plan      { get; protected set; }

        public SubscriberEventArgs(UserNoticeEventArgs args) : base(args)
        {
            if (args.message_irc.tags.IsValid())
            {
                msg_param_months            = TagsUtil.ToUInt16(message_irc.tags, "msg-param-months");
                msg_param_sub_plan_name     = TagsUtil.ToString(message_irc.tags, "msg-param-sub-plan-name").Replace("\\s", " ");
                msg_param_sub_plan          = TagsUtil.ToSubscriptionPlan(message_irc.tags, "msg-param-sub-plan");
            }
        }
    }
}
