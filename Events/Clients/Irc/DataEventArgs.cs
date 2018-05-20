// standard namespaces
using System;

// project namespaces
using TwitchNet.Debugger;
using TwitchNet.Enums.Debugger;

namespace
TwitchNet.Events.Clients.Irc
{
    public class
    DataEventArgs : EventArgs
    {
        /// <summary>
        /// The byte data.
        /// </summary>
        [ValidateMember(Check.IsValid)]
        public byte[] data { get; protected set; }

        /// <summary>
        /// The UTF-8 encoded data.
        /// </summary>
        [ValidateMember(Check.IsValid)]
        public string message { get; protected set; }

        public DataEventArgs(byte[] data, string message)
        {
            this.data = data;

            this.message = message;
        } 
    }
}
