using System;

// project namespaces
using TwitchNet.Enums.Clients.Irc.Twitch;
using TwitchNet.Extensions;

namespace TwitchNet.Models.Clients.Irc.Twitch
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
            badge = GetBadge(badge_pair.TextBefore('/'));
        }

        /// <summary>
        /// Gets the <see cref="BadgeType"/> from 
        /// </summary>
        /// <param name="badge_string"></param>
        /// <returns>
        /// Returns the badge parsed from the badge pair if valid.
        /// Returns <see cref="BadgeType.None"/> otherwise.
        /// </returns>
        private BadgeType 
        GetBadge(string badge_string)
        {
            BadgeType _badge = BadgeType.None;

            // TODO: Create an enum dictionary cache instead of doing switch/case for every enum?

            // This has the possibility of being called several thousand times a second.
            // Don't use reflection since it is really slow.
            switch (badge_string)
            {
                case "admin":
                {
                    _badge = BadgeType.Admin;
                }
                break;

                case "bits":
                {
                    _badge = BadgeType.Bits;
                }
                break;

                case "broadcaster":
                {
                    _badge = BadgeType.Broadcaster;
                }
                break;

                case "global_mod":
                {
                    _badge = BadgeType.GlobalMod;
                }
                break;

                case "moderator":
                {
                    _badge = BadgeType.Moderator;
                }
                break;

                case "subscriber":
                {
                    _badge = BadgeType.Subscriber;
                }
                break;

                case "staff":
                {
                    _badge = BadgeType.Staff;
                }
                break;

                case "premium":
                {
                    _badge = BadgeType.Premium;
                }
                break;

                case "turbo":
                {
                    _badge = BadgeType.Turbo;
                }
                break;
            }

            return _badge;
        }
    }
}
