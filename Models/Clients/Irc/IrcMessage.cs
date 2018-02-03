// standard namespaces
using System.Collections.Generic;

// project namespaces
using TwitchNet.Extensions;

namespace
TwitchNet.Models.Clients.Irc
{
    public class
    IrcMessage
    {
        #region Properties

        /// <summary>
        /// The raw string ingested by the reader.
        /// </summary>
        public string                       raw             { get; protected set; }

        /// <summary>
        /// The optional tags prefixed to the message.
        /// </summary>
        public Dictionary<string, string>   tags            { get; protected set; }

        /// <summary>
        /// An optional part of the message.
        /// If the prefix is provided, the server name or nick is always provided, and the user and/or host may also be included.
        /// </summary>
        public string                       prefix          { get; protected set; }

        /// <summary>
        /// The server name or the nick of the user.
        /// Contained within the prefix.
        /// </summary>
        public string                       server_or_nick  { get; protected set; }

        /// <summary>
        /// The irc user.
        /// Contained within the prefix.
        /// </summary>
        public string                       user            { get; protected set; }

        /// <summary>
        /// The host of the irc.
        /// Contained within the prefix.
        /// </summary>
        public string                       host            { get; protected set; }

        /// <summary>
        /// The irc command.
        /// </summary>
        public string                       command         { get; protected set; }

        /// <summary>
        /// A message parameter.
        /// Any, possibly empty, sequence of octets not including NUL or CR or LF.
        /// </summary>
        public string                       trailing        { get; protected set; }

        /// <summary>
        /// An array of message parameters.
        /// Any non-empty sequence of octets not including SPACE or NUL or CR or LF.
        /// </summary>
        public string[]                     middle          { get; protected set; }

        /// <summary>
        /// An array of all middle parameters and trailing.
        /// </summary>
        public string[]                     parameters      { get; protected set; }

        #endregion

        #region Constructor

        public IrcMessage(string message)
        {

            raw = message;

            tags = new Dictionary<string, string>();

            prefix = string.Empty;
            server_or_nick = string.Empty;
            user = string.Empty;
            host = string.Empty;

            command = string.Empty;

            trailing = string.Empty;

            if (!message.IsValid())
            {
                return;
            }

            string message_post_tags = ParseTags(message);
            string message_post_prefix = ParsePrefix(message_post_tags);
            string message_post_command = ParseCommand(message_post_prefix);

            middle = ParseParameters(message_post_command).ToArray();
            parameters = AssembleParameters(middle, trailing);
        }

        #endregion        

        #region Parsing

        /// <summary>
        /// Parses an irc message for tags, if present.
        /// </summary>
        /// <param name="message">The irc message to parse.</param>
        /// <returns>Returns the irc message after the tags.</returns>
        private string
        ParseTags(string message)
        {
            string message_no_tags = message;

            // irc message only conmtains tags when it is preceeded with "@"
            if (message[0] != '@')
            {
                return message_no_tags;
            }

            string all_tags = message.TextBetween('@', ' ');
            string[] array = all_tags.StringToArray<string>(';');
            foreach (string element in array)
            {
                string[] tag = element.StringToArray<string>('=');
                if(tag.IsNull() || tag.Length != 2)
                {
                    continue;
                }

                tags[tag[0]] = tag[1];
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
        ParsePrefix(string message_post_tags)
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

            if(user_index < 0 && host_index < 0)
            {
                server_or_nick = prefix;
            }
            else if(user_index != -1 && host_index < 0)
            {
                server_or_nick = prefix.TextBefore('!');
                user = prefix.TextAfter('!');
            }
            else
            {
                server_or_nick = prefix.TextBefore('!');
                user = prefix.TextBetween('!', '@');
                host = prefix.TextAfter('@');
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
        ParseCommand(string message_post_prefix)
        {
            string message_post_command = string.Empty;

            if (!message_post_prefix.IsValid())
            {
                return message_post_command;
            }

            command = message_post_prefix.TextBefore(' ');

            message_post_command = message_post_prefix.TextAfter(' ').TrimStart(' ');

            return message_post_command;
        }

        /// <summary>
        /// Parses an irc message for the parameters (middle and trailing).
        /// </summary>
        /// <param name="message_post_command">The irc message after the command.</param>
        /// <returns>Returns an middle array of parameters.</returns>
        private List<string>
        ParseParameters(string message_post_command)
        {
            List<string> _middle = new List<string>();

            if (!message_post_command.IsValid())
            {
                return _middle;
            }

            if(message_post_command[0] == ':')
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

                    List<string> temp = ParseParameters(message_post_command);
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
        /// <param name="_middle">The array of middle parameters.</param>
        /// <param name="_trailing">The trailing parameter.</param>
        /// <returns>The combined parameters array.</returns>
        private string[]
        AssembleParameters(string[] _middle, string _trailing)
        {
            List<string> _parameters = new List<string>();

            foreach(string element in _middle)
            {
                _parameters.Add(element);
            }

            _parameters.Add(trailing);

            return _parameters.ToArray();
        }

        #endregion
    }
}
