// standard namespaces
using System.Runtime.Serialization;

namespace TwitchNet.Enums.Api
{
    public enum BroadcasterType
    {
        [EnumMember(Value = "")]
        Empty       = 0,
        [EnumMember(Value = "partner")]
        Partner     = 1,
        [EnumMember(Value = "affiliate")]
        Affiliate   = 2
    }
}
