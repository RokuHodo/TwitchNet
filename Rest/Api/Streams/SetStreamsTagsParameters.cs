// standard namespaces
using System.Collections.Generic;

using Newtonsoft.Json;

namespace
TwitchNet.Rest.Api.Tags
{
    public class
    SetStreamsTagsParameters
    {
        static string test = "fskjfsdhkjfds";

        /// <summary>
        /// A user ID to update the stream tags for.
        /// </summary>
        [QueryParameter("broadcaster_id")]
        public virtual string broadcaster_id { get; set; }

        /// <summary>
        /// A user ID who is live to get the strema tags for.
        /// </summary>
        [Body("tag_ids")]
        public virtual List<string> tag_ids { get; set; }

        public SetStreamsTagsParameters()
        {
            tag_ids = new List<string>();
        }
    }
}
