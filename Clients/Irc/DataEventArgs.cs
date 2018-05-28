// standard namespaces
using System;

// project namespaces
using TwitchNet.Debugger;

namespace
TwitchNet.Clients.Irc
{
    public class
    DataEventArgs : EventArgs
    {
        /// <summary>
        /// The byte data.
        /// </summary>
        [ValidateMember(Check.IsValid)]
        public byte[]   data    { get; protected set; }

        /// <summary>
        /// The UTF-8 encoded data.
        /// </summary>
        [ValidateMember(Check.IsValid)]
        public string   message { get; protected set; }

        /// <summary>
        /// Creates a new instance of the <see cref="DataEventArgs"/> class.
        /// </summary>
        /// <param name="data">The byte sata receieved.</param>
        /// <param name="message">The UTF-8 encoded data.</param>
        public DataEventArgs(byte[] data, string message)
        {
            this.data = data;

            this.message = message;
        } 
    }
}
