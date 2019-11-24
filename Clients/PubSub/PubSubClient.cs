// standard namespaces
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

// project namespaces
using TwitchNet.Extensions;
using TwitchNet.Helpers.Json;
using TwitchNet.Utilities;

// imported .dll's
using Newtonsoft.Json;

namespace
TwitchNet.Clients.PubSub
{   
    public class
    PubSubClient : WebSocketClient, IDisposable
    {
        public
        PubSubClient() : base()
        {
            URI = new Uri("wss://pubsub-edge.twitch.tv");
        }

        public bool
        Ping()
        {
            // I tried sending this with a proper Ping control frame, but Twitch apparently only accepts Text data.
            // Twitch broke spec again? * fake shocked face*
            return Send("{\"type\": \"PING\"}");
        }

        public bool
        Listen(ListenPayload payload)
        {
            if (!ValidateListenPayload(payload, PubSubMessageType.Listen))
            {
                return false;
            }

            string _payload = JsonConvert.SerializeObject(payload);

            return Send(_payload);
        }

        public bool
        Listen(ListenPayload[] payloads)
        {
            if (!payloads.IsValid())
            {
                return false;
            }

            foreach (ListenPayload payload in payloads)
            {
                if (!Listen(payload))
                {
                    return false;
                }
            }

            return true;
        }

        public bool
        Listen(PubsubTopic topic, string oauth_token, string nonce, params string[] args)
        {
            if (args.IsNull())
            {
                return false;
            }

            string _topic = EnumUtil.GetName(topic) + '.' + string.Join(".", args);

            ListenPayload payload = new ListenPayload();
            payload.type = PubSubMessageType.Listen;
            payload.nonce = nonce;
            payload.data.auth_token = oauth_token;
            payload.data.topics.Add(_topic);

            return Listen(payload);
        }

        private bool
        ValidateListenArguments(PubsubTopic topic, string oauth_token, params string[] args)
        {
            if (!args.IsValid())
            {
                // Error

                return false;
            }

            if (!oauth_token.HasContent())
            {
                // Error, can't be null, empty, or contain only whitespace

                return false;
            }

            return true;
        }

        private bool
        ValidateListenPayload(ListenPayload payload, PubSubMessageType expected_type = PubSubMessageType.Other)
        {
            if (payload.IsNull())
            {
                return false;
            }

            if(expected_type == PubSubMessageType.Other)
            {
                if (payload.type != PubSubMessageType.Listen && payload.type == PubSubMessageType.Unlisten)
                {
                    return false;
                }
            }
            else
            {
                if (payload.type != expected_type)
                {
                    return false;
                }
            }

            if (payload.data.IsNull())
            {
                return false;
            }

            if (!payload.data.auth_token.HasContent())
            {
                return false;
            }

            if (!payload.data.topics.IsValid())
            {
                return false;
            }

            string[] topic;
            for(int index = 0; index < payload.data.topics.Count; ++index)
            {
                if (!payload.data.topics[index].HasContent())
                {
                    return false;
                }

                topic = payload.data.topics[index].Split(new char[] { '.' }, StringSplitOptions.RemoveEmptyEntries);
                if (!EnumUtil.TryParse(topic[0], out PubsubTopic _topic))
                {
                    // Failed to parse the string into a topic

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
    }

    #region Data Structures

    public class
    ListenPayload
    {
        [JsonProperty("type")]
        public PubSubMessageType type;

        [JsonProperty("nonce")]
        public string nonce;

        [JsonProperty("data")]
        public ListenPayloadData data;

        public ListenPayload()
        {
            data = new ListenPayloadData();
        }
    }

    public class
    ListenPayloadData
    {
        [JsonProperty("topics")]
        public List<string> topics;

        [JsonProperty("auth_token")]
        public string auth_token;

        public ListenPayloadData()
        {
            topics = new List<string>();
        }
    }

    [JsonConverter(typeof(EnumConverter))]
    public enum
    PubSubMessageType
    {
        [EnumMember(Value = "")]
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
        [EnumMember(Value = "")]
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