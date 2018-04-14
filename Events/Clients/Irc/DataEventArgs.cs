// standard namespaces
using System;

namespace
TwitchNet.Events.Clients.Irc
{
    public class
    DataEventArgs : EventArgs
    {
        /// <summary>
        /// The byte data.
        /// </summary>
        public byte[] data { get; protected set; }

        /// <summary>
        /// The UTF-8 encoded data.
        /// </summary>
        public string message { get; protected set; }

        public DataEventArgs(byte[] data, string message)
        {
            this.data = data;

            this.message = message;
        } 
    }
}
