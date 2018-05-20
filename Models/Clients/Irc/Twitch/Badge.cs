// standard namespaces
using System;

// project namespaces
using TwitchNet.Debugger;
using TwitchNet.Enums.Clients.Irc.Twitch;
using TwitchNet.Enums.Debugger;
using TwitchNet.Extensions;
using TwitchNet.Utilities;

namespace
TwitchNet.Models.Clients.Irc.Twitch
{
    [ValidateObject]
    public struct
    Badge
    {
        /// <summary>
        /// <para>The badge verison.</para>
        /// <para>The version is set to -1 when <see cref="type"/> is equal to <see cref="BadgeType.None"/>.</para>
        /// </summary>
        [ValidateMember(Check.IsNotEqualTo, -1)]
        public int version;

        /// <summary>
        /// <para>The badge type.</para>
        /// <para>The badge is set to <see cref="BadgeType.None"/> if no valid badge type is found.</para>
        /// </summary>
        [ValidateMember(Check.IsNotEqualTo, BadgeType.None)]
        public BadgeType type;

        public Badge(string pair)
        {
            char separator = '/';
            if(!Int32.TryParse(pair.TextAfter(separator), out version))
            {
                version = -1;
            }

            type = EnumCacheUtil.ToBadgeType(pair.TextBefore('/'));
        }
    }
}
