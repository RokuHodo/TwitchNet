// project namespaces
using System;

namespace
TwitchNet.Extensions
{
    internal static class
    EventExtensions
    {
        /// <summary>
        /// Raises an event handler.
        /// </summary>
        /// <typeparam name="event_args_type">The <see cref="Type"/> of the the event arguments.</typeparam>
        /// <param name="handler">The handler to raise.</param>
        /// <param name="sender">The event sender.</param>
        /// <param name="args">The event arguments.</param>
        public static void
        Raise<event_args_type>(this EventHandler<event_args_type> handler, object sender, event_args_type args)
        where event_args_type : EventArgs
        {
            if (handler.IsNull())
            {
                return;
            }

            handler(sender, args);
        }
    }
}
