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
