// standard namespaces
using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;

// project namespaces
using TwitchNet.Rest;
using TwitchNet.Rest.Api.Entitlements;
using TwitchNet.Rest.Api.Streams;
using TwitchNet.Rest.Api.Users;
using TwitchNet.Rest.Api.Videos;
using TwitchNet.Clients.Irc.Twitch;
using TwitchNet.Debugger;
using TwitchNet.Extensions;

namespace
TwitchNet.Utilities
{
    public static class
    EnumCacheUtil
    {
        private static readonly ConcurrentDictionary<Type, EnumTypeCache> CACHE = new ConcurrentDictionary<Type, EnumTypeCache>();

        private struct
        EnumResult
        {
            public  string name;

            public  object value;

            public Exception inner_exception;

            public void
            Throw(string message)
            {
                Exception exception = new Exception(message, inner_exception);

                throw exception;
            }

            public void
            Throw(string message, string param_name)
            {
                ArgumentException exception = new ArgumentException(message, param_name, inner_exception);

                throw exception;
            }
        }

        public readonly struct
        EnumTypeCache
        {
            #region Fields

            /// <summary>
            /// The enum's type.
            /// </summary>
            public readonly Type        type;

            /// <summary>
            /// The Enums's underlying type code.
            /// </summary>
            public readonly TypeCode    type_code;

            /// <summary>
            /// Whether or not the enum has the <see cref="FlagsAttribute"/> and is a collection of flags.
            /// </summary>
            public readonly bool        is_flags;

            /// <summary>
            /// The enum's default value.
            /// </summary>
            public readonly object      default_value;

            /// <summary>
            /// The native (unresolved) enum names.
            /// </summary>
            public readonly string[]    names;

            /// <summary>
            /// The native (unresolved) enum values.
            /// </summary>
            public readonly Array       values;

            /// <summary>
            /// <para>The resolved enum names.</para>
            /// <para>The names extraced from the <see cref="EnumMemberAttribute"/>, otherwise the native names.</para>
            /// </summary>
            public readonly string[]    resolved_names;

            /// <summary>
            /// The resolved enum values converted to <see cref="UInt64"/>.
            /// </summary>
            public readonly ulong[]     resolved_values;

            #endregion

            #region Constructors

            /// <summary>
            /// <para>Creates a new instance of the <see cref="EnumTypeCache"/> struct.</para>
            /// <para>Builds the cache for this enum.</para>
            /// </summary>
            /// <param name="type">The enum's type.</param>
            /// <exception cref="ArgumentNullException">Thrown if the <paramref name="type"/> is null.</exception>
            /// <exception cref="ArgumentException">
            /// Thrown if the <paramref name="type"/> is not an enum.
            /// </exception>
            public EnumTypeCache(Type type)
            {
                ExceptionUtil.ThrowIfNull(type, nameof(type));
                if (!type.IsEnum)
                {
                    throw new NotSupportedException("Type " + type.Name.WrapQuotes() + " is not an enum.");
                }

                this.type       = type;
                type_code       = Type.GetTypeCode(type);

                is_flags        = type.IsDefined(typeof(FlagsAttribute), false);

                default_value   = type.GetDefaultValue();

                names           = Enum.GetNames(type);
                values          = Enum.GetValues(type);

                resolved_names  = new string[names.Length];
                resolved_values = new ulong[names.Length];

                for (long index = 0; index < names.Length; ++index)
                {
                    object value = values.GetValue(index);
                    TryToUInt64(type_code, value, out resolved_values[index]);

                    FieldInfo field = type.GetField(names[index], BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
                    resolved_names[index] = field.TryGetAttribute(out EnumMemberAttribute attribute) ? attribute.Value : names[index];
                }
            }

            #endregion

            #region Methods

            /// <summary>
            /// <para>Gets the name of the specified enum's value.</para>
            /// <para>Supports bitfield enum values.</para>
            /// </summary>
            /// <param name="value">The enum value to get the name of.</param>
            /// <returns>
            /// Returns resolved name of the enum value.
            /// </returns>
            /// <exception cref="ArgumentNullException">Thrown if the specified value is null.</exception>
            /// <exception cref="ArgumentException">
            /// Thrown if no names or values exist in the enum.
            /// Thrown if the specified value cannot be converted to a UInt64.
            /// Thrown if one or more flags in the specified value could not be converted if the enum is a collection of flags.
            /// Thrown if the specified value could not be found in the enum type.
            /// </exception>
            public string
            GetName(object value)
            {
                EnumResult enum_result = new EnumResult();

                if(!TryGetNameInternal(value, ref enum_result))
                {
                    enum_result.Throw("The specified value could not be converted into an equivalent enum name of type " + type.Name.WrapQuotes() + ".", nameof(value));
                }

                return enum_result.name;
            }

            /// <summary>
            /// <para>Attempts to get the name of the specified enum's value.</para>
            /// <para>Supports bitfield enum values.</para>
            /// </summary>
            /// <param name="value">The enum value to get the name of.</param>
            /// <param name="result">
            /// Set to the resolved name, if successful.
            /// Set to null otherwise.
            /// </param>
            /// <returns>
            /// Returns true if the name was successfully retrieved.
            /// Returns false otherwise.
            /// </returns>
            public bool
            TryGetName(object value, out string result)
            {
                EnumResult enum_result = new EnumResult();

                bool success = TryGetNameInternal(value, ref enum_result);

                result = enum_result.name;

                return success;
            }

            /// <summary>
            /// <para>Attempts to get the name of the specified enum's value.</para>
            /// <para>Supports bitfield enum values.</para>
            /// </summary>
            /// <param name="value">The enum value to get the name of.</param>
            /// <param name="enum_result">The internal enum result used for error handling.</param>
            /// <returns>
            /// Returns true if the name was successfully retrieved.
            /// Returns false otherwise.
            /// </returns>
            private bool
            TryGetNameInternal(object value, ref EnumResult enum_result)
            {
                enum_result.name = null;

                if (value.IsNull())
                {
                    ArgumentNullException exception = new ArgumentNullException(nameof(value));
                    enum_result.inner_exception = exception;

                    return false;
                }

                if (!resolved_names.IsValid())
                {
                    ArgumentException exception = new ArgumentException("No names exist for the enum type " + type.Name.WrapQuotes() + ".");
                    enum_result.inner_exception = exception;

                    return false;
                }

                if (!resolved_values.IsValid())
                {
                    ArgumentException exception = new ArgumentException("No values exist for the enum type " + type.Name.WrapQuotes() + ".");
                    enum_result.inner_exception = exception;

                    return false;
                }

                TypeCode code = Type.GetTypeCode(value.GetType());
                if(!TryToUInt64(code, value, out ulong _value))
                {
                    ArgumentException exception = new ArgumentException("Failed to convert specified value " + value.ToString().WrapQuotes() + " to UInt64.");
                    enum_result.inner_exception = exception;

                    return false;
                }

                if (is_flags)
                {
                    enum_result.name = FormatValueAsFlags(_value);
                    if (!enum_result.name.IsNull())
                    {
                        return true;
                    }

                    ArgumentException exception = new ArgumentException("Failed to convert one or more flags in the specified value into a name for the enum type " + type.Name.WrapQuotes() + ".", nameof(value));
                    enum_result.inner_exception = exception;
                }
                else
                {
                    int index = Array.BinarySearch(resolved_values, _value);
                    if (index > -1)
                    {
                        enum_result.name = resolved_names[index];

                        return true;
                    }

                    index = Array.BinarySearch(resolved_values, _value);
                    if (index > -1)
                    {
                        enum_result.name = names[index];

                        return true;
                    }

                    ArgumentException exception = new ArgumentException("Could not find the specified value " + value.ToString().WrapQuotes() + " in the enum type " + type.Name.WrapQuotes() + ".", nameof(value));
                    enum_result.inner_exception = exception;
                }

                return false;
            }

            /// <summary>
            /// <para>Converts a string representation of an enum value into the equivalent constant enum value.</para>
            /// <para>Supports bitfield formatted strings.</para>
            /// </summary>
            /// <param name="value">
            /// <para>The string to convert.</para>
            /// <para>This can either be the resolved or native (unresolved) name.</para>
            /// </param>
            /// <returns>Returns the equivalent constant enum value of its string representation.</returns>
            /// <exception cref="ArgumentException">Thrown if the string value could not be converted into the equivalent constant enum value.</exception>
            public object
            Parse(string value)
            {
                return Parse(value, false);
            }

            /// <summary>
            /// <para>Converts a string representation of an enum value into the equivalent constant enum value.</para>
            /// <para>Supports bitfield formatted strings.</para>
            /// </summary>
            /// <param name="value">
            /// <para>The string to convert.</para>
            /// <para>This can either be the resolved or native (unresolved) name.</para>
            /// </param>
            /// <param name="ignore_case">Whether or not to ignore string case when parsing.</param>
            /// <returns>Returns the equivalent constant enum value of its string representation.</returns>
            /// <exception cref="ArgumentException">Thrown if the string value could not be converted into the equivalent constant enum value.</exception>
            public object
            Parse(string value, bool ignore_case)
            {
                if (!TryParse(value, ignore_case, out object result))
                {
                    throw new ArgumentException("Could not convert " + value.WrapQuotes() + " into an enum member of type " + type.Name.WrapQuotes() + ".");
                }

                return result;
            }

            /// <summary>
            /// <para>Attempts to convert a string representation of an enum value into the equivalent constant enum value.</para>
            /// <para>Supports bitfield formatted strings.</para>
            /// </summary>
            /// <param name="value">
            /// <para>The string to convert.</para>
            /// <para>This can either be the resolved or native (unresolved) name.</para>
            /// </param>
            /// <param name="result">
            /// Set to the equivalent constant enum value, if successful.
            /// Set to the enum's default value otherwise.
            /// </param>
            /// <returns>
            /// Returns true if the string value was successfully parsed.
            /// Returns false otherwise.
            /// </returns>
            public bool
            TryParse(string value, out object result)
            {
                return TryParse(value, false, out result);
            }

            /// <summary>
            /// <para>Attempts to convert a string representation of an enum value into the equivalent constant enum value.</para>
            /// <para>Supports bitfield formatted strings.</para>
            /// </summary>
            /// <param name="value">
            /// <para>The string to convert.</para>
            /// <para>This can either be the resolved or native (unresolved) name.</para>
            /// </param>
            /// <param name="ignore_case">Whether or not to ignore string case when parsing.</param>
            /// <param name="result">
            /// Set to the equivalent constant enum value, if successful.
            /// Set to the enum's default value otherwise.
            /// </param>
            /// <returns>
            /// Returns true if the string value was successfully parsed.
            /// Returns false otherwise.
            /// </returns>
            public bool
            TryParse(string value, bool ignore_case, out object result)
            {
                result = default_value;

                if (value.IsNull())
                {
                    return false;
                }

                StringComparison comparison = ignore_case ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal;

                // Search by matching the exact value against the resolved and unresolved names
                int index = FindValueIndexByName(value, comparison);
                if (index != -1)
                {
                    result = Enum.ToObject(type, resolved_values[index]);

                    return true;
                }

                // Search by getting the value directly if it's a number
                value = value.Trim();
                if (UInt64.TryParse(value, out ulong _result))
                {
                    result = Enum.ToObject(type, _result);

                    return true;
                }

                if (!is_flags)
                {
                    return false;
                }

                // If we get here, the only other option is that the value is a bitfield
                ulong bitfield_result = 0;

                string[] elements = value.Split(',');
                foreach (string element in elements)
                {
                    index = FindValueIndexByName(element.Trim(), comparison);
                    if (index != -1)
                    {
                        bitfield_result |= resolved_values[index];

                        continue;
                    }
                    else
                    {
                        break;
                    }
                }

                if (index != -1)
                {
                    result = Enum.ToObject(type, bitfield_result);

                    return true;
                }

                return false;
            }

            #endregion

            #region Helpers                        

            /// <summary>
            /// Formats the <see cref="UInt64"/> equivalent of an enum value into a bitfield formatted string.
            /// </summary>
            /// <param name="value">The equivalent of an enum value.</param>
            /// <returns>
            /// Returns the resolved bitfield formatted string if successful.
            /// Returns the resolved default enum value if no flags were set in the specified value.
            /// Returns null otherwise.
            /// </returns>
            private string
            FormatValueAsFlags(ulong value)
            {
                string result = null;

                // Case 1: The value had no set flags to begin with. "Failed" conversion.
                if (value == 0)
                {
                    return resolved_values.IsValid() && resolved_values[0] == 0 ? resolved_names[0] : result;
                }

                bool first_flag = true;

                StringBuilder _result = new StringBuilder();

                // Since enums are sorted, search for flags in reverse order so we don't start with "0".
                int index = resolved_values.Length - 1;
                while (index >= 0)
                {
                    // We reached the end of all possible resolved flags or the default value.
                    // All possible flags that could be accounted for have already been accounted for.
                    if (index == 0 && resolved_values[index] == 0)
                    {
                        break;
                    }

                    if ((value & resolved_values[index]) == resolved_values[index])
                    {
                        // Flip the bit to show that the flag was successfully accounted for.
                        value ^= resolved_values[index];

                        if (!first_flag)
                        {
                            _result.Insert(0, ", ");
                        }
                        _result.Insert(0, resolved_names[index]);

                        first_flag = false;
                    }

                    index--;
                }

                // Case 2: At least one flag in the value could not be found in the resolved enum values and could not be fully converted. Failed/incomplete converison.
                if (value != 0)
                {
                    result = null;
                }
                // Case 3: All flags in the value were found in the resolved enum values. Successful converison.
                else
                {
                    result = _result.ToString();
                }

                return result;
            }

            /// <summary>
            /// Finds the index of the enum value in the resolved value array that corresponds to the specified enum name.
            /// </summary>
            /// <param name="name">The resolved or native (unresolved) enum name to search for.</param>
            /// <param name="comparison">One of the enumeration values that specifies the rules to use in the comparison.</param>
            /// <returns>
            /// Returns the index of the enum value in the reolved value array if a match was found.
            /// Returns -1 otherwise.
            /// </returns>
            private int
            FindValueIndexByName(string name, StringComparison comparison)
            {
                for (int index = 0; index < resolved_names.Length; ++index)
                {
                    if (string.Compare(resolved_names[index], name, comparison) == 0)
                    {
                        return index;
                    }
                }

                for (int index = 0; index < names.Length; ++index)
                {
                    if (string.Compare(names[index], name, comparison) == 0)
                    {
                        return index;
                    }
                }

                return -1;
            }

            #endregion
        }

        private static void
        Benchmark_FromStreamLanguage_Single()
        {
            StreamLanguage language = StreamLanguage.EnGb;
            EnumTypeCache cache = CACHE.GetOrAdd(typeof(StreamLanguage), CreateCache);

            cache.TryGetName(language, out string name);
        }

        private static void
        Benchmark_FromStreamLanguage_Flags()
        {
            StreamLanguage language = StreamLanguage.EnGb | StreamLanguage.Ar | StreamLanguage.ZhTw | (StreamLanguage)(1 << 58);
            EnumTypeCache cache = CACHE.GetOrAdd(typeof(StreamLanguage), CreateCache);

            cache.TryGetName(language, out string name);
        }

        private static void
        Benchmark_ToStreamLanguage_Single()
        {
            string name = "en-gb";
            EnumTypeCache cache = CACHE.GetOrAdd(typeof(StreamLanguage), CreateCache);

            cache.TryParse(name, out object value);
            StreamLanguage language = (StreamLanguage)value;
        }

        #region Client enum caches

        private static readonly
        Dictionary<string, BadgeType>               CACHE_TO_BADGE_TYPE                     = new Dictionary<string, BadgeType>
        {
            { "",                           BadgeType.None                   },

            { "admin",                      BadgeType.Admin                  },
            { "bits",                       BadgeType.Bits                   },
            { "broadcaster",                BadgeType.Broadcaster            },
            { "global_mod",                 BadgeType.GlobalMod              },
            { "moderator",                  BadgeType.Moderator              },
            { "subscriber",                 BadgeType.Subscriber             },
            { "staff",                      BadgeType.Staff                  },
            { "premium",                    BadgeType.Premium                },
            { "turbo",                      BadgeType.Turbo                  },
            { "partner",                    BadgeType.Partner                },

            { "sub-gifter",                 BadgeType.SubGifter              },
            { "clip-champ",                 BadgeType.ClipChamp              },
            { "twitchcon2017",              BadgeType.Twitchcon2017          },
            { "overwatch-league-insider_1", BadgeType.OverwatchLeagueInsider },
        };

        private static readonly
        Dictionary<BadgeType, string>               CACHE_FROM_BADGE_TYPE                   = new Dictionary<BadgeType, string>
        {
            { BadgeType.None,                   ""                           },

            { BadgeType.Admin,                  "admin"                      },
            { BadgeType.Bits,                   "bits"                       },
            { BadgeType.Broadcaster,            "broadcaster"                },
            { BadgeType.GlobalMod,              "global_mod"                 },
            { BadgeType.Moderator,              "moderator"                  },
            { BadgeType.Subscriber,             "moderator"                  },
            { BadgeType.Staff,                  "staff"                      },
            { BadgeType.Premium,                "premium"                    },
            { BadgeType.Turbo,                  "turbo"                      },
            { BadgeType.Partner,                "partner"                    },

            { BadgeType.SubGifter,              "sub-gifter" },
            { BadgeType.ClipChamp,              "clip-champ"                 },
            { BadgeType.Twitchcon2017,          "twitchcon2017"              },
            { BadgeType.OverwatchLeagueInsider, "overwatch-league-insider_1" },
        };

        private static readonly
        Dictionary<string, ChatCommand>             CACHE_TO_CHAT_COMMAND                   = new Dictionary<string, ChatCommand>
        {
            { "",                   ChatCommand.Other },

            { "/color",             ChatCommand.Color          },
            { "/disconnect",        ChatCommand.Disconnect     },
            { "/help",              ChatCommand.Help           },
            { "/me",                ChatCommand.Me             },
            { "/mods",              ChatCommand.Mods           },
            { "/w",                 ChatCommand.Whisper        },

            { "/ban",               ChatCommand.Ban            },
            { "/mod",               ChatCommand.Mod            },
            { "/timeout",           ChatCommand.Timeout        },
            { "/unban",             ChatCommand.Unban          },
            { "/unmod",             ChatCommand.Unmod          },
            { "/untimeout",         ChatCommand.Untimeout      },

            { "/clear",             ChatCommand.Clear          },
            { "/emoteonly",         ChatCommand.EmoteOnly      },
            { "/emoteonlyoff",      ChatCommand.EmoteOnlyOff   },
            { "/followers",         ChatCommand.Followers      },
            { "/followersoff",      ChatCommand.FollowersOff   },
            { "/r9kbeta",           ChatCommand.R9kBeta        },
            { "/r9kbetaoff",        ChatCommand.R9kBetaOff     },
            { "/slow",              ChatCommand.Slow           },
            { "/slowoff",           ChatCommand.SlowOff        },
            { "/subscribers",       ChatCommand.Subscribers    },
            { "/Subscribersoff",    ChatCommand.SubscribersOff },

            { "/commercial",        ChatCommand.Commercial     },
            { "/host",              ChatCommand.Host           },
            { "/raid",              ChatCommand.Raid           },
            { "/unhost",            ChatCommand.Unhost         },
            { "/unraid",            ChatCommand.Unraid         },
        };

        private static readonly
        Dictionary<ChatCommand, string>             CACHE_FROM_CHAT_COMMAND                 = new Dictionary<ChatCommand, string>
        {
            { ChatCommand.Other,            ""                },

            { ChatCommand.Color,            "/color"          },
            { ChatCommand.Disconnect,       "/disconnect"     },
            { ChatCommand.Help,             "/help"           },
            { ChatCommand.Me,               "/me"             },
            { ChatCommand.Mods,             "/mods"           },
            { ChatCommand.Whisper,          "/w"              },

            { ChatCommand.Ban,              "/ban"            },
            { ChatCommand.Mod,              "/mod"            },
            { ChatCommand.Timeout,          "/timeout"        },
            { ChatCommand.Unban,            "/unban"          },
            { ChatCommand.Unmod,            "/unmod"          },
            { ChatCommand.Untimeout,        "/untimeout"      },

            { ChatCommand.Clear,            "/clear"          },
            { ChatCommand.EmoteOnly,        "/emoteonly"      },
            { ChatCommand.EmoteOnlyOff,     "/emoteonlyoff"   },
            { ChatCommand.Followers,        "/followers"      },
            { ChatCommand.FollowersOff,     "/followersoff"   },
            { ChatCommand.R9kBeta,          "/r9kbeta"        },
            { ChatCommand.R9kBetaOff,       "/r9kbetaoff"     },
            { ChatCommand.Slow,             "/slow"           },
            { ChatCommand.SlowOff,          "/slowoff"        },
            { ChatCommand.Subscribers,      "/subscribers"    },
            { ChatCommand.SubscribersOff,   "/Subscribersoff" },

            { ChatCommand.Commercial,       "/commercial"     },
            { ChatCommand.Host,             "/host"           },
            { ChatCommand.Raid,             "/raid"           },
            { ChatCommand.Unhost,           "/unhost"         },
            { ChatCommand.Unraid,           "/unraid"         },
        };

        private static readonly
        Dictionary<string, CommercialLength>        CACHE_TO_COMMERCIAL_LENGTH              = new Dictionary<string, CommercialLength>
        {
            { "",       CommercialLength.Other      },

            { "30",     CommercialLength.Seconds30  },
            { "60",     CommercialLength.Seconds60  },
            { "90",     CommercialLength.Seconds90  },
            { "120",    CommercialLength.Seconds120 },
            { "150",    CommercialLength.Seconds150 },
            { "180",    CommercialLength.Seconds180 },
        };

        private static readonly
        Dictionary<CommercialLength, string>        CACHE_FROM_COMMERCIAL_LENGTH            = new Dictionary<CommercialLength, string>
        {
            { CommercialLength.Other,       ""    },

            { CommercialLength.Seconds30,   "30"  },
            { CommercialLength.Seconds60,   "60"  },
            { CommercialLength.Seconds90,   "90"  },
            { CommercialLength.Seconds120,  "120" },
            { CommercialLength.Seconds150,  "150" },
            { CommercialLength.Seconds180,  "180" },
        };

        private static readonly
        Dictionary<string, DisplayNameColor>        CACHE_TO_DISPLAY_NAME_COLOR             = new Dictionary<string, DisplayNameColor>
        {
            { "",               DisplayNameColor.Other       },

            { "blue",           DisplayNameColor.Blue        },
            { "blueviolet",     DisplayNameColor.BlueViolet  },
            { "cadetblue",      DisplayNameColor.CadetBlue   },
            { "chocloate",      DisplayNameColor.Chocloate   },
            { "coral",          DisplayNameColor.Coral       },
            { "dodgerblue",     DisplayNameColor.DodgerBlue  },
            { "firebrick",      DisplayNameColor.FireBrick   },
            { "goldenrod",      DisplayNameColor.GoldenRod   },
            { "green",          DisplayNameColor.Green       },
            { "hotpink",        DisplayNameColor.HotPink     },
            { "orangered",      DisplayNameColor.OrangeRed   },
            { "red",            DisplayNameColor.Red         },
            { "seagreen",       DisplayNameColor.SeaGreen    },
            { "springgreen",    DisplayNameColor.SpringGreen },
            { "yellowgreen",    DisplayNameColor.YellowGreen },
        };

        private static readonly
        Dictionary<DisplayNameColor, string>        CACHE_FROM_DISPLAY_NAME_COLOR           = new Dictionary<DisplayNameColor, string>
        {
            { DisplayNameColor.Other,       ""            },

            { DisplayNameColor.Blue,        "blue"        },
            { DisplayNameColor.BlueViolet,  "blueviolet"  },
            { DisplayNameColor.CadetBlue,   "cadetblue"   },
            { DisplayNameColor.Chocloate,   "chocloate"   },
            { DisplayNameColor.Coral,       "coral"       },
            { DisplayNameColor.DodgerBlue,  "dodgerblue"  },
            { DisplayNameColor.FireBrick,   "firebrick"   },
            { DisplayNameColor.GoldenRod,   "goldenrod"   },
            { DisplayNameColor.Green,       "green"       },
            { DisplayNameColor.HotPink,     "hotpink"     },
            { DisplayNameColor.OrangeRed,   "orangered"   },
            { DisplayNameColor.Red,         "red"         },
            { DisplayNameColor.SeaGreen,    "seagreen"    },
            { DisplayNameColor.SpringGreen, "springgreen" },
            { DisplayNameColor.YellowGreen, "yellowgreen" },
        };

        private static readonly
        Dictionary<string, FollowersDurationPeriod> CACHE_TO_FOLLOWERS_DURATION_PERIOD      = new Dictionary<string, FollowersDurationPeriod>
        {
            { "",           FollowersDurationPeriod.Other  },

            { "mo",         FollowersDurationPeriod.Months  },
            { "month",      FollowersDurationPeriod.Months  },
            { "months",     FollowersDurationPeriod.Months  },

            { "w",          FollowersDurationPeriod.Weeks   },
            { "week",       FollowersDurationPeriod.Weeks   },
            { "weeks",      FollowersDurationPeriod.Weeks   },

            { "d",          FollowersDurationPeriod.Days    },
            { "day",        FollowersDurationPeriod.Days    },
            { "days",       FollowersDurationPeriod.Days    },

            { "h",          FollowersDurationPeriod.Hours   },
            { "hour",       FollowersDurationPeriod.Hours   },
            { "hours",      FollowersDurationPeriod.Hours   },

            { "m",          FollowersDurationPeriod.Minutes },
            { "minute",     FollowersDurationPeriod.Minutes },
            { "minutes",    FollowersDurationPeriod.Minutes },

            { "s",          FollowersDurationPeriod.Seconds },
            { "second",     FollowersDurationPeriod.Seconds },
            { "seconds",    FollowersDurationPeriod.Seconds },
        };

        private static readonly
        Dictionary<FollowersDurationPeriod, string> CACHE_FROM_FOLLOWERS_DURATION_PERIOD    = new Dictionary<FollowersDurationPeriod, string>
        {
            { FollowersDurationPeriod.Other,   ""         },

            { FollowersDurationPeriod.Months,   "months"  },
            { FollowersDurationPeriod.Weeks,    "weeks"   },
            { FollowersDurationPeriod.Days,     "days"    },
            { FollowersDurationPeriod.Hours,    "hours"   },
            { FollowersDurationPeriod.Minutes,  "minutes" },
            { FollowersDurationPeriod.Seconds,  "seconds" },
        };

        private static readonly
        Dictionary<string, NoticeType>              CACHE_TO_NOTICE_TYPE                    = new Dictionary<string, NoticeType>
        {
            { "",                           NoticeType.Other                   },

            { "already_banned",             NoticeType.AlreadyBanned           },
            { "already_emote_only_off",     NoticeType.AlreadyEmoteOnlyOff     },
            { "already_emote_only_on",      NoticeType.AlreadyEmoteOnlyOn      },
            { "already_r9k_off",            NoticeType.AlreadyR9kOff           },
            { "already_r9k_on",             NoticeType.AlreadyR9kOn            },
            { "already_subs_off",           NoticeType.AlreadySubsOff          },
            { "already_subs_on",            NoticeType.AlreadySubsOn           },

            { "bad_commercial_error",       NoticeType.BadCommercialError      },
            { "bad_host_hosting",           NoticeType.BadHostHosting          },
            { "bad_host_rate_exceeded",     NoticeType.BadHostRateExceeded     },
            { "bad_mod_mod",                NoticeType.BadModMod               },
            { "bad_slow_duration",          NoticeType.BadSlowDuration         },
            { "bad_unmod_mod",              NoticeType.BadUnmodMod             },
            { "ban_success",                NoticeType.BanSuccess              },
            { "bad_unban_no_ban",           NoticeType.BadUnbanNoBan           },

            { "cmds_available",             NoticeType.CmdsAvailable           },
            { "color_changed",              NoticeType.ColorChanged            },
            { "emote_only_off",             NoticeType.EmoteOnlyOff            },
            { "emote_only_on",              NoticeType.EmoteOnlyOn             },
            { "host_off",                   NoticeType.HostOff                 },
            { "host_on",                    NoticeType.HostOn                  },
            { "hosts_remaining",            NoticeType.HostsRemaining          },
            { "invalid_user",               NoticeType.InvalidUser             },
            { "mod_success",                NoticeType.ModSuccess              },
            { "msg_channel_suspended",      NoticeType.MsgChannelSuspended     },
            { "msg_room_not_found",         NoticeType.MsgRoomNotFound         },
            { "no_help",                    NoticeType.NoHelp                  },
            { "no_permission",              NoticeType.NoPermission            },
            { "r9k_off",                    NoticeType.R9kOff                  },
            { "r9k_on",                     NoticeType.R9kOn                   },
            { "room_mods",                  NoticeType.RoomMods                },
            { "slow_off",                   NoticeType.SlowOff                 },
            { "slow_on",                    NoticeType.SlowOn                  },
            { "subs_off",                   NoticeType.SubsOff                 },
            { "subs_on",                    NoticeType.SubsOn                  },
            { "timeout_success",            NoticeType.TimeoutSuccess          },
            { "turbo_only_color",           NoticeType.TurboOnlyColor          },
            { "unban_success",              NoticeType.UnbanSuccess            },
            { "unmod_success",              NoticeType.UnmodSuccess            },
            { "unrecognized_cmd",           NoticeType.UnrecognizedCmd         },

            { "usage_color",                NoticeType.UsageColor              },
            { "usage_disconnect",           NoticeType.UsageDisconnect         },
            { "usage_help",                 NoticeType.UsageHelp               },
            { "usage_me",                   NoticeType.UsageMe                 },
            { "usage_mods",                 NoticeType.UsageMods               },
            { "usage_ban",                  NoticeType.UsageBan                },
            { "usage_timeout",              NoticeType.UsageTimeout            },
            { "usage_unban",                NoticeType.UsageUnban              },
            { "usage_untimeout",            NoticeType.UsageUntimeout          },
            { "usage_clear",                NoticeType.UsageClear              },
            { "usage_emote_only_on",        NoticeType.UsageEmoteOnlyOn        },
            { "usage_emote_only_off",       NoticeType.UsageEmoteOnlyOff       },
            { "usage_followers_on",         NoticeType.UsageFollowersOn        },
            { "usage_followers_off",        NoticeType.UsageFollowersOff       },
            { "usage_r9k_on",               NoticeType.UsageR9kOn              },
            { "usage_slow_on",              NoticeType.UsageSlowOn             },
            { "usage_slow_off",             NoticeType.UsageSlowOff            },
            { "usage_subs_on",              NoticeType.UsageSubsOn             },
            { "usage_subs_off",             NoticeType.UsageSubsOff            },
            { "usage_commercial",           NoticeType.UsageCommercial         },
            { "usage_host",                 NoticeType.UsageHost               },
            { "usage_unhost",               NoticeType.UsageUnhost             },

            { "unsupported_chatrooms_cmd",  NoticeType.UnsupportedChatRoomsCmd },
            { "whisper_invalid_args",       NoticeType.WhisperInvalidArgs      },
        };

        private static readonly
        Dictionary<NoticeType, string>              CACHE_FROM_NOTICE_TYPE                  = new Dictionary<NoticeType, string>
        {
            { NoticeType.Other,                     ""                          },

            { NoticeType.AlreadyBanned,             "already_banned"            },
            { NoticeType.AlreadyEmoteOnlyOff,       "already_emote_only_off"    },
            { NoticeType.AlreadyEmoteOnlyOn,        "already_emote_only_on"     },
            { NoticeType.AlreadyR9kOff,             "already_r9k_off"           },
            { NoticeType.AlreadyR9kOn,              "already_r9k_on"            },
            { NoticeType.AlreadySubsOff,            "already_subs_off"          },
            { NoticeType.AlreadySubsOn,             "already_subs_on"           },

            { NoticeType.BadCommercialError,        "bad_commercial_error"      },
            { NoticeType.BadHostHosting,            "bad_host_hosting"          },
            { NoticeType.BadHostRateExceeded,       "bad_host_rate_exceeded"    },
            { NoticeType.BadModMod,                 "bad_mod_mod"               },
            { NoticeType.BadSlowDuration,           "bad_slow_duration"         },
            { NoticeType.BadUnmodMod,               "bad_unmod_mod"             },
            { NoticeType.BanSuccess,                "ban_success"               },
            { NoticeType.BadUnbanNoBan,             "bad_unban_no_ban"          },

            { NoticeType.CmdsAvailable,             "cmds_available"            },
            { NoticeType.ColorChanged,              "color_changed"             },
            { NoticeType.EmoteOnlyOff,              "emote_only_off"            },
            { NoticeType.EmoteOnlyOn,               "emote_only_on"             },
            { NoticeType.HostOff,                   "host_off"                  },
            { NoticeType.HostOn,                    "host_on"                   },
            { NoticeType.HostsRemaining,            "hosts_remaining"           },
            { NoticeType.InvalidUser,               "invalid_user"              },
            { NoticeType.ModSuccess,                "mod_success"               },
            { NoticeType.MsgChannelSuspended,       "msg_channel_suspended"     },
            { NoticeType.MsgRoomNotFound,           "msg_room_not_found"        },
            { NoticeType.NoHelp,                    "no_help"                   },
            { NoticeType.NoPermission,              "no_permission"             },
            { NoticeType.R9kOff,                    "r9k_off"                   },
            { NoticeType.R9kOn,                     "r9k_on"                    },
            { NoticeType.RoomMods,                  "room_mods"                 },
            { NoticeType.SlowOff,                   "slow_off"                  },
            { NoticeType.SlowOn,                    "slow_on"                   },
            { NoticeType.SubsOff,                   "subs_off"                  },
            { NoticeType.SubsOn,                    "subs_on"                   },
            { NoticeType.TimeoutSuccess,            "timeout_success"           },
            { NoticeType.TurboOnlyColor,            "turbo_only_color"          },
            { NoticeType.UnbanSuccess,              "unban_success"             },
            { NoticeType.UnmodSuccess,              "unmod_success"             },
            { NoticeType.UnrecognizedCmd,           "unrecognized_cmd"          },

            { NoticeType.UsageColor,                "usage_color"               },
            { NoticeType.UsageDisconnect,           "usage_disconnect"          },
            { NoticeType.UsageHelp,                 "usage_help"                },
            { NoticeType.UsageMe,                   "usage_me"                  },
            { NoticeType.UsageMods,                 "usage_mods"                },
            { NoticeType.UsageBan,                  "usage_ban"                 },
            { NoticeType.UsageTimeout,              "usage_timeout"             },
            { NoticeType.UsageUnban,                "usage_unban"               },
            { NoticeType.UsageUntimeout,            "usage_untimeout"           },
            { NoticeType.UsageClear,                "usage_clear"               },
            { NoticeType.UsageEmoteOnlyOn,          "usage_emote_only_on"       },
            { NoticeType.UsageEmoteOnlyOff,         "usage_emote_only_off"      },
            { NoticeType.UsageFollowersOn,          "usage_followers_on"        },
            { NoticeType.UsageFollowersOff,         "usage_followers_off"       },
            { NoticeType.UsageR9kOn,                "usage_r9k_on"              },
            { NoticeType.UsageSlowOn,               "usage_slow_on"             },
            { NoticeType.UsageSlowOff,              "usage_slow_off"            },
            { NoticeType.UsageSubsOn,               "usage_subs_on"             },
            { NoticeType.UsageSubsOff,              "usage_subs_off"            },
            { NoticeType.UsageCommercial,           "usage_commercial"          },
            { NoticeType.UsageHost,                 "usage_host"                },
            { NoticeType.UsageUnhost,               "usage_unhost"              },

            { NoticeType.UnsupportedChatRoomsCmd,   "unsupported_chatrooms_cmd" },
            { NoticeType.WhisperInvalidArgs,        "whisper_invalid_args"      },
        };

        private static readonly
        Dictionary<string, RitualType>              CACHE_TO_RITUAL_TYPE                    = new Dictionary<string, RitualType>
        {
            { "",               RitualType.Other      },

            { "new_chatter",    RitualType.NewChatter },
        };

        private static readonly
        Dictionary<RitualType, string>              CACHE_FROM_RITUAL_TYPE                  = new Dictionary<RitualType, string>
        {
            { RitualType.Other,         ""            },

            { RitualType.NewChatter,    "new_chatter" },
        };

        private static readonly
        Dictionary<string, SubscriptionPlan>        CACHE_TO_SUBSCRIPTION_PLAN              = new Dictionary<string, SubscriptionPlan>
        {
            { "",       SubscriptionPlan.Other },

            { "Prime",  SubscriptionPlan.Prime },
            { "1000",   SubscriptionPlan.Tier1 },
            { "2000",   SubscriptionPlan.Tier2 },
            { "3000",   SubscriptionPlan.Tier3 },
        };

        private static readonly
        Dictionary<SubscriptionPlan, string>        CACHE_FROM_SUBSCRIPTION_PLAN            = new Dictionary<SubscriptionPlan, string>
        {
            { SubscriptionPlan.Other, ""      },

            { SubscriptionPlan.Prime, "Prime" },
            { SubscriptionPlan.Tier1, "1000"  },
            { SubscriptionPlan.Tier2, "2000"  },
            { SubscriptionPlan.Tier3, "3000"  },
        };

        private static readonly
        Dictionary<string, UserNoticeType>          CACHE_TO_USER_NOTICE_TYPE               = new Dictionary<string, UserNoticeType>
        {
            { "",           UserNoticeType.Other  },

            { "sub",        UserNoticeType.Sub    },
            { "resub",      UserNoticeType.Resub  },
            { "giftsub",    UserNoticeType.Resub  },
            { "raid",       UserNoticeType.Raid   },
            { "ritual",     UserNoticeType.Ritual },
        };

        private static readonly
        Dictionary<UserNoticeType, string>          CACHE_FROM_USER_NOTICE_TYPE             = new Dictionary<UserNoticeType, string>
        {
            { UserNoticeType.Other,     ""        },

            { UserNoticeType.Sub,       "sub"     },
            { UserNoticeType.Resub,     "resub"   },
            { UserNoticeType.GiftSub,     "giftsub" },
            { UserNoticeType.Raid,      "raid"    },
            { UserNoticeType.Ritual,    "ritual"  },
        };

        #endregion

        #region Rest enum caches         

        private static readonly
        Dictionary<string, EntitlementType>         CACHE_TO_ENTITLEMENTR_TYPE              = new Dictionary<string, EntitlementType>
        {
            { "bulk_drops_grant", EntitlementType.BulkDropsGrant }
        };

        private static readonly
        Dictionary<EntitlementType, string>         CACHE_FROM_ENTITLEMENTR_TYPE            = new Dictionary<EntitlementType, string>
        {
            { EntitlementType.BulkDropsGrant, "bulk_drops_grant" }
        };

        private static readonly
        Dictionary<string, StreamLanguage>          CACHE_TO_STREAM_LANGUAGE                = new Dictionary<string, StreamLanguage>()
        {
            { "",       StreamLanguage.None },

            { "da",     StreamLanguage.Da   },
            { "de",     StreamLanguage.De   },
            { "en",     StreamLanguage.En   },
            { "en-gb",  StreamLanguage.EnGb },
            { "es",     StreamLanguage.Es   },
            { "es-mx",  StreamLanguage.EsMx },
            { "fr",     StreamLanguage.Fr   },
            { "it",     StreamLanguage.It   },
            { "hu",     StreamLanguage.Hu   },
            { "nl",     StreamLanguage.Nl   },
            { "no",     StreamLanguage.No   },
            { "pl",     StreamLanguage.Pl   },
            { "pt",     StreamLanguage.Pt   },
            { "pt-br",  StreamLanguage.PtBr },
            { "sk",     StreamLanguage.Sk   },
            { "fi",     StreamLanguage.Fi   },
            { "sv",     StreamLanguage.Sv   },
            { "vi",     StreamLanguage.Vi   },
            { "tr",     StreamLanguage.Tr   },
            { "cs",     StreamLanguage.Cs   },
            { "el",     StreamLanguage.El   },
            { "bg",     StreamLanguage.Bg   },
            { "ru",     StreamLanguage.Ru   },
            { "ar",     StreamLanguage.Ar   },
            { "th",     StreamLanguage.Th   },
            { "zh-cn",  StreamLanguage.ZhCn },
            { "zh-tw",  StreamLanguage.ZhTw },
            { "ja",     StreamLanguage.Ja   },
            { "ko",     StreamLanguage.Ko   },
        };

        private static readonly
        Dictionary<StreamLanguage, string>          CACHE_FROM_STREAM_LANGUAGE              = new Dictionary<StreamLanguage, string>()
        {
            { StreamLanguage.None,  ""      },

            { StreamLanguage.Da,    "da"    },
            { StreamLanguage.De,    "de"    },
            { StreamLanguage.En,    "en"    },
            { StreamLanguage.EnGb,  "en-gb" },
            { StreamLanguage.Es,    "es"    },
            { StreamLanguage.EsMx,  "es-mx" },
            { StreamLanguage.Fr,    "fr"    },
            { StreamLanguage.It,    "it"    },
            { StreamLanguage.Hu,    "hu"    },
            { StreamLanguage.Nl,    "nl"    },
            { StreamLanguage.No,    "no"    },
            { StreamLanguage.Pl,    "pl"    },
            { StreamLanguage.Pt,    "pt"    },
            { StreamLanguage.PtBr,  "pt-br" },
            { StreamLanguage.Sk,    "sk"    },
            { StreamLanguage.Fi,    "fi"    },
            { StreamLanguage.Sv,    "sv"    },
            { StreamLanguage.Vi,    "vi"    },
            { StreamLanguage.Tr,    "tr"    },
            { StreamLanguage.Cs,    "cs"    },
            { StreamLanguage.El,    "el"    },
            { StreamLanguage.Bg,    "bg"    },
            { StreamLanguage.Ru,    "ru"    },
            { StreamLanguage.Ar,    "ar"    },
            { StreamLanguage.Th,    "th"    },
            { StreamLanguage.ZhCn,  "zh-cn" },
            { StreamLanguage.ZhTw,  "zh-tw" },
            { StreamLanguage.Ja,    "ja"    },
            { StreamLanguage.Ko,    "ko"    },
        };

        private static readonly
        Dictionary<string, StreamType>              CACHE_TO_STREAM_TYPE                    = new Dictionary<string, StreamType>()
        {
            { "",           StreamType.Other   },

            { "live",       StreamType.Live    },
            { "vodcast",    StreamType.Vodcast },
            { "all",        StreamType.All     },
        };

        private static readonly
        Dictionary<StreamType, string>              CACHE_FROM_STREAM_TYPE                  = new Dictionary<StreamType, string>()
        {
            { StreamType.Other,     ""        },

            { StreamType.Live,      "live"    },
            { StreamType.Vodcast,   "vodcast" },
            { StreamType.All,       "all"     },
        };

        private static readonly
        Dictionary<string, BroadcasterType>         CACHE_TO_BROADCASTER_TYPE               = new Dictionary<string, BroadcasterType>()
        {
            { "",           BroadcasterType.Empty     },

            { "partner",    BroadcasterType.Partner   },
            { "affiliate",  BroadcasterType.Affiliate },
        };

        private static readonly
        Dictionary<BroadcasterType, string>         CACHE_FROM_BROADCASTER_TYPE             = new Dictionary<BroadcasterType, string>()
        {
            { BroadcasterType.Empty,        ""          },

            { BroadcasterType.Partner,      "partner"   },
            { BroadcasterType.Affiliate,    "affiliate" },
        };

        private static readonly
        Dictionary<string, BroadcasterLanguage>     CACHE_TO_BROADCASTER_LANGUAGE           = new Dictionary<string, BroadcasterLanguage>
        {
            { "",       BroadcasterLanguage.None  },

            { "en",     BroadcasterLanguage.En    },
            { "da",     BroadcasterLanguage.Da    },
            { "de",     BroadcasterLanguage.De    },
            { "es",     BroadcasterLanguage.Es    },
            { "fr",     BroadcasterLanguage.Fr    },
            { "it",     BroadcasterLanguage.It    },
            { "hu",     BroadcasterLanguage.Hu    },
            { "nl",     BroadcasterLanguage.Nl    },
            { "no",     BroadcasterLanguage.No    },
            { "pl",     BroadcasterLanguage.Pl    },
            { "pt",     BroadcasterLanguage.Pt    },
            { "sk",     BroadcasterLanguage.Sk    },
            { "fi",     BroadcasterLanguage.Fi    },
            { "sv",     BroadcasterLanguage.Sv    },
            { "vi",     BroadcasterLanguage.Vi    },
            { "tr",     BroadcasterLanguage.Tr    },
            { "cs",     BroadcasterLanguage.Cs    },
            { "el",     BroadcasterLanguage.El    },
            { "bg",     BroadcasterLanguage.Bg    },
            { "ru",     BroadcasterLanguage.Ru    },
            { "ar",     BroadcasterLanguage.Ar    },
            { "th",     BroadcasterLanguage.Th    },
            { "zh",     BroadcasterLanguage.Zh    },
            { "zh-hk",  BroadcasterLanguage.ZhHk  },
            { "ja",     BroadcasterLanguage.Ja    },
            { "ko",     BroadcasterLanguage.Ko    },
            { "asl",    BroadcasterLanguage.Asl   },
            { "other",  BroadcasterLanguage.Other },
        };

        private static
        Dictionary<BroadcasterLanguage, string>     CACHE_FROM_BROADCASTER_LANGUAGE         = new Dictionary<BroadcasterLanguage, string>
        {
            { BroadcasterLanguage.None,     ""      },

            { BroadcasterLanguage.En,       "en"    },
            { BroadcasterLanguage.Da,       "da"    },
            { BroadcasterLanguage.De,       "de"    },
            { BroadcasterLanguage.Es,       "es"    },
            { BroadcasterLanguage.Fr,       "fr"    },
            { BroadcasterLanguage.It,       "it"    },
            { BroadcasterLanguage.Hu,       "hu"    },
            { BroadcasterLanguage.Nl,       "nl"    },
            { BroadcasterLanguage.No,       "no"    },
            { BroadcasterLanguage.Pl,       "pl"    },
            { BroadcasterLanguage.Pt,       "pt"    },
            { BroadcasterLanguage.Sk,       "sk"    },
            { BroadcasterLanguage.Fi,       "fi"    },
            { BroadcasterLanguage.Sv,       "sv"    },
            { BroadcasterLanguage.Vi,       "vi"    },
            { BroadcasterLanguage.Tr,       "tr"    },
            { BroadcasterLanguage.Cs,       "cs"    },
            { BroadcasterLanguage.El,       "el"    },
            { BroadcasterLanguage.Bg,       "bg"    },
            { BroadcasterLanguage.Ru,       "ru"    },
            { BroadcasterLanguage.Ar,       "ar"    },
            { BroadcasterLanguage.Th,       "th"    },
            { BroadcasterLanguage.Zh,       "zh"    },
            { BroadcasterLanguage.ZhHk,     "zh-hk" },
            { BroadcasterLanguage.Ja,       "ja"    },
            { BroadcasterLanguage.Ko,       "ko"    },
            { BroadcasterLanguage.Asl,      "asl"   },
            { BroadcasterLanguage.Other,    "other" },
        };

        private static readonly
        Dictionary<string, VideoPeriod>             CACHE_TO_VIDEO_PERIOD                   = new Dictionary<string, VideoPeriod>()
        {
            { "all",    VideoPeriod.All   },
            { "day",    VideoPeriod.Day   },
            { "week",   VideoPeriod.Week  },
            { "month",  VideoPeriod.Month },
        };
        
        private static readonly
        Dictionary<VideoPeriod, string>             CACHE_FROM_VIDEO_PERIOD                 = new Dictionary<VideoPeriod, string>()
        {
            { VideoPeriod.All,      "all"   },
            { VideoPeriod.Day,      "day"   },
            { VideoPeriod.Week,     "week"  },
            { VideoPeriod.Month,    "month" },
        };

        private static readonly
        Dictionary<string, VideoSort>               CACHE_TO_VIDEO_SORT                     = new Dictionary<string, VideoSort>()
        {
            { "time",       VideoSort.Time     },
            { "trending",   VideoSort.Trending },
            { "views",      VideoSort.Views    },
        };

        private static readonly
        Dictionary<VideoSort, string>               CACHE_FROM_VIDEO_SORT                   = new Dictionary<VideoSort, string>()
        {
            { VideoSort.Time,       "time"     },
            { VideoSort.Trending,   "trending" },
            { VideoSort.Views,      "views"    },
        };

        private static readonly
        Dictionary<string, VideoType>               CACHE_TO_VIDEO_TYPE                     = new Dictionary<string, VideoType>()
        {
            { "upload",     VideoType.Upload    },
            { "archive",    VideoType.Archive   },
            { "highlight",  VideoType.Highlight },
            { "all",        VideoType.All       },
        };

        private static readonly
        Dictionary<VideoType, string>               CACHE_FROM_VIDEO_TYPE                   = new Dictionary<VideoType, string>()
        {
            { VideoType.Upload,     "upload"    },
            { VideoType.Archive,    "archive"   },
            { VideoType.Highlight,  "highlight" },
            { VideoType.All,        "all"       },
        };

        private static readonly
        Dictionary<string, Scopes>                  CACHE_TO_SCOPES                         = new Dictionary<string, Scopes>()
        {
            { "",                       Scopes.Other              },

            { "analytics:read:games",   Scopes.AnalyticsReadGames },
            { "bits:read",              Scopes.BitsRead           },
            { "clips:edit",             Scopes.ClipsEdit          },
            { "user:edit",              Scopes.UserEdit           },
            { "user:read:email",        Scopes.UserReadEmail      },
        };

        private static readonly
        Dictionary<Scopes, string>                  CACHE_FROM_SCOPES                       = new Dictionary<Scopes, string>()
        {
            { Scopes.Other,                 ""                     },

            { Scopes.AnalyticsReadGames,    "analytics:read:games" },
            { Scopes.BitsRead,              "bits:read"            },
            { Scopes.ClipsEdit,             "clips:edit"           },
            { Scopes.UserEdit,              "user:edit"            },
            { Scopes.UserReadEmail,         "user:read:email"      },
        };

        #endregion

        #region Shared enum caches

        private static readonly
        Dictionary<string, UserType>                CACHE_TO_USER_TYPE                      = new Dictionary<string, UserType>
        {
            { "",           UserType.None      },
            { "mod",        UserType.Mod       },
            { "global_mod", UserType.GlobalMod },
            { "admin",      UserType.Admin     },
            { "staff",      UserType.Staff     },
        };

        private static readonly
        Dictionary<UserType, string>                CACHE_FROM_USER_TYPE                    = new Dictionary<UserType, string>
        {
            { UserType.None,        ""           },
            { UserType.Mod,         "mod"        },
            { UserType.GlobalMod,   "global_mod" },
            { UserType.Admin,       "admin"      },
            { UserType.Staff,       "stafrf"     },
        };

        #endregion

        #region Client enum converters

        /// <summary>
        /// Converts a string into a <see cref="BadgeType"/> value.
        /// </summary>
        /// <param name="str">The string to convert.</param>
        /// <returns>
        /// Returns the corresponding <see cref="BadgeType"/> value if the string is found in the cache.
        /// Returns default enum value otherwise.
        /// </returns>
        public static BadgeType
        ToBadgeType(string str)
        {
            if (str.IsNull())
            {
                return default(BadgeType);
            }

            CACHE_TO_BADGE_TYPE.TryGetValue(str, out BadgeType value);

            return value;
        }

        /// <summary>
        /// Converts a <see cref="BadgeType"/> value to a string.
        /// </summary>
        /// <param name="value">The enum value to convert.</param>
        /// <returns>
        /// Returns the corresponding enum name if the <see cref="BadgeType"/> value is found in the cache.
        /// Returns the enum name obtained from <see cref="Enum.GetName(Type, object)"/> otherwise.
        /// </returns>
        public static string
        FromBadgeType(BadgeType value)
        {
            if (!CACHE_FROM_BADGE_TYPE.TryGetValue(value, out string name))
            {
                name = Enum.GetName(value.GetType(), value);

                Debug.WriteWarning(ErrorLevel.Minor, "The enum value " + name.WrapQuotes() + " found in the " + nameof(BadgeType) + " enum.");
            }

            return name;
        }

        /// <summary>
        /// Converts a string into a <see cref="ChatCommand"/> value.
        /// </summary>
        /// <param name="str">The string to convert.</param>
        /// <returns>
        /// Returns the corresponding <see cref="ChatCommand"/> value if the string is found in the cache.
        /// Returns default enum value otherwise.
        /// </returns>
        public static ChatCommand
        ToChatCommand(string str)
        {
            if (str.IsNull())
            {
                return default(ChatCommand);
            }

            CACHE_TO_CHAT_COMMAND.TryGetValue(str, out ChatCommand value);

            return value;
        }

        /// <summary>
        /// Converts a <see cref="ChatCommand"/> value to a string.
        /// </summary>
        /// <param name="value">The enum value to convert.</param>
        /// <returns>
        /// Returns the corresponding enum name if the <see cref="ChatCommand"/> value is found in the cache.
        /// Returns the enum name obtained from <see cref="Enum.GetName(Type, object)"/> otherwise.
        /// </returns>
        public static string
        FromChatCommand(ChatCommand value)
        {
            if (!CACHE_FROM_CHAT_COMMAND.TryGetValue(value, out string name))
            {
                name = Enum.GetName(value.GetType(), value);

                Debug.WriteWarning(ErrorLevel.Minor, "The enum value " + name.WrapQuotes() + " found in the " + nameof(ChatCommand) + " enum.");
            }

            return name;
        }

        /// <summary>
        /// Converts a string into a <see cref="CommercialLength"/> value.
        /// </summary>
        /// <param name="str">The string to convert.</param>
        /// <returns>
        /// Returns the corresponding <see cref="CommercialLength"/> value if the string is found in the cache.
        /// Returns default enum value otherwise.
        /// </returns>
        public static CommercialLength
        ToCommercialLength(string str)
        {
            if (str.IsNull())
            {
                return default(CommercialLength);
            }

            CACHE_TO_COMMERCIAL_LENGTH.TryGetValue(str, out CommercialLength value);

            return value;
        }

        /// <summary>
        /// Converts a <see cref="CommercialLength"/> value to a string.
        /// </summary>
        /// <param name="value">The enum value to convert.</param>
        /// <returns>
        /// Returns the corresponding enum name if the <see cref="CommercialLength"/> value is found in the cache.
        /// Returns the enum name obtained from <see cref="Enum.GetName(Type, object)"/> otherwise.
        /// </returns>
        public static string
        FromCommercialLength(CommercialLength value)
        {
            if (!CACHE_FROM_COMMERCIAL_LENGTH.TryGetValue(value, out string name))
            {
                name = Enum.GetName(value.GetType(), value);

                Debug.WriteWarning(ErrorLevel.Minor, "The enum value " + name.WrapQuotes() + " found in the " + nameof(CommercialLength) + " enum.");
            }

            return name;
        }

        /// <summary>
        /// Converts a string into a <see cref="DisplayNameColor"/> value.
        /// </summary>
        /// <param name="str">The string to convert.</param>
        /// <returns>
        /// Returns the corresponding <see cref="DisplayNameColor"/> value if the string is found in the cache.
        /// Returns default enum value otherwise.
        /// </returns>
        public static DisplayNameColor
        ToDisplayNameColor(string str)
        {
            if (str.IsNull())
            {
                return default(DisplayNameColor);
            }

            CACHE_TO_DISPLAY_NAME_COLOR.TryGetValue(str, out DisplayNameColor value);

            return value;
        }

        /// <summary>
        /// Converts a <see cref="DisplayNameColor"/> value to a string.
        /// </summary>
        /// <param name="value">The enum value to convert.</param>
        /// <returns>
        /// Returns the corresponding enum name if the <see cref="DisplayNameColor"/> value is found in the cache.
        /// Returns the enum name obtained from <see cref="Enum.GetName(Type, object)"/> otherwise.
        /// </returns>
        public static string
        FromDisplayNameColor(DisplayNameColor value)
        {
            if (!CACHE_FROM_DISPLAY_NAME_COLOR.TryGetValue(value, out string name))
            {
                name = Enum.GetName(value.GetType(), value);

                Debug.WriteWarning(ErrorLevel.Minor, "The enum value " + name.WrapQuotes() + " found in the " + nameof(DisplayNameColor) + " enum.");
            }

            return name;
        }

        /// <summary>
        /// Converts a string into a <see cref="FollowersDurationPeriod"/> value.
        /// </summary>
        /// <param name="str">The string to convert.</param>
        /// <returns>
        /// Returns the corresponding <see cref="FollowersDurationPeriod"/> value if the string is found in the cache.
        /// Returns default enum value otherwise.
        /// </returns>
        public static FollowersDurationPeriod
        ToFollowersDurationPeriod(string str)
        {
            if (str.IsNull())
            {
                return default(FollowersDurationPeriod);
            }

            CACHE_TO_FOLLOWERS_DURATION_PERIOD.TryGetValue(str, out FollowersDurationPeriod value);

            return value;
        }

        /// <summary>
        /// Converts a <see cref="FollowersDurationPeriod"/> value to a string.
        /// </summary>
        /// <param name="value">The enum value to convert.</param>
        /// <returns>
        /// Returns the corresponding enum name if the <see cref="FollowersDurationPeriod"/> value is found in the cache.
        /// Returns the enum name obtained from <see cref="Enum.GetName(Type, object)"/> otherwise.
        /// </returns>
        public static string
        FromFollowersDurationPeriod(FollowersDurationPeriod value)
        {
            if (!CACHE_FROM_FOLLOWERS_DURATION_PERIOD.TryGetValue(value, out string name))
            {
                name = Enum.GetName(value.GetType(), value);

                Debug.WriteWarning(ErrorLevel.Minor, "The enum value " + name.WrapQuotes() + " found in the " + nameof(FollowersDurationPeriod) + " enum.");
            }

            return name;
        }

        /// <summary>
        /// Converts a string into a <see cref="NoticeType"/> value.
        /// </summary>
        /// <param name="str">The string to convert.</param>
        /// <returns>
        /// Returns the corresponding <see cref="NoticeType"/> value if the string is found in the cache.
        /// Returns default enum value otherwise.
        /// </returns>
        public static NoticeType
        ToNoticeType(string str)
        {
            if (str.IsNull())
            {
                return default(NoticeType);
            }

            CACHE_TO_NOTICE_TYPE.TryGetValue(str, out NoticeType value);

            return value;
        }

        /// <summary>
        /// Converts a <see cref="NoticeType"/> value to a string.
        /// </summary>
        /// <param name="value">The enum value to convert.</param>
        /// <returns>
        /// Returns the corresponding enum name if the <see cref="NoticeType"/> value is found in the cache.
        /// Returns the enum name obtained from <see cref="Enum.GetName(Type, object)"/> otherwise.
        /// </returns>
        public static string
        FromNoticeType(NoticeType value)
        {
            if (!CACHE_FROM_NOTICE_TYPE.TryGetValue(value, out string name))
            {
                name = Enum.GetName(value.GetType(), value);

                Debug.WriteWarning(ErrorLevel.Minor, "The enum value " + name.WrapQuotes() + " found in the " + nameof(NoticeType) + " enum.");
            }

            return name;
        }

        /// <summary>
        /// Converts a string into a <see cref="RitualType"/> value.
        /// </summary>
        /// <param name="str">The string to convert.</param>
        /// <returns>
        /// Returns the corresponding <see cref="RitualType"/> value if the string is found in the cache.
        /// Returns default enum value otherwise.
        /// </returns>
        public static RitualType
        ToRitualType(string str)
        {
            if (str.IsNull())
            {
                return default(RitualType);
            }

            CACHE_TO_RITUAL_TYPE.TryGetValue(str, out RitualType value);

            return value;
        }

        /// <summary>
        /// Converts a <see cref="RitualType"/> value to a string.
        /// </summary>
        /// <param name="value">The enum value to convert.</param>
        /// <returns>
        /// Returns the corresponding enum name if the <see cref="RitualType"/> value is found in the cache.
        /// Returns the enum name obtained from <see cref="Enum.GetName(Type, object)"/> otherwise.
        /// </returns>
        public static string
        FromRitualType(RitualType value)
        {
            if (!CACHE_FROM_RITUAL_TYPE.TryGetValue(value, out string name))
            {
                name = Enum.GetName(value.GetType(), value);

                Debug.WriteWarning(ErrorLevel.Minor, "The enum value " + name.WrapQuotes() + " found in the " + nameof(RitualType) + " enum.");
            }

            return name;
        }

        /// <summary>
        /// Converts a string into a <see cref="SubscriptionPlan"/> value.
        /// </summary>
        /// <param name="str">The string to convert.</param>
        /// <returns>
        /// Returns the corresponding <see cref="SubscriptionPlan"/> value if the string is found in the cache.
        /// Returns default enum value otherwise.
        /// </returns>
        public static SubscriptionPlan
        ToSubscriptionPlan(string str)
        {
            if (str.IsNull())
            {
                return default(SubscriptionPlan);
            }

            CACHE_TO_SUBSCRIPTION_PLAN.TryGetValue(str, out SubscriptionPlan value);

            return value;
        }

        /// <summary>
        /// Converts a <see cref="SubscriptionPlan"/> value to a string.
        /// </summary>
        /// <param name="value">The enum value to convert.</param>
        /// <returns>
        /// Returns the corresponding enum name if the <see cref="SubscriptionPlan"/> value is found in the cache.
        /// Returns the enum name obtained from <see cref="Enum.GetName(Type, object)"/> otherwise.
        /// </returns>
        public static string
        FromSubscriptionPlan(SubscriptionPlan value)
        {
            if (!CACHE_FROM_SUBSCRIPTION_PLAN.TryGetValue(value, out string name))
            {
                name = Enum.GetName(value.GetType(), value);

                Debug.WriteWarning(ErrorLevel.Minor, "The enum value " + name.WrapQuotes() + " found in the " + nameof(SubscriptionPlan) + " enum.");
            }

            return name;
        }

        /// <summary>
        /// Converts a string into a <see cref="UserNoticeType"/> value.
        /// </summary>
        /// <param name="str">The string to convert.</param>
        /// <returns>
        /// Returns the corresponding <see cref="UserNoticeType"/> value if the string is found in the cache.
        /// Returns default enum value otherwise.
        /// </returns>
        public static UserNoticeType
        ToUserNoticeType(string str)
        {
            if (str.IsNull())
            {
                return default(UserNoticeType);
            }

            CACHE_TO_USER_NOTICE_TYPE.TryGetValue(str, out UserNoticeType value);

            return value;
        }

        /// <summary>
        /// Converts an <see cref="UserNoticeType"/> value to a string.
        /// </summary>
        /// <param name="value">The enum value to convert.</param>
        /// <returns>
        /// Returns the corresponding enum name if the <see cref="UserNoticeType"/> value is found in the cache.
        /// Returns the enum name obtained from <see cref="Enum.GetName(Type, object)"/> otherwise.
        /// </returns>
        public static string
        FromUserNoticeType(UserNoticeType value)
        {
            if (!CACHE_FROM_USER_NOTICE_TYPE.TryGetValue(value, out string name))
            {
                name = Enum.GetName(value.GetType(), value);

                Debug.WriteWarning(ErrorLevel.Minor, "The enum value " + name.WrapQuotes() + " found in the " + nameof(UserNoticeType) + " enum.");
            }

            return name;
        }

        #endregion

        #region Rest enum converters

        /// <summary>
        /// Converts a string into an <see cref="EntitlementType"/> value.
        /// </summary>
        /// <param name="str">The string to convert.</param>
        /// <returns>
        /// Returns the corresponding <see cref="EntitlementType"/> value if the string is found in the cache.
        /// Returns default enum value otherwise.
        /// </returns>
        public static EntitlementType
        ToEntitlementType(string str)
        {
            if (str.IsNull())
            {
                return default(EntitlementType);
            }

            CACHE_TO_ENTITLEMENTR_TYPE.TryGetValue(str, out EntitlementType value);

            return value;
        }

        /// <summary>
        /// Converts an <see cref="EntitlementType"/> value to a string.
        /// </summary>
        /// <param name="value">The enum value to convert.</param>
        /// <returns>
        /// Returns the corresponding enum name if the <see cref="EntitlementType"/> value is found in the cache.
        /// Returns the enum name obtained from <see cref="Enum.GetName(Type, object)"/> otherwise.
        /// </returns>
        public static string
        FromEntitlementType(EntitlementType value)
        {
            if (!CACHE_FROM_ENTITLEMENTR_TYPE.TryGetValue(value, out string name))
            {
                name = Enum.GetName(value.GetType(), value);

                Debug.WriteWarning(ErrorLevel.Minor, "The enum value " + name.WrapQuotes() + " found in the " + nameof(EntitlementType) + " enum.");
            }

            return name;
        }

        /// <summary>
        /// Converts a string into a <see cref="StreamLanguage"/> value.
        /// </summary>
        /// <param name="str">The string to convert.</param>
        /// <returns>
        /// Returns the corresponding <see cref="StreamLanguage"/> value if the string is found in the cache.
        /// Returns default enum value otherwise.
        /// </returns>
        public static StreamLanguage
        ToStreamLanguage(string str)
        {
            if (str.IsNull())
            {
                return default(StreamLanguage);
            }

            CACHE_TO_STREAM_LANGUAGE.TryGetValue(str, out StreamLanguage value);

            return StreamLanguage.No;
        }

        /// <summary>
        /// Converts a <see cref="StreamLanguage"/> value to a string.
        /// </summary>
        /// <param name="value">The enum value to convert.</param>
        /// <returns>
        /// Returns the corresponding enum name if the <see cref="StreamLanguage"/> value is found in the cache.
        /// Returns the enum name obtained from <see cref="Enum.GetName(Type, object)"/> otherwise.
        /// </returns>
        public static string
        FromStreamLanguage(StreamLanguage value)
        {
            BenchmarkRun run = new BenchmarkRun();
            run.name = "Benchmark_FromStreamLanguage_Single()";
            run.iterations = 1_000_000;
            run.action = Benchmark_FromStreamLanguage_Single;

            BenchmarkRun run2 = new BenchmarkRun();
            run2.name = "Benchmark_ToStreamLanguage_Single()";
            run2.iterations = 1_000_000;
            run2.action = Benchmark_ToStreamLanguage_Single;

            BenchmarkRun run3 = new BenchmarkRun();
            run3.name = "Benchmark_FromStreamLanguage_Flags()";
            run3.iterations = 1_000_000;
            run3.action = Benchmark_FromStreamLanguage_Flags;

            Benchmark benchmark = new Benchmark();
            //benchmark.Add(run);
            //benchmark.Add(run2);
            benchmark.Add(run3);
            benchmark.Execute();

            if (!CACHE_FROM_STREAM_LANGUAGE.TryGetValue(value, out string name))
            {
                name = Enum.GetName(value.GetType(), value);

                Debug.WriteWarning(ErrorLevel.Minor, "The enum value " + name.WrapQuotes() + " found in the " + nameof(StreamLanguage) + " enum.");
            }

            return name;
        }        

        /// <summary>
        /// Converts a string into a <see cref="StreamType"/> value.
        /// </summary>
        /// <param name="str">The string to convert.</param>
        /// <returns>
        /// Returns the corresponding <see cref="StreamType"/> value if the string is found in the cache.
        /// Returns default enum value otherwise.
        /// </returns>
        public static StreamType
        ToStreamType(string str)
        {
            if (str.IsNull())
            {
                return default(StreamType);
            }

            CACHE_TO_STREAM_TYPE.TryGetValue(str, out StreamType value);

            return value;
        }

        /// <summary>
        /// Converts a <see cref="StreamType"/> value to a string.
        /// </summary>
        /// <param name="value">The enum value to convert.</param>
        /// <returns>
        /// Returns the corresponding enum name if the <see cref="StreamType"/> value is found in the cache.
        /// Returns the enum name obtained from <see cref="Enum.GetName(Type, object)"/> otherwise.
        /// </returns>
        public static string
        FromStreamType(StreamType value)
        {
            if (!CACHE_FROM_STREAM_TYPE.TryGetValue(value, out string name))
            {
                name = Enum.GetName(value.GetType(), value);

                Debug.WriteWarning(ErrorLevel.Minor, "The enum value " + name.WrapQuotes() + " found in the " + nameof(StreamType) + " enum.");
            }

            return name;
        }

        /// <summary>
        /// Converts a string into a <see cref="BroadcasterType"/> value.
        /// </summary>
        /// <param name="str">The string to convert.</param>
        /// <returns>
        /// Returns the corresponding <see cref="BroadcasterType"/> value if the string is found in the cache.
        /// Returns default enum value otherwise.
        /// </returns>
        public static BroadcasterType
        ToBroadcasterType(string str)
        {
            if (str.IsNull())
            {
                return default(BroadcasterType);
            }

            CACHE_TO_BROADCASTER_TYPE.TryGetValue(str, out BroadcasterType value);

            return value;
        }

        /// <summary>
        /// Converts a <see cref="BroadcasterType"/> value to a string.
        /// </summary>
        /// <param name="value">The enum value to convert.</param>
        /// <returns>
        /// Returns the corresponding enum name if the <see cref="BroadcasterType"/> value is found in the cache.
        /// Returns the enum name obtained from <see cref="Enum.GetName(Type, object)"/> otherwise.
        /// </returns>
        public static string
        FromBroadcasterType(BroadcasterType value)
        {
            if (!CACHE_FROM_BROADCASTER_TYPE.TryGetValue(value, out string name))
            {
                name = Enum.GetName(value.GetType(), value);

                Debug.WriteWarning(ErrorLevel.Minor, "The enum value " + name.WrapQuotes() + " found in the " + nameof(BroadcasterType) + " enum.");
            }

            return name;
        }

        /// <summary>
        /// Converts a string into a <see cref="BroadcasterLanguage"/> value.
        /// </summary>
        /// <param name="str">The string to convert.</param>
        /// <returns>
        /// Returns the corresponding <see cref="BroadcasterLanguage"/> value if the string is found in the cache.
        /// Returns default enum value otherwise.
        /// </returns>
        public static BroadcasterLanguage
        ToBroadcasterLanguage(string str)
        {
            if (str.IsNull())
            {
                return default(BroadcasterLanguage);
            }

            CACHE_TO_BROADCASTER_LANGUAGE.TryGetValue(str, out BroadcasterLanguage value);

            return value;
        }

        /// <summary>
        /// Converts a <see cref="BroadcasterLanguage"/> value to a string.
        /// </summary>
        /// <param name="value">The enum value to convert.</param>
        /// <returns>
        /// Returns the corresponding enum name if the <see cref="BroadcasterLanguage"/> value is found in the cache.
        /// Returns the enum name obtained from <see cref="Enum.GetName(Type, object)"/> otherwise.
        /// </returns>
        public static string
        FromBroadcasterLanguage(BroadcasterLanguage value)
        {
            if (!CACHE_FROM_BROADCASTER_LANGUAGE.TryGetValue(value, out string name))
            {
                name = Enum.GetName(value.GetType(), value);

                Debug.WriteWarning(ErrorLevel.Minor, "The enum value " + name.WrapQuotes() + " found in the " + nameof(BroadcasterLanguage) + " enum.");
            }

            return name;
        }

        /// <summary>
        /// Converts a string into a <see cref="VideoPeriod"/> value.
        /// </summary>
        /// <param name="str">The string to convert.</param>
        /// <returns>
        /// Returns the corresponding <see cref="VideoPeriod"/> value if the string is found in the cache.
        /// Returns default enum value otherwise.
        /// </returns>
        public static VideoPeriod
        ToVideoPeriod(string str)
        {
            if (str.IsNull())
            {
                return default(VideoPeriod);
            }

            CACHE_TO_VIDEO_PERIOD.TryGetValue(str, out VideoPeriod value);

            return value;
        }

        /// <summary>
        /// Converts a <see cref="VideoPeriod"/> value to a string.
        /// </summary>
        /// <param name="value">The enum value to convert.</param>
        /// <returns>
        /// Returns the corresponding enum name if the <see cref="VideoPeriod"/> value is found in the cache.
        /// Returns the enum name obtained from <see cref="Enum.GetName(Type, object)"/> otherwise.
        /// </returns>
        public static string
        FromVideoPeriod(VideoPeriod value)
        {
            if (!CACHE_FROM_VIDEO_PERIOD.TryGetValue(value, out string name))
            {
                name = Enum.GetName(value.GetType(), value);

                Debug.WriteWarning(ErrorLevel.Minor, "The enum value " + name.WrapQuotes() + " found in the " + nameof(VideoPeriod) + " enum.");
            }

            return name;
        }

        /// <summary>
        /// Converts a string into a <see cref="VideoSort"/> value.
        /// </summary>
        /// <param name="str">The string to convert.</param>
        /// <returns>
        /// Returns the corresponding <see cref="VideoSort"/> value if the string is found in the cache.
        /// Returns default enum value otherwise.
        /// </returns>
        public static VideoSort
        ToVideoSort(string str)
        {
            if (str.IsNull())
            {
                return default(VideoSort);
            }

            CACHE_TO_VIDEO_SORT.TryGetValue(str, out VideoSort value);

            return value;
        }

        /// <summary>
        /// Converts a <see cref="VideoSort"/> value to a string.
        /// </summary>
        /// <param name="value">The enum value to convert.</param>
        /// <returns>
        /// Returns the corresponding enum name if the <see cref="VideoSort"/> value is found in the cache.
        /// Returns the enum name obtained from <see cref="Enum.GetName(Type, object)"/> otherwise.
        /// </returns>
        public static string
        FromVideoSort(VideoSort value)
        {
            if (!CACHE_FROM_VIDEO_SORT.TryGetValue(value, out string name))
            {
                name = Enum.GetName(value.GetType(), value);

                Debug.WriteWarning(ErrorLevel.Minor, "The enum value " + name.WrapQuotes() + " found in the " + nameof(VideoSort) + " enum.");
            }

            return name;
        }

        /// <summary>
        /// Converts a string into a <see cref="VideoType"/> value.
        /// </summary>
        /// <param name="str">The string to convert.</param>
        /// <returns>
        /// Returns the corresponding <see cref="VideoType"/> value if the string is found in the cache.
        /// Returns default enum value otherwise.
        /// </returns>
        public static VideoType
        ToVideoType(string str)
        {
            if (str.IsNull())
            {
                return default(VideoType);
            }

            CACHE_TO_VIDEO_TYPE.TryGetValue(str, out VideoType value);

            return value;
        }

        /// <summary>
        /// Converts a <see cref="VideoType"/> value to a string.
        /// </summary>
        /// <param name="value">The enum value to convert.</param>
        /// <returns>
        /// Returns the corresponding enum name if the <see cref="VideoType"/> value is found in the cache.
        /// Returns the enum name obtained from <see cref="Enum.GetName(Type, object)"/> otherwise.
        /// </returns>
        public static string
        FromVideoType(VideoType value)
        {
            if (!CACHE_FROM_VIDEO_TYPE.TryGetValue(value, out string name))
            {
                name = Enum.GetName(value.GetType(), value);

                Debug.WriteWarning(ErrorLevel.Minor, "The enum value " + name.WrapQuotes() + " found in the " + nameof(VideoType) + " enum.");
            }

            return name;
        }

        /// <summary>
        /// Converts a string into a <see cref="Scopes"/> value.
        /// </summary>
        /// <param name="str">The string to convert.</param>
        /// <returns>
        /// Returns the corresponding <see cref="Scopes"/> value if the string is found in the cache.
        /// Returns default enum value otherwise.
        /// </returns>
        public static Scopes
        ToScopes(string str)
        {
            if (str.IsNull())
            {
                return default(Scopes);
            }

            CACHE_TO_SCOPES.TryGetValue(str, out Scopes value);

            return value;
        }

        /// <summary>
        /// Converts a <see cref="Scopes"/> value to a string.
        /// </summary>
        /// <param name="value">The enum value to convert.</param>
        /// <returns>
        /// Returns the corresponding enum name if the <see cref="Scopes"/> value is found in the cache.
        /// Returns the enum name obtained from <see cref="Enum.GetName(Type, object)"/> otherwise.
        /// </returns>
        public static string
        FromScopes(Scopes value)
        {
            if (!CACHE_FROM_SCOPES.TryGetValue(value, out string name))
            {
                name = Enum.GetName(value.GetType(), value);

                Debug.WriteWarning(ErrorLevel.Minor, "The enum value " + name.WrapQuotes() + " found in the " + nameof(Scopes) + " enum.");
            }

            return name;
        }

        #endregion

        #region Shared enum converters

        /// <summary>
        /// Converts a string into a <see cref="UserType"/> value.
        /// </summary>
        /// <param name="str">The string to convert.</param>
        /// <returns>
        /// Returns the corresponding <see cref="UserType"/> value if the string is found in the cache.
        /// Returns default enum value otherwise.
        /// </returns>
        public static UserType
        ToUserType(string str)
        {
            if (str.IsNull())
            {
                return default(UserType);
            }

            CACHE_TO_USER_TYPE.TryGetValue(str, out UserType value);

            return value;
        }

        /// <summary>
        /// Converts an <see cref="UserType"/> value to a string.
        /// </summary>
        /// <param name="value">The enum value to convert.</param>
        /// <returns>
        /// Returns the corresponding enum name if the <see cref="UserType"/> value is found in the cache.
        /// Returns the enum name obtained from <see cref="Enum.GetName(Type, object)"/> otherwise.
        /// </returns>
        public static string
        FromUserType(UserType value)
        {
            if (!CACHE_FROM_USER_TYPE.TryGetValue(value, out string name))
            {
                name = Enum.GetName(value.GetType(), value);

                Debug.WriteWarning(ErrorLevel.Minor, "The enum value " + name.WrapQuotes() + " found in the " + nameof(UserType) + " enum.");
            }

            return name;
        }

        #endregion

        #region Converters

        /// <summary>
        /// <para>Gets the name of the specified enum's value.</para>
        /// <para>Supports bitfield enum values.</para>
        /// </summary>
        /// <param name="type">The enum's type.</param>
        /// <param name="value">The enum value to get the name of.</param>
        /// <returns>
        /// Returns resolved name of the enum value.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown if the <paramref name="type"/> is null.
        /// Thrown if the specified value is null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// Thrown if the <paramref name="type"/> is not an enum.
        /// Thrown if no names or values exist in the enum type.
        /// Thrown if the specified value cannot be converted to a UInt64.
        /// Thrown if one or more flags in the specified value could not be converted if the enum is a collection of flags.
        /// Thrown if the specified value could not be found in the enum type.
        /// </exception>
        public static string
        GetName(Type type, object value)
        {
            EnumTypeCache cache = GetOrAddCache(type);
            string result = cache.GetName(value);

            return result;
        }

        /// <summary>
        /// <para>Attempts to get the name of the specified enum's value.</para>
        /// <para>Supports bitfield enum values.</para>
        /// </summary>
        /// <param name="type">The enum's type.</param>
        /// <param name="value">The enum value to get the name of.</param>
        /// <param name="result">
        /// Set to the resolved name, if successful.
        /// Set to null otherwise.
        /// </param>
        /// <returns>
        /// Returns true if the name was successfully retrieved.
        /// Returns false otherwise.
        /// </returns>
        public static bool
        TryGetName(Type type, object value, out string result)
        {
            if (type.IsNull() || !type.IsEnum)
            {
                result = null;

                return false;
            }

            EnumTypeCache cache = GetOrAddCache(type);
            bool success = cache.TryGetName(value, out result);

            return success;
        }

        /// <summary>
        /// <para>Converts a string representation of an enum value into the equivalent constant enum value.</para>
        /// <para>Supports bitfield formatted strings.</para>
        /// </summary>
        /// <param name="type">The enum's type.</param>
        /// <param name="value">
        /// <para>The string to convert.</para>
        /// <para>This can either be the resolved or native (unresolved) name.</para>
        /// </param>        
        /// <returns>Returns the equivalent constant enum value of its string representation.</returns>
        /// <exception cref="ArgumentNullException">Thrown if the <paramref name="type"/> is null.</exception>
        /// <exception cref="ArgumentException">
        /// Thrown if the <paramref name="type"/> is not an enum.
        /// Thrown if the string value could not be converted into the equivalent constant enum value.
        /// </exception>
        public static object
        Parse(Type type, string value)
        {
            object result = Parse(type, value, false);

            return result;
        }

        /// <summary>
        /// <para>Converts a string representation of an enum value into the equivalent constant enum value.</para>
        /// <para>Supports bitfield formatted strings.</para>
        /// </summary>
        /// <param name="type">The enum's type.</param>
        /// <param name="value">
        /// <para>The string to convert.</para>
        /// <para>This can either be the resolved or native (unresolved) name.</para>
        /// </param>
        /// <param name="ignore_case">Whether or not to ignore string case when parsing.</param>
        /// <returns>Returns the equivalent constant enum value of its string representation.</returns>
        /// <exception cref="ArgumentNullException">Thrown if the <paramref name="type"/> is null.</exception>
        /// <exception cref="ArgumentException">
        /// Thrown if the <paramref name="type"/> is not an enum.
        /// Thrown if the string value could not be converted into the equivalent constant enum value.
        /// </exception>
        public static object
        Parse(Type type, string value, bool ignore_case)
        {
            EnumTypeCache cache = GetOrAddCache(type);
            object result = cache.Parse(value, ignore_case);

            return result;
        }

        /// <summary>
        /// <para>Attempts to convert a string representation of an enum value into the equivalent constant enum value.</para>
        /// <para>Supports bitfield formatted strings.</para>
        /// </summary>
        /// <param name="type">The enum's type.</param>
        /// <param name="value">
        /// <para>The string to convert.</para>
        /// <para>This can either be the resolved or native (unresolved) name.</para>
        /// </param>
        /// <param name="result">
        /// Set to null if the type is null or if the type is not an enum.
        /// Set to the equivalent constant enum value if the conversion was successful.
        /// Set to the enum's default value otherwise.
        /// </param>
        /// <returns>
        /// Returns true if the string value was successfully parsed.
        /// Returns false otherwise.
        /// </returns>
        public static bool
        TryParse(Type type, string value, out object result)
        {
            bool success = TryParse(type, value, false, out result);

            return success;
        }

        /// <summary>
        /// <para>Attempts to convert a string representation of an enum value into the equivalent constant enum value.</para>
        /// <para>Supports bitfield formatted strings.</para>
        /// </summary>
        /// <param name="type">The enum's type.</param>
        /// <param name="value">
        /// <para>The string to convert.</para>
        /// <para>This can either be the resolved or native (unresolved) name.</para>
        /// </param>
        /// <param name="ignore_case">Whether or not to ignore string case when parsing.</param>
        /// <param name="result">
        /// Set to null if the type is null or if the type is not an enum.
        /// Set to the equivalent constant enum value if the conversion was successful.
        /// Set to the enum's default value otherwise.
        /// </param>
        /// <returns>
        /// Returns true if the string value was successfully parsed.
        /// Returns false otherwise.
        /// </returns>
        public static bool
        TryParse(Type type, string value, bool ignore_case, out object result)
        {
            if(type.IsNull() || !type.IsEnum)
            {
                result = null;

                return false;
            }

            EnumTypeCache cache = GetOrAddCache(type);
            bool success = cache.TryParse(value, ignore_case, out result);

            return success;
        }        

        #endregion

        #region Helpers

        /// <summary>
        /// Gets the enum cache for the specified type.
        /// If no cache exists for the specified enum type, one is created.
        /// </summary>
        /// <typeparam name="enum_type">
        /// The enum's type as a generic parameter.
        /// Restricted to a struct.
        /// </typeparam>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">Thrown if the <paramref name="type"/> is null when creating a cache.</exception>
        /// <exception cref="ArgumentException">
        /// Thrown if the <typeparamref name="enum_type"/> is not an enum when creating a cache.
        /// </exception>
        public static EnumTypeCache
        GetOrAddCache<enum_type>()
        where enum_type : struct
        {
            EnumTypeCache cache = GetOrAddCache(typeof(enum_type));

            return cache;
        }

        /// <summary>
        /// Gets the enum cache for the specified type.
        /// If no cache exists for the specified enum type, one is created.
        /// </summary>
        /// <param name="type">The enum's type.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">Thrown if the <paramref name="type"/> is null when creating a cache.</exception>
        /// <exception cref="ArgumentException">
        /// Thrown if the <paramref name="type"/> is not an enum when creating a cache.
        /// </exception>
        public static EnumTypeCache
        GetOrAddCache(Type type)
        {
            EnumTypeCache cache = CACHE.GetOrAdd(type, CreateCache);

            return cache;
        }

        /// <summary>
        /// Creates a cache for the specified enum type and adds it to the master cache.
        /// </summary>
        /// <param name="type">The enum type.</param>
        /// <returns>Returns the cache for the enum type</returns>
        /// <exception cref="ArgumentNullException">Thrown if the <paramref name="type"/> is null.</exception>
        /// <exception cref="ArgumentException">
        /// Thrown if the <paramref name="type"/> is not an enum.
        /// </exception>
        private static EnumTypeCache
        CreateCache(Type type)
        {
            return new EnumTypeCache(type);
        }

        /// <summary>
        /// Converts an any of the valid <see cref="Enum"/> types to a <see cref="UInt64"/>.
        /// </summary>
        /// <param name="code">The type to convert the object to.</param>
        /// <param name="value">The object to convert.</param>
        /// <returns>Returns the <see cref="UInt64"/> equivalent of the object.</returns>
        /// <exception cref="NotSupportedException">Thrown of the value's type is not one of he supported <see cref="Enum"/> types.</exception>
        private static bool
        TryToUInt64(TypeCode code, object value, out ulong result)
        {
            result = 0;

            // Any negative numbers will be wrapped.
            switch (code)
            {
                case TypeCode.SByte:
                {
                    result = (ulong)(sbyte)value;

                    return true;
                }

                case TypeCode.Byte:
                {
                    result = (byte)value;

                    return true;
                }

                case TypeCode.Int16:
                {
                    result = (ulong)(short)value;

                    return true;
                }

                case TypeCode.UInt16:
                {
                    result = (ushort)value;

                    return true;
                }

                case TypeCode.UInt32:
                {
                    result = (uint)value;

                    return true;
                }

                case TypeCode.Int32:
                {
                    result = (ulong)(int)value;

                    return true;
                }

                case TypeCode.UInt64:
                {
                    result = (ulong)value;

                    return true;
                }

                case TypeCode.Int64:
                {
                    result = (ulong)(long)value;

                    return true;
                }
            }

            return false;
        }

        #endregion

    }
}
