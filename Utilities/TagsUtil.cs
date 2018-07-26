// standard namespaces
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.CompilerServices;

//project namespaces
//using TwitchNet.Rest.Api.Videos;
using TwitchNet.Clients.Irc;
using TwitchNet.Clients.Irc.Twitch;
using TwitchNet.Extensions;

namespace
TwitchNet.Utilities
{
    internal class
    TagsUtil
    {
        /// <summary>
        /// Converts a tag to an equivalent <see cref="String"/> value.
        /// </summary>
        /// <param name="tags">The IRC message tags.</param>
        /// <param name="key">The tag to convert.</param>
        /// <returns>
        /// Returns the equivalent <see cref="string"/> value if the tag was able to be converted.
        /// Returns <see cref="string.Empty"/> otherwise.
        /// </returns>
        public static string
        ToString(in IrcMessage message, string key)
        {
            string value = string.Empty;

            if(!IsTagValid(message, key))
            {
                return value;
            }

            value = message.tags[key];

            return value;
        }

        /// <summary>
        /// Converts a tag to an equivalent <see cref="UInt16"/> value.
        /// </summary>
        /// <param name="tags">The IRC message tags.</param>
        /// <param name="key">The tag to convert.</param>
        /// <returns>
        /// Returns the equivalent <see cref="UInt16"/> value if the tag was able to be converted.
        /// Returns 0 otherwise.
        /// </returns>
        public static ushort
        ToUInt16(in IrcMessage message, string key)
        {
            ushort value = 0;

            if (!IsTagValid(message, key))
            {
                return value;
            }

            UInt16.TryParse(message.tags[key], out value);

            return value;
        }

        /// <summary>
        /// Converts a tag to an equivalent <see cref="UInt32"/> value.
        /// </summary>
        /// <param name="tags">The IRC message tags.</param>
        /// <param name="key">The tag to convert.</param>
        /// <returns>
        /// Returns the equivalent <see cref="UInt32"/> value if the tag was able to be converted.
        /// Returns 0 otherwise.
        /// </returns>
        public static uint
        ToUInt32(in IrcMessage message, string key)
        {
            uint value = 0;

            if (!IsTagValid(message, key))
            {
                return value;
            }

            UInt32.TryParse(message.tags[key], out value);

            return value;
        }

        /// <summary>
        /// Converts a tag to an equivalent <see cref="TimeSpan"/> value.
        /// </summary>
        /// <param name="tags">The IRC message tags.</param>
        /// <param name="key">The tag to convert.</param>
        /// <returns>
        /// Returns the equivalent <see cref="TimeSpan"/> value if the tag was able to be converted.
        /// Returns <see cref="TimeSpan.Zero"/> otherwise.
        /// </returns>
        public static TimeSpan
        ToTimeSpanFromSeconds(in IrcMessage message, string key)
        {
            int seconds = ToInt32(message, key);

            TimeSpan time = seconds == 0 ? TimeSpan.Zero : new TimeSpan(0, 0, seconds);

            return time;
        }

        /// <summary>
        /// Converts a tag to an equivalent <see cref="Int32"/> value.
        /// </summary>
        /// <param name="tags">The IRC message tags.</param>
        /// <param name="key">The tag to convert.</param>
        /// <returns>
        /// Returns the equivalent <see cref="Int32"/> value if the tag was able to be converted.
        /// Returns -1 otherwise.
        /// </returns>
        public static int
        ToInt32(in IrcMessage message, string key)
        {
            int value = -1;

            if (!IsTagValid(message, key))
            {
                return value;
            }

            Int32.TryParse(message.tags[key], out value);

            return value;
        }

        /// <summary>
        /// Converts a tag to an equivalent <see cref="Boolean"/> value.
        /// </summary>
        /// <param name="tags">The IRC message tags.</param>
        /// <param name="key">The tag to convert.</param>
        /// <returns>
        /// Returns the equivalent <see cref="Boolean"/> value if the tag was able to be converted.
        /// Returns false otherwise.
        /// </returns>
        public static bool
        ToBool(in IrcMessage message, string key)
        {
            bool value = default;

            if (!IsTagValid(message, key))
            {
                return value;
            }

            if(!Byte.TryParse(message.tags[key], out byte _value))
            {
                return value;
            }

            value = Convert.ToBoolean(_value);

            return value;
        }        

        /// <summary>
        /// Converts a tag to an equivalent <see cref="UserType"/> value.
        /// </summary>
        /// <param name="tags">The IRC message tags.</param>
        /// <param name="key">The tag to convert.</param>
        /// <returns>
        /// Returns the equivalent <see cref="UserType"/> value if the tag was able to be converted.
        /// Returns <see cref="UserType.None"/> otherwise.
        /// </returns>
        public static UserType
        ToUserType(in IrcMessage message, string key)
        {
            UserType user_type = UserType.None;

            if (!IsTagValid(message, key))
            {
                return user_type;
            }

            user_type = EnumUtil.Parse<UserType>(message.tags[key]);

            return user_type;
        }

        /// <summary>
        /// Converts a tag to an equivalent <see cref="NoticeType"/> value.
        /// </summary>
        /// <param name="tags">The IRC message tags.</param>
        /// <param name="key">The tag to convert.</param>
        /// <returns>
        /// Returns the equivalent <see cref="NoticeType"/> value if the tag was able to be converted.
        /// Returns <see cref="NoticeType.Other"/> otherwise.
        /// </returns>
        public static NoticeType
        ToNoticeType(in IrcMessage message, string key)
        {
            NoticeType notice = NoticeType.Other;

            if (!IsTagValid(message, key))
            {
                return notice;
            }

            EnumUtil.TryParse(message.tags[key], out notice);

            return notice;
        }

        /// <summary>
        /// Converts a tag to an equivalent <see cref="UserNoticeType"/> value.
        /// </summary>
        /// <param name="tags">The IRC message tags.</param>
        /// <param name="key">The tag to convert.</param>
        /// <returns>
        /// Returns the equivalent <see cref="UserNoticeType"/> value if the tag was able to be converted.
        /// Returns <see cref="UserNoticeType.Other"/> otherwise.
        /// </returns>
        public static UserNoticeType
        ToUserNoticeType(in IrcMessage message, string key)
        {
            UserNoticeType user_notice = UserNoticeType.Other;

            if(!IsTagValid(message, key))
            {
                return user_notice;
            }

            EnumUtil.TryParse(message.tags[key], out user_notice);

            return user_notice;
        }

        /// <summary>
        /// Converts a tag to an equivalent <see cref="SubscriptionPlan"/> value.
        /// </summary>
        /// <param name="tags">The IRC message tags.</param>
        /// <param name="key">The tag to convert.</param>
        /// <returns>
        /// Returns the equivalent <see cref="SubscriptionPlan"/> value if the tag was able to be converted.
        /// Returns <see cref="SubscriptionPlan.Other"/> otherwise.
        /// </returns>
        public static SubscriptionPlan
        ToSubscriptionPlan(in IrcMessage message, string key)
        {
            SubscriptionPlan plan = SubscriptionPlan.Other;

            if (!IsTagValid(message, key))
            {
                return plan;
            }

            EnumUtil.TryParse(message.tags[key], out plan);

            return plan;
        }

        /// <summary>
        /// Converts a tag to an equivalent <see cref="RitualType"/> value.
        /// </summary>
        /// <param name="tags">The IRC message tags.</param>
        /// <param name="key">The tag to convert.</param>
        /// <returns>
        /// Returns the equivalent <see cref="RitualType"/> value if the tag was able to be converted.
        /// Returns <see cref="RitualType.Other"/> otherwise.
        /// </returns>
        public static RitualType
        ToRitualType(in IrcMessage message, string key)
        {
            RitualType type = RitualType.Other;

            if (!IsTagValid(message, key))
            {
                return type;
            }

            EnumUtil.TryParse(message.tags[key], out type);

            return type;
        }

        /// <summary>
        /// Converts a tag to an equivalent <see cref="BroadcasterLanguage"/> value.
        /// </summary>
        /// <param name="tags">The IRC message tags.</param>
        /// <param name="key">The tag to convert.</param>
        /// <returns>
        /// Returns the equivalent <see cref="BroadcasterLanguage"/> value if the tag was able to be converted.
        /// Returns <see cref="BroadcasterLanguage.None"/> otherwise.
        /// </returns>
        public static BroadcasterLanguage
        ToBroadcasterLanguage(in IrcMessage message, string key)
        {
            BroadcasterLanguage language = BroadcasterLanguage.None;

            if (!IsTagValid(message, key))
            {
                return language;
            }

            EnumUtil.TryParse(message.tags[key], out language);

            return language;
        }

        /// <summary>
        /// Converts a tag to an equivalent <see cref="Color"/> value.
        /// </summary>
        /// <param name="tags">The IRC message tags.</param>
        /// <param name="key">The tag to convert.</param>
        /// <returns>
        /// Returns the equivalent <see cref="Color"/> value if the tag was able to be converted.
        /// Returns <see cref="Color.Empty"/> otherwise.
        /// </returns>
        public static Color
        FromtHtml(in IrcMessage message, string key)
        {
            Color color = Color.Empty;

            if (!IsTagValid(message, key))
            {
                return color;
            }

            if (!message.tags[key].IsValidHtmlColor())
            {
                return color;
            }

            color = ColorTranslator.FromHtml(message.tags[key]);

            return color;
        }

        /// <summary>
        /// Converts a tag Unix Epoch time to an equivalent <see cref="DateTime"/> value.
        /// </summary>
        /// <param name="tags">The IRC message tags.</param>
        /// <param name="key">The tag to convert.</param>
        /// <returns>
        /// Returns the equivalent <see cref="Color"/> value if the tag was able to be converted.
        /// Returns <see cref="Color.Empty"/> otherwise.
        /// </returns>
        public static DateTime
        FromUnixEpochMilliseconds(in IrcMessage message, string key)
        {
            DateTime time = DateTime.MinValue;

            if (!IsTagValid(message, key))
            {
                return time;
            }

            if (!Int64.TryParse(message.tags[key], out long time_epoch))
            {
                return time;
            }

            time = time_epoch.FromUnixEpochMilliseconds();

            return time;
        }

        /// <summary>
        /// Converts a tag to an equivalent array of <see cref="Badge"/> values.
        /// </summary>
        /// <param name="tags">The IRC message tags.</param>
        /// <param name="key">The tag to convert.</param>
        /// <returns>
        /// Returns the equivalent array of <see cref="Badge"/> values if the tag was able to be converted.
        /// Returns an empty <see cref="Badge"/> array otherwise.
        /// </returns>
        public static Badge[]
        ToBadges(in IrcMessage message, string key)
        {
            if(!IsTagValid(message, key))
            {
                return new Badge[0];
            }

            string[] badge_pairs = message.tags[key].Split(',');
            if (!badge_pairs.IsValid())
            {
                return new Badge[0];
            }

            List<Badge> badges = new List<Badge>();
            foreach(string pair in badge_pairs)
            {
                Badge badge = new Badge(pair);
                badges.Add(badge);
            }

            return badges.ToArray();
        }

        /// <summary>
        /// Converts a tag to an equivalent array of <see cref="Emote"/> values.
        /// </summary>
        /// <param name="tags">The IRC message tags.</param>
        /// <param name="key">The tag to convert.</param>
        /// <returns>
        /// Returns the equivalent array of <see cref="Emote"/> values if the tag was able to be converted.
        /// Returns an empty <see cref="Emote"/> array otherwise.
        /// </returns>
        public static Emote[]
        ToEmotes(in IrcMessage message, string key)
        {
            if (!IsTagValid(message, key))
            {
                return new Emote[0];
            }

            string[] pairs = message.tags[key].Split('/');
            if (!pairs.IsValid())
            {
                return new Emote[0];
            }

            List<Emote> emotes = new List<Emote>();
            foreach (string pair in pairs)
            {
                Emote emote = new Emote(pair);
                emotes.Add(emote);
            }

            return emotes.ToArray();
        }

        /// <summary>
        /// Converts a tag to an equivalent array of <see cref="String"/> values.
        /// </summary>
        /// <param name="tags">The IRC message tags.</param>
        /// <param name="key">The tag to convert.</param>
        /// <returns>
        /// Returns the equivalent array of <see cref="String"/> values if the tag was able to be converted.
        /// Returns an empty <see cref="String"/> array otherwise.
        /// </returns>
        public static string[]
        ToStringArray(in IrcMessage message, string key, char separator)
        {
            if (!IsTagValid(message, key))
            {
                return new string[0];
            }

            string[] array = message.tags[key].Split(separator);

            return array;
        }

        /// <summary>
        /// Checks to see if the specified tag was included in the attached tags.
        /// </summary>
        /// <param name="tags">The IRC message tags.</param>
        /// <param name="key">The tag to check.</param>
        /// <returns>
        /// Returns true if tags were attached in the IRC message and if the specified tag was included in the attached tags.
        /// Returns false otherwise.
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool
        IsTagValid(in IrcMessage message, string key)
        {
            bool valid = true;

            if (message.IsNullOrDefault())
            {
                return false;
            }

            if(!key.IsValid() || !message.tags.IsValid())
            {
                return false;
            }

            if (!message.tags.ContainsKey(key) || !message.tags[key].IsValid())
            {
                return false;
            }

            return valid;
        }        
    }
}
