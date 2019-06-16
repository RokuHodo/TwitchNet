using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwitchNet.Rest.Api.Users
{
    public class
    DuplicateExtensionException : BodyParameterException
    {
        public string extension_name { get; protected set; }
        public string extension_id { get; protected set; }
        public string extension_version { get; protected set; }

        public DuplicateExtensionException(ActiveExtension extension, string message = null) : base(message)
        {
            extension_id = extension.id;
            extension_name = extension.name;
            extension_version = extension.version;
        }
    }
}
