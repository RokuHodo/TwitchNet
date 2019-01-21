using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace TwitchNet.Rest.OAuth
{
    public enum
    GrantType
    {
        [EnumMember(Value = "client_credentials")]
        ClientCredentials = 0
    }
}
