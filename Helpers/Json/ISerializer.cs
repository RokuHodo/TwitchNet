using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwitchNet.Helpers.Json
{
    public interface
    ISerializer
    {
        string content_type { get; set; }

        string Serialize(object obj);
    }
}
