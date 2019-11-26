﻿// standard namespaces
using System;

// imported .dll's
using Newtonsoft.Json;

namespace
TwitchNet.Clients.PubSub
{
    public partial class
    PubSubClient : WebSocketClient, IDisposable
    {
        public event EventHandler<PubSubTypeEventArgs>              OnPupSubUnsupportedType;

        public event EventHandler<PubSubTypeEventArgs>              OnPupSubPong;

        public event EventHandler<PubSubTypeEventArgs>              OnPupSubReconnect;

        public event EventHandler<PubSubResponseEventArgs>          OnPupSubResponse;

        public event EventHandler<PubSubMessageEventArgs>           OnPupSubUnsupportedTopic;

        public event EventHandler<PubSubMessageEventArgs>           OnPupSubMessage;

        public event EventHandler<PubSubWhisperEventArgs>           OnPupSubWhisper;

        public event EventHandler<PubSubModeratorActionsEventArgs>  OnPupSubModeratorAction;
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

        public
        PubSubMessageEventArgs(PubSubMessageEventArgs args) : base(args.time, args.uri, args.web_socket_id)
        {
            message = args.message;
        }
    }

    public class
    PubSubWhisperEventArgs : PubSubMessageEventArgs
    {
        public PubSubWhisper whisper { get; }

        public
        PubSubWhisperEventArgs(PubSubMessageEventArgs args) : base(args)
        {
            whisper = JsonConvert.DeserializeObject<PubSubWhisper>(args.message.data.message);
        }
    }

    public class
    PubSubModeratorActionsEventArgs : PubSubMessageEventArgs
    {
        public ModeratorAction action { get; }

        public
        PubSubModeratorActionsEventArgs(PubSubMessageEventArgs args) : base(args)
        {
            action = JsonConvert.DeserializeObject<ModeratorAction>(args.message.data.message);
        }
    }
}