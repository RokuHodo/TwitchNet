using System;

// imported .dll's
using Newtonsoft.Json;

using RestSharp;
using RestSharp.Deserializers;

namespace TwitchNet.Helpers.Json
{
    internal class JsonDeserializer : IDeserializer
    {
        public string RootElement   { get; set; }
        public string Namespace     { get; set; }
        public string DateFormat    { get; set; }
        
        /// <summary>
        /// Custom deserializer that utilizies Newtonsoft to handle Json responses with RestSharp
        /// </summary>
        public return_type Deserialize<return_type>(IRestResponse response)
        {
            JsonSerializerSettings settings = new JsonSerializerSettings();
            settings.NullValueHandling      = NullValueHandling.Ignore;
            settings.DateTimeZoneHandling   = DateTimeZoneHandling.Local;
            settings.FloatParseHandling     = FloatParseHandling.Double;

            // NOTE: Deserialize - For debugging purposes only, change MissingMemberHandling to 'ignrore' on release build
            settings.MissingMemberHandling  = MissingMemberHandling.Error;

            // TODO: Deserialize - Implemenent system to handle '429: Too many requests'. This is critical for multi-plaged requests that can easily go over the limit. This will most likely reside in the 'RestRequestUtil', when I make it.
            return_type result = JsonConvert.DeserializeObject<return_type>(response.Content, settings);

            return result;
        }
    }
}
