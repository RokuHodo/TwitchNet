// standard namespaces
using System.Collections.Generic;

namespace
TwitchNet.Rest.Api.Users
{
    public class
    UsersParameters
    {
        /// <summary>
        /// <para>A list of user ID's to query.</para>
        /// <para>
        /// A maximum of 100 total elements can be specified between ids and logins.
        /// All elements that are null, empty, or only contain whitespace are filtered out and all duplicate elements are removed before calculating the final count.
        /// </para>
        /// </summary>
        [QueryParameter("id", typeof(SeparateQueryConverter))]
        public virtual List<string> ids     { get; set; }

        /// <summary>
        /// <para>A list of user login names to query.</para>
        /// <para>
        /// A maximum of 100 total elements can be specified between ids and logins.
        /// All elements that are null, empty, or only contain whitespace are filtered out and all duplicate elements are removed before calculating the final count.
        /// </para>
        /// </summary>
        [QueryParameter("login", typeof(SeparateQueryConverter))]
        public virtual List<string> logins  { get; set; }

        public UsersParameters()
        {
            ids = new List<string>();

            logins = new List<string>();
        }
    }
}
