// project namespaces
using RestSharp.Deserializers;

namespace TwitchNet.Models.Api
{
    public struct
    ClientHandler
    {
        /// <summary>
        /// The handler's content type.
        /// </summary>
        public string content_type;

        /// <summary>
        /// The content type's deserializer.
        /// </summary>
        public IDeserializer deserializer;

        /// <summary>
        /// Creates a new instance of the <see cref="ClientHandler"/> struct.
        /// </summary>
        /// <param name="content_type">The handler's content type.</param>
        /// <param name="deserializer">The content type's deserializer.</param>
        public ClientHandler(string content_type, IDeserializer deserializer)
        {
            this.content_type = content_type;

            this.deserializer = deserializer;
        }
    }
}
