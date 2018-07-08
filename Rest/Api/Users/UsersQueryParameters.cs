// standard namespaces
using System.Collections.Generic;

// project namespaces
using TwitchNet.Helpers;

namespace
TwitchNet.Rest.Api.Users
{
    public class
    UsersQueryParameters
    {
        private ClampedList<string> _ids    = new ClampedList<string>();
        private ClampedList<string> _logins = new ClampedList<string>();

        /// <summary>
        /// <para>A list of user id's names to get information about.</para>
        /// <para>
        /// Maximum: 100 id's.
        /// If more than 100 id's are specified, only the first 100 will be added.
        /// </para>
        /// </summary>
        [QueryParameter("id", typeof(SeparateValuesListFormatter))]
        public List<string> ids
        {
            get
            {
                return _ids.values;
            }
            set
            {
                _ids.values = value;
            }
        }

        /// <summary>
        /// <para>A list of user login names to get information about.</para>
        /// <para>
        /// Maximum: 100 logins.
        /// If more than 100 logins are specified, only the first 100 will be added.
        /// </para>
        /// </summary>
        [QueryParameter("login", typeof(SeparateValuesListFormatter))]
        public List<string> logins
        {
            get
            {
                return _logins.values;
            }
            set
            {
                _logins.values = value;
            }
        }

        /// <summary>
        /// Creates a new blank instance of the <see cref="UsersQueryParameters"/> class.
        /// </summary>
        public UsersQueryParameters()
        {

        }
    }
}
