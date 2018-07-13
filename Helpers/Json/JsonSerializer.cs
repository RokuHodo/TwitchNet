// imported .dll's
using Newtonsoft.Json;

using RestSharp.Serializers;

namespace
TwitchNet.Helpers.Json
{
    internal class
    JsonSerializer : ISerializer
    {
        public string RootElement   { get; set; }
        public string Namespace     { get; set; }
        public string DateFormat    { get; set; }
        public string ContentType   { get; set; }

        public JsonSerializer()
        {
            ContentType = "application/json";
        }

        /// <summary>
        /// Serializes an object into a JSON string..
        /// </summary>
        /// <param name="obj">The object to serialize.</param>
        /// <returns>Returns the serialized object.</returns>
        public string
        Serialize(object obj)
        {
            string result = JsonConvert.SerializeObject(obj);

            return result;
        }
    }
}
