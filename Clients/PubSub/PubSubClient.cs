// standard namespaces
using System;
using System.Drawing;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Timers;
using System.Threading.Tasks;

// project namespaces
using TwitchNet.Debugger;
using TwitchNet.Extensions;
using TwitchNet.Helpers.Json;
using TwitchNet.Utilities;

// imported .dll's
using Newtonsoft.Json;

namespace
TwitchNet.Clients.PubSub
{   
    public partial class
    PubSubClient : WebSocketClient, IDisposable
    {
        private Timer timer_ping;

        private Random timer_ping_jitter;

        public new PubSubSettings settings { get; set; }

        public
        PubSubClient() : base()
        {
            URI = new Uri("wss://pubsub-edge.twitch.tv");

            timer_ping = new Timer(10 * 60 * 1000);
            timer_ping.Elapsed += Callback_OnElapsed;

            Guid guid = Guid.NewGuid();
            timer_ping_jitter = new Random(guid.GetHashCode());

            settings = new PubSubSettings();
            // This is needed to synchronize the two settings. DO NOT REMOVE.
            base.settings = settings;

            OnOpened        += Callback_OnOpen;
            OnClosed        += Callback_OnClosed;
            OnWebSocketText += CallBack_OnWebSocketText;

            ResetPubSubHandlers();
        }

        #region Callbacks

        private void
        Callback_OnClosed(object sender, CloseEventArgs args)
        {
            Debug.WriteLine("Timers stopped.");
            timer_ping.Stop();
        }

        private void
        Callback_OnOpen(object sender, WebSocketEventArgs args)
        {
            Debug.WriteLine("Timers started.");
            timer_ping.Start();
        }

        private void
        CallBack_OnWebSocketText(object sender, MessageTextEventArgs args)
        {
            PubSubType check = JsonConvert.DeserializeObject<PubSubType>(args.message);
            RunPubSubHandler(check.type, args);
        }

        private async void
        Callback_OnElapsed(object sender, ElapsedEventArgs args)
        {
            int jitter = timer_ping_jitter.Next(-100, 100);
            await Task.Delay(jitter);

            Ping();
        }

        #endregion

        /// <summary>
        /// Force disconnects and frees all managed resources. The client will need to be re-instantiated to reconnect.
        /// This method should only be calle dafter disconnecting from the PubSub server.
        /// </summary>
        public sealed override void
        Dispose()
        {
            Dispose(true, Callback_Disposed);
        }

        private void
        Callback_Disposed()
        {
            if (!timer_ping.IsNull())
            {
                timer_ping.Stop();
                timer_ping.Dispose();
                timer_ping = null;
            }
        }

        #region Writing

        /// <summary>
        /// Sends a PING message to the Pubsub server.
        /// </summary>
        /// <returns></returns>
        public bool
        Ping()
        {
            // I tried sending this with a proper Ping control frame, but Twitch apparently only accepts Text data.
            // Twitch broke spec again? *fake_shocked_face.bmp*
            return Send("{\"type\": \"PING\"}");
        }

        public bool
        ListenBits(string oauth_token, string user_id, string nonce = "")
        {
            return SendListenOrUnlisten(new ListenUnlistenPayload(PubSubMessageType.Listen, oauth_token, string.Format(TopicFormat.BITS, user_id)));
        }

        public bool
        ListenBitsV2(string oauth_token, string user_id, string nonce = "")
        {
            return SendListenOrUnlisten(new ListenUnlistenPayload(PubSubMessageType.Listen, oauth_token, string.Format(TopicFormat.BITS_V2, user_id)));
        }

        public bool
        ListenBitsBadge(string oauth_token, string user_id, string nonce = "")
        {
            return SendListenOrUnlisten(new ListenUnlistenPayload(PubSubMessageType.Listen, oauth_token, string.Format(TopicFormat.BITS_BADGE, user_id)));
        }

        public bool
        ListenModeratorActions(string oauth_token, string user_id, string nonce = "")
        {
            return SendListenOrUnlisten(new ListenUnlistenPayload(PubSubMessageType.Listen, oauth_token, string.Format(TopicFormat.MODERATOR_ACTIONS, user_id)));
        }

        public bool
        ListenSubscriptions(string oauth_token, string user_id, string nonce = "")
        {
            return SendListenOrUnlisten(new ListenUnlistenPayload(PubSubMessageType.Listen, oauth_token, string.Format(TopicFormat.SUBSCRIPTIONS, user_id)));
        }

        public bool
        ListenWhispers(string oauth_token, string user_id, string nonce = "")
        {
            return SendListenOrUnlisten(new ListenUnlistenPayload(PubSubMessageType.Listen, oauth_token, string.Format(TopicFormat.WHISPERS, user_id)));
        }

        public bool
        Listen(string oauth_token, string topic, string nonce = "")
        {
            return SendListenOrUnlisten(new ListenUnlistenPayload(PubSubMessageType.Listen, oauth_token, topic, nonce));
        }

        public bool
        Listen(string oauth_token, string[] topics, string nonce = "")
        {
            return SendListenOrUnlisten(new ListenUnlistenPayload(PubSubMessageType.Listen, oauth_token, topics, nonce));
        }

        public bool
        UnlistenBits(string oauth_token, string user_id, string nonce = "")
        {
            return SendListenOrUnlisten(new ListenUnlistenPayload(PubSubMessageType.Unlisten, oauth_token, string.Format(TopicFormat.BITS, user_id)));
        }

        public bool
        UnlistenBitsV2(string oauth_token, string user_id, string nonce = "")
        {
            return SendListenOrUnlisten(new ListenUnlistenPayload(PubSubMessageType.Unlisten, oauth_token, string.Format(TopicFormat.BITS_V2, user_id)));
        }

        public bool
        UnlistenBitsBadge(string oauth_token, string user_id, string nonce = "")
        {
            return SendListenOrUnlisten(new ListenUnlistenPayload(PubSubMessageType.Unlisten, oauth_token, string.Format(TopicFormat.BITS_BADGE, user_id)));
        }

        public bool
        UnlistenModeratorActions(string oauth_token, string user_id, string nonce = "")
        {
            return SendListenOrUnlisten(new ListenUnlistenPayload(PubSubMessageType.Unlisten, oauth_token, string.Format(TopicFormat.MODERATOR_ACTIONS, user_id)));
        }

        public bool
        UnlistenSubscriptions(string oauth_token, string user_id, string nonce = "")
        {
            return SendListenOrUnlisten(new ListenUnlistenPayload(PubSubMessageType.Unlisten, oauth_token, string.Format(TopicFormat.SUBSCRIPTIONS, user_id)));
        }

        public bool
        UnlistenWhispers(string oauth_token, string user_id, string nonce = "")
        {
            return SendListenOrUnlisten(new ListenUnlistenPayload(PubSubMessageType.Unlisten, oauth_token, string.Format(TopicFormat.WHISPERS, user_id)));
        }

        public bool
        Unlisten(string oauth_token, string topic, string nonce = "")
        {
            return SendListenOrUnlisten(new ListenUnlistenPayload(PubSubMessageType.Unlisten, oauth_token, topic, nonce));
        }

        public bool
        Unlisten(string oauth_token, string[] topics, string nonce = "")
        {
            return SendListenOrUnlisten(new ListenUnlistenPayload(PubSubMessageType.Unlisten, oauth_token, topics, nonce));
        }

        public bool
        SendListenOrUnlisten(ListenUnlistenPayload payload)
        {
            if (!ValidateListenOrUnlistenPayload(payload))
            {
                return false;
            }

            return Send(JsonConvert.SerializeObject(payload));
        }

        private bool
        ValidateListenOrUnlistenPayload(ListenUnlistenPayload payload)
        {
            if (payload.IsNull())
            {
                ArgumentNullException inner_exception = new ArgumentNullException(nameof(payload));
                SetError(new PubSubException(PubSubInternalError.Payload_Null, "The payload data cannot be null.", inner_exception));

                return false;
            }

            if (payload.type != PubSubMessageType.Listen && payload.type != PubSubMessageType.Unlisten)
            {
                SetError(new PubSubArgumentException(PubSubInternalError.Payload_Type, "The payload type must either be 'Listen' or 'Unlisten'.", nameof(payload.type)));

                return false;
            }

            if (payload.data.IsNull())
            {
                ArgumentNullException inner_exception = new ArgumentNullException(nameof(payload.data));
                SetError(new PubSubException(PubSubInternalError.Payload_Data_Null, "The payload data cannot be null.", inner_exception));

                return false;
            }

            if (!payload.data.auth_token.HasContent())
            {
                SetError(new PubSubArgumentException(PubSubInternalError.Payload_OAuthToken_NoContent, "The OAuth token cannot be null, empty, or contain only whitesapce.", nameof(payload.data.auth_token)));

                return false;
            }

            if (!payload.data.topics.IsValid())
            {
                SetError(new PubSubException(PubSubInternalError.Payload_Topics_Empty, "The payload topics was null or an empty list."));

                return false;
            }

            string[] topic;
            for(int index = 0; index < payload.data.topics.Count; ++index)
            {
                if (!payload.data.topics[index].HasContent())
                {
                    SetError(new PubSubArgumentException(PubSubInternalError.Payload_Topic_NoContent, "The payload topic cannot be null, empty, or contain only whitespace."));

                    return false;
                }

                topic = payload.data.topics[index].Split(new char[] { '.' }, StringSplitOptions.RemoveEmptyEntries);
                if (!EnumUtil.TryParse(topic[0], out PubSubTopic _topic))
                {
                    SetError(new PubSubArgumentException(PubSubInternalError.Payload_Topic_Malformed, "The payload topic, " + payload.data.topics[index].WrapQuotes() + ", did not match any supported root topic.", nameof(topic)));

                    return false;
                }

                if (topic.Length == 1)
                {
                    SetError(new PubSubArgumentException(PubSubInternalError.Payload_Topic_Arguments, "No user ID was provided in the topic, " + payload.data.topics[index].WrapQuotes(), nameof(topic)));

                    return false;
                }
                else if (topic.Length > 2)
                {
                    SetError(new PubSubArgumentException(PubSubInternalError.Payload_Topic_Arguments, "Only one user ID can be provided in the topic, " + topic[0].WrapQuotes(), nameof(topic)));

                    return false;
                }

                if (!topic[1].HasContent())
                {
                    SetError(new PubSubArgumentException(PubSubInternalError.Payload_Topic_Arguments, "The topic user ID cannot be null, empty, or contain only whitespace.", nameof(topic)));

                    return false;
                }
            }

            return true;
        }

        private void
        SetError<type>(type exception)
        where type : Exception, IPubSubException
        {
            if(settings.handling_pubsub_internal_error == ErrorHandling.Return)
            {
                return;
            }

            throw exception;
        }
    }

    #endregion

    #region Settings and Errors

    public interface
    IPubSbSettings : IWebSocketSettings
    {
        /// <summary>
        /// How to handle intenral errors encountered within the library.
        /// </summary>
        ErrorHandling handling_pubsub_internal_error { get; set; }
    }

    public class
    PubSubSettings : WebSocketSettings, IPubSbSettings
    {
        /// <summary>
        /// How to handle intenral errors encountered within the library.
        /// </summary>
        public ErrorHandling handling_pubsub_internal_error { get; set; }

        public
        PubSubSettings()
        {
            handling_pubsub_internal_error = ErrorHandling.Error;
        }
    }

    public enum
    PubSubInternalError
    {
        /// <summary>
        /// The LISTEN/UNLISTEN payload is null.
        /// </summary>
        Payload_Null,

        /// <summary>
        /// The payload type is not set to <see cref="PubSubMessageType.Listen"/> or <see cref="PubSubMessageType.Unlisten"/>.
        /// </summary>
        Payload_Type,

        /// <summary>
        /// The LISTEN/UNLISTEN payload data is null.
        /// </summary>
        Payload_Data_Null,

        /// <summary>
        /// The LISTEN/UNLISTEN payload OAuth token is null, empty, or contains only whitesapce.
        /// </summary>
        Payload_OAuthToken_NoContent,

        /// <summary>
        /// The LISTEN/UNLISTEN payload topics is null or empty.
        /// </summary>
        Payload_Topics_Empty,

        /// <summary>
        /// A LISTEN/UNLISTEN payload topic is null, empty, or contains only whitesapce.
        /// </summary>
        Payload_Topic_NoContent,

        /// <summary>
        /// A LISTEN/UNLISTEN payload topic could not be parsed into one of the supported <see cref="PubSubTopic"/>'s.
        /// </summary>
        Payload_Topic_Malformed,

        /// <summary>
        /// A LISTEN/UNLISTEN payload topic contained no user ID, more than one user ID, or the user ID was null, empty, or contains only whitesapce.
        /// </summary>
        Payload_Topic_Arguments
    }

    public interface
    IPubSubException
    {
        /// <summary>
        /// The LISTEN/UNLISTEN payload request format error that was encountered.
        /// </summary>
        PubSubInternalError error { get; }
    }

    public class
    PubSubArgumentException : ArgumentException, IPubSubException
    {
        /// <summary>
        /// The LISTEN/UNLISTEN payload request format error that was encountered.
        /// </summary>
        public PubSubInternalError error { get; }

        /// <param name="error">The LISTEN/UNLISTEN payload request format error that was encountered.</param>
        /// <param name="message">The error message.</param>
        public
        PubSubArgumentException(PubSubInternalError error, string message) : base(message)
        {
            this.error = error;
        }

        /// <param name="error">The LISTEN/UNLISTEN payload request format error that was encountered.</param>
        /// <param name="message">The error message.</param>
        /// <param name="param_name">The name of the parameter that was formatted wrong.</param>
        public
        PubSubArgumentException(PubSubInternalError error, string message, string param_name) : base(message, param_name)
        {
            this.error = error;
        }

        /// <param name="error">The LISTEN/UNLISTEN payload request format error that was encountered.</param>
        /// <param name="message">The error message.</param>
        /// <param name="inner_exception">The secondary error that occured.</param>
        public
        PubSubArgumentException(PubSubInternalError error, string message, Exception inner_exception) : base(message, inner_exception)
        {
            this.error = error;
        }

        /// <param name="error">The LISTEN/UNLISTEN payload request format error that was encountered.</param>
        /// <param name="message">The error message.</param>
        /// <param name="param_name">The name of the parameter that was formatted wrong.</param>
        /// <param name="inner_exception">The secondary error that occured.</param>
        public
        PubSubArgumentException(PubSubInternalError error, string message, string param_name, Exception inner_exception) : base(message, param_name, inner_exception)
        {
            this.error = error;
        }
    }

    public class
    PubSubException : Exception, IPubSubException
    {
        public PubSubInternalError error { get; }

        /// <param name="error">The LISTEN/UNLISTEN payload request format error that was encountered.</param>
        /// <param name="message">The error message.</param>
        public
        PubSubException(PubSubInternalError error, string message) : base(message)
        {
            this.error = error;
        }

        /// <param name="error">The LISTEN/UNLISTEN payload request format error that was encountered.</param>
        /// <param name="message">The error message.</param>
        /// <param name="inner_exception">The secondary error that occured.</param>
        public
        PubSubException(PubSubInternalError error, string message, Exception inner_exception) : base(message, inner_exception)
        {
            this.error = error;
        }
    }

    #endregion

    #region General Structures

    [Flags]
    public enum
    PubSubScopes
    {
        /// <summary>
        /// <para>bits:read</para>
        /// <para>Required for the <see cref="PubSubTopic.Bits"/>, <see cref="PubSubTopic.BitsV2"/>, and <see cref="PubSubTopic.BitsBadge"/> topics.</para>
        /// </summary>
        [EnumMember(Value = "bits:read")]
        BitsRead = 1 << 0,

        /// <summary>
        /// <para>channel:moderate</para>
        /// <para>Required for the <see cref="PubSubTopic.ModeratorActions"/> topic.</para>
        /// </summary>
        [EnumMember(Value = "channel:moderate")]
        ChannelModerate = 1 << 1,

        /// <summary>
        /// <para>channel_subscriptions</para>
        /// <para>Required for the <see cref="PubSubTopic.Subscriptions"/> topic.</para>
        /// </summary>
        [EnumMember(Value = "channel_subscriptions")]
        ChannelSubscriptions = 1 << 2,

        /// <summary>
        /// <para>channel_subscriptions</para>
        /// <para>Required for the <see cref="PubSubTopic.Whispers"/> topic.</para>
        /// </summary>
        [EnumMember(Value = "whispers:read")]
        WhispersRead = 1 << 3,
    }

    [JsonConverter(typeof(EnumConverter))]
    public enum
    PubSubMessageType
    {
        /// <summary>
        /// Unsupported or unknown PubSub message.
        /// </summary>
        [EnumMember(Value = "Other")]
        Other = 0,

        /// <summary>
        /// A response from the server.
        /// This is a message contains the data from a topic that the client LISTEN'd to.
        /// </summary>
        [EnumMember(Value = "MESSAGE")]
        Message,

        /// <summary>
        /// A response from the server.
        /// This is an echo from the server after the client LISTEN's to a topic.
        /// </summary>
        [EnumMember(Value = "RESPONSE")]
        Response,

        /// <summary>
        /// A message sent by the client.
        /// This is sent by the client to start receiving updates from a specific topic.
        /// </summary>
        [EnumMember(Value = "LISTEN")]
        Listen,

        /// <summary>
        /// A message sent by the client.
        /// This is sent by the client to stop receiving updates from a specific topic.
        /// </summary>
        [EnumMember(Value = "UNLISTEN")]
        Unlisten,

        /// <summary>
        /// A message sent by the client.
        /// This is sent by the client to the server in order to keep the connection alive.
        /// </summary>
        [EnumMember(Value = "PING")]
        Ping,

        /// <summary>
        /// A response from the server.
        /// This is an echo indicating that the server received the client's PING.
        /// </summary>
        [EnumMember(Value = "PONG")]
        Pong,

        /// <summary>
        /// A response from the server.
        /// The client should reconnect to the server when this is received.
        /// </summary>
        [EnumMember(Value = "RECONNECT")]
        Reconnect,
    }

    public enum
    PubSubTopic
    {
        /// <summary>
        /// Unsupported or unknown PubSub topic.
        /// </summary>
        [EnumMember(Value = "Other")]
        Other = 0,

        /// <summary>
        /// <para>channel-bits-events-v1</para>
        /// <para>Required Scope: <see cref="PubSubScopes.BitsRead"/></para>
        /// </summary>
        [EnumMember(Value = "channel-bits-events-v1")]
        Bits,

        /// <summary>
        /// <para>channel-bits-events-v2</para>
        /// <para>Required Scope: <see cref="PubSubScopes.BitsRead"/></para>
        /// </summary>
        [EnumMember(Value = "channel-bits-events-v2")]
        BitsV2,

        /// <summary>
        /// <para>channel-bits-badge-unlocks</para>
        /// <para>Required Scope: <see cref="PubSubScopes.BitsRead"/></para>
        /// </summary>
        [EnumMember(Value = "channel-bits-badge-unlocks")]
        BitsBadge,

        /// <summary>
        /// <para>chat_moderator_actions</para>
        /// <para>Required Scope: <see cref="PubSubScopes.ChannelModerate"/></para>
        /// </summary>
        [EnumMember(Value = "chat_moderator_actions")]
        ModeratorActions,

        /// <summary>
        /// <para>channel-subscribe-events-v1</para>
        /// <para>Required Scope: <see cref="PubSubScopes.ChannelSubscriptions"/></para>
        /// </summary>
        [EnumMember(Value = "channel-subscribe-events-v1")]
        Subscriptions,

        /// <summary>
        /// <para>whispers</para>
        /// <para>Required Scope: <see cref="PubSubScopes.WhispersRead"/></para>
        /// </summary>
        [EnumMember(Value = "whispers")]
        Whispers,
    }

    public static class
    TopicFormat
    {
        /// <summary>
        /// channel-bits-events-v1.{0}
        /// </summary>
        public static readonly string BITS = "channel-bits-events-v1.{0}";

        /// <summary>
        /// channel-bits-events-v2.{0}
        /// </summary>
        public static readonly string BITS_V2 = "channel-bits-events-v2.{0}";

        /// <summary>
        /// channel-bits-badge-unlocks.{0}
        /// </summary>
        public static readonly string BITS_BADGE = "channel-bits-badge-unlocks.{0}";

        /// <summary>
        /// chat_moderator_actions.{0}
        /// </summary>
        public static readonly string MODERATOR_ACTIONS = "chat_moderator_actions.{0}";

        /// <summary>
        /// channel-subscribe-events-v1.{0}
        /// </summary>
        public static readonly string SUBSCRIPTIONS = "channel-subscribe-events-v1.{0}";

        /// <summary>
        /// whispers.{0}
        /// </summary>
        public static readonly string WHISPERS = "whispers.{0}";
    }

    [JsonConverter(typeof(EnumConverter))]
    public enum
    PubSubMessageDataType
    {
        /// <summary>
        /// Unsupported or unknown event data type.
        /// </summary>
        [EnumMember(Value = "Other")]
        Other = 0,

        /// <summary>
        /// The received data contains bits information.
        /// </summary>
        [EnumMember(Value = "bits_event")]
        BitsEvent,

        /// <summary>
        /// The received data contains whisper information.
        /// </summary>
        [EnumMember(Value = "whisper_received")]
        WhisperRceived,
    }

    #endregion

    #region Type: RESPONSE, MESSAGE, LISTEN, UNLISTEN

    public class
    PubSubType
    {
        /// <summary>
        /// The message type received from the PubSub server.
        /// </summary>
        [JsonProperty("type")]
        public PubSubMessageType type { get; set; }
    }

    public class
    PubSubResponse
    {
        /// <summary>
        /// The message type received from the PubSub server.
        /// </summary>
        [JsonProperty("type")]
        public PubSubMessageType type { get; set; }

        /// <summary>
        /// <para>An error that is associated with the PubSub request, if any.</para>
        /// <para>Set to an empty string if there is no error.</para>
        /// </summary>
        [JsonProperty("error")]
        public string error { get; set; }

        /// <summary>
        /// Random and unique string provided by the client to identify the response with the appropriate request.
        /// </summary>
        [JsonProperty("nonce")]
        public string nonce { get; set; }
    }

    public class
    PubSubMessage
    {
        /// <summary>
        /// The message type received from the PubSub server.
        /// </summary>
        [JsonProperty("type")]
        public PubSubMessageType type { get; set; }

        /// <summary>
        /// The response data sent pack from the PubSub servers.
        /// </summary>
        [JsonProperty("data")]
        public PubSubMessageData data { get; set; }
    }

    public class
    PubSubMessageData
    {
        /// <summary>
        /// The type of event data that was returned.
        /// </summary>
        [JsonProperty("topic")]
        public string topic { get; set; }

        /// <summary>
        /// The topic response data as an escaped JSON string.
        /// </summary>
        [JsonProperty("message")]
        public string message { get; set; }
    }

    public class
    ListenUnlistenPayload
    {
        /// <summary>
        /// The type of message being sent to the PubSub server.
        /// </summary>
        [JsonProperty("type")]
        public PubSubMessageType type;

        /// <summary>
        /// Random and unique string provided by the client to identify the response with the appropriate request.
        /// </summary>
        [JsonProperty("nonce")]
        public string nonce;

        /// <summary>
        /// The request data to be sent to the PubSub servers.
        /// </summary>
        [JsonProperty("data")]
        public ListenUnlistenPayloadData data;

        public
        ListenUnlistenPayload()
        {
            data = new ListenUnlistenPayloadData();
        }

        /// <param name="type">
        /// The type of message being sent to the PubSub server.
        /// Must be either <see cref="PubSubMessageType.Listen"/> or <see cref="PubSubMessageType.Unlisten"/>
        /// </param>
        /// <param name="oauth_token">The OAuth token to authorize the request.</param>
        /// <param name="topic">The formatted topic to be requested.</param>
        /// <param name="nonce">Random and unique string provided by the client to identify the response with the appropriate request.</param>
        public
        ListenUnlistenPayload(PubSubMessageType type, string oauth_token, string topic, string nonce = "")
        {
            this.type = type;
            this.nonce = nonce;

            data = new ListenUnlistenPayloadData();
            data.auth_token = oauth_token;
            data.topics.Add(topic);
        }

        /// <param name="type">
        /// The type of message being sent to the PubSub server.
        /// Must be either <see cref="PubSubMessageType.Listen"/> or <see cref="PubSubMessageType.Unlisten"/>
        /// </param>
        /// <param name="oauth_token">The OAuth token to authorize the request.</param>
        /// <param name="topic">The formatted topics to be requested.</param>
        /// <param name="nonce">Random and unique string provided by the client to identify the response with the appropriate request.</param>
        public
        ListenUnlistenPayload(PubSubMessageType type, string oauth_token, string[] topics, string nonce = "")
        {
            this.type = type;
            this.nonce = nonce;

            data = new ListenUnlistenPayloadData();
            data.auth_token = oauth_token;

            if (topics.IsNull())
            {
                return;
            }

            data.topics.AddRange(topics);
        }
    }

    public class
    ListenUnlistenPayloadData
    {
        /// <summary>
        /// The topics to LISTEN or UNISTEN to.
        /// </summary>
        [JsonProperty("topics")]
        public List<string> topics;

        /// <summary>
        /// The OAuth token to authorize the request.
        /// </summary>
        [JsonProperty("auth_token")]
        public string auth_token;

        public
        ListenUnlistenPayloadData()
        {
            topics = new List<string>();
        }
    }

    #endregion

    #region Topic: chat_moderator_actions.{user-id}

    public class
    ModeratorAction
    {
        [JsonProperty("data")]
        public ModeratorActionData data { get; protected set; }
    }

    public class
    ModeratorActionData
    {
        [JsonProperty("type")]
        public ModerationActionType type { get; protected set; }

        /// <summary>
        /// The moderation action that was performed, e.g., ban, unban, etc.
        /// </summary>
        [JsonProperty("moderation_action")]
        public ModerationAction moderation_action { get; protected set; }

        /// <summary>
        /// <para>
        /// The moderation action arguments used. The tpyical arguments are as follows (arguments in {brackets} are optional and may not be included):
        /// </para>
        /// <para>Ban: target_user_login, reason {optional}.</para>
        /// <para>Unban: target_user_login.</para>
        /// <para>Timeout: target_user_login, duration, {reason}.</para>
        /// <para>Untimeout: target_user_login.</para>
        /// </summary>
        [JsonProperty("args")]
        public List<string> args { get; protected set; }

        /// <summary>
        /// The ID of the user who performed the action.
        /// </summary>
        [JsonProperty("created_by")]
        public string created_by { get; protected set; }

        /// <summary>
        /// The login of the user who performed the action.
        /// </summary>
        [JsonProperty("created_by_user_id")]
        public string created_by_user_id { get; protected set; }

        /// <summary>
        /// The action message ID.
        /// </summary>
        [JsonProperty("msg_id")]
        public string msg_id { get; protected set; }

        /// <summary>
        /// The ID of the user that the action was performed on.
        /// </summary>
        [JsonProperty("target_user_id")]
        public string target_user_id { get; protected set; }

        /// <summary>
        /// The login of the user that the action was performed on.
        /// </summary>
        [JsonProperty("target_user_login")]
        public string target_user_login { get; protected set; }
    }

    [JsonConverter(typeof(EnumConverter))]
    public enum
    ModerationActionType
    {
        /// <summary>
        /// Unknown moderation type.
        /// </summary>
        [EnumMember(Value = "Other")]
        Other = 0,

        /// <summary>
        /// The user that performed the moderation action is the broadcaster.
        /// </summary>
        [EnumMember(Value = "chat_login_moderation")]
        ChatLoginModeration,

        /// <summary>
        /// The user that performed the moderation action is a moderator.
        /// </summary>
        [EnumMember(Value = "chat_channel_moderation")]
        ChatChannelModeration,
    }

    [JsonConverter(typeof(EnumConverter))]
    public enum
    ModerationAction
    {
        /// <summary>
        /// Unknown moderation action.
        /// </summary>
        [EnumMember(Value = "Other")]
        Other = 0,

        /// <summary>
        /// A user was banned.
        /// </summary>
        [EnumMember(Value = "ban")]
        Ban,

        /// <summary>
        /// A user was unbanned.
        /// </summary>
        [EnumMember(Value = "unban")]
        Unban,

        /// <summary>
        /// A user was timed out.
        /// </summary>
        [EnumMember(Value = "timeout")]
        Timeout,

        /// <summary>
        /// A user was untimed out.
        /// </summary>
        [EnumMember(Value = "untimeout")]
        Untimeout
    }

    #endregion

    #region Topic: channel-bits-events-v1.{user-id} and channel-bits-events-v2.{user-id}

    public class
    PubSubBits
    {
        /// <summary>
        /// <para>The ID of the user who used the bits.</para>
        /// <para>Set to null if the cheer was anonymous.</para>
        /// </summary>
        [JsonProperty("user_id")]
        public string user_id { get; protected set; }

        /// <summary>
        /// <para>The login name of the user who used the bits.</para>
        /// <para>Set to null if the cheer was anonymous.</para>
        /// </summary>
        [JsonProperty("user_name")]
        public string user_name { get; protected set; }

        /// <summary>
        /// The ID of the user who received the bits.
        /// </summary>
        [JsonProperty("channel_id")]
        public string channel_id { get; protected set; }

        /// <summary>
        /// The login name of the user who received the bits.
        /// </summary>
        [JsonProperty("channel_name")]
        public string channel_name { get; protected set; }

        /// <summary>
        /// When the cheer occured/bits were used.
        /// </summary>
        [JsonProperty("time")]
        public DateTime time { get; protected set; }

        /// <summary>
        /// The message that was sent with the cheer.
        /// All instances of "cheerXXX" or equivalent are still present in the message.
        /// </summary>
        [JsonProperty("chat_message")]
        public string chat_message { get; protected set; }

        /// <summary>
        /// The number of bits that were used.
        /// </summary>
        [JsonProperty("bits_used")]
        public int bits_used { get; protected set; }

        /// <summary>
        /// The total number of bits that the user has cheered in the corresponding channel.
        /// </summary>
        [JsonProperty("total_bits_used")]
        public int total_bits_used { get; protected set; }

        /// <summary>
        /// The bits event context.
        /// </summary>
        [JsonProperty("context")]
        public BitsContext context { get; protected set; }

        /// <summary>
        /// <para>Bits batch entitlement information.</para>
        /// <para>Set to null if the cheer was anonymous.</para>
        /// </summary>
        [JsonProperty("badge_entitlement")]
        public PubsubBadgeEntitlement badge_entitlement { get; protected set; }

        /// <summary>
        /// The version of the bits topic requested? (Or is this the version of the badge?)
        /// </summary>
        [JsonProperty("version")]
        public string version { get; protected set; }

        /// <summary>
        /// The PubSub message data type, i.e., the type of data contianed within the payload data.
        /// </summary>
        [JsonProperty("message_type")]
        public PubSubMessageDataType message_type { get; protected set; }

        /// <summary>
        /// The PubSub message ID.
        /// </summary>
        [JsonProperty("message_id")]
        public string message_id { get; protected set; }

        /// <summary>
        /// <para>Whether the cheer was dony anonymously.</para>
        /// <para>This field is always false if channel-bits-events-v1 was requested.</para>
        /// </summary>
        [JsonProperty("is_anonymous")]
        public bool is_anonymous { get; protected set; }
    }

    public class
    PubsubBadgeEntitlement
    {
        /// <summary>
        /// The bits badge tier the user just unlocked.
        /// </summary>
        [JsonProperty("new_version")]
        public int new_version { get; protected set; }

        /// <summary>
        /// The bits badge tier the user previously had.
        /// </summary>
        [JsonProperty("previous_version")]
        public int previous_version { get; protected set; }
    }

    [JsonConverter(typeof(EnumConverter))]
    public enum
    BitsContext
    {
        /// <summary>
        /// Uknown bits context.
        /// </summary>
        [EnumMember(Value = "ther")]
        Other = 0,

        /// <summary>
        /// Cheer
        /// </summary>
        [EnumMember(Value = "cheer")]
        Cheer,
    }

    #endregion
        
    #region Topic: channel-bits-badge-unlocks.{user-id}

    public class
    PubSubBitsBadge
    {
        /// <summary>
        /// The ID of the user who earned unlocked the new bits badge.
        /// </summary>
        [JsonProperty("user_id")]
        public string user_id { get; protected set; }

        /// <summary>
        /// The login name of the user who earned unlocked the new bits badge.
        /// </summary>
        [JsonProperty("user_name")]
        public string user_name { get; protected set; }

        /// <summary>
        /// The ID of the user who received the bits.
        /// </summary>
        [JsonProperty("channel_id")]
        public string channel_id { get; protected set; }

        /// <summary>
        /// The login name of the user who received the bits.
        /// </summary>
        [JsonProperty("channel_name")]
        public string channel_name { get; protected set; }

        /// <summary>
        /// The number of bits badge tier that was unlocked (1000, 10000, etc).
        /// </summary>
        [JsonProperty("badge_tier")]
        public int badge_tier { get; protected set; }

        /// <summary>
        /// The message that was sent with the cheer.
        /// All instances of "cheerXXX" or equivalent are removed from the message.
        /// </summary>
        [JsonProperty("chat_message")]
        public string chat_message { get; protected set; }

        /// <summary>
        /// When the bits badge was unlocked.
        /// </summary>
        [JsonProperty("time")]
        public DateTime time { get; protected set; }
    }

    #endregion

    #region Topic: channel-subscribe-events-v1.{user-id}

    public class
    PubSubSubscription
    {
        // ---------------------------------
        // Universal Subscripion Properties
        // ---------------------------------

        /// <summary>
        /// <para>The ID of the user who subscribed, or the ID of the user gifted the sub.</para>
        /// <para>Set to an empty string if the subscription was an anonymous gift subscription.</para>
        /// </summary>
        [JsonProperty("user_id")]
        public string user_id { get; protected set; }

        /// <summary>
        /// <para>The login name of the user who subscribed, or the login name of the user gifted the sub.</para>
        /// <para>Set to an empty string if the subscription was an anonymous gift subscription.</para>
        /// </summary>
        [JsonProperty("user_name")]
        public string user_name { get; protected set; }

        /// <summary>
        /// <para>The display name of the user who subscribed, or the display name of the user gifted the sub.</para>
        /// <para>Set to an empty string if the user never explicitly set their display name, or if the subscription was an anonymous gift subscription.</para>
        /// </summary>
        [JsonProperty("display_name")]
        public string display_name { get; protected set; }

        /// <summary>
        /// The ID of the user that subscribed was subscribed to.
        /// </summary>
        [JsonProperty("channel_id")]
        public string channel_id { get; protected set; }

        /// <summary>
        /// The login name of the user that was subscribed to.
        /// </summary>
        [JsonProperty("channel_name")]
        public string channel_name { get; protected set; }

        /// <summary>
        /// The time the subscription occured.
        /// </summary>
        [JsonProperty("time")]
        public DateTime time { get; protected set; }

        /// <summary>
        /// The subscription tier.
        /// </summary>
        [JsonProperty("sub_plan")]
        public SubscriptionTier sub_plan { get; protected set; }

        /// <summary>
        /// The name of the subscription plan.
        /// </summary>
        [JsonProperty("sub_plan_name")]
        public string sub_plan_name { get; protected set; }

        /// <summary>
        /// The total number of months the user has been subscribed.
        /// </summary>
        [JsonProperty("cumulative_months")]
        public int cumulative_months { get; protected set; }

        /// <summary>
        /// The number of consecutive months the user has been subscribed.
        /// </summary>
        [JsonProperty("streak_months")]
        public int streak_months { get; protected set; }

        /// <summary>
        /// The subscription context, i.e., (sub, resub, etc).
        /// </summary>
        [JsonProperty("context")]
        public SubscriptionContext context { get; protected set; }

        /// <summary>
        /// The chat messaged that was sent with the subscription.
        /// </summary>
        [JsonProperty("sub_message")]
        public SubscriptionMessage sub_message { get; protected set; }

        // ---------------------------------
        // Gift Subscripion Properties
        // ---------------------------------

        /// <summary>
        /// <para>The user ID of the user who was gifted the subscription.</para>
        /// <para>Set to an empty string if the subscription wasn't a gift.</para>
        /// </summary>
        [JsonProperty("recipient_id")]
        public string recipient_id { get; protected set; }

        /// <summary>
        /// <para>The login name of the user who was gifted the subscription.</para>
        /// <para>Set to an empty string if the subscription wasn't a gift.</para>
        /// </summary>
        [JsonProperty("recipient_user_name")]
        public string recipient_user_name { get; protected set; }

        /// <summary>
        /// <para>The display name of the user who was gifted the subscription.</para>
        /// <para>Set to an empty string if the user never explicitly set their display name or if the subscription wasn't a gift.</para>
        /// </summary>
        [JsonProperty("recipient_display_name")]
        public string recipient_display_name { get; protected set; }
    }

    public class
    SubscriptionMessage
    {
        /// <summary>
        /// The message body.
        /// </summary>
        public string message { get; protected set; }

        /// <para>The emotes that were used in the body of the message.</para>
        /// <para>Set to an empty list of the user didn't use any emotes.</para>
        public List<PubSubEmote> emotes { get; protected set; }
    }

    [JsonConverter(typeof(EnumConverter))]
    public enum
    SubscriptionContext
    {
        /// <summary>
        /// Unsupported or unknown subscription context.
        /// </summary>
        [EnumMember(Value = "other")]
        Other = 0,

        /// <summary>
        /// A user subscribed.
        /// </summary>
        [EnumMember(Value = "sub")]
        Sub,

        /// <summary>
        /// A user resubscribed.
        /// </summary>
        [EnumMember(Value = "resub")]
        Resub,

        /// <summary>
        /// A user gifted a subscription to another user.
        /// </summary>
        [EnumMember(Value = "subgift")]
        SubGift,

        /// <summary>
        /// A user anonymously gifted a subscription to another user.
        /// </summary>
        [EnumMember(Value = "anonsubguft")]
        AnonSubGift
    }

    #endregion

    #region Topic: whispers.{user-id}

    public class
    PubSubWhisper
    {
        /// <summary>
        /// The PubSub message data type, i.e., the type of data contianed within the payload data.
        /// </summary>
        [JsonProperty("type")]
        public PubSubMessageDataType type { get; protected set; }

        /// <summary>
        /// The whisper data in an escaped string form.
        /// </summary>
        [JsonProperty("data")]
        public string data { get; protected set; }

        /// <summary>
        /// The whisper data.
        /// </summary>
        [JsonProperty("data_object")]
        public PubSubWhisperDataObject data_object { get; protected set; }
    }

    public class
    PubSubWhisperDataObject
    {
        /// <summary>
        /// The PubSub message ID.
        /// </summary>
        [JsonProperty("message_id")]
        public string message_id { get; protected set; }

        /// <summary>
        /// The total number of whispers sent between each user.
        /// </summary>
        [JsonProperty("id")]
        public string id { get; protected set; }

        /// <summary>
        /// The user ID of the sender and the user ID of the recipient concatenated with an underscore.
        /// </summary>
        [JsonProperty("thread_id")]
        public string thread_id { get; protected set; }

        /// <summary>
        /// The whisper message that was sent.
        /// </summary>
        [JsonProperty("body")]
        public string body { get; protected set; }

        /// <summary>
        /// The time the whisper was sent.
        /// </summary>
        [JsonConverter(typeof(UnixEpochSecondsConverter))]
        [JsonProperty("sent_ts")]
        public DateTime sent_ts { get; protected set; }

        /// <summary>
        /// The sender's user ID.
        /// </summary>
        [JsonProperty("from_id")]
        public string from_id { get; protected set; }

        /// <summary>
        /// The sender's user information.
        /// </summary>
        [JsonProperty("tags")]
        public PubSubWhisperTags tags { get; protected set; }

        /// <summary>
        /// The recipient's user information.
        /// </summary>
        [JsonProperty("recipient")]
        public PubSubRecipient recipient { get; protected set; }

        /// <summary>
        /// A unique string generated by Twitch to identify the PubSub payload.
        /// </summary>
        [JsonProperty("nonce")]
        public string nonce { get; protected set; }
    }

    public class
    PubSubWhisperTags
    {
        /// <summary>
        /// The sender's login name.
        /// </summary>
        [JsonProperty("login")]
        public string login { get; protected set; }

        /// <summary>
        /// <para>The sender's display name.</para>
        /// <para>Set to an empty string if the user never explicitly set their display name.</para>
        /// </summary>
        [JsonProperty("display_name")]
        public string display_name { get; protected set; }

        /// <summary>
        /// <para>The sender's display name color.</para>
        /// <para>Set to <see cref="Color.Empty"/> if the user never explicitly set their display name color.</para>
        /// </summary>
        [JsonProperty("color")]
        public Color color { get; protected set; }

        /// <summary>
        /// <para>The emotes that were used in the body of the whisper.</para>
        /// <para>Set to an empty list of the user didn't use any emotes.</para>
        /// </summary>
        [JsonProperty("emotes")]
        public List<PubSubEmote> emotes { get; protected set; }

        /// <summary>
        /// <para>The sender's badges.</para>
        /// <para>Set to an empty list of the user has no badges.</para>
        /// </summary>
        [JsonProperty("badges")]
        public List<PubSubBadge> badges { get; protected set; }
    }

    public class
    PubSubEmote
    {
        /// <summary>
        /// The emote ID.
        /// </summary>
        [JsonProperty("id")]
        public string id { get; protected set; }

        /// <summary>
        /// The index in the message where the first emote character is located.
        /// </summary>
        [JsonProperty("start")]
        public int start { get; protected set; }

        /// <summary>
        /// The index in the message where the last emote character is located.
        /// </summary>
        [JsonProperty("end")]
        public int end { get; protected set; }
    }

    public class
    PubSubBadge
    {
        /// <summary>
        /// The badge ID.
        /// </summary>
        [JsonProperty("id")]
        public string id { get; protected set; }

        /// <summary>
        /// The badge version.
        /// </summary>
        [JsonProperty("version")]
        public int version { get; protected set; }
    }

    public class
    PubSubRecipient
    {
        /// <summary>
        /// The user ID of the recipient.
        /// </summary>
        [JsonProperty("id")]
        public string id { get; protected set; }

        /// <summary>
        /// The recipient's login name.
        /// </summary>
        [JsonProperty("username")]
        public string username { get; protected set; }

        /// <summary>
        /// <para>The recipient's display name.</para>
        /// <para>Set to an empty string if the user never explicitly set their display name.</para>
        /// </summary>
        [JsonProperty("display_name")]
        public string display_name { get; protected set; }

        /// <summary>
        /// <para>The recipient's display name color.</para>
        /// <para>Set to <see cref="Color.Empty"/> if the user never explicitly set their display name color.</para>
        /// </summary>
        [JsonProperty("color")]
        public Color color { get; protected set; }

        /// <summary>
        /// <para>The recipient's profile image.</para>
        /// <para>Set to null if the user never uploaded a profile image.</para>
        /// </summary>
        [JsonProperty("profile_image")]
        public string profile_image { get; protected set; }
    }

    #endregion
}