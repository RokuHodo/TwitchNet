using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwitchNet.Rest
{
    public class
    QueryParameter
    {
        public string name;

        public string value;
    }

    public class
    RestParameter
    {
        public string name;

        public object value;

        public string content_type;

        public HttpParameterType type;
    }
}
