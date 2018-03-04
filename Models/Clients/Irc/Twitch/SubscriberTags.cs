// project namespaces
using TwitchNet.Enums.Clients.Irc.Twitch;
using TwitchNet.Events.Clients.Irc.Twitch;
using TwitchNet.Interfaces.Clients.Irc;
using TwitchNet.Utilities;

namespace
TwitchNet.Models.Clients.Irc.Twitch
{
    public class
    SubscriberTags : UserNoticeTags, ITags
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

        public SubscriberTags(UserNoticeEventArgs args) : base(args)
        {
            if (!is_valid)
            {
                return;
            }

            msg_param_months        = TagsUtil.ToUInt16(args.message_irc.tags, "msg-param-months");
            msg_param_sub_plan_name = TagsUtil.ToString(args.message_irc.tags, "msg-param-sub-plan-name").Replace("\\s", " ");
            msg_param_sub_plan      = TagsUtil.ToSubscriptionPlan(args.message_irc.tags, "msg-param-sub-plan");
        }
    }
}