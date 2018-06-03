// standard namespaces
using System;
using System.Collections.Generic;

// project namespaces
using TwitchNet.Rest;
using TwitchNet.Rest.Api.Entitlements;
using TwitchNet.Rest.Api.Streams;
using TwitchNet.Rest.Api.Users;
using TwitchNet.Rest.Api.Videos;
using TwitchNet.Clients.Irc.Twitch;
using TwitchNet.Debugger;
using TwitchNet.Extensions;

// TODO: Add support for bitfield enums when converting.
//       This should only be a concern when adding enums as query parameters during rest requests.
//       Right now this is handled within the method that adds the parameters to the request, but we should prbably do this here to have "native" support.

namespace
TwitchNet.Utilities
{
    public static class
    EnumCacheUtil
    {
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

            return value;
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

        #region Universal enum converters

        /// <summary>
        /// Converts a string into an <see cref="Enum"/> value.
        /// </summary>
        /// <param name="str">The string to convert.</param>
        /// <param name="type">The type of the enum to convert the string into.</param>
        /// <returns>
        /// Returns the corresponding <see cref="Enum"/> value if the string is found in the enum's type cache, or exists in the enum names.
        /// Returns the default value of the enum type otherwise.
        /// </returns>
        /// <exception cref="NotSupportedException">Thrown if the type is not an enum.</exception>
        public static object
        ToEnum(string str, Type type)
        {
            if (!type.IsEnum)
            {
                throw new NotSupportedException("Type " + type.Name.WrapQuotes() + " is not an enum.");
            }

            object value = type.GetDefaultValue();

            if (str.IsNull())
            {
                return value;
            }

            if (type == typeof(BadgeType))
            {
                value = ToBadgeType(str);
            }
            else if (type == typeof(ChatCommand))
            {
                value = ToChatCommand(str);
            }
            else if (type == typeof(CommercialLength))
            {
                value = ToCommercialLength(str);
            }
            else if (type == typeof(DisplayNameColor))
            {
                value = ToDisplayNameColor(str);
            }
            else if (type == typeof(FollowersDurationPeriod))
            {
                value = ToFollowersDurationPeriod(str);
            }
            else if (type == typeof(NoticeType))
            {
                value = ToNoticeType(str);
            }
            else if (type == typeof(RitualType))
            {
                value = ToRitualType(str);
            }
            else if (type == typeof(SubscriptionPlan))
            {
                value = ToSubscriptionPlan(str);
            }
            else if (type == typeof(UserNoticeType))
            {
                value = ToUserNoticeType(str);
            }
            else if (type == typeof(EntitlementType))
            {
                value = ToEntitlementType(str);
            }
            else if (type == typeof(StreamLanguage))
            {
                value = ToStreamLanguage(str);
            }
            else if (type == typeof(StreamType))
            {
                value = ToStreamType(str);
            }
            else if (type == typeof(BroadcasterType))
            {
                value = ToBroadcasterType(str);
            }
            else if (type == typeof(BroadcasterLanguage))
            {
                value = ToBroadcasterLanguage(str);
            }
            else if (type == typeof(VideoPeriod))
            {
                value = ToVideoPeriod(str);
            }
            else if (type == typeof(VideoSort))
            {
                value = ToVideoSort(str);
            }
            else if (type == typeof(VideoType))
            {
                value = ToVideoType(str);
            }
            else if (type == typeof(Scopes))
            {
                value = ToScopes(str);
            }
            else if (type == typeof(UserType))
            {
                value = ToUserType(str);
            }
            else
            {
                Debug.WriteError(ErrorLevel.Minor, "The enum type " + type.Name.WrapQuotes() + " is not natively supported by the EnumCacheUtil.");

                string[] names = Enum.GetNames(type);
                if (!names.IsValid())
                {
                    return value;
                }

                HashSet<string> hash = new HashSet<string>(names);
                if (!hash.Contains(str))
                {
                    return value;
                }

                value = Enum.Parse(type, str);
            }

            return value;
        }

        /// <summary>
        /// Converts an <see cref="Enum"/> value to a string.
        /// </summary>
        /// <param name="value">The enum value to convert.</param>
        /// <returns>
        /// Returns the corresponding enum name if the <see cref="Enum"/> value is found in the cache.
        /// Returns the enum name obtained from <see cref="Enum.GetName(Type, object)"/> otherwise.
        /// </returns>
        public static string
        FromEnum(Enum value)
        {
            string name = string.Empty;

            Type type = value.GetType();
            if (type == typeof(BadgeType))
            {
                name = FromBadgeType((BadgeType)value);
            }
            else if (type == typeof(ChatCommand))
            {
                name = FromChatCommand((ChatCommand)value);
            }
            else if (type == typeof(CommercialLength))
            {
                name = FromCommercialLength((CommercialLength)value);
            }
            else if (type == typeof(DisplayNameColor))
            {
                name = FromDisplayNameColor((DisplayNameColor)value);
            }
            else if (type == typeof(FollowersDurationPeriod))
            {
                name = FromFollowersDurationPeriod((FollowersDurationPeriod)value);
            }
            else if (type == typeof(NoticeType))
            {
                name = FromNoticeType((NoticeType)value);
            }
            else if (type == typeof(RitualType))
            {
                name = FromRitualType((RitualType)value);
            }
            else if (type == typeof(SubscriptionPlan))
            {
                name = FromSubscriptionPlan((SubscriptionPlan)value);
            }
            else if (type == typeof(UserNoticeType))
            {
                name = FromUserNoticeType((UserNoticeType)value);
            }
            else if (type == typeof(EntitlementType))
            {
                name = FromEntitlementType((EntitlementType)value);
            }
            else if (type == typeof(StreamLanguage))
            {
                name = FromStreamLanguage((StreamLanguage)value);
            }
            else if (type == typeof(StreamType))
            {
                name = FromStreamType((StreamType)value);
            }
            else if (type == typeof(BroadcasterType))
            {
                name = FromBroadcasterType((BroadcasterType)value);
            }
            else if (type == typeof(BroadcasterLanguage))
            {
                name = FromBroadcasterLanguage((BroadcasterLanguage)value);
            }
            else if (type == typeof(VideoPeriod))
            {
                name = FromVideoPeriod((VideoPeriod)value);
            }
            else if (type == typeof(VideoSort))
            {
                name = FromVideoSort((VideoSort)value);
            }
            else if (type == typeof(VideoType))
            {
                name = FromVideoType((VideoType)value);
            }
            else if (type == typeof(Scopes))
            {
                name = FromScopes((Scopes)value);
            }
            else if (type == typeof(UserType))
            {
                name = FromUserType((UserType)value);
            }
            else
            {
                Debug.WriteError(ErrorLevel.Minor, "The enum type " + type.Name.WrapQuotes() + " is not natively supported by the EnumCacheUtil.");

                name = Enum.GetName(type, value);
            }

            return name;
        }

        #endregion
    }
}
