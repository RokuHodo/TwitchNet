// standard namespaces
using System;

namespace TwitchNet.Models.Clients.Irc
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false)]
    public class TagAttribute : Attribute
    {
        public string name { get; private set; }

        public TagAttribute(string name)
        {
            this.name = name;
        }
    }
}
