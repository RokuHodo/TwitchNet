// standard namespaces
using System.Runtime.Serialization;

namespace
TwitchNet.Clients.Irc.Twitch
{
    public enum
    ChatCommand
    {
        #region Other

        /// <summary>
        /// Unsupported chat command.
        /// </summary>
        [EnumMember(Value = "")]
        Other = 0,

        #endregion

        #region General commands

        /// <summary>
        /// <para>Changes the client's display name color.</para>
        /// <para>Command: /color</para>
        /// <para>
        /// Usage (Non-Turbo/Prime): /color {color_name}.
        /// Usage (Turbo/Prime): /color {html_color} OR /color {color_name}.
        /// </para>
        /// </summary>
        [EnumMember(Value = "/color")]
        Color,

        /// <summary>
        /// <para>Disconnects the client from a chat room.</para>
        /// <para>Command: /disconnect</para>
        /// <para>Usage: /disconnect</para>
        /// </summary>
        [EnumMember(Value = "/disconnect")]
        Disconnect,

        /// <summary>
        /// <para>Gets information on how to use a specirfic command.</para>
        /// <para>Command: /help</para>
        /// <para>Usage: /help {command}</para>
        /// </summary>
        [EnumMember(Value = "/help")]
        Help,

        /// <summary>
        /// <para>Makes all text in a message the client's display name color.</para>
        /// <para>Command: /me</para>
        /// <para>Command: /me {message}</para>
        /// </summary>
        [EnumMember(Value = "/me")]
        Me,

        /// <summary>
        /// <para>Prints a list of all the mods in a specific chat.</para>
        /// <para>Command: /mods</para>
        /// <para>Usage: /mods</para>
        /// </summary>
        [EnumMember(Value = "/mods")]
        Mods,

        /// <summary>
        /// <para>Sends a private message to another user.</para>
        /// <para>Command: /w {user_nick} {message}</para>
        /// </summary>
        [EnumMember(Value = "/w")]
        Whisper,

        #endregion

        #region User moderation commands

        /// <summary>
        /// <para>Bans a user in a chat.</para>
        /// <para>Command: /ban</para>
        /// <para>Usage: /ban {user_nick}</para>
        /// </summary>
        [EnumMember(Value = "/ban")]
        Ban,

        /// <summary>
        /// <para>Grants a user moderator status.</para>
        /// <para>Command: /mod</para>
        /// <para>Usage: /mod {user_nick}</para>
        /// </summary>
        [EnumMember(Value = "/mod")]
        Mod,

        /// <summary>
        /// <para>Times out a user for a certain amount of time in a chat.</para>
        /// <para>Command: /timeout</para>
        /// <para>Usage: /timeout {useR_nick} [{length_seconds}] [{reason}]</para>
        /// </summary>
        [EnumMember(Value = "/timeout")]
        Timeout,

        /// <summary>
        /// <para>Unbans a user from a chat.</para>
        /// <para>Command: /unban</para>
        /// <para>Usage: /unban {user_nick}</para>
        /// </summary>
        [EnumMember(Value = "/unban")]
        Unban,

        /// <summary>
        /// <para>MRemoves a user's moderator status.</para>
        /// <para>Command: /unmod</para>
        /// <para>Usage: /unmod {user_nick}</para>
        /// </summary>
        [EnumMember(Value = "/unmod")]
        Unmod,

        /// <summary>
        /// <para>Untimesout a user from a chat.</para>
        /// <para>Command: /untimeout</para>
        /// <para>Usage: /untimeout {user_nick}</para>
        /// </summary>
        [EnumMember(Value = "/untimeout")]
        Untimeout,

        #endregion

        #region Room moderation commands

        /// <summary>
        /// <para>
        /// Clears all messages and chat history from a chat.
        /// This command does not work in a chat room.
        /// </para>
        /// <para>Command: /clear</para>
        /// <para>Usage: /clear</para>
        /// </summary>
        [EnumMember(Value = "/clear")]
        Clear,

        /// <summary>
        /// <para>
        /// Makes it so only emotes can be sent in chat.
        /// The broadcaster and moderators are exempt from this command.
        /// </para>
        /// <para>Command: /emoteonly</para>
        /// <para>Usage: /emoteonly</para>
        /// </summary>
        [EnumMember(Value = "/emoteonly")]
        EmoteOnly,

        /// <summary>
        /// <para>Disables emote only mode.</para>
        /// <para>Command: /emoteonlyoff</para>
        /// <para>Usage: /emoteonlyoff</para>
        /// </summary>
        [EnumMember(Value = "/emoteonlyoff")]
        EmoteOnlyOff,

        /// <summary>
        /// <para>
        /// Makes it so only followers can send messages in chat.
        /// This can require they be following in general or for a certain amount of time.
        /// The broadcaster and moderators are exempt from this command.
        /// This command does not work in a chat room.
        /// </para>
        /// <para>Command: /followers</para>
        /// <para>Usage: /followers [{length_custom}]</para>
        /// </summary>
        [EnumMember(Value = "/followers")]
        Followers,

        /// <summary>
        /// <para>
        /// Disables follower only mode.
        /// This command does not work in a chat room.
        /// </para>
        /// <para>Command: /followersoff</para>
        /// <para>Usage: /followersoff</para>
        /// </summary>
        [EnumMember(Value = "/followersoff")]
        FollowersOff,

        /// <summary>
        /// <para>
        /// Makes it so messages longer than 9 non-symbol unicode chacter messages must be unquie to be sent.
        /// The broadcaster and moderators are exempt from this command.
        /// </para>
        /// <para>Command: /r9kbeta</para>
        /// <para>Usage: /r9kbeta</para>
        /// </summary>
        [EnumMember(Value = "/r9kbeta")]
        R9kBeta,

        /// <summary>
        /// <para>Disables r9k beta mode.</para>
        /// <para>Command: /r9kbetaoff</para>
        /// <para>Usage: /r9kbetaoff</para>
        /// </summary>
        [EnumMember(Value = "/r9kbetaoff")]
        R9kBetaOff,

        /// <summary>
        /// <para>
        /// Makes it so users can only send messages every so often (rate limiting).
        /// The broadcaster and moderators are exempt from this command.
        /// </para>
        /// <para>Command: /slow</para>
        /// <para>Usage: /slow [{length_seconds}]</para>
        /// </summary>
        [EnumMember(Value = "/slow")]
        Slow,

        /// <summary>
        /// <para>Disables slow mode.</para>
        /// <para>Command: /slowoff</para>
        /// <para>Usage: /slowoff</para>
        /// </summary>
        [EnumMember(Value = "/slowoff")]
        SlowOff,

        /// <summary>
        /// <para>
        /// Makes it so only subscribers can send messages in chat.
        /// The broadcaster and moderators are exempt from this command.
        /// This command does not work in a chat room.
        /// </para>
        /// <para>Command: /subscribers</para>
        /// <para>Usage: /subscribers</para>
        /// </summary>
        [EnumMember(Value = "/subscribers")]
        Subscribers,

        /// <summary>
        /// <para>
        /// Disables subsdriber only mode.
        /// This command does not work in a chat room.
        /// </para>
        /// <para>Command: /subscribersoff</para>
        /// <para>Usage: /subscribersoff</para>
        /// </summary>
        [EnumMember(Value = "/subscribersoff")]
        SubscribersOff,

        #endregion

        #region Broadcaster and editor commands

        /// <summary>
        /// <para>
        /// Runs a commercial.
        /// This is a partner only command.
        /// This command does not work in a chat room.
        /// </para>
        /// <para>Command: /commercial</para>
        /// <para>Usage: /commercial [{length_seconds}]</para>
        /// </summary>
        [EnumMember(Value = "/commercial")]
        Commercial,

        /// <summary>
        /// <para>
        /// Hosts another user who is streaming.
        /// This command does not work in a chat room.
        /// </para>
        /// <para>Command: /host</para>
        /// <para>Usage: /host {user_nick} [{message}]</para>
        /// </summary>
        [EnumMember(Value = "/host")]
        Host,

        /// <summary>
        /// <para>
        /// Raids another user who is streaming.
        /// This command does not work in a chat room.
        /// </para>
        /// <para>Command: /raid</para>
        /// <para>Usage: /raid {user_nick}</para>
        /// </summary>
        [EnumMember(Value = "/raid")]
        Raid,

        /// <summary>
        /// <para>
        /// Stops hosting a user.
        /// This command does not work in a chat room.
        /// </para>
        /// <para>Command: /unhost</para>
        /// <para>Usage: /unhost</para>
        /// </summary>
        [EnumMember(Value = "/unhost")]
        Unhost,

        /// <summary>
        /// <para>
        /// Stops a raid in progress from happening.
        /// This command does not work in a chat room.
        /// </para>
        /// <para>Command: /unraid</para>
        /// <para>Usage: /unraid</para>
        /// </summary>
        [EnumMember(Value = "/unraid")]
        Unraid,

        // TODO: /marker 

        #endregion
    }
}