// standard namespaces
using System;
using System.Drawing;

namespace
TwitchNet.Clients.Irc.Twitch
{
    public interface
    ISharedPrivmsgTags : ITags
    {
        /// <summary>
        /// Whether or not the sender is a moderator.
        /// </summary>
        bool        mod             { get; }

        /// <summary>
        /// Whether or not the sender is subscribed to the channel.
        /// </summary>
        bool        subscriber      { get; }

        /// <summary>
        /// Whether or not the sender has Twitch turbo.
        /// </summary>
        bool        turbo           { get; }

        /// <summary>
        /// Whether or not the body of the message only contains emotes.
        /// </summary>
        bool        emote_only      { get; }

        /// <summary>
        /// The unique message id.
        /// </summary>
        string      id              { get; }

        /// <summary>
        /// <para>The display name of the sender.</para>
        /// <para>This is empty if it was never set by the sender.</para>
        /// </summary>
        string      display_name    { get; }

        /// <summary>
        /// The user id of the sender.
        /// </summary>
        string      user_id         { get; }

        /// <summary>
        /// The user id of the channel the message was sent in.
        /// </summary>
        string      room_id         { get; }

        /// <summary>
        /// <para>The sender's user type</para>
        /// <para>Set to <see cref="UserType.None"/> if the sender has no elevated privileges.</para>
        /// </summary>
        UserType    user_type       { get; }

        /// <summary>
        /// <para>The color of the sender's display name.</para>
        /// <para>The color is <see cref="Color.Empty"/> if it was never set by the sender.</para>
        /// </summary>
        Color       color           { get; }

        /// <summary>
        /// The time the message was sent.
        /// </summary>
        DateTime    tmi_sent_ts     { get; }

        /// <summary>
        /// <para>The chat badges that the sender has, if any.</para>
        /// <para>The array is empty if the sender has no chat badges.</para>
        /// </summary>
        Badge[]     badges          { get; }

        /// <summary>
        /// <para>The emotes the sender used in the message, if any.</para>
        /// <para>The array is empty if the sender did not use any emotes.</para>
        /// </summary>
        Emote[]                     emotes { get; }
    }
}