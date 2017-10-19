﻿// standard namespaces
using System.Collections.Generic;
using System.Threading.Tasks;

// project namespaces
using TwitchNet.Api.Internal;
using TwitchNet.Enums.Utilities;
using TwitchNet.Interfaces.Api;
using TwitchNet.Models.Api.Streams;
using TwitchNet.Models.Api.Users;
using TwitchNet.Models.Paging.Streams;
using TwitchNet.Models.Paging.Users;

namespace TwitchNet.Api
{
    public static partial class
    TwitchApi
    {
        #region Users

        /// <summary>
        /// Asynchronously gets a user's information by their id.
        /// </summary>
        /// <param name="client_id">The Client Id to authorize the request.</param>
        /// <param name="ids">The id(s) of the user(s).</param>
        /// <returns></returns>
        public static async Task<ITwitchResponse<Users>>
        GetUsersByIdAsync(string client_id, params string[] ids)
        {
            ITwitchResponse<Users> users = await TwitchApiInternal.GetUsersAsync(Authentication.Client_ID, client_id, "id", ids);

            return users;
        }

        /// <summary>
        /// Asynchronously gets a user's information by their login name.
        /// </summary>
        /// <param name="client_id">The Client Id to authorize the request.</param>
        /// <param name="logins">The name(s) of the user(s).</param>
        /// <returns></returns>
        public static async Task<ITwitchResponse<Users>>
        GetUsersByLoginAsync(string client_id, params string[] logins)
        {
            ITwitchResponse<Users> users = await TwitchApiInternal.GetUsersAsync(Authentication.Client_ID, client_id, "login", logins);

            return users;
        }

        /// <summary>
        /// Asynchronously gets the relationship between two users.
        /// </summary>
        /// <param name="client_id">The Client Id to authorize the request.</param>
        /// <param name="to_id">The user to compare to.</param>
        /// <param name="from_id">The user to compare from.</param>
        /// <returns></returns>
        public static async Task<ITwitchResponse<Follows>>
        GetUserRelationshipAsync(string client_id, string to_id, string from_id)
        {
            ITwitchResponse<Follows> relationship = await TwitchApiInternal.GetUserRelationshipPageAsync(Authentication.Client_ID, client_id, to_id, from_id);

            return relationship;
        }

        /// <summary>
        /// Asynchronously gets a single paged of a user's followers list.
        /// </summary>
        /// <param name="client_id">The Client Id to authorize the request.</param>
        /// <param name="to_id">The user to get the followers for.</param>
        /// <param name="parameters">Optional. A set of parameters to customize the requests. The 'to_id' and 'from_id' properties in the parameters are ignored if specified.</param>
        /// <returns></returns>
        public static async Task<ITwitchResponse<Follows>>
        GetUserFollowersPageAsync(string client_id, string to_id, FollowsQueryParameters parameters = null)
        {
            ITwitchResponse<Follows> followers = await TwitchApiInternal.GetUserRelationshipPageAsync(Authentication.Client_ID, client_id, to_id, string.Empty, parameters);

            return followers;
        }

        /// <summary>
        /// Asynchronously gets a user's followers list.
        /// </summary>
        /// <param name="client_id">The Client Id to authorize the request.</param>
        /// <param name="to_id">The user to get the followers for.</param>
        /// <returns></returns>
        public static async Task<ITwitchResponse<IList<Follow>>>
        GetUserFollowersAsync(string client_id, string to_id)
        {
            ITwitchResponse<IList<Follow>> followers = await TwitchApiInternal.GetUserRelationshipAsync(Authentication.Client_ID, client_id, to_id, string.Empty);

            return followers;
        }

        /// <summary>
        /// Asynchronously gets a single page of a user's following list.
        /// </summary>
        /// <param name="client_id">The Client Id to authorize the request.</param>
        /// <param name="from_id">The user to get the following list from.</param>
        /// <param name="parameters">Optional. A set of parameters to customize the requests. The 'to_id' and 'from_id' properties in the parameters are ignored if specified.</param>
        /// <returns></returns>
        public static async Task<ITwitchResponse<Follows>>
        GetUserFollowingPageAsync(string client_id, string from_id, FollowsQueryParameters parameters = null)
        {
            ITwitchResponse<Follows> following = await TwitchApiInternal.GetUserRelationshipPageAsync(Authentication.Client_ID, client_id, string.Empty, from_id, parameters);

            return following;
        }

        /// <summary>
        /// Asynchronously gets a user's following list.
        /// </summary>
        /// <param name="client_id">The Client Id to authorize the request.</param>
        /// <param name="from_id">The user to get the following list from.</param>
        /// <returns></returns>
        public static async Task<ITwitchResponse<IList<Follow>>>
        GetUserFollowingAsync(string client_id, string from_id)
        {
            ITwitchResponse<IList<Follow>> following = await TwitchApiInternal.GetUserRelationshipAsync(Authentication.Client_ID, client_id, string.Empty, from_id);

            return following;
        }

        /// <summary>
        /// Asynchronously checks to see if a user (from_id) is following another user (to_id).
        /// </summary>
        /// <param name="client_id">The Client Id to authorize the request.</param>
        /// <param name="to_id">The user to compare to.</param>
        /// <param name="from_id">The user to compare from.</param>
        /// <returns>
        /// Returns true if a user (from_id) is following another user (to_id).
        /// Returns false otherwise.
        /// </returns>
        public static async Task<ITwitchResponse<bool>>
        IsUserFollowingAsync(string client_id, string to_id, string from_id)
        {
            ITwitchResponse<bool> is_following = await TwitchApiInternal.IsUserFollowingAsync(Authentication.Client_ID, client_id, to_id, from_id);

            return is_following;
        }

        #endregion

        #region Streams

        /// <summary>
        /// Asynchronously gets a single page of streams.
        /// </summary>
        /// <param name="client_id">The Client Id to authorize the request.</param>
        /// <param name="parameters">Optional. A set of parameters to customize the requests.</param>
        /// <returns></returns>
        public static async Task<ITwitchResponse<Streams>>
        GetStreamsPageAsync(string client_id, StreamsQueryParameters parameters = null)
        {
            ITwitchResponse<Streams> streams = await TwitchApiInternal.GetStreamsPageAsync(Authentication.Client_ID, client_id, parameters);

            return streams;
        }

        /// <summary>
        /// Asynchronously gets a complete list of streams.
        /// </summary>
        /// <param name="client_id">The Client Id to authorize the request.</param>
        /// <param name="parameters">Optional. A set of parameters to customize the requests.</param>
        /// <returns></returns>
        public static async Task<ITwitchResponse<IList<Stream>>>
        GetStreamsAsync(string client_id, StreamsQueryParameters parameters = null)
        {
            ITwitchResponse<IList<Stream>> streams = await TwitchApiInternal.GetStreamsAsync(Authentication.Client_ID, client_id, parameters);

            return streams;
        }

        #endregion
    }
}
