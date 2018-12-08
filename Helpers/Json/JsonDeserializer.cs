// imported .dll's
using Newtonsoft.Json;

namespace
TwitchNet.Helpers.Json
{
    internal class
    JsonDeserializer : IDeserializer
    {
        /// <summary>
        /// Custom deserializer that utilizies Newtonsoft to handle Json responses with RestSharp
        /// </summary>
        /// <typeparam name="return_type">The <see cref="Type"/> of the object to deserialize.</typeparam>
        /// <param name="str">The rest response to deserialzie.</param>
        /// <returns>Returns a deserialized <typeparamref name="return_type"/> object.</returns>
        public return_type
        Deserialize<return_type>(string str)
        {
            JsonSerializerSettings settings = new JsonSerializerSettings();
            settings.NullValueHandling      = NullValueHandling.Ignore;
            settings.DateParseHandling      = DateParseHandling.DateTime;
            settings.DateTimeZoneHandling   = DateTimeZoneHandling.Local;
            settings.FloatParseHandling     = FloatParseHandling.Double;

            #if DEBUG
            settings.MissingMemberHandling  = MissingMemberHandling.Error;
            #endif

            return_type result = JsonConvert.DeserializeObject<return_type>(str, settings);

            return result;
        }
        
    }
}
