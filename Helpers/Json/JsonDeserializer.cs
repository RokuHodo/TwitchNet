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

            try
            {
                return JsonConvert.DeserializeObject<return_type>(response.Content, settings);
            }
            catch(Exception exception)
            {
                return default(return_type);
            }            
        }
    }
}
