// standard namespaces
using System.Collections.Generic;

// project namespaces
using TwitchNet.Extensions;

namespace
TwitchNet.Clients.Irc
{
    public readonly struct
    IrcMessage
    {
        #region Properties

        /// <summary>
        /// The byte data received from the socket.
        /// </summary>
        public readonly byte[]                      data;

        /// <summary>
        /// The UTF-8 encoded byte data received from the socket.
        /// </summary>
        public readonly string                      raw;

        /// <summary>
        /// The optional tags prefixed to the message.
        /// </summary>
        public readonly Dictionary<string, string>  tags;

        /// <summary>
        /// An optional part of the message.
        /// If the prefix is provided, the server name or nick is always provided, and the user and/or host may also be included.
        /// </summary>
        public readonly string                      prefix;

        /// <summary>
        /// The server name or the nick of the user.
        /// Contained within the prefix.
        /// </summary>
        public readonly string                      server_or_nick;

        /// <summary>
        /// The irc user.
        /// Contained within the prefix.
        /// </summary>
        public readonly string                      user;

        /// <summary>
        /// The host of the irc.
        /// Contained within the prefix.
        /// </summary>
        public readonly string                      host;

        /// <summary>
        /// The irc command.
        /// </summary>
        public readonly string                      command;

        /// <summary>
        /// A message parameter.
        /// Any, possibly empty, sequence of octets not including NUL or CR or LF.
        /// </summary>
        public readonly string                      trailing;

        /// <summary>
        /// An array of message parameters.
        /// Any non-empty sequence of octets not including SPACE or NUL or CR or LF.
        /// </summary>
        public readonly string[]                    middle;

        /// <summary>
        /// An array of all middle parameters and trailing.
        /// </summary>
        public readonly string[]                    parameters;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of the <see cref="IrcMessage"/> class.
        /// </summary>
        /// <param name="data">The data received from the socket.</param>
        /// <param name="raw">The UTF-8 encoded byte data received from the socket.</param>
        public IrcMessage(byte[] data, string raw)
        {
            this.data                   = data;
            this.raw                    = raw;

            tags                        = new Dictionary<string, string>();

            prefix                      = string.Empty;
            server_or_nick              = string.Empty;
            user                        = string.Empty;
            host                        = string.Empty;

            command                     = string.Empty;

            middle                      = new string[0];
            trailing                    = string.Empty;
            parameters                  = new string[0];

            string message_post_tags    = ParseTags(raw, ref tags);
            string message_post_prefix  = ParsePrefix(message_post_tags, ref prefix, ref server_or_nick, ref user, ref host);
            string message_post_command = ParseCommand(message_post_prefix, ref command);

            middle                      = ParseParameters(message_post_command, ref trailing).ToArray();
            parameters                  = AssembleParameters(middle, trailing);
        }

        #endregion        

        #region Parsing

        /// <summary>
        /// Parses an irc message for tags, if present.
        /// </summary>
        /// <param name="message">The irc message to parse.</param>
        /// <returns>Returns the irc message after the tags.</returns>
        private string
        ParseTags(string message, ref Dictionary<string, string> tags)
        {
            string message_no_tags = message;

            // irc message only conmtains tags when it is preceeded with "@"
            if (message[0] != '@')
            {
                return message_no_tags;
            }

            string all_tags = message.TextBetween('@', ' ');
            string[] array = all_tags.Split(';');
            foreach (string element in array)
            {
                string tag = element.TextBefore('=');
                string value = element.TextAfter('=');
                if (!tag.IsValid())
                {
                    continue;
                }

                tags[tag] = value;
            }

            // Get rid of the tags to make later parsing easier
            message_no_tags = message.TextAfter(' ').TrimStart(' ');

            return message_no_tags;
        }

        /// <summary>
        /// Parses an irc message for the prefix, if present.
        /// </summary>
        /// <param name="message_post_tags">The irc message after the tags.</param>
        /// <returns>Returns the irc message after the prefix.</returns>
        public string
        ParsePrefix(string message_post_tags, ref string prefix, ref string server_or_nick, ref string user, ref string host)
        {
            string message_post_prefix = string.Empty;

            if (!message_post_tags.IsValid())
            {
                return message_post_prefix;
            }

            if (message_post_tags[0] != ':')
            {
                message_post_prefix = message_post_tags;

                return message_post_prefix;
            }

            prefix = message_post_tags.TextBetween(':', ' ');

            int user_index = prefix.IndexOf('!');
            int host_index = prefix.IndexOf('@');

            if (user_index < 0 && host_index < 0)
            {
                server_or_nick = prefix;
            }
            else if (user_index != -1 && host_index < 0)
            {
                server_or_nick  = prefix.TextBefore('!');
                user            = prefix.TextAfter('!');
            }
            else
            {
                server_or_nick  = prefix.TextBefore('!');
                user            = prefix.TextBetween('!', '@');
                host            = prefix.TextAfter('@');
            }

            message_post_prefix = message_post_tags.TextAfter(' ').TrimStart(' ');

            return message_post_prefix;
        }

        /// <summary>
        /// Parses an irc message for the commmand.
        /// </summary>
        /// <param name="message_post_prefix">The irc message after the prefix.</param>
        /// <returns>Returns the irc message after the command.</returns>
        private string
        ParseCommand(string message_post_prefix, ref string command)
        {
            string message_post_command = string.Empty;

            if (!message_post_prefix.IsValid())
            {
                return message_post_command;
            }

            command = message_post_prefix.TextBefore(' ');
            if (!command.IsValid())
            {
                //If there's no space after the command, it's the end of the message
                command = message_post_prefix;
            }
            else
            {
                message_post_command = message_post_prefix.TextAfter(' ').TrimStart(' ');
            }

            return message_post_command;
        }

        /// <summary>
        /// Parses an irc message for the parameters (middle and trailing).
        /// </summary>
        /// <param name="message_post_command">The irc message after the command.</param>
        /// <returns>Returns an middle array of parameters.</returns>
        private List<string>
        ParseParameters(string message_post_command, ref string trailing)
        {
            List<string> _middle = new List<string>();

            if (!message_post_command.IsValid())
            {
                return _middle;
            }

            if (message_post_command[0] == ':')
            {
                string parameter = message_post_command.TextAfter(':');

                trailing = parameter;
            }
            else
            {
                string parameter = message_post_command.TextBefore(' ');
                if (parameter.IsValid())
                {
                    _middle.Add(parameter);

                    message_post_command = message_post_command.TextAfter(' ').TrimStart(' ');

                    List<string> temp = ParseParameters(message_post_command, ref trailing);
                    if (temp.IsValid())
                    {
                        _middle.AddRange(temp);
                    }
                }
                else
                {
                    _middle.Add(message_post_command);
                }
            }

            return _middle;
        }

        /// <summary>
        /// Combines the middle and trailing into a single parameters array.
        /// </summary>
        /// <param name="middle">The array of middle parameters.</param>
        /// <param name="trailing">The trailing parameter.</param>
        /// <returns>The combined parameters array.</returns>
        private string[]
        AssembleParameters(string[] middle, string trailing)
        {
            List<string> parameters = new List<string>();

            foreach (string element in middle)
            {
                parameters.Add(element);
            }

            parameters.Add(trailing);

            return parameters.ToArray();
        }

        #endregion
    }
}