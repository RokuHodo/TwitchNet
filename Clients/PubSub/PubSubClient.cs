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

        public Random timer_ping_jitter;

        public
        PubSubClient() : base()
        {
            URI = new Uri("wss://pubsub-edge.twitch.tv");

            timer_ping = new Timer(10 * 60 * 1000);
            timer_ping.Elapsed += Callback_TimerPing_OnElapsed;

            Guid guid = Guid.NewGuid();
            timer_ping_jitter = new Random(guid.GetHashCode());

            OnOpen += Callback_OnOpen;
            OnMessageText += CallBack_OnMessageText;
        }

        private void
        Callback_OnOpen(object sender, WebSocketEventArgs e)
        {
            Debug.WriteLine("Timers started.");
            timer_ping.Start();
        }

        private void
        CallBack_OnMessageText(object sender, MessageTextEventArgs e)
        {
            PubSubType check = JsonConvert.DeserializeObject<PubSubType>(e.message);
            switch (check.type)
            {
                case PubSubMessageType.Pong:        OnPupSubPong.Raise(this, new PubSubTypeEventArgs(e, check.type));       break;
                case PubSubMessageType.Reconnect:   OnPupSubReconnect.Raise(this, new PubSubTypeEventArgs(e, check.type));  break;
                case PubSubMessageType.Response:    OnPupSubResponse.Raise(this, new PubSubResponseEventArgs(e));           break;
                case PubSubMessageType.Message:     ProcessPubSubMessage(e);                                                break;
            }
        }

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

        private async void
        Callback_TimerPing_OnElapsed(object sender, ElapsedEventArgs e)
        {
            int jitter = timer_ping_jitter.Next(-100, 100);
            await Task.Delay(jitter);

            timer_ping.Dispose();

            Ping();
        }

        private void
        ProcessPubSubMessage(MessageTextEventArgs args)
        {
            PubSubMessageEventArgs args_pub_sub = new PubSubMessageEventArgs(args);
            OnPupSubMessage.Raise(this, args_pub_sub);

            string _topic = args_pub_sub.message.data.topic.TextBefore('.');
            if (!EnumUtil.TryParse(_topic, out PubsubTopic topic))
            {
                // TODO: OnPubSubUnsupportedTopic

                return;
            }

            switch (topic)
            {
                case PubsubTopic.Whispers: OnPupSubMessageWhisper.Raise(this, new PubSubWhisperEventArgs(args_pub_sub, args)); break;
            }
        }

        #region Writing

        public bool
        Ping()
        {
            // I tried sending this with a proper Ping control frame, but Twitch apparently only accepts Text data.
            // Twitch broke spec again? * fake shocked face*
            return Send("{\"type\": \"PING\"}");
        }

        public bool
        Listen(string oauth_token, string topic, string nonce = "")
        {
            ListenUnlistenPayload payload = new ListenUnlistenPayload();
            payload.type = PubSubMessageType.Listen;
            payload.nonce = nonce;
            payload.data.auth_token = oauth_token;
            payload.data.topics.Add(topic);

            return SendListenUnlistenPayload(payload);
        }

        public bool
        Listen(string oauth_token, string[] topics, string nonce = "")
        {
            if (topics.IsNull())
            {
                return false;
            }

            ListenUnlistenPayload payload = new ListenUnlistenPayload();
            payload.type = PubSubMessageType.Listen;
            payload.nonce = nonce;
            payload.data.auth_token = oauth_token;
            payload.data.topics.AddRange(topics);

            return SendListenUnlistenPayload(payload);
        }

        public bool
        Unlisten(string oauth_token, string topic, string nonce = "")
        {
            ListenUnlistenPayload payload = new ListenUnlistenPayload();
            payload.type = PubSubMessageType.Unlisten;
            payload.nonce = nonce;
            payload.data.auth_token = oauth_token;
            payload.data.topics.Add(topic);

            return SendListenUnlistenPayload(payload);
        }

        public bool
        Unlisten(string oauth_token, string[] topics, string nonce = "")
        {
            if (topics.IsNull())
            {
                return false;
            }

            ListenUnlistenPayload payload = new ListenUnlistenPayload();
            payload.type = PubSubMessageType.Unlisten;
            payload.nonce = nonce;
            payload.data.auth_token = oauth_token;
            payload.data.topics.AddRange(topics);

            return SendListenUnlistenPayload(payload);
        }

        public bool
        SendListenUnlistenPayload(ListenUnlistenPayload payload)
        {
            if (!ValidateListenUnlistenPayload(payload))
            {
                return false;
            }

            string _payload = JsonConvert.SerializeObject(payload);

            return Send(_payload);
        }

        private bool
        ValidateListenUnlistenPayload(ListenUnlistenPayload payload)
        {
            if (payload.IsNull())
            {
                // Error, payload is null

                return false;
            }

            if (payload.type != PubSubMessageType.Listen && payload.type != PubSubMessageType.Unlisten)
            {
                // Error, type mismatch

                return false;
            }

            if (payload.data.IsNull())
            {
                // Error, data is null

                return false;
            }

            if (!payload.data.auth_token.HasContent())
            {
                // Error, no authorization provided

                return false;
            }

            if (!payload.data.topics.IsValid())
            {
                // Error, no topics provided

                return false;
            }

            string[] topic;
            for(int index = 0; index < payload.data.topics.Count; ++index)
            {
                if (!payload.data.topics[index].HasContent())
                {
                    // Error, empty topic

                    return false;
                }

                topic = payload.data.topics[index].Split(new char[] { '.' }, StringSplitOptions.RemoveEmptyEntries);
                if (!EnumUtil.TryParse(topic[0], out PubsubTopic _topic))
                {
                    // Error, failed to parse the string into a topic

                    return false;
                }

                if(topic.Length == 1)
                {
                    // Error, no arguments provided

                    return false;
                }
                else if (topic.Length > 2)
                {
                    if (_topic == PubsubTopic.ModeratorActions && topic.Length != 3)
                    {
                        // Error, expected 2 arguments

                        return false;
                    }
                    else
                    {
                        // Error, expected only 1 argument

                        return false;
                    }
                }

                if (!topic[1].HasContent() || (_topic == PubsubTopic.ModeratorActions && !topic[2].HasContent()))
                {
                    // Error, can't be null, empty, or contain only whitespace

                    return false;
                }
            }

            return true;
        }

        #endregion
    }

    #region Data Structures

    public class
    PubSubType
    {
        [JsonProperty("type")]
        public PubSubMessageType type { get; set; }
    }

    public class
    PubSubResponse
    {
        [JsonProperty("type")]
        public PubSubMessageType type { get; set; }

        [JsonProperty("error")]
        public string error { get; set; }

        [JsonProperty("nonce")]
        public string nonce { get; set; }
    }

    public class
    PubSubMessage
    {
        [JsonProperty("type")]
        public PubSubMessageType type { get; set; }

        [JsonProperty("data")]
        public PubSubMessageData data { get; set; }
    }

    public class
    PubSubMessageData
    {
        [JsonProperty("topic")]
        public string topic { get; set; }

        [JsonProperty("message")]
        public string message { get; set; }
    }

    public class
    PubSubWhisper
    {
        /// <summary>
        /// The whisper type.
        /// </summary>
        [JsonProperty("type")]
        public string type { get; set; }

        /// <summary>
        /// The whisper data in an escaped string form.
        /// </summary>
        [JsonProperty("data")]
        public string data { get; set; }

        /// <summary>
        /// The whisper data.
        /// </summary>
        [JsonProperty("data_object")]
        PubSubWhisperDataObject data_object { get; set; }
    }

    public class
    PubSubWhisperDataObject
    {
        /// <summary>
        /// The whisper ID.
        /// </summary>
        [JsonProperty("message_id")]
        public string message_id { get; set; }

        /// <summary>
        /// The total number of whispers sent between each user.
        /// </summary>
        [JsonProperty("id")]
        public string id { get; set; }

        /// <summary>
        /// The user ID of the sender and the user ID of the recipient concatenated with an underscore.
        /// </summary>
        [JsonProperty("thread_id")]
        public string thread_id { get; set; }

        /// <summary>
        /// The whisper message that was sent.
        /// </summary>
        [JsonProperty("body")]
        public string body { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("sent_ts")]
        public ulong sent_ts { get; set; }

        /// <summary>
        /// The sender's user ID.
        /// </summary>
        [JsonProperty("from_id")]
        public string from_id { get; set; }

        /// <summary>
        /// The sender's user information.
        /// </summary>
        [JsonProperty("tags")]
        public PubSubWhisperTags tags { get; set; }

        /// <summary>
        /// The recipient's user information.
        /// </summary>
        [JsonProperty("recipient")]
        public PubSubRecipient recipient { get; set; }

        /// <summary>
        /// A unique string generated by Twitch to identify the PubSub payload.
        /// </summary>
        [JsonProperty("nonce")]
        public string nonce { get; set; }
    }

    public class
    PubSubWhisperTags
    {
        /// <summary>
        /// The sender's login name.
        /// </summary>
        [JsonProperty("login")]
        public string login { get; set; }

        /// <summary>
        /// <para>The sender's display name.</para>
        /// <para>Set to an empty string if the user never explicitly set their display name.</para>
        /// </summary>
        [JsonProperty("display_name")]
        public string display_name { get; set; }

        /// <summary>
        /// <para>The sender's display name color.</para>
        /// <para>Set to <see cref="Color.Empty"/> if the user never explicitly set their display name color.</para>
        /// </summary>
        [JsonProperty("color")]
        public Color color { get; set; }

        /// <summary>
        /// The emotes that were used in the body of the whisper.
        /// <para>Set to an empty list of the user didn't use any emotes.</para>
        /// </summary>
        [JsonProperty("emotes")]
        public List<PubSubEmote> emotes { get; set; }

        /// <summary>
        /// <para>The sender's badges.</para>
        /// <para>Set to an empty list of the user has no badges.</para>
        /// </summary>
        [JsonProperty("badges")]
        public List<PubSubBadge> badges { get; set; }
    }

    public class
    PubSubEmote
    {
        /// <summary>
        /// The emote ID.
        /// </summary>
        [JsonProperty("id")]
        public string id { get; set; }

        /// <summary>
        /// The index in the message where the first emote character is located.
        /// </summary>
        [JsonProperty("start")]
        public int start { get; set; }

        /// <summary>
        /// The index in the message where the last emote character is located.
        /// </summary>
        [JsonProperty("end")]
        public int end { get; set; }
    }

    public class
    PubSubBadge
    {
        /// <summary>
        /// The badge ID.
        /// </summary>
        [JsonProperty("id")]
        public string id { get; set; }

        /// <summary>
        /// The badge version.
        /// </summary>
        [JsonProperty("version")]
        public int version { get; set; }
    }

    public class
    PubSubRecipient
    {
        /// <summary>
        /// The user ID of the recipient.
        /// </summary>
        [JsonProperty("id")]
        public string id { get; set; }

        /// <summary>
        /// The recipient's login name.
        /// </summary>
        [JsonProperty("username")]
        public string username { get; set; }

        /// <summary>
        /// <para>The recipient's display name.</para>
        /// <para>Set to an empty string if the user never explicitly set their display name.</para>
        /// </summary>
        [JsonProperty("display_name")]
        public string display_name { get; set; }

        /// <summary>
        /// <para>The recipient's display name color.</para>
        /// <para>Set to <see cref="Color.Empty"/> if the user never explicitly set their display name color.</para>
        /// </summary>
        [JsonProperty("color")]
        public Color color { get; set; }

        /// <summary>
        /// <para>The recipient's profile image.</para>
        /// <para>Set to null if the user never uploaded a profile image.</para>
        /// </summary>
        [JsonProperty("profile_image")]
        public string profile_image { get; set; }
    }

    public class
    ListenUnlistenPayload
    {
        [JsonProperty("type")]
        public PubSubMessageType type;

        [JsonProperty("nonce")]
        public string nonce;

        [JsonProperty("data")]
        public ListenUnlistenPayloadData data;

        public ListenUnlistenPayload()
        {
            data = new ListenUnlistenPayloadData();
        }
    }

    public class
    ListenUnlistenPayloadData
    {
        [JsonProperty("topics")]
        public List<string> topics;

        [JsonProperty("auth_token")]
        public string auth_token;

        public ListenUnlistenPayloadData()
        {
            topics = new List<string>();
        }
    }

    [JsonConverter(typeof(EnumConverter))]
    public enum
    PubSubMessageType
    {
        [EnumMember(Value = "Other")]
        Other = 0,

        [EnumMember(Value = "MESSAGE")]
        Message,

        [EnumMember(Value = "RESPONSE")]
        Response,

        [EnumMember(Value = "LISTEN")]
        Listen,

        [EnumMember(Value = "UNLISTEN")]
        Unlisten,

        [EnumMember(Value = "PING")]
        Ping,

        [EnumMember(Value = "PONG")]
        Pong,

        [EnumMember(Value = "RECONNECT")]
        Reconnect,
    }

    public enum
    PubsubTopic
    {
        [EnumMember(Value = "Other")]
        Other = 0,

        [EnumMember(Value = "channel-bits-events-v1")]
        Bits,

        [EnumMember(Value = "channel-bits-events-v2")]
        BitsV2,

        [EnumMember(Value = "channel-bits-badge-unlocks")]
        BitsBadge,

        [EnumMember(Value = "chat_moderator_actions")]
        ModeratorActions,

        [EnumMember(Value = "channel-subscribe-events-v1")]
        Subscriptions,

        [EnumMember(Value = "whispers")]
        Whispers,
    }

    public enum
    PubSubClientError
    {
        Listen_ArgumentCountMismatch
    }

    #endregion
}