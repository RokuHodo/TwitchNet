// standard namespaces
using System;
using System.Collections.Generic;

// projetc namespaces
using TwitchNet.Extensions;
using TwitchNet.Utilities;

namespace
TwitchNet.Clients.PubSub
{
    public partial class
    PubSubClient : WebSocketClient, IDisposable
    {
        public delegate void PubSubHandlerType(PubSubMessageType type, MessageTextEventArgs args);
        public delegate void PubSubHandlerTopic(PubSubTopic topic, PubSubMessageEventArgs args);

        private Dictionary<PubSubMessageType, PubSubHandlerType> handlers_pub_sub_type;
        private Dictionary<PubSubTopic, PubSubHandlerTopic> handlers_pub_sub_topic;

        public void
        ResetPubSubHandlers()
        {
            handlers_pub_sub_type = new Dictionary<PubSubMessageType, PubSubHandlerType>();
            handlers_pub_sub_topic = new Dictionary<PubSubTopic, PubSubHandlerTopic>();

            SetPubSubHandler(PubSubMessageType.Other,       PubSubHandler_Type_Unsupported);
            SetPubSubHandler(PubSubMessageType.Pong,        PubSubHandler_Type_Pong);
            SetPubSubHandler(PubSubMessageType.Reconnect,   PubSubHandler_Type_Reconnect);
            SetPubSubHandler(PubSubMessageType.Response,    PubSubHandler_Type_Response);
            SetPubSubHandler(PubSubMessageType.Message,     PubSubHandler_Type_Message);

            SetPubSubHandler(PubSubTopic.Other,             PubSubHandler_Topic_Unsupported);
            SetPubSubHandler(PubSubTopic.Whispers,          PubSubHandler_Topic_Whispers);
            SetPubSubHandler(PubSubTopic.ModeratorActions,  PubSubHandler_Topic_ModeratorActions);
        }

        public bool
        SetPubSubHandler(PubSubMessageType type, PubSubHandlerType handler)
        {
            ExceptionUtil.ThrowIfNull(handler, nameof(handler));

            if (handlers_pub_sub_type.IsNull())
            {
                return false;
            }

            handlers_pub_sub_type[type] = handler;

            return true;
        }

        public bool
        SetPubSubHandler(PubSubTopic topic, PubSubHandlerTopic handler)
        {
            ExceptionUtil.ThrowIfNull(handler, nameof(handler));

            if (handlers_pub_sub_topic.IsNull())
            {
                return false;
            }

            handlers_pub_sub_topic[topic] = handler;

            return true;
        }

        public bool
        RemovePubSubHandler(PubSubMessageType type)
        {
            if (handlers_pub_sub_type.IsNull())
            {
                return true;
            }

            if (!handlers_pub_sub_type.ContainsKey(type))
            {
                return false;
            }

            return handlers_pub_sub_type.Remove(type);
        }

        public bool
        RemovePubSubHandler(PubSubTopic topic)
        {
            if (handlers_pub_sub_topic.IsNull())
            {
                return true;
            }

            if (!handlers_pub_sub_topic.ContainsKey(topic))
            {
                return false;
            }

            return handlers_pub_sub_topic.Remove(topic);
        }

        private void
        RunPubSubHandler(PubSubMessageType type, MessageTextEventArgs args)
        {
            if (handlers_pub_sub_type.IsNull())
            {
                return;
            }

            if (!handlers_pub_sub_type.ContainsKey(type))
            {
                return;
            }

            handlers_pub_sub_type[type](type, args);
        }

        private void
        RunPubSubHandler(PubSubTopic topic, PubSubMessageEventArgs args)
        {
            if (handlers_pub_sub_topic.IsNull())
            {
                return;
            }

            if (!handlers_pub_sub_topic.ContainsKey(topic))
            {
                return;
            }

            handlers_pub_sub_topic[topic](topic, args);
        }

        private void
        PubSubHandler_Type_Unsupported(PubSubMessageType type, MessageTextEventArgs args)
        {
            OnPupSubUnsupportedType.Raise(this, new PubSubTypeEventArgs(args, type));
        }

        private void
        PubSubHandler_Type_Pong(PubSubMessageType type, MessageTextEventArgs args)
        {
            OnPupSubPong.Raise(this, new PubSubTypeEventArgs(args, type));
        }

        private void
        PubSubHandler_Type_Reconnect(PubSubMessageType type, MessageTextEventArgs args)
        {
            OnPupSubReconnect.Raise(this, new PubSubTypeEventArgs(args, type));
        }

        private void
        PubSubHandler_Type_Response(PubSubMessageType type, MessageTextEventArgs args)
        {
            OnPupSubResponse.Raise(this, new PubSubResponseEventArgs(args));
        }

        private void
        PubSubHandler_Type_Message(PubSubMessageType type, MessageTextEventArgs args)
        {
            PubSubMessageEventArgs args_pub_sub = new PubSubMessageEventArgs(args);
            OnPupSubMessage.Raise(this, args_pub_sub);

            string _topic = args_pub_sub.message.data.topic.TextBefore('.');
            EnumUtil.TryParse(_topic, out PubSubTopic topic);

            RunPubSubHandler(topic, args_pub_sub);
        }

        private void
        PubSubHandler_Topic_Unsupported(PubSubTopic type, PubSubMessageEventArgs args)
        {
            OnPupSubUnsupportedTopic.Raise(this, new PubSubMessageEventArgs(args));
        }

        private void
        PubSubHandler_Topic_Whispers(PubSubTopic type, PubSubMessageEventArgs args)
        {
            OnPupSubWhisper.Raise(this, new PubSubWhisperEventArgs(args));
        }

        private void
        PubSubHandler_Topic_ModeratorActions(PubSubTopic type, PubSubMessageEventArgs args)
        {
            OnPupSubModeratorAction.Raise(this, new PubSubModeratorActionsEventArgs(args));
        }
    }
}