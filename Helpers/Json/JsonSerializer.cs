// imported .dll's
using Newtonsoft.Json;

namespace
TwitchNet.Helpers.Json
{
    internal class
    JsonSerializer : ISerializer
    {
        public string content_type { get; set; }

        public JsonSerializer()
        {
            content_type = "application/json";
        }

        /// <summary>
        /// Serializes an object into a JSON string..
        /// </summary>
        /// <param name="obj">The object to serialize.</param>
        /// <returns>Returns the serialized object.</returns>
        public string
        Serialize(object obj)
        {
            JsonSerializerSettings settings = new JsonSerializerSettings();
            settings.NullValueHandling = NullValueHandling.Ignore;
            settings.DateParseHandling = DateParseHandling.DateTime;
            settings.DateTimeZoneHandling = DateTimeZoneHandling.Local;
            settings.FloatParseHandling = FloatParseHandling.Double;

            string result = JsonConvert.SerializeObject(obj, settings);

            return result;
        }
    }
}
