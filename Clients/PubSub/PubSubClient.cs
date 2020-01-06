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
            // IMPORTANT: This is needed to synchronize the settings. DO NOT REMOVE.
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

        #region Managed Resource Handling

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

        #endregion

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

        /// <summary>
        /// <para>
        /// Listens to a PubSub topic.
        /// If the topic has already been listened to, it is silently ignored.
        /// </para>
        /// <para>
        /// Required Scope: Varies.
        /// See <see cref="PubSubScopes"/> to see what topic(s) each scope applies to.
        /// </para>
        /// </summary>
        /// <param name="oauth_token">The OAuth token to authorize the request.</param>
        /// <param name="topic">The topic to listen to.</param>
        /// <param name="nonce">A random and unique string used to identify the response with the request.</param>
        /// <returns>
        /// Returns true if the payload was successfully sent to the PubSub websocket.
        /// Returns false otherwise.
        /// </returns>
        /// <exception cref="PubSubArgumentException">
        /// Thrown if the payload OAuth token or the topic is null, empty, or contained only whitespace.
        /// Thrown if the payload topic could not be parsed into one of the supported <see cref="PubSubTopic"/>'s.
        /// Thrown if the payload topic contained no user ID, more than one user ID, or the user ID was null, empty, or contains only whitesapce
        /// </exception>
        public bool
        Listen(string oauth_token, string topic, string nonce = "")
        {
            return Send(new PubSubSendPayload(PubSubMessageType.Listen, oauth_token, topic, nonce));
        }

        /// <summary>
        /// Unlistens from a PubSub topic.
        /// If the topic hasn't been previously listened to, it is silently ignored.
        /// </summary>
        /// <param name="oauth_token">The OAuth token to authorize the request.</param>
        /// <param name="topic">The topic to unlisten to.</param>
        /// <param name="nonce">A random and unique string used to identify the response with the request.</param>
        /// <returns>
        /// Returns true if the payload was successfully sent to the PubSub websocket.
        /// Returns false otherwise.
        /// </returns>
        /// <exception cref="PubSubArgumentException">
        /// Thrown if the payload OAuth token or the topic is null, empty, or contained only whitespace.
        /// Thrown if the payload topic could not be parsed into one of the supported <see cref="PubSubTopic"/>'s.
        /// Thrown if the payload topic contained no user ID, more than one user ID, or the user ID was null, empty, or contains only whitesapce
        /// </exception>
        public bool
        Unlisten(string oauth_token, string topic, string nonce = "")
        {
            return Send(new PubSubSendPayload(PubSubMessageType.Unlisten, oauth_token, topic, nonce));
        }

        /// <summary>
        /// <para>
        /// Listens to a list of PubSub topics.
        /// If a topic has already been listened to, it is silently ignored.
        /// </para>
        /// <para>
        /// Required Scope: Varies.
        /// See <see cref="PubSubScopes"/> to see what topic(s) each scope applies to.
        /// </para>
        /// </summary>
        /// <param name="oauth_token">The OAuth token to authorize the request.</param>
        /// <param name="topics">The topics to listen to.</param>
        /// <param name="nonce">A random and unique string used to identify the response with the request.</param>
        /// <returns>
        /// Returns true if the payload was successfully sent to the PubSub websocket.
        /// Returns false otherwise.
        /// </returns>
        /// <exception cref="PubSubArgumentException">
        /// Thrown if the payload OAuth token or any topic is null, empty, or contained only whitespace.
        /// Thrown if any payload topic could not be parsed into one of the supported <see cref="PubSubTopic"/>'s.
        /// Thrown if any payload topic contained no user ID, more than one user ID, or the user ID was null, empty, or contains only whitesapce
        /// </exception>
        public bool
        Listen(string oauth_token, string[] topics, string nonce = "")
        {
            return Send(new PubSubSendPayload(PubSubMessageType.Listen, oauth_token, topics, nonce));
        }

        /// <summary>
        /// Unlistens from a list of PubSub topics.
        /// If a topic hasn't been previously listened to, it is silently ignored.
        /// </summary>
        /// <param name="oauth_token">The OAuth token to authorize the request.</param>
        /// <param name="topics">The topics to unlisten to.</param>
        /// <param name="nonce">A random and unique string used to identify the response with the request.</param>
        /// <returns>
        /// Returns true if the payload was successfully sent to the PubSub websocket.
        /// Returns false otherwise.
        /// </returns>
        /// <exception cref="PubSubArgumentException">
        /// Thrown if the payload OAuth token or any topic is null, empty, or contained only whitespace.
        /// Thrown if any payload topic could not be parsed into one of the supported <see cref="PubSubTopic"/>'s.
        /// Thrown if any payload topic contained no user ID, more than one user ID, or the user ID was null, empty, or contains only whitesapce
        /// </exception>
        public bool
        Unlisten(string oauth_token, string[] topics, string nonce = "")
        {
            return Send(new PubSubSendPayload(PubSubMessageType.Unlisten, oauth_token, topics, nonce));
        }

        /// <summary>
        /// <para>
        /// Listens to a PubSub moderator action topic.
        /// Receieve a payload when any moderator bans, unbans, timeouts, or untimeouts someone in the provided user's chat.
        /// The moderator who performed the action is identified.
        /// If the topic has already been listened to, it is silently ignored.
        /// </para>
        /// <para>Required Scope: <see cref="PubSubScopes.ChannelModerate"/>.</para>
        /// </summary>
        /// <param name="oauth_token">The OAuth token to authorize the request.</param>
        /// <param name="user_id">The user ID associated with the request.</param>
        /// <param name="nonce">A random and unique string used to identify the response with the request.</param>
        /// <returns>
        /// Returns true if the payload was successfully sent to the PubSub websocket.
        /// Returns false otherwise.
        /// </returns>
        /// <exception cref="PubSubArgumentException">
        /// Thrown if the payload OAuth token is null, empty, or contained only whitespace.
        /// Thrown if more than one user ID was provided, or the user ID was null, empty, or contains only whitesapce.
        /// </exception>
        public bool
        ListenModeratorActions(string oauth_token, string user_id, string nonce = "")
        {
            return Send(new PubSubSendPayload(PubSubMessageType.Listen, oauth_token, string.Format(TopicFormat.MODERATOR_ACTIONS, user_id), nonce));
        }

        /// <summary>
        /// Unlistens from a PubSub moderator action topic.
        /// If the topic hasn't been previously listened to, it is silently ignored.
        /// </summary>
        /// <param name="oauth_token">The OAuth token to authorize the request.</param>
        /// <param name="user_id">The user ID associated with the request.</param>
        /// <param name="nonce">A random and unique string used to identify the response with the request.</param>
        /// <returns>
        /// Returns true if the payload was successfully sent to the PubSub websocket.
        /// Returns false otherwise.
        /// </returns>
        /// <exception cref="PubSubArgumentException">
        /// Thrown if the payload OAuth token is null, empty, or contained only whitespace.
        /// Thrown if more than one user ID was provided, or the user ID was null, empty, or contains only whitesapce.
        /// </exception>
        public bool
        UnlistenModeratorActions(string oauth_token, string user_id, string nonce = "")
        {
            return Send(new PubSubSendPayload(PubSubMessageType.Unlisten, oauth_token, string.Format(TopicFormat.MODERATOR_ACTIONS, user_id), nonce));
        }

        /// <summary>
        /// <para>
        /// Listens to a PubSub bits topic.
        /// Receieve a payload when anyone cheers in the provided user's chat.
        /// If the topic has already been listened to, it is silently ignored.
        /// </para>
        /// <para>Required Scope: <see cref="PubSubScopes.BitsRead"/>.</para>
        /// </summary>
        /// <param name="oauth_token">The OAuth token to authorize the request.</param>
        /// <param name="user_id">The user ID associated with the request.</param>
        /// <param name="nonce">A random and unique string used to identify the response with the request.</param>
        /// <returns>
        /// Returns true if the payload was successfully sent to the PubSub websocket.
        /// Returns false otherwise.
        /// </returns>
        /// <exception cref="PubSubArgumentException">
        /// Thrown if the payload OAuth token is null, empty, or contained only whitespace.
        /// Thrown if more than one user ID was provided, or the user ID was null, empty, or contains only whitesapce.
        /// </exception>
        public bool
        ListenBits(string oauth_token, string user_id, string nonce = "")
        {
            return Send(new PubSubSendPayload(PubSubMessageType.Listen, oauth_token, string.Format(TopicFormat.BITS, user_id), nonce));
        }

        /// <summary>
        /// Unlistens from a PubSub bits topic.
        /// If the topic hasn't been previously listened to, it is silently ignored.
        /// </summary>
        /// <param name="oauth_token">The OAuth token to authorize the request.</param>
        /// <param name="user_id">The user ID associated with the request.</param>
        /// <param name="nonce">A random and unique string used to identify the response with the request.</param>
        /// <returns>
        /// Returns true if the payload was successfully sent to the PubSub websocket.
        /// Returns false otherwise.
        /// </returns>
        /// <exception cref="PubSubArgumentException">
        /// Thrown if the payload OAuth token is null, empty, or contained only whitespace.
        /// Thrown if more than one user ID was provided, or the user ID was null, empty, or contains only whitesapce.
        /// </exception>
        public bool
        UnlistenBits(string oauth_token, string user_id, string nonce = "")
        {
            return Send(new PubSubSendPayload(PubSubMessageType.Unlisten, oauth_token, string.Format(TopicFormat.BITS, user_id), nonce));
        }

        /// <summary>
        /// <para>
        /// Listens to a PubSub bits v2 topic.
        /// Receieve a payload when anyone cheers in the provided user's chat.
        /// If the topic has already been listened to, it is silently ignored.
        /// </para>
        /// <para>Required Scope: <see cref="PubSubScopes.BitsRead"/>.</para>
        /// </summary>
        /// <param name="oauth_token">The OAuth token to authorize the request.</param>
        /// <param name="user_id">The user ID associated with the request.</param>
        /// <param name="nonce">A random and unique string used to identify the response with the request.</param>
        /// <returns>
        /// Returns true if the payload was successfully sent to the PubSub websocket.
        /// Returns false otherwise.
        /// </returns>
        /// <exception cref="PubSubArgumentException">
        /// Thrown if the payload OAuth token is null, empty, or contained only whitespace.
        /// Thrown if more than one user ID was provided, or the user ID was null, empty, or contains only whitesapce.
        /// </exception>
        public bool
        ListenBitsV2(string oauth_token, string user_id, string nonce = "")
        {
            return Send(new PubSubSendPayload(PubSubMessageType.Listen, oauth_token, string.Format(TopicFormat.BITS_V2, user_id), nonce));
        }

        /// <summary>
        /// Unlistens from a PubSub bits v2 topic.
        /// If the topic hasn't been previously listened to, it is silently ignored.
        /// </summary>
        /// <param name="oauth_token">The OAuth token to authorize the request.</param>
        /// <param name="user_id">The user ID associated with the request.</param>
        /// <param name="nonce">A random and unique string used to identify the response with the request.</param>
        /// <returns>
        /// Returns true if the payload was successfully sent to the PubSub websocket.
        /// Returns false otherwise.
        /// </returns>
        /// <exception cref="PubSubArgumentException">
        /// Thrown if the payload OAuth token is null, empty, or contained only whitespace.
        /// Thrown if more than one user ID was provided, or the user ID was null, empty, or contains only whitesapce.
        /// </exception>
        public bool
        UnlistenBitsV2(string oauth_token, string user_id, string nonce = "")
        {
            return Send(new PubSubSendPayload(PubSubMessageType.Unlisten, oauth_token, string.Format(TopicFormat.BITS_V2, user_id), nonce));
        }

        /// <summary>
        /// <para>
        /// Listens to a PubSub bits badge topic.
        /// Receieve a payload when anyone who cheered in the provided user's chat earns a new Bits badge and chooses to share the notification.
        /// If the topic has already been listened to, it is silently ignored.
        /// </para>
        /// <para>Required Scope: <see cref="PubSubScopes.BitsRead"/>.</para>
        /// </summary>
        /// <param name="oauth_token">The OAuth token to authorize the request.</param>
        /// <param name="user_id">The user ID associated with the request.</param>
        /// <param name="nonce">A random and unique string used to identify the response with the request.</param>
        /// <returns>
        /// Returns true if the payload was successfully sent to the PubSub websocket.
        /// Returns false otherwise.
        /// </returns>
        /// <exception cref="PubSubArgumentException">
        /// Thrown if the payload OAuth token is null, empty, or contained only whitespace.
        /// Thrown if more than one user ID was provided, or the user ID was null, empty, or contains only whitesapce.
        /// </exception>
        public bool
        ListenBitsBadge(string oauth_token, string user_id, string nonce = "")
        {
            return Send(new PubSubSendPayload(PubSubMessageType.Listen, oauth_token, string.Format(TopicFormat.BITS_BADGE, user_id), nonce));
        }

        /// <summary>
        /// Unlistens from a PubSub bits badge topic.
        /// If the topic hasn't been previously listened to, it is silently ignored.
        /// </summary>
        /// <param name="oauth_token">The OAuth token to authorize the request.</param>
        /// <param name="user_id">The user ID associated with the request.</param>
        /// <param name="nonce">A random and unique string used to identify the response with the request.</param>
        /// <returns>
        /// Returns true if the payload was successfully sent to the PubSub websocket.
        /// Returns false otherwise.
        /// </returns>
        /// <exception cref="PubSubArgumentException">
        /// Thrown if the payload OAuth token is null, empty, or contained only whitespace.
        /// Thrown if more than one user ID was provided, or the user ID was null, empty, or contains only whitesapce.
        /// </exception>
        public bool
        UnlistenBitsBadge(string oauth_token, string user_id, string nonce = "")
        {
            return Send(new PubSubSendPayload(PubSubMessageType.Unlisten, oauth_token, string.Format(TopicFormat.BITS_BADGE, user_id), nonce));
        }

        /// <summary>
        /// <para>
        /// Listens to a PubSub channel points topic.
        /// Receieve a payload when anyone redeems a custom channel point reward in the provided user's chat.
        /// Payloads are not sent when a user redeems standard/built-in rewards, i.e., "Highlight My Message".
        /// If the topic has already been listened to, it is silently ignored.
        /// </para>
        /// <para>Required Scope: <see cref="PubSubScopes.BitsRead"/>.</para>
        /// </summary>
        /// <param name="oauth_token">The OAuth token to authorize the request.</param>
        /// <param name="user_id">The user ID associated with the request.</param>
        /// <param name="nonce">A random and unique string used to identify the response with the request.</param>
        /// <returns>
        /// Returns true if the payload was successfully sent to the PubSub websocket.
        /// Returns false otherwise.
        /// </returns>
        /// <exception cref="PubSubArgumentException">
        /// Thrown if the payload OAuth token is null, empty, or contained only whitespace.
        /// Thrown if more than one user ID was provided, or the user ID was null, empty, or contains only whitesapce.
        /// </exception>
        public bool
        ListenChannelPoints(string oauth_token, string user_id, string nonce = "")
        {
            return Send(new PubSubSendPayload(PubSubMessageType.Listen, oauth_token, string.Format(TopicFormat.CHANNEL_POINTS, user_id), nonce));
        }

        /// <summary>
        /// Unlistens from a PubSub channel points topic.
        /// If the topic hasn't been previously listened to, it is silently ignored.
        /// </summary>
        /// <param name="oauth_token">The OAuth token to authorize the request.</param>
        /// <param name="user_id">The user ID associated with the request.</param>
        /// <param name="nonce">A random and unique string used to identify the response with the request.</param>
        /// <returns>
        /// Returns true if the payload was successfully sent to the PubSub websocket.
        /// Returns false otherwise.
        /// </returns>
        /// <exception cref="PubSubArgumentException">
        /// Thrown if the payload OAuth token is null, empty, or contained only whitespace.
        /// Thrown if more than one user ID was provided, or the user ID was null, empty, or contains only whitesapce.
        /// </exception>
        public bool
        UnlistenChannelPoints(string oauth_token, string user_id, string nonce = "")
        {
            return Send(new PubSubSendPayload(PubSubMessageType.Unlisten, oauth_token, string.Format(TopicFormat.CHANNEL_POINTS, user_id), nonce));
        }

        /// <summary>
        /// <para>
        /// Listens to a PubSub subscription topic.
        /// Receieve a payload when someone 
        /// If the topic has already been listened to, it is silently ignored.
        /// </para>
        /// <para>Required Scope: <see cref="PubSubScopes.ChannelSubscriptions"/>.</para>
        /// </summary>
        /// <param name="oauth_token">The OAuth token to authorize the request.</param>
        /// <param name="user_id">The user ID associated with the request.</param>
        /// <param name="nonce">A random and unique string used to identify the response with the request.</param>
        /// <returns>
        /// Returns true if the payload was successfully sent to the PubSub websocket.
        /// Returns false otherwise.
        /// </returns>
        /// <exception cref="PubSubArgumentException">
        /// Thrown if the payload OAuth token is null, empty, or contained only whitespace.
        /// Thrown if more than one user ID was provided, or the user ID was null, empty, or contains only whitesapce.
        /// </exception>
        public bool
        ListenSubscriptions(string oauth_token, string user_id, string nonce = "")
        {
            return Send(new PubSubSendPayload(PubSubMessageType.Listen, oauth_token, string.Format(TopicFormat.SUBSCRIPTIONS, user_id), nonce));
        }

        /// <summary>
        /// Unlistens from a PubSub subscription topic.
        /// If the topic hasn't been previously listened to, it is silently ignored.
        /// </summary>
        /// <param name="oauth_token">The OAuth token to authorize the request.</param>
        /// <param name="user_id">The user ID associated with the request.</param>
        /// <param name="nonce">A random and unique string used to identify the response with the request.</param>
        /// <returns>
        /// Returns true if the payload was successfully sent to the PubSub websocket.
        /// Returns false otherwise.
        /// </returns>
        /// <exception cref="PubSubArgumentException">
        /// Thrown if the payload OAuth token is null, empty, or contained only whitespace.
        /// Thrown if more than one user ID was provided, or the user ID was null, empty, or contains only whitesapce.
        /// </exception>
        public bool
        UnlistenSubscriptions(string oauth_token, string user_id, string nonce = "")
        {
            return Send(new PubSubSendPayload(PubSubMessageType.Unlisten, oauth_token, string.Format(TopicFormat.SUBSCRIPTIONS, user_id), nonce));
        }

        /// <summary>
        /// <para>
        /// Listens to a PubSub whisper topic.
        /// Receieve a payload when anyone sends a whisper to the provided user.
        /// If the topic has already been listened to, it is silently ignored.
        /// </para>
        /// <para>Required Scope: <see cref="PubSubScopes.WhispersRead"/>.</para>
        /// </summary>
        /// <param name="oauth_token">The OAuth token to authorize the request.</param>
        /// <param name="user_id">The user ID associated with the request.</param>
        /// <param name="nonce">A random and unique string used to identify the response with the request.</param>
        /// <returns>
        /// Returns true if the payload was successfully sent to the PubSub websocket.
        /// Returns false otherwise.
        /// </returns>
        /// <exception cref="PubSubArgumentException">
        /// Thrown if the payload OAuth token is null, empty, or contained only whitespace.
        /// Thrown if more than one user ID was provided, or the user ID was null, empty, or contains only whitesapce.
        /// </exception>
        public bool
        ListenWhispers(string oauth_token, string user_id, string nonce = "")
        {
            return Send(new PubSubSendPayload(PubSubMessageType.Listen, oauth_token, string.Format(TopicFormat.WHISPERS, user_id), nonce));
        }

        /// <summary>
        /// Unlistens from a PubSub whisper topic.
        /// If the topic hasn't been previously listened to, it is silently ignored.
        /// </summary>
        /// <param name="oauth_token">The OAuth token to authorize the request.</param>
        /// <param name="user_id">The user ID associated with the request.</param>
        /// <param name="nonce">A random and unique string used to identify the response with the request.</param>
        /// <returns>
        /// Returns true if the payload was successfully sent to the PubSub websocket.
        /// Returns false otherwise.
        /// </returns>
        /// <exception cref="PubSubArgumentException">
        /// Thrown if the payload OAuth token is null, empty, or contained only whitespace.
        /// Thrown if more than one user ID was provided, or the user ID was null, empty, or contains only whitesapce.
        /// </exception>
        public bool
        UnlistenWhispers(string oauth_token, string user_id, string nonce = "")
        {
            return Send(new PubSubSendPayload(PubSubMessageType.Unlisten, oauth_token, string.Format(TopicFormat.WHISPERS, user_id), nonce));
        }

        /// <summary>
        /// LISTEN/UNLISTEN to/from a list of PubSub topics.
        /// If a topic has already been listened to, it is silently ignored.
        /// If the topic hasn't been previously listened to, it is silently ignored.
        /// </summary>
        /// <param name="payload">The payload data to send to the PubSub websocket.</param>
        /// <returns>
        /// Returns true if the payload was successfully sent to the PubSub websocket.
        /// Returns false otherwise.
        /// </returns>
        /// <exception cref="PubSubException">
        /// Thrown if the payload or payload data is null.
        /// Thrown if the list of payload topis is null or empty.
        /// </exception>
        /// <exception cref="PubSubArgumentException">
        /// Thrown if the payload type is not <see cref="PubSubMessageType.Listen"/> or <see cref="PubSubMessageType.Unlisten"/>.
        /// Thrown if the payload OAuth token any topic is null, empty, or contained only whitespace.
        /// Thrown if any payload topic could not be parsed into one of the supported <see cref="PubSubTopic"/>'s.
        /// Thrown if any payload topic contained no user ID, more than one user ID, or the user ID was null, empty, or contains only whitesapce
        /// </exception>
        public bool
        Send(PubSubSendPayload payload)
        {
            if (!ValidateSendPayload(payload))
            {
                return false;
            }

            return Send(JsonConvert.SerializeObject(payload));
        }

        private bool
        ValidateSendPayload(PubSubSendPayload payload)
        {
            if (payload.IsNull())
            {
                ArgumentNullException inner_exception = new ArgumentNullException(nameof(payload));
                SetError(new PubSubException(PubSubClientError.SendPayload_Null, "The payload data cannot be null.", inner_exception));

                return false;
            }

            if (payload.type != PubSubMessageType.Listen && payload.type != PubSubMessageType.Unlisten)
            {
                SetError(new PubSubArgumentException(PubSubClientError.SendPayload_Type, "The payload type must either be 'Listen' or 'Unlisten'.", nameof(payload.type)));

                return false;
            }

            if (payload.data.IsNull())
            {
                ArgumentNullException inner_exception = new ArgumentNullException(nameof(payload.data));
                SetError(new PubSubException(PubSubClientError.SendPayload_Data_Null, "The payload data cannot be null.", inner_exception));

                return false;
            }

            // TODO: Check against an (optional) list of provided scopes against the required scope(s) for each topic.
            if (!payload.data.auth_token.HasContent())
            {
                SetError(new PubSubArgumentException(PubSubClientError.SendPayload_OAuthToken_NoContent, "The OAuth token cannot be null, empty, or contain only whitesapce.", nameof(payload.data.auth_token)));

                return false;
            }

            if (!payload.data.topics.IsValid())
            {
                SetError(new PubSubException(PubSubClientError.SendPayload_Topics_Empty, "The payload topics was null or an empty list."));

                return false;
            }

            string[] topic;
            for(int index = 0; index < payload.data.topics.Count; ++index)
            {
                if (!payload.data.topics[index].HasContent())
                {
                    SetError(new PubSubArgumentException(PubSubClientError.SendPayload_Topic_NoContent, "The payload topic cannot be null, empty, or contain only whitespace."));

                    return false;
                }

                topic = payload.data.topics[index].Split(new char[] { '.' }, StringSplitOptions.RemoveEmptyEntries);
                if (!EnumUtil.TryParse(topic[0], out PubSubTopic _topic))
                {
                    SetError(new PubSubArgumentException(PubSubClientError.SendPayload_Topic_Malformed, "The payload topic, " + payload.data.topics[index].WrapQuotes() + ", did not match any supported root topic.", nameof(topic)));

                    return false;
                }

                if (topic.Length == 1)
                {
                    SetError(new PubSubArgumentException(PubSubClientError.SendPayload_Topic_Arguments, "No user ID was provided in the topic, " + payload.data.topics[index].WrapQuotes(), nameof(topic)));

                    return false;
                }
                else if (topic.Length > 2)
                {
                    SetError(new PubSubArgumentException(PubSubClientError.SendPayload_Topic_Arguments, "Only one user ID can be provided in the topic, " + topic[index].WrapQuotes(), nameof(topic)));

                    return false;
                }

                if (!topic[1].HasContent())
                {
                    SetError(new PubSubArgumentException(PubSubClientError.SendPayload_Topic_Arguments, "The topic user ID cannot be null, empty, or contain only whitespace.", nameof(topic)));

                    return false;
                }
            }

            return true;
        }

        private void
        SetError<type>(type exception)
        where type : Exception, IPubSubException
        {
            if(settings.handling_pubsub_client_error == ErrorHandling.Return)
            {
                return;
            }

            throw exception;
        }

        #endregion
    }

    #region Settings and Errors

    public interface
    IPubSbSettings : IWebSocketSettings
    {
        /// <summary>
        /// How to handle intenral errors encountered within the library.
        /// </summary>
        ErrorHandling handling_pubsub_client_error { get; set; }
    }

    public sealed class
    PubSubSettings : WebSocketSettings, IPubSbSettings
    {
        /// <summary>
        /// How to handle intenral errors encountered within the library.
        /// </summary>
        public ErrorHandling handling_pubsub_client_error { get; set; }

        public
        PubSubSettings()
        {
            handling_pubsub_client_error = ErrorHandling.Error;
        }
    }

    public enum
    PubSubClientError
    {
        /// <summary>
        /// Null payload was attempted to be sent to the PubSub websocket.
        /// </summary>
        SendPayload_Null,

        /// <summary>
        /// The send payload type is not set to <see cref="PubSubMessageType.Listen"/> or <see cref="PubSubMessageType.Unlisten"/>.
        /// </summary>
        SendPayload_Type,

        /// <summary>
        /// Null LISTEN/UNLISTEN payload data was attempted to be sent to the PubSub websocket.
        /// </summary>
        SendPayload_Data_Null,

        /// <summary>
        /// The OAuth token in the send payload data is null, empty, or contains only whitesapce.
        /// </summary>
        SendPayload_OAuthToken_NoContent,

        /// <summary>
        /// The list of topics in the send payload data is null or an empty.
        /// </summary>
        SendPayload_Topics_Empty,

        /// <summary>
        /// One or more topic in the send payload data contained only whitespace.
        /// </summary>
        SendPayload_Topic_NoContent,

        /// <summary>
        /// One or more topic in the send payload data could not be parsed into one of the supported <see cref="PubSubTopic"/>'s.
        /// Consider using one of the templates provided in <see cref="TopicFormat"/>.
        /// </summary>
        SendPayload_Topic_Malformed,

        /// <summary>
        /// One or more topic in the send payload data contained no user ID, more than one user ID, or the user ID was null, empty, or contains only whitesapce.
        /// </summary>
        SendPayload_Topic_Arguments
    }

    public interface
    IPubSubException
    {
        /// <summary>
        /// The LISTEN/UNLISTEN payload request format error that was encountered.
        /// </summary>
        PubSubClientError error { get; }
    }

    public class
    PubSubArgumentException : ArgumentException, IPubSubException
    {
        /// <summary>
        /// The LISTEN/UNLISTEN payload request format error that was encountered.
        /// </summary>
        public PubSubClientError error { get; }

        /// <param name="error">The LISTEN/UNLISTEN payload request format error that was encountered.</param>
        /// <param name="message">The error message.</param>
        public
        PubSubArgumentException(PubSubClientError error, string message) : base(message)
        {
            this.error = error;
        }

        /// <param name="error">The LISTEN/UNLISTEN payload request format error that was encountered.</param>
        /// <param name="message">The error message.</param>
        /// <param name="param_name">The name of the parameter that was formatted wrong.</param>
        public
        PubSubArgumentException(PubSubClientError error, string message, string param_name) : base(message, param_name)
        {
            this.error = error;
        }

        /// <param name="error">The LISTEN/UNLISTEN payload request format error that was encountered.</param>
        /// <param name="message">The error message.</param>
        /// <param name="inner_exception">The secondary error that occured.</param>
        public
        PubSubArgumentException(PubSubClientError error, string message, Exception inner_exception) : base(message, inner_exception)
        {
            this.error = error;
        }

        /// <param name="error">The LISTEN/UNLISTEN payload request format error that was encountered.</param>
        /// <param name="message">The error message.</param>
        /// <param name="param_name">The name of the parameter that was formatted wrong.</param>
        /// <param name="inner_exception">The secondary error that occured.</param>
        public
        PubSubArgumentException(PubSubClientError error, string message, string param_name, Exception inner_exception) : base(message, param_name, inner_exception)
        {
            this.error = error;
        }
    }

    public class
    PubSubException : Exception, IPubSubException
    {
        public PubSubClientError error { get; }

        /// <param name="error">The LISTEN/UNLISTEN payload request format error that was encountered.</param>
        /// <param name="message">The error message.</param>
        public
        PubSubException(PubSubClientError error, string message) : base(message)
        {
            this.error = error;
        }

        /// <param name="error">The LISTEN/UNLISTEN payload request format error that was encountered.</param>
        /// <param name="message">The error message.</param>
        /// <param name="inner_exception">The secondary error that occured.</param>
        public
        PubSubException(PubSubClientError error, string message, Exception inner_exception) : base(message, inner_exception)
        {
            this.error = error;
        }
    }

    #endregion

    #region Data Structures

    #region General Enums and Formats

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
        /// <para>Required for the <see cref="PubSubTopic.ChannelPoints"/> topic.</para>
        /// </summary>
        [EnumMember(Value = "channel:read:redemptions")]
        ChannelReadRedemptions = 1 << 2,

        /// <summary>
        /// <para>channel_subscriptions</para>
        /// <para>Required for the <see cref="PubSubTopic.Subscriptions"/> topic.</para>
        /// </summary>
        [EnumMember(Value = "channel_subscriptions")]
        ChannelSubscriptions = 1 << 3,

        /// <summary>
        /// <para>channel_subscriptions</para>
        /// <para>Required for the <see cref="PubSubTopic.Whispers"/> topic.</para>
        /// </summary>
        [EnumMember(Value = "whispers:read")]
        WhispersRead = 1 << 4,
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
        /// <para>channel-points-channel-v1</para>
        /// <para>Required Scope: <see cref="PubSubScopes.ChannelReadRedemptions"/></para>
        /// </summary>
        [EnumMember(Value = "channel-points-channel-v1")]
        ChannelPoints,

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
        /// channel-points-channel-v1.{0}
        /// </summary>
        public static readonly string CHANNEL_POINTS = "channel-points-channel-v1.{0}";

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

    #region General Message Payloads

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
    PubSubSendPayload
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
        public PubSubSendPayloadData data;

        public
        PubSubSendPayload()
        {
            data = new PubSubSendPayloadData();
        }

        /// <param name="type">
        /// The type of message being sent to the PubSub server.
        /// Must be either <see cref="PubSubMessageType.Listen"/> or <see cref="PubSubMessageType.Unlisten"/>
        /// </param>
        /// <param name="oauth_token">The OAuth token to authorize the request.</param>
        /// <param name="topic">The formatted topic to be requested.</param>
        /// <param name="nonce">Random and unique string provided by the client to identify the response with the appropriate request.</param>
        public
        PubSubSendPayload(PubSubMessageType type, string oauth_token, string topic, string nonce = "")
        {
            this.type = type;
            this.nonce = nonce;

            data = new PubSubSendPayloadData();
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
        PubSubSendPayload(PubSubMessageType type, string oauth_token, string[] topics, string nonce = "")
        {
            this.type = type;
            this.nonce = nonce;

            data = new PubSubSendPayloadData();
            data.auth_token = oauth_token;

            if (topics.IsNull())
            {
                return;
            }

            data.topics.AddRange(topics);
        }
    }

    public class
    PubSubSendPayloadData
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
        PubSubSendPayloadData()
        {
            topics = new List<string>();
        }
    }

    #endregion

    #region Topic: chat_moderator_actions.{user-id}

    public class
    ModeratorAction
    {
        /// <summary>
        /// The moderation action payload.
        /// </summary>
        [JsonProperty("data")]
        public ModeratorActionData data { get; protected set; }
    }

    public class
    ModeratorActionData
    {
        /// <summary>
        /// The type of user that performed the moderator action.
        /// </summary>
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
        [EnumMember(Value = "other")]
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

    #region Topic: channel-points-channel-v1

    public class
    PubSubChannelPoints
    {
        /// <summary>
        /// The type of channel point action that occured.
        /// </summary>
        [JsonProperty("type")]
        public ChannelPointsType type { get; protected set; }

        /// <summary>
        /// The channel points payload.
        /// </summary>
        [JsonProperty("data")]
        public ChannelPointsData data { get; protected set; }
    }

    public class
    ChannelPointsData
    {
        /// <summary>
        /// The time the PubSub payload was sent.
        /// </summary>
        [JsonProperty("timestamp")]
        public DateTime timestamp { get; protected set; }

        /// <summary>
        /// The data associated with the redemption that occured.
        /// </summary>
        [JsonProperty("redemption")]
        public ChannelPointsRedemption redemption { get; protected set; }
    }

    public class
    ChannelPointsRedemption
    {
        /// <summary>
        /// The redemption ID.
        /// </summary>
        [JsonProperty("id")]
        public string id { get; protected set; }

        /// <summary>
        /// The user who redeemed the reward.
        /// </summary>
        [JsonProperty("user")]
        public RedemptionUser user { get; protected set; }

        /// <summary>
        /// The ID of the user where the reward was redeemed.
        /// </summary>
        [JsonProperty("channel_id")]
        public string channel_id { get; protected set; }

        /// <summary>
        /// The time the reward was redeemed.
        /// </summary>
        [JsonProperty("redeemed_at")]
        public DateTime redeemed_at { get; protected set; }

        /// <summary>
        /// The reward that was redeemed.
        /// </summary>
        [JsonProperty("reward")]
        public ChannelPointsReward reward { get; protected set; }

        /// <summary>
        /// <para>The input that was sent with the redemption.</para>
        /// <para>Set to an ampty string if no input was provided.</para>
        /// </summary>
        [JsonProperty("user_input")]
        public string user_input { get; protected set; }

        /// <summary>
        /// <para>Whether or not the redemption was fulfilled.</para>
        /// <para>Set to <see cref="ChannelPointsStatus.Unfulfilled"/> on initial redemption if the reward has not been set to skip the reward queue.</para>
        /// </summary>
        [JsonProperty("status")]
        public ChannelPointsStatus status { get; protected set; }
    }

    public class
    RedemptionUser
    {
        /// <summary>
        /// The ID of the user redeemed the reward.
        /// </summary>
        [JsonProperty("id")]
        public string id { get; protected set; }

        /// <summary>
        /// The login name of the user redeemed the reward.
        /// </summary>
        [JsonProperty("login")]
        public string login { get; protected set; }

        /// <summary>
        /// <para>The display name of the user redeemed the reward.</para>
        /// <para>Set to an empty string if the user never explicitly set their display name.</para>
        /// </summary>
        [JsonProperty("display_name")]
        public string display_name { get; protected set; }
    }

    public class
    ChannelPointsReward
    {
        /// <summary>
        /// The reward ID.
        /// </summary>
        [JsonProperty("id")]
        public string id { get; protected set; }

        // TODO: Verify this property description.
        /// <summary>
        /// The ID of the user where the reward was redeemed.
        /// </summary>
        [JsonProperty("channel_id")]
        public string channel_id { get; protected set; }

        /// <summary>
        /// The name of the reward.
        /// </summary>
        [JsonProperty("title")]
        public string title { get; protected set; }

        /// <summary>
        /// The reward's description or flavor text.
        /// </summary>
        [JsonProperty("prompt")]
        public string prompt { get; protected set; }

        /// <summary>
        /// The amount of points needed to redeem the reward.
        /// </summary>
        [JsonProperty("cost")]
        public int cost { get; protected set; }

        /// <summary>
        /// Whether or not a user must input information to redeem the reward.
        /// </summary>
        [JsonProperty("is_user_input_required")]
        public bool is_user_input_required { get; protected set; }

        /// <summary>
        /// Whether or not the reward is only available to subscribers.
        /// </summary>
        [JsonProperty("is_sub_only")]
        public bool is_sub_only { get; protected set; }

        /// <summary>
        /// <para>The reward's thumbnail URL's.</para>
        /// <para>Set to null if no custom thumbnail has been uploaded.</para>
        /// </summary>
        [JsonProperty("image")]
        public ChannelPointImage image { get; protected set; }

        /// <summary>
        /// The reward's default thumbnail URL's.
        /// </summary>
        [JsonProperty("default_image")]
        public ChannelPointImage default_image { get; protected set; }

        /// <summary>
        /// The background fill color of the reward button.
        /// </summary>
        [JsonProperty("background_color")]
        public Color background_color { get; protected set; }

        /// <summary>
        /// Whether or not the reward is snabled and can be redeemed.
        /// </summary>
        [JsonProperty("is_enabled")]
        public bool is_enabled { get; protected set; }

        // TODO: Verify this property description.
        /// <summary>
        /// Whether or not the reward queue is paused.
        /// </summary>
        [JsonProperty("is_paused")]
        public bool is_paused { get; protected set; }

        /// <summary>
        /// Whether or not this reward can still be redeemed/honored if there is a maximum on the number of redemptions per stream.
        /// </summary>
        [JsonProperty("is_in_stock")]
        public bool is_in_stock { get; protected set; }

        /// <summary>
        /// The maximum number of times this reward can be redeemed.
        /// </summary>
        [JsonProperty("max_per_stream")]
        public RewardMax max_per_stream { get; protected set; }

        [JsonProperty("should_redemptions_skip_request_queue")]
        public bool should_redemptions_skip_request_queue { get; protected set; }
    }

    public class
    RewardMax
    {
        /// <summary>
        /// Whether or not there is a limit to how many times a rewward can be redeemed per stream.
        /// </summary>
        [JsonProperty("is_enabled")]
        public bool is_enabled { get; protected set; }

        /// <summary>
        /// <para>The maximum amount of times on the reward can be redeemed per stream.</para>
        /// <para>Set to 0 if there is no limit.</para>
        /// </summary>
        [JsonProperty("max_per_stream")]
        public int max_per_stream { get; protected set; }
    }

    public class
    ChannelPointImage
    {
        /// <summary>
        /// The URL to the reaward's standard size thumbnail, 28px x 28px.
        /// </summary>
        [JsonProperty("url_1x")]
        public string url_1x { get; protected set; }

        /// <summary>
        /// The URL to the reaward's medium size thumbnail, 58px x 58px.
        /// </summary>
        [JsonProperty("url_2x")]
        public string url_2x { get; protected set; }

        /// <summary>
        /// The URL to the reaward's large size thumbnail, 112px x 112px.
        /// </summary>
        [JsonProperty("url_4x")]
        public string url_4x { get; protected set; }
    }

    [JsonConverter(typeof(EnumConverter))]
    public enum
    ChannelPointsType
    {
        /// <summary>
        /// Uknown bits context.
        /// </summary>
        [EnumMember(Value = "other")]
        Other = 0,

        /// <summary>
        /// A user redeemed a channel point reward.
        /// </summary>
        [EnumMember(Value = "reward-redeemed")]
        RewardRedeemed
    }

    [JsonConverter(typeof(EnumConverter))]
    public enum
    ChannelPointsStatus
    {
        /// <summary>
        /// <para>The reward redemption has not been fulfilled.</para>
        /// <para>This is the default state when a reward has been redeemed unless the reward has been set to skip the reward queue.</para>
        /// </summary>
        [EnumMember(Value = "UNFULFILLED")]
        Unfulfilled = 0,

        /// <summary>
        /// The reward redemption has been fulfilled.
        /// </summary>
        [EnumMember(Value = "FULFILLED")]
        Fulfilled
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

    #endregion
}