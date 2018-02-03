// imported .dll's
using Newtonsoft.Json;

using RestSharp;
using RestSharp.Deserializers;

namespace
TwitchNet.Helpers.Json
{
    internal class
    JsonDeserializer : IDeserializer
    {
        public string RootElement   { get; set; }
        public string Namespace     { get; set; }
        public string DateFormat    { get; set; }

        /// <summary>
        /// Custom deserializer that utilizies Newtonsoft to handle Json responses with RestSharp
        /// </summary>
        /// <typeparam name="return_type">The <see cref="Type"/> of the object to deserialize.</typeparam>
        /// <param name="response">The rest response to deserialzie.</param>
        /// <returns>Returns a deserialized <typeparamref name="return_type"/> object.</returns>
        public return_type
        Deserialize<return_type>(IRestResponse response)
        {
            JsonSerializerSettings settings = new JsonSerializerSettings();
            settings.Converters.Add(new TimeSpanConverter());

            settings.NullValueHandling      = NullValueHandling.Ignore;
            settings.DateParseHandling      = DateParseHandling.DateTime;
            settings.DateTimeZoneHandling   = DateTimeZoneHandling.Local;
            settings.FloatParseHandling     = FloatParseHandling.Double;

#if DEBUG

            // NOTE: Deserialize - For debugging purposes only, change MissingMemberHandling to 'ignrore' on release build
            // settings.MissingMemberHandling  = MissingMemberHandling.Error;

#endif

            return_type result = JsonConvert.DeserializeObject<return_type>(response.Content, settings);

            return result;
        }
        
    }
}
