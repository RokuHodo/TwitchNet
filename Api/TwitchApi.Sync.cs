// standard namespaces
using System.Collections.Generic;

// project namespaces
using TwitchNet.Helpers.Paging.Users;
using TwitchNet.Models.Api.Users;

namespace TwitchNet.Api
{
    public static partial class TwitchApi
    {
        #region Users

        /// <summary>
        /// Gets a user's information by their id.
        /// </summary>
        /// <param name="client_id">The Client Id to authorize the request.</param>
        /// <param name="ids">The id(s) of the user(s).</param>
        /// <returns></returns>
        public static Users GetUsersById(string client_id, params string[] ids)
        {
            Users users = GetUsersByIdAsync(client_id, ids).Result;

            return users;
        }

        /// <summary>
        /// Gets a user's information by their login name.
        /// </summary>
        /// <param name="client_id">The Client Id to authorize the request.</param>
        /// <param name="logins">The name(s) of the user(s).</param>
        /// <returns></returns>
        public static Users GetUsersByLogin(string client_id, params string[] logins)
        {
            Users users = GetUsersByLoginAsync(client_id, logins).Result;

            return users;
        }

        /// <summary>
        /// Gets the relationship between two users.
        /// </summary>
        /// <param name="client_id">The Client Id to authorize the request.</param>
        /// <param name="to_id">The user to compare to.</param>
        /// <param name="from_id">The user to compare from.</param>
        /// <returns></returns>
        public static Follows GetUserRelationship(string client_id, string to_id, string from_id)
        {
            Follows relationship = GetUserRelationshipAsync(client_id, to_id, from_id).Result;

            return relationship;
        }

        /// <summary>
        /// Gets a single paged of a user's followers list.
        /// </summary>
        /// <param name="client_id">The Client Id to authorize the request.</param>
        /// <param name="to_id">The user to get the followers for.</param>
        /// <param name="parameters">Optional. A set of parameters to customize the requests. The 'to_id' and 'from_id' properties in the parameters are ignored if specified.</param>
        /// <returns></returns>
        public static Follows GetUserFollowersPage(string client_id, string to_id, FollowsQueryParameters parameters = null)
        {
            Follows followers = GetUserFollowersPageAsync(client_id, to_id, parameters).Result;

            return followers;
        }

        /// <summary>
        /// Gets a user's followers list.
        /// </summary>
        /// <param name="client_id">The Client Id to authorize the request.</param>
        /// <param name="to_id">The user to get the followers for.</param>
        /// <returns></returns>
        public static List<Follow> GetUserFollowers(string client_id, string to_id)
        {
            List<Follow> followers = GetUserFollowersAsync(client_id, to_id).Result;

            return followers;
        }

        /// <summary>
        /// Gets a single page of a user's following list.
        /// </summary>
        /// <param name="client_id">The Client Id to authorize the request.</param>
        /// <param name="from_id">The user to get the following list from.</param>
        /// <param name="parameters">Optional. A set of parameters to customize the requests. The 'to_id' and 'from_id' properties in the parameters are ignored if specified.</param>
        /// <returns></returns>
        public static Follows GetUserFollowingPage(string client_id, string from_id, FollowsQueryParameters parameters = null)
        {
            Follows following = GetUserFollowingPageAsync(client_id, from_id, parameters).Result;

            return following;
        }

        /// <summary>
        /// Gets a user's following list.
        /// </summary>
        /// <param name="client_id">The Client Id to authorize the request.</param>
        /// <param name="from_id">The user to get the following list from.</param>
        /// <returns></returns>
        public static List<Follow> GetUserFollowing(string client_id, string from_id)
        {
            List<Follow> following = GetUserFollowingAsync(client_id, from_id).Result;

            return following;
        }

        /// <summary>
        /// Checks to see if a user (from_id) is following another user (to_id).
        /// </summary>
        /// <param name="client_id">The Client Id to authorize the request.</param>
        /// <param name="to_id">The user to compare to.</param>
        /// <param name="from_id">The user to compare from.</param>
        /// <returns>
        /// Returns true if a user (from_id) is following another user (to_id).
        /// Returns false otherwise.
        /// </returns>
        public static bool IsUserFollowing(string client_id, string to_id, string from_id)
        {
            bool is_following = IsUserFollowingAsync(client_id, to_id, from_id).Result;

            return is_following;
        }

        #endregion
    }
}
