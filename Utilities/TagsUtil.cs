// standard namespaces
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

//project namespaces
using TwitchNet.Enums;
using TwitchNet.Extensions;
using TwitchNet.Models.Clients.Irc.Twitch;

namespace
TwitchNet.Utilities
{
    public class
    TagsUtil
    {
        public static string
        ToString(Dictionary<string, string> tags, string key)
        {
            // this method is more for syntactic sugar than anything else
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
            // this method is more for syntactic sugar than anything else
            uint value = 0;

            if (!IsTagValid(tags, key))
            {
                return value;
            }

            UInt32.TryParse(tags[key], out value);

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

            Regex regex = new Regex("^#([A-Fa-f0-9]{6}|[A-Fa-f0-9]{3})$");
            if (!regex.IsMatch(tags[key]))
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

            // TODO: Create an enum dictionary cache instead of doing switch/case for every enum?
            switch (tags[key])
            {
                case "mod":
                {
                    user_type = UserType.Mod;
                }
                break;

                case "global_mod":
                {
                    user_type = UserType.GlobalMod;
                }
                break;

                case "admin":
                {
                    user_type = UserType.Admin;
                }
                break;

                case "staff":
                {
                    user_type = UserType.Staff;
                }
                break;
            }

            return user_type;
        }

        public static DateTime
        FromUnixEpoch(Dictionary<string, string> tags, string key)
        {
            DateTime time = DateTime.MinValue;

            if (!IsTagValid(tags, key))
            {
                return time;
            }

            if(!Int64.TryParse(tags[key], out long time_epoch))
            {
                return time;
            }

            time = time_epoch.FromUnixEpochMilliseconds();

            return time;
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

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool
        IsTagValid(Dictionary<string, string> tags, string key)
        {
            bool valid = true;

            if (!key.IsValid() || !tags.ContainsKey(key))
            {
                return false;
            }

            if (!tags[key].IsValid())
            {
                return false;
            }

            return valid;
        }
    }
}
