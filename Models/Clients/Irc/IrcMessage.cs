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
        public Dictionary<string, string>   tags        { get; protected set; }

        public string                       prefix      { get; protected set; }
        public string                       command     { get; protected set; }
        public string                       trailing    { get; protected set; }

        public string[]                     middle      { get; protected set; }
        public string[]                     parameters  { get; protected set; }

        public IrcMessage(string message)
        {
            if (!message.IsValid())
            {
                return;
            }

            string message_post_tags    = ParseTags(message);
            string message_post_prefix  = ParsePrefix(message_post_tags);
            string message_post_command = ParseCommand(message_post_prefix);

            middle                      = ParseParameters(message_post_command).ToArray();
            parameters                  = AssembleParameters(middle, trailing);
        }

        #region Message parsing

        private string
        ParseTags(string message)
        {
            tags = new Dictionary<string, string>();

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

        public string
        ParsePrefix(string message_post_tags)
        {
            prefix = string.Empty;

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

            message_post_prefix = message_post_tags.TextAfter(' ').TrimStart(' ');

            return message_post_prefix;
        }

        private string
        ParseCommand(string message_post_prefix)
        {
            command = string.Empty;

            string message_post_command = string.Empty;

            if (!message_post_prefix.IsValid())
            {
                return message_post_command;
            }

            command = message_post_prefix.TextBefore(' ');

            message_post_command = message_post_prefix.TextAfter(' ').TrimStart(' ');

            return message_post_command;
        }

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
