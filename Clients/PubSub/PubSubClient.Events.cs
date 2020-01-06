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
        /// <summary>
        /// Raised when a message is received from the PubSub websocket with the type <see cref="PubSubMessageType.Message"/>.
        /// Signifies that a payload has been sent associated with one of the subscribed topics.
        /// </summary>
        public event EventHandler<PubSubMessageEventArgs> OnPupSubMessage;

        /// <summary>
        /// Raised when a message is received from the PubSub websocket with the type <see cref="PubSubMessageType.Pong"/>.
        /// Signifies that the PING sent by the client was receieved and processed by PubSub sever, and that the websocket connection is still open.
        /// </summary>
        public event EventHandler<PubSubTypeEventArgs> OnPupSubPong;

        /// <summary>
        /// Raised when a message is received from the PubSub websocket with the type <see cref="PubSubMessageType.Reconnect"/>.
        /// Signifies that PubSub server is about to restart, and will disconnect the client within 30 seconds.
        /// It is recommended to reconnect to the PubSub websocket during this time.
        /// </summary>
        public event EventHandler<PubSubTypeEventArgs> OnPupSubReconnect;

        /// <summary>
        /// <para>
        /// Raised when a message is received from the PubSub websocket with the type <see cref="PubSubMessageType.Response"/>.
        /// Signifies that the subscription (LISTEN) or unsubscription (UNLISTEN) request sent by the client was receieved and processed by PubSub sever.
        /// </para>
        /// <para>
        /// If no error is returned with the response to a subscription request, the topic was succesfully subscribed to.
        /// If the same topic with the same user ID is subscribed or unsubscribed from multiple times, a response will be sent for each request.        
        /// </para>
        /// <para>Any successive identical subscription request(s) after the initial request will not count towards the maximum subscription limit.</para>
        /// </summary>
        public event EventHandler<PubSubResponseEventArgs> OnPupSubResponse;

        /// <summary>
        /// <para>
        /// Raised when a message is received from the PubSub websocket with the type <see cref="PubSubMessageType.Other"/>.
        /// Signifies that a payload has been sent but the associated message type is unrecognized or unsupported by the client.
        /// </para>
        /// </summary>
        public event EventHandler<PubSubTypeEventArgs> OnPupSubUnsupportedType;

        /// <summary>
        /// <para>
        /// Raised when a message is received from the PubSub websocket with the topic <see cref="PubSubTopic.ModeratorActions"/>.
        /// Signifies that a moderator banned, unbanned, timed-out, or untimed-out someone in a user's chat.
        /// </para>
        /// </summary>
        public event EventHandler<PubSubModeratorActionsEventArgs> OnPupSubModeratorAction;

        /// <summary>
        /// <para>
        /// Raised when a message is received from the PubSub websocket with the topic <see cref="PubSubTopic.Bits"/> or <see cref="PubSubTopic.BitsV2"/>.
        /// Signifies that someone cheered in a user's chat.
        /// </para>
        /// </summary>
        public event EventHandler<PubSubBitsEventArgs> OnPupSubBits;

        /// <summary>
        /// <para>
        /// Raised when a message is received from the PubSub websocket with the topic <see cref="PubSubTopic.BitsBadge"/>.
        /// Signifies that someone who cheered earned a new Bits badge and chose to share the notification
        /// </para>
        /// </summary>
        public event EventHandler<PubSubBitsBadgeEventArgs> OnPupSubBitsBadge;

        /// <summary>
        /// <para>
        /// Raised when a message is received from the PubSub websocket with the topic <see cref="PubSubTopic.ChannelPoints"/>.
        /// Signifies that someone redeemed a custom channel point reward in a user's chat.
        /// This does not occur when someone redeemed a standard/built-in reward, i.e., "Highlight My Message".
        /// </para>
        /// </summary>
        public event EventHandler<PubSubChannelPointsEventArgs> OnPupSubChannelPoints;

        /// <summary>
        /// <para>
        /// Raised when a message is received from the PubSub websocket with the topic <see cref="PubSubTopic.Subscriptions"/>.
        /// Signifies that someone subscribed, resubscribed, or was gifted a subscription to a user.
        /// </para>
        /// </summary>
        public event EventHandler<PubSubSubscriptionEventArgs> OnPupSubSubscription;

        /// <summary>
        /// <para>
        /// Raised when a message is received from the PubSub websocket with the topic <see cref="PubSubTopic.Whispers"/>.
        /// Signifies that the client received a whisper from another user.
        /// </para>
        /// </summary>
        public event EventHandler<PubSubWhisperEventArgs> OnPupSubWhisper;

        /// <summary>
        /// <para>
        /// Raised when a message is received from the PubSub websocket with the topic <see cref="PubSubTopic.Other"/>.
        /// Signifies that a payload has been sent but the associated topic is unrecognized or unsupported by the client.
        /// </para>
        /// </summary>
        public event EventHandler<PubSubMessageEventArgs> OnPupSubUnsupportedTopic;
    }

    public class
    PubSubMessageEventArgs : WebSocketEventArgs
    {
        /// <summary>
        /// The message received from the PubSub websocket.
        /// </summary>
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
    PubSubTypeEventArgs : WebSocketEventArgs
    {
        /// <summary>
        /// The message type received from the PubSub websocket.
        /// </summary>
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
        /// <summary>
        /// The response message received from the PubSub websocket after a subscription (LISTEN) or unsubscription (UNLISTEN) request.
        /// </summary>
        public PubSubResponse response { get; }

        public
        PubSubResponseEventArgs(MessageTextEventArgs args) : base(args)
        {
            response = JsonConvert.DeserializeObject<PubSubResponse>(args.message);
        }
    }    

    public class
    PubSubModeratorActionsEventArgs : PubSubMessageEventArgs
    {
        /// <summary>
        /// The deserialized moderator action payload data.
        /// </summary>
        public ModeratorAction action { get; }

        public
        PubSubModeratorActionsEventArgs(PubSubMessageEventArgs args) : base(args)
        {
            action = JsonConvert.DeserializeObject<ModeratorAction>(args.message.data.message);
        }
    }

    public class
    PubSubBitsEventArgs : PubSubMessageEventArgs
    {
        /// <summary>
        /// The deserialized Bits or Bits V2 payload data.
        /// </summary>
        public PubSubBits bits { get; }

        public
        PubSubBitsEventArgs(PubSubMessageEventArgs args) : base(args)
        {
            bits = JsonConvert.DeserializeObject<PubSubBits>(args.message.data.message);
        }
    }

    public class
    PubSubBitsBadgeEventArgs : PubSubMessageEventArgs
    {
        /// <summary>
        /// The deserialized Bits badge payload data.
        /// </summary>
        public PubSubBitsBadge bits_badge { get; }

        public
        PubSubBitsBadgeEventArgs(PubSubMessageEventArgs args) : base(args)
        {
            bits_badge = JsonConvert.DeserializeObject<PubSubBitsBadge>(args.message.data.message);
        }
    }

    public class
    PubSubChannelPointsEventArgs : PubSubMessageEventArgs
    {
        /// <summary>
        /// The deserialized channel points payload data.
        /// </summary>
        public PubSubChannelPoints channel_points { get; }

        public
        PubSubChannelPointsEventArgs(PubSubMessageEventArgs args) : base(args)
        {
            channel_points = JsonConvert.DeserializeObject<PubSubChannelPoints>(args.message.data.message);
        }
    }

    public class
    PubSubSubscriptionEventArgs : PubSubMessageEventArgs
    {
        /// <summary>
        /// The deserialized Twitch subscription payload data.
        /// </summary>
        public PubSubSubscription subscription { get; }

        public
        PubSubSubscriptionEventArgs(PubSubMessageEventArgs args) : base(args)
        {
            subscription = JsonConvert.DeserializeObject<PubSubSubscription>(args.message.data.message);
        }
    }

    public class
    PubSubWhisperEventArgs : PubSubMessageEventArgs
    {
        /// <summary>
        /// The deserialized whisper payload data.
        /// </summary>
        public PubSubWhisper whisper { get; }

        public
        PubSubWhisperEventArgs(PubSubMessageEventArgs args) : base(args)
        {
            whisper = JsonConvert.DeserializeObject<PubSubWhisper>(args.message.data.message);
        }
    }
}