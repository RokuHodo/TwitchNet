using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwitchNet.Helpers.Json
{
    public interface
    IDeserializer
    {
        result_type Deserialize<result_type>(string content);
    }
}
