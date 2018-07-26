// standard namespaces
using System;

// project namespaces
using TwitchNet.Extensions;

namespace TwitchNet.Rest
{
    public class
    MissingScopesException : Exception
    {
        public Scopes[] missing_scopes { get; private set; }

        public MissingScopesException()
        {

        }

        public MissingScopesException(string message) : base(message)
        {

        }

        public MissingScopesException(Scopes[] scopes)
        {
            missing_scopes = scopes.IsValid() ? scopes : new Scopes[0];
        }

        public MissingScopesException(string message, Scopes[] scopes) : base(message)
        {
            missing_scopes = scopes.IsValid() ? scopes : new Scopes[0];
        }
    }
}
