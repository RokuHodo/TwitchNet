﻿// standard namespaces
using System.Collections.Generic;

// project namespaces
using TwitchNet.Helpers.Paging.Streams;
using TwitchNet.Helpers.Paging.Users;
using TwitchNet.Models.Api.Streams;
using TwitchNet.Models.Api.Users;

namespace TwitchNet.Api
{
    public static partial class TwitchApiOauth
    {
        #region Users

        /// <summary>
        /// Gets a user's information by their id.
        /// Optional Scope: 'user:read:email'
        /// </summary>
        /// <param name="oauth_token">The OAuth token to authorize the request.</param>
        /// <param name="ids">The id(s) of the user(s).</param>
        /// <returns></returns>
        public static Users GetUsersById(string oauth_token, params string[] ids)
        {
            Users users = GetUsersByIdAsync(oauth_token, ids).Result;

            return users;
        }

        /// <summary>
        /// Gets a user's information by their login name.
        /// Optional Scope: 'user:read:email'
        /// </summary>
        /// <param name="oauth_token">The OAuth token to authorize the request.</param>
        /// <param name="logins">The name(s) of the user(s).</param>
        /// <returns></returns>
        public static Users GetUsersByLogin(string oauth_token, params string[] logins)
        {
            Users users = GetUsersByLoginAsync(oauth_token, logins).Result;

            return users;
        }

        /// <summary>
        /// Gets the relationship between two users.
        /// </summary>
        /// <param name="oauth_token">The OAuth token to authorize the request.</param>
        /// <param name="to_id">The user to compare to.</param>
        /// <param name="from_id">The user to compare from.</param>
        /// <returns></returns>
        public static Follows GetUserRelationship(string oauth_token, string to_id, string from_id)
        {
            Follows relationship = GetUserRelationshipAsync(oauth_token, to_id, from_id).Result;

            return relationship;
        }

        /// <summary>
        /// Gets a single paged of a user's followers list.
        /// </summary>
        /// <param name="oauth_token">The OAuth token to authorize the request.</param>
        /// <param name="to_id">The user to get the followers for.</param>
        /// <param name="parameters">Optional. A set of parameters to customize the requests. The 'to_id' and 'from_id' properties in the parameters are ignored if specified.</param>
        /// <returns></returns>
        public static Follows GetUserFollowersPage(string oauth_token, string to_id, FollowsQueryParameters parameters = null)
        {
            Follows followers = GetUserFollowersPageAsync(oauth_token, to_id, parameters).Result;

            return followers;
        }

        /// <summary>
        /// Gets a user's followers list.
        /// </summary>
        /// <param name="oauth_token">The OAuth token to authorize the request.</param>
        /// <param name="to_id">The user to get the followers for.</param>
        /// <returns></returns>
        public static List<Follow> GetUserFollowers(string oauth_token, string to_id)
        {
            List<Follow> followers = GetUserFollowersAsync(oauth_token, to_id).Result;

            return followers;
        }

        /// <summary>
        /// Gets a single page of a user's following list.
        /// </summary>
        /// <param name="oauth_token">The OAuth token to authorize the request.</param>
        /// <param name="from_id">The user to get the following list from.</param>
        /// <param name="parameters">Optional. A set of parameters to customize the requests. The 'to_id' and 'from_id' properties in the parameters are ignored if specified.</param>
        /// <returns></returns>
        public static Follows GetUserFollowingPage(string oauth_token, string from_id, FollowsQueryParameters parameters = null)
        {
            Follows following = GetUserFollowingPageAsync(oauth_token, from_id, parameters).Result;

            return following;
        }

        /// <summary>
        /// Gets a user's following list.
        /// </summary>
        /// <param name="oauth_token">The OAuth token to authorize the request.</param>
        /// <param name="from_id">The user to get the following list from.</param>
        /// <returns></returns>
        public static List<Follow> GetUserFollowing(string oauth_token, string from_id)
        {
            List<Follow> following = GetUserFollowingAsync(oauth_token, from_id).Result;

            return following;
        }

        /// <summary>
        /// Checks to see if a user (from_id) is following another user (to_id).
        /// </summary>
        /// <param name="oauth_token">The OAuth token to authorize the request.</param>
        /// <param name="to_id">The user to compare to.</param>
        /// <param name="from_id">The user to compare from.</param>
        /// <returns>
        /// Returns true if a user (from_id) is following another user (to_id).
        /// Returns false otherwise.
        /// </returns>
        public static bool IsUserFollowing(string oauth_token, string to_id, string from_id)
        {
            bool is_following = IsUserFollowingAsync(oauth_token, to_id, from_id).Result;

            return is_following;
        }

        /// <summary>
        /// Asynchronously sets the description of a user specified by their OAuth token.
        /// </summary>
        /// <param name="user_oauth_token">The user's OAuth token used to validate the request and determine which description to update.</param>
        /// <param name="description">The new description to set.</param>
        /// <returns>
        /// Returns true if the description of the user was successfully updated.
        /// Returns false otherwise.
        /// </returns>
        public static bool SetUserDescription(string user_oauth_token, string description)
        {
            bool success = SetUserDescriptionAsync(user_oauth_token, description).Result;

            return success;
        }

        /// <summary>
        /// Asynchronously sets the description of a user specified by their OAuth token.
        /// </summary>
        /// <param name="oauth_token">The OAuth token to authorize the request.</param>
        /// <param name="user_oauth_token">The user's OAuth token used to determine which description to update.</param>
        /// <param name="supplementary_token">The Client Id of the application to validate the request.</param>
        /// <param name="description">The new description to set.</param>
        /// <returns>
        /// Returns true if the description of the user was successfully updated.
        /// Returns false otherwise.
        /// </returns>
        public static bool SetUserDescription(string user_oauth_token, string supplementary_token, string description)
        {
            bool success = SetUserDescriptionAsync(user_oauth_token, supplementary_token, description).Result;

            return success;
        }

        #endregion

        #region Streams

        /// <summary>
        /// Gets a single page of streams.
        /// </summary>
        /// <param name="oauth_token">The OAuth token to authorize the request.</param>
        /// <param name="parameters">Optional. A set of parameters to customize the requests.</param>
        /// <returns></returns>
        public static Streams GetStreamsPage(string oauth_token, StreamsQueryParameters parameters = null)
        {
            Streams streams = GetStreamsPageAsync(oauth_token, parameters).Result;

            return streams;
        }

        /// <summary>
        /// Gets a complete list of streams.
        /// </summary>
        /// <param name="oauth_token">The OAuth token to authorize the request.</param>
        /// <param name="parameters">Optional. A set of parameters to customize the requests.</param>
        /// <returns></returns>
        public static List<Stream> GetStreams(string oauth_token, StreamsQueryParameters parameters = null)
        {
            List<Stream> streams = GetStreamsAsync(oauth_token, parameters).Result;

            return streams;
        }

        #endregion
    }
}
