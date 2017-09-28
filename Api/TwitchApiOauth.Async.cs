// standard namespaces
using System.Collections.Generic;
using System.Threading.Tasks;

// project namespaces
using TwitchNet.Api.Internal;
using TwitchNet.Enums.Api;
using TwitchNet.Extensions;
using TwitchNet.Helpers.Paging.Users;
using TwitchNet.Models.Api.Users;

namespace TwitchNet.Api
{
    public static partial class TwitchApiOauth
    {
        #region Users

        /// <summary>
        /// Optional Scope - user:read:email
        /// </summary>
        /// <param name="oauth_token"></param>
        /// <param name="ids"></param>
        /// <returns></returns>
        public static async Task<Users> GetUsersByIdAsync(string oauth_token, params string[] ids)
        {
            Users users = await TwitchApiInternal.GetUsersAsync(Authentication.Authorization, oauth_token, "id", ids);

            return users;
        }

        /// <summary>
        /// Optional Scope - user:read:email
        /// </summary>
        /// <param name="oauth_token"></param>
        /// <param name="logins"></param>
        /// <returns></returns>
        public static async Task<Users> GetUsersByLoginAsync(string oauth_token, params string[] logins)
        {
            Users users = await TwitchApiInternal.GetUsersAsync(Authentication.Authorization, oauth_token, "login", logins);

            return users;
        }

        // TODO: (Api.TwitchApiOauth.Async) 'GetUserFollowsPageAsync' - Implement GetUserFollowsAsync
        public static async Task<Follows> GetUserFollowsPageAsync(string oauth_token, FollowsParameters parameters)
        {
            Follows follows = await TwitchApiInternal.GetUserFollowsPageAsync(Authentication.Authorization, oauth_token, parameters);

            return follows;
        }

        // TODO: (Api.TwitchApiOauth.Async) 'GetUserFollowersPageAsync' - Implement GetUserFollowersAsync
        public static async Task<Follows> GetUserFollowersPageAsync(string oauth_token, string to_id, FollowsParameters parameters = null)
        {
            Follows followers = await TwitchApiInternal.GetUserRelationshipPageAsync(Authentication.Authorization, oauth_token, to_id, string.Empty, parameters);

            return followers;
        }

        public static async Task<List<Follower>> GetUserFollowersAsync(string oauth_token, string to_id)
        {
            List<Follower> followers = await TwitchApiInternal.GetUserRelationshipAsync(Authentication.Authorization, oauth_token, to_id, string.Empty);

            return followers;
        }

        // TODO: (Api.TwitchApiOauth.Async) 'GetUserFollowingPageAsync' - Implement GetUserFollowingAsync
        public static async Task<Follows> GetUserFollowingPageAsync(string oauth_token, string from_id, FollowsParameters parameters = null)
        {
            Follows followers = await TwitchApiInternal.GetUserRelationshipPageAsync(Authentication.Authorization, oauth_token, string.Empty, from_id, parameters);

            return followers;
        }

        /// <summary>
        /// Checks to see if from_id is following to_id.
        /// </summary>
        /// <param name="oauth_token"></param>
        /// <param name="to_id"></param>
        /// <param name="from_id"></param>
        /// <returns></returns>
        public static async Task<bool> IsUserFollowing(string oauth_token, string to_id, string from_id)
        {
            Follows relationship = await TwitchApiInternal.GetUserRelationshipPageAsync(Authentication.Authorization, oauth_token, to_id, from_id);

            return relationship.data.isValid();
        }

        #endregion
    }
}
