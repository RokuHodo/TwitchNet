// project namespaces
using System;

namespace
TwitchNet.Extensions
{
    internal static class
    EventExtensions
    {
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
