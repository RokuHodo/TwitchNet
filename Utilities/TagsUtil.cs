// standard namespaces
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.CompilerServices;

//project namespaces
using TwitchNet.Rest.Api.Videos;
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
        ToString(Dictionary<string, string> tags, string key)
        {
            string value = string.Empty;

            if(!IsTagValid(tags, key))
            {
                return value;
            }

            value = tags[key];

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
        ToUInt16(Dictionary<string, string> tags, string key)
        {
            ushort value = 0;

            if (!IsTagValid(tags, key))
            {
                return value;
            }

            UInt16.TryParse(tags[key], out value);

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
        ToUInt32(Dictionary<string, string> tags, string key)
        {
            uint value = 0;

            if (!IsTagValid(tags, key))
            {
                return value;
            }

            UInt32.TryParse(tags[key], out value);

            return value;
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
        ToInt32(Dictionary<string, string> tags, string key)
        {
            int value = -1;

            if (!IsTagValid(tags, key))
            {
                return value;
            }

            Int32.TryParse(tags[key], out value);

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
        ToBool(Dictionary<string, string> tags, string key)
        {
            bool value = default(bool);

            if (!IsTagValid(tags, key))
            {
                return value;
            }

            if(!Byte.TryParse(tags[key], out byte _value))
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
        ToUserType(Dictionary<string, string> tags, string key)
        {
            UserType user_type = UserType.None;

            if (!IsTagValid(tags, key))
            {
                return user_type;
            }

            user_type = EnumCacheUtil.ToUserType(tags[key]);

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
        ToNoticeType(Dictionary<string, string> tags, string key)
        {
            NoticeType notice = NoticeType.Other;

            if (!IsTagValid(tags, key))
            {
                return notice;
            }

            notice = EnumCacheUtil.ToNoticeType(tags[key]);

            return notice;
        }

        /// <summary>
        /// Converts a tag to an equivalent <see cref="UserNoticeType"/> value.
        /// </summary>
        /// <param name="tags">The IRC message tags.</param>
        /// <param name="key">The tag to convert.</param>
        /// <returns>
        /// Returns the equivalent <see cref="UserNoticeType"/> value if the tag was able to be converted.
        /// Returns <see cref="UserNoticeType.None"/> otherwise.
        /// </returns>
        public static UserNoticeType
        ToUserNoticeType(Dictionary<string, string> tags, string key)
        {
            UserNoticeType user_notice = UserNoticeType.None;

            if(!IsTagValid(tags, key))
            {
                return user_notice;
            }

            user_notice = EnumCacheUtil.ToUserNoticeType(tags[key]);

            return user_notice;
        }

        /// <summary>
        /// Converts a tag to an equivalent <see cref="SubscriptionPlan"/> value.
        /// </summary>
        /// <param name="tags">The IRC message tags.</param>
        /// <param name="key">The tag to convert.</param>
        /// <returns>
        /// Returns the equivalent <see cref="SubscriptionPlan"/> value if the tag was able to be converted.
        /// Returns <see cref="SubscriptionPlan.None"/> otherwise.
        /// </returns>
        public static SubscriptionPlan
        ToSubscriptionPlan(Dictionary<string, string> tags, string key)
        {
            SubscriptionPlan plan = SubscriptionPlan.None;

            if (!IsTagValid(tags, key))
            {
                return plan;
            }

            plan = EnumCacheUtil.ToSubscriptionPlan(tags[key]);

            return plan;
        }

        /// <summary>
        /// Converts a tag to an equivalent <see cref="RitualType"/> value.
        /// </summary>
        /// <param name="tags">The IRC message tags.</param>
        /// <param name="key">The tag to convert.</param>
        /// <returns>
        /// Returns the equivalent <see cref="RitualType"/> value if the tag was able to be converted.
        /// Returns <see cref="RitualType.None"/> otherwise.
        /// </returns>
        public static RitualType
        ToRitualType(Dictionary<string, string> tags, string key)
        {
            RitualType type = RitualType.None;

            if (!IsTagValid(tags, key))
            {
                return type;
            }

            type = EnumCacheUtil.ToRitualType(tags[key]);

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
        ToBroadcasterLanguage(Dictionary<string, string> tags, string key)
        {
            BroadcasterLanguage language = BroadcasterLanguage.None;

            if (!IsTagValid(tags, key))
            {
                return language;
            }

            language = EnumCacheUtil.ToBroadcasterLanguage(tags[key]);

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
        FromtHtml(Dictionary<string, string> tags, string key)
        {
            Color color = Color.Empty;

            if (!IsTagValid(tags, key))
            {
                return color;
            }

            if (!tags[key].IsValidHtmlColor())
            {
                return color;
            }

            color = ColorTranslator.FromHtml(tags[key]);

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
        FromUnixEpoch(Dictionary<string, string> tags, string key)
        {
            DateTime time = DateTime.MinValue;

            if (!IsTagValid(tags, key))
            {
                return time;
            }

            if (!Int64.TryParse(tags[key], out long time_epoch))
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
        ToBadges(Dictionary<string, string> tags, string key)
        {
            if(!IsTagValid(tags, key))
            {
                return new Badge[0];
            }

            string[] badge_pairs = tags[key].StringToArray<string>(',');
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
        ToEmotes(Dictionary<string, string> tags, string key)
        {
            if (!IsTagValid(tags, key))
            {
                return new Emote[0];
            }

            string[] pairs = tags[key].StringToArray<string>('/');
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
        ToStringArray(Dictionary<string, string> tags, string key, char separator)
        {
            if (!IsTagValid(tags, key))
            {
                return new string[0];
            }

            string[] array = tags[key].Split(new char[] { separator }, StringSplitOptions.RemoveEmptyEntries);

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
        IsTagValid(Dictionary<string, string> tags, string key)
        {
            bool valid = true;

            if(!key.IsValid() || !tags.IsValid())
            {
                return false;
            }

            if (!tags.ContainsKey(key) || !tags[key].IsValid())
            {
                return false;
            }

            return valid;
        }        
    }
}
