// standard namespaces
using System;

// imported .dll's
using Newtonsoft.Json;

namespace
TwitchNet.Clients.PubSub
{
    public partial class
    PubSubClient : WebSocketClient, IDisposable
    {
        public event EventHandler<PubSubTypeEventArgs>      OnPupSubPong;

        public event EventHandler<PubSubTypeEventArgs>      OnPupSubReconnect;

        public event EventHandler<PubSubResponseEventArgs>  OnPupSubResponse;

        public event EventHandler<PubSubMessageEventArgs>   OnPupSubMessage;

        public event EventHandler<PubSubWhisperEventArgs>   OnPupSubMessageWhisper;

    }

    public class
    PubSubTypeEventArgs : WebSocketEventArgs
    {
        public PubSubMessageType type { get; }

        public
        PubSubTypeEventArgs(MessageTextEventArgs args, PubSubMessageType type) : base(args)
        {
            this.type = type;
        }
    }

    public class
    PubSubResponseEventArgs : WebSocketEventArgs
    {
        public PubSubResponse response { get; }

        public
        PubSubResponseEventArgs(MessageTextEventArgs args) : base(args)
        {
            response = JsonConvert.DeserializeObject<PubSubResponse>(args.message);
        }
    }

    public class
    PubSubMessageEventArgs : WebSocketEventArgs
    {
        public PubSubMessage message { get; }

        public
        PubSubMessageEventArgs(MessageTextEventArgs args) : base(args)
        {
            message = JsonConvert.DeserializeObject<PubSubMessage>(args.message);
        }
    }

    public class
    PubSubWhisperEventArgs : PubSubMessageEventArgs
    {
        public PubSubWhisper whisper { get; }

        public
        PubSubWhisperEventArgs(PubSubMessageEventArgs args_pub_sub, MessageTextEventArgs args) : base(args)
        {
            whisper = JsonConvert.DeserializeObject<PubSubWhisper>(args_pub_sub.message.data.message);
        }
    }
}