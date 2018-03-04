// standard namespaces
using System;

// project namespaces
using TwitchNet.Enums.Clients.Irc.Twitch;
using TwitchNet.Extensions;
using TwitchNet.Utilities;

namespace
TwitchNet.Models.Clients.Irc.Twitch
{
    public struct
    Badge
    {
        /// <summary>
        /// <para>The badge verison.</para>
        /// <para>The version is set to -1 when <see cref="badge"/> is equal to <see cref="BadgeType.None"/>.</para>
        /// </summary>
        short version;

        /// <summary>
        /// <para>The badge type.</para>
        /// <para>The badge is set to <see cref="BadgeType.None"/> if no valid badge type is found.</para>
        /// </summary>
        BadgeType badge;

        public Badge(string badge_pair)
        {

            if(!Int16.TryParse(badge_pair.TextAfter('/'), out version))
            {
                version = -1;
            }

            badge = BadgeType.None;
            badge = EnumCacheUtil.ToBadge(badge_pair.TextBefore('/'));
        }
    }
}
