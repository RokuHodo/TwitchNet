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
        private Dictionary<PubSubMessageType, PubSubHandlerType> handlers_pub_sub_type;
        private Dictionary<PubSubTopic, PubSubHandlerTopic> handlers_pub_sub_topic;

        /// <summary>
        /// The function signature for the <see cref="PubSubMessageType"/> handlers.
        /// </summary>
        /// <param name="type">The PubSub message type.</param>
        /// <param name="args">The text event arguments from the WebSocket</param>
        public delegate void PubSubHandlerType(PubSubMessageType type, MessageTextEventArgs args);

        /// <summary>
        /// The function signature for the <see cref="PubSubTopic"/> handlers.
        /// </summary>
        /// <param name="topic">The event data type.</param>
        /// <param name="args">The message event arguments from the PubSub server.</param>
        public delegate void PubSubHandlerTopic(PubSubTopic topic, PubSubMessageEventArgs args);

        /// <summary>
        /// Resets the <see cref="PubSubMessageType"/> and <see cref="PubSubTopic"/> PubSub handlers.
        /// </summary>
        public void
        ResetPubSubHandlers()
        {
            handlers_pub_sub_type = new Dictionary<PubSubMessageType, PubSubHandlerType>();
            handlers_pub_sub_topic = new Dictionary<PubSubTopic, PubSubHandlerTopic>();

            SetPubSubHandler(PubSubMessageType.Message,     PubSubHandler_Type_Message);
            SetPubSubHandler(PubSubMessageType.Pong,        PubSubHandler_Type_Pong);
            SetPubSubHandler(PubSubMessageType.Reconnect,   PubSubHandler_Type_Reconnect);
            SetPubSubHandler(PubSubMessageType.Response,    PubSubHandler_Type_Response);
            SetPubSubHandler(PubSubMessageType.Other,       PubSubHandler_Type_Unsupported);

            SetPubSubHandler(PubSubTopic.ModeratorActions,  PubSubHandler_Topic_ModeratorActions);
            SetPubSubHandler(PubSubTopic.Bits,              PubSubHandler_Topic_Bits);
            SetPubSubHandler(PubSubTopic.BitsV2,            PubSubHandler_Topic_Bits);
            SetPubSubHandler(PubSubTopic.BitsBadge,         PubSubHandler_Topic_BitsBadge);
            SetPubSubHandler(PubSubTopic.ChannelPoints,     PubSubHandler_Topic_ChannelPoints);
            SetPubSubHandler(PubSubTopic.Subscriptions,     PubSubHandler_Topic_Subscription);
            SetPubSubHandler(PubSubTopic.Whispers,          PubSubHandler_Topic_Whispers);
            SetPubSubHandler(PubSubTopic.Other,             PubSubHandler_Topic_Unsupported);
        }

        /// <summary>
        /// Set a <see cref="PubSubMessageType"/> message handler.
        /// </summary>
        /// <param name="type">The PubSub message type.</param>
        /// <param name="handler">The message handler</param>
        /// <returns>
        /// Returns true if the handler was successfully set.
        /// Returns false if the handler is null.
        /// Returns false otherwise.
        /// </returns>
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

        /// <summary>
        /// Removes a <see cref="PubSubMessageType"/> message handler.
        /// </summary>
        /// <param name="type">The PubSub message type.</param>
        /// <returns>
        /// Returns true if the handler was successfully removed.
        /// Returns false if no handler exists for the specified type.
        /// Returns false otherwise.
        /// </returns>
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

        /// <summary>
        /// Set a <see cref="PubSubMessageType"/> message handler.
        /// </summary>
        /// <param name="topic">The event data type.</param>
        /// <param name="handler">The message handler</param>
        /// <returns>
        /// Returns true if the handler was successfully set.
        /// Returns false if the handler is null.
        /// Returns false otherwise.
        /// </returns>
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

        /// <summary>
        /// Removes a <see cref="PubSubMessageType"/> message handler.
        /// </summary>
        /// <param name="topic">The event data type.</param>
        /// <returns>
        /// Returns true if the handler was successfully removed.
        /// Returns false if no handler exists for the specified type.
        /// Returns false otherwise.
        /// </returns>
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
        PubSubHandler_Type_Message(PubSubMessageType type, MessageTextEventArgs args)
        {
            PubSubMessageEventArgs args_pub_sub = new PubSubMessageEventArgs(args);
            OnPupSubMessage.Raise(this, args_pub_sub);

            string _topic = args_pub_sub.message.data.topic.TextBefore('.');
            EnumUtil.TryParse(_topic, out PubSubTopic topic);

            RunPubSubHandler(topic, args_pub_sub);
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
        PubSubHandler_Type_Unsupported(PubSubMessageType type, MessageTextEventArgs args)
        {
            OnPupSubUnsupportedType.Raise(this, new PubSubTypeEventArgs(args, type));
        }

        private void
        PubSubHandler_Topic_ModeratorActions(PubSubTopic type, PubSubMessageEventArgs args)
        {
            OnPupSubModeratorAction.Raise(this, new PubSubModeratorActionsEventArgs(args));
        }

        private void
        PubSubHandler_Topic_Bits(PubSubTopic type, PubSubMessageEventArgs args)
        {
            OnPupSubBits.Raise(this, new PubSubBitsEventArgs(args));
        }

        private void
        PubSubHandler_Topic_BitsBadge(PubSubTopic type, PubSubMessageEventArgs args)
        {
            OnPupSubBitsBadge.Raise(this, new PubSubBitsBadgeEventArgs(args));
        }

        private void
        PubSubHandler_Topic_ChannelPoints(PubSubTopic type, PubSubMessageEventArgs args)
        {
            OnPupSubChannelPoints.Raise(this, new PubSubChannelPointsEventArgs(args));
        }

        private void
        PubSubHandler_Topic_Subscription(PubSubTopic type, PubSubMessageEventArgs args)
        {
            OnPupSubSubscription.Raise(this, new PubSubSubscriptionEventArgs(args));
        }

        private void
        PubSubHandler_Topic_Whispers(PubSubTopic type, PubSubMessageEventArgs args)
        {
            OnPupSubWhisper.Raise(this, new PubSubWhisperEventArgs(args));
        }

        private void
        PubSubHandler_Topic_Unsupported(PubSubTopic type, PubSubMessageEventArgs args)
        {
            OnPupSubUnsupportedTopic.Raise(this, new PubSubMessageEventArgs(args));
        }
    }
}