// standard namespaces
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

//project namespaces
using TwitchNet.Enums;
using TwitchNet.Enums.Api.Videos;
using TwitchNet.Enums.Clients.Irc.Twitch;
using TwitchNet.Extensions;
using TwitchNet.Models.Clients.Irc.Twitch;
using TwitchNet.Utilities;

namespace
TwitchNet.Utilities
{
    public class
    TagsUtil
    {
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

        public static int
        ToInt32(Dictionary<string, string> tags, string key)
        {
            int value = 0;

            if (!IsTagValid(tags, key))
            {
                return value;
            }

            Int32.TryParse(tags[key], out value);

            return value;
        }

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

        public static Color
        FromtHtml(Dictionary<string, string> tags, string key)
        {
            Color color = Color.Empty;

            if (!IsTagValid(tags, key))
            {
                return color;
            }

            if (!TwitchUtil.IsValidHtmlColor(tags[key]))
            {
                return color;
            }

            color = ColorTranslator.FromHtml(tags[key]);

            return color;
        }

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

        public static Emote[]
        ToEmotes(Dictionary<string, string> tags, string key)
        {
            if (!IsTagValid(tags, key))
            {
                return new Emote[0];
            }

            string[] emote_pairs = tags[key].StringToArray<string>('/');
            if (!emote_pairs.IsValid())
            {
                return new Emote[0];
            }

            List<Emote> emotes = new List<Emote>();
            foreach (string pair in emote_pairs)
            {
                Emote emote = new Emote(pair);
                emotes.Add(emote);
            }

            return emotes.ToArray();
        }        

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

        public static type[]
        ToArray<type>(Dictionary<string, string> tags, string key, char separator)
        {
            if (!IsTagValid(tags, key))
            {
                return new type[0];
            }

            type[] array = tags[key].StringToArray<type>(separator);

            return array;
        }

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
