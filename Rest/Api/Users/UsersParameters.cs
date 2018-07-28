// standard namespaces
using System.Collections.Generic;

namespace
TwitchNet.Rest.Api.Users
{
    public class
    UsersParameters
    {
        private List<string> _ids = new List<string>(100);
        private List<string> _logins = new List<string>(100);

        /// <summary>
        /// <para>A list of user id's names to get information about.</para>
        /// <para>
        /// Maximum: 100 id's.
        /// If more than 100 id's are specified, only the first 100 will be added.
        /// </para>
        /// </summary>
        [QueryParameter("id", typeof(SeparateQueryConverter))]
        public virtual List<string> ids     => _ids;

        /// <summary>
        /// <para>A list of user login names to get information about.</para>
        /// <para>
        /// Maximum: 100 logins.
        /// If more than 100 logins are specified, only the first 100 will be added.
        /// </para>
        /// </summary>
        [QueryParameter("login", typeof(SeparateQueryConverter))]
        public virtual List<string> logins  => _logins;
}
}
