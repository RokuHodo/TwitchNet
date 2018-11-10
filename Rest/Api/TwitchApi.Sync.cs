//// project namespaces
//using TwitchNet.Rest.Api.Clips;
//using TwitchNet.Rest.Api.Games;
//using TwitchNet.Rest.Api.Streams;
//using TwitchNet.Rest.Api.Users;
//using TwitchNet.Rest.Api.Videos;

//namespace
//TwitchNet.Rest.Api
//{
//    public static partial class
//    TwitchApi
//    {
//        #region /users

//        /// <summary>
//        /// Gets the information about one or more users.
//        /// </summary>
//        /// <param name="client_id">The application ID to identify the source of the request.</param>
//        /// <param name="settings">Settings to customize how the inputs, request, and response are handled.</param>
//        /// <returns>
//        /// Returns data that adheres to the <see cref="IHelixResponse{result_type}"/> interface.
//        /// <see cref="IHelixResponse{result_type}.result"/> contains the information about each requested user.
//        /// </returns>
//        /// <exception cref="ArgumentNullException">Thrown if parameters is null.</exception>
//        /// <exception cref="ArgumentException">
//        /// Thrown if the client ID is null, empty, or contains only whitespace.
//        /// Thrown if all specified user logins and user ID's are null, empty, or only contains whitespace.
//        /// Thrown if more than 100 total user logins and/or user IDs are specified.
//        /// </exception>
//        /// <exception cref="Exception">Thrown if an error occurred in an external assembly while assembling or executing a request, or while deserializing a response.</exception>
//        /// <exception cref="StatusException">Thrown if an error was returned by Twitch after executing the request.</exception>
//        /// <exception cref="RetryLimitReachedException">Thrown if the retry limit was reached.</exception>
//        public static IHelixResponse<Data<User>>
//        GetUsers(string client_id, UsersParameters parameters, HelixRequestSettings settings = default)
//        {
//            RestInfo<Data<User>> info = new RestInfo<Data<User>>(RestClients.Helix, settings);
//            info.client_id = client_id;

//            IHelixResponse<Data<User>> response = GetUsersAsync(client_id, parameters, settings).Result;

//            return response;
//        }

//        #endregion

//        #region /users/follows

//        /// <summary>
//        /// Gets a single page of user's following list.
//        /// </summary>
//        /// <param name="client_id">The application ID to identify the source of the request.</param>
//        /// <param name="from_id">The user ID to get the following list for.</param>
//        /// <param name="settings">Settings to customize how the inputs, request, and response are handled.</param>
//        /// <returns>
//        /// Returns data that adheres to the <see cref="IHelixResponse{result_type}"/> interface.
//        /// <see cref="IHelixResponse{result_type}.result"/> contains the single page of user's following list.
//        /// </returns>
//        /// <exception cref="ArgumentException">
//        /// Thrown if the client ID is null, empty, or contains only whitespace.
//        /// Thrown if <paramref name="from_id"/> is null, empty, or contains only whitespace.
//        /// </exception>
//        /// <exception cref="Exception">Thrown if an error occurred in an external assembly while assembling or executing a request, or while deserializing a response.</exception>
//        /// <exception cref="StatusException">Thrown if an error was returned by Twitch after executing the request.</exception>
//        /// <exception cref="RetryLimitReachedException">Thrown if the retry limit was reached.</exception>
//        public static IHelixResponse<FollowsDataPage<Follow>>
//        GetUserFollowingPage(string client_id, string from_id, HelixRequestSettings settings = default)
//        {
//            IHelixResponse<FollowsDataPage<Follow>> response = GetUserFollowingPageAsync(client_id, from_id, settings).Result;

//            return response;
//        }

//        /// <summary>
//        /// Gets a single page of user's following list.
//        /// </summary>
//        /// <param name="client_id">The application ID to identify the source of the request.</param>
//        /// <param name="from_id">The user ID to get the following list for.</param>
//        /// <param name="parameters">
//        /// A set of rest parameters to add to the request.
//        /// If specified, from_id and to_id are ignored.
//        /// </param>
//        /// <param name="settings">Settings to customize how the inputs, request, and response are handled.</param>
//        /// <returns>
//        /// Returns data that adheres to the <see cref="IHelixResponse{result_type}"/> interface.
//        /// <see cref="IHelixResponse{result_type}.result"/> contains the single page of user's following list.
//        /// </returns>
//        /// <exception cref="ArgumentException">
//        /// Thrown if the client ID is null, empty, or contains only whitespace.
//        /// Thrown if <paramref name="from_id"/> is null, empty, or contains only whitespace.
//        /// </exception>
//        /// <exception cref="Exception">Thrown if an error occurred in an external assembly while assembling or executing a request, or while deserializing a response.</exception>
//        /// <exception cref="StatusException">Thrown if an error was returned by Twitch after executing the request.</exception>
//        /// <exception cref="RetryLimitReachedException">Thrown if the retry limit was reached.</exception>
//        public static IHelixResponse<FollowsDataPage<Follow>>
//        GetUserFollowingPage(string client_id, string from_id, FollowsParameters parameters, HelixRequestSettings settings = default)
//        {
//            IHelixResponse<FollowsDataPage<Follow>> response = GetUserFollowingPageAsync(client_id, from_id, parameters, settings).Result;

//            return response;
//        }

//        /// <summary>
//        /// Gets a user's complete following list.
//        /// </summary>
//        /// <param name="client_id">The application ID to identify the source of the request.</param>
//        /// <param name="from_id">The user ID to get the following list for.</param>
//        /// <param name="settings">Settings to customize how the inputs, request, and response are handled.</param>
//        /// <returns>
//        /// Returns data that adheres to the <see cref="IHelixResponse{result_type}"/> interface.
//        /// <see cref="IHelixResponse{result_type}.result"/> contains the user's complete following list.
//        /// </returns>
//        /// <exception cref="ArgumentException">
//        /// Thrown if the client ID is null, empty, or contains only whitespace.
//        /// Thrown if <paramref name="from_id"/> is null, empty, or contains only whitespace.
//        /// </exception>
//        /// <exception cref="Exception">Thrown if an error occurred in an external assembly while assembling or executing a request, or while deserializing a response.</exception>
//        /// <exception cref="StatusException">Thrown if an error was returned by Twitch after executing the request.</exception>
//        /// <exception cref="RetryLimitReachedException">Thrown if the retry limit was reached.</exception>
//        public static IHelixResponse<FollowsDataPage<Follow>>
//        GetUserFollowing(string client_id, string from_id, HelixRequestSettings settings = default)
//        {
//            IHelixResponse<FollowsDataPage<Follow>> response = GetUserFollowingAsync(client_id, from_id, settings).Result;

//            return response;
//        }

//        /// <summary>
//        /// Gets a user's complete following list.
//        /// </summary>
//        /// <param name="client_id">The application ID to identify the source of the request.</param>
//        /// <param name="from_id">The user ID to get the following list for.</param>
//        /// <param name="parameters">
//        /// A set of rest parameters to add to the request.
//        /// If specified, from_id and to_id are ignored.
//        /// </param>
//        /// <param name="settings">Settings to customize how the inputs, request, and response are handled.</param>
//        /// <returns>
//        /// Returns data that adheres to the <see cref="IHelixResponse{result_type}"/> interface.
//        /// <see cref="IHelixResponse{result_type}.result"/> contains the user's complete following list.
//        /// </returns>
//        /// <exception cref="ArgumentException">
//        /// Thrown if the client ID is null, empty, or contains only whitespace.
//        /// Thrown if <paramref name="from_id"/> is null, empty, or contains only whitespace.
//        /// </exception>
//        /// <exception cref="Exception">Thrown if an error occurred in an external assembly while assembling or executing a request, or while deserializing a response.</exception>
//        /// <exception cref="StatusException">Thrown if an error was returned by Twitch after executing the request.</exception>
//        /// <exception cref="RetryLimitReachedException">Thrown if the retry limit was reached.</exception>
//        public static IHelixResponse<FollowsDataPage<Follow>>
//        GetUserFollowing(string client_id, string from_id, FollowsParameters parameters, HelixRequestSettings settings = default)
//        {
//            IHelixResponse<FollowsDataPage<Follow>> response = GetUserFollowingAsync(client_id, from_id, parameters, settings).Result;

//            return response;
//        }

//        /// <summary>
//        /// Gets a single page of a user's followers list.
//        /// </summary>
//        /// <param name="client_id">The application ID to identify the source of the request.</param>
//        /// <param name="to_id">The user ID to get the follower list for.</param>
//        /// <param name="settings">Settings to customize how the inputs, request, and response are handled.</param>
//        /// <returns>
//        /// Returns data that adheres to the <see cref="IHelixResponse{result_type}"/> interface.
//        /// <see cref="IHelixResponse{result_type}.result"/> contains the single page of a user's followers list.
//        /// </returns> 
//        /// <exception cref="ArgumentException">
//        /// Thrown if the client ID is null, empty, or contains only whitespace.
//        /// Thrown if <paramref name="to_id"/> is null, empty, or contains only whitespace.
//        /// </exception>
//        /// <exception cref="Exception">Thrown if an error occurred in an external assembly while assembling or executing a request, or while deserializing a response.</exception>
//        /// <exception cref="StatusException">Thrown if an error was returned by Twitch after executing the request.</exception>
//        /// <exception cref="RetryLimitReachedException">Thrown if the retry limit was reached.</exception>
//        public static IHelixResponse<FollowsDataPage<Follow>>
//        GetUserFollowersPage(string client_id, string to_id, HelixRequestSettings settings = default)
//        {
//            IHelixResponse<FollowsDataPage<Follow>> response = GetUserFollowersPageAsync(client_id, to_id, settings).Result;

//            return response;
//        }

//        /// <summary>
//        /// Gets a single page of a user's followers list.
//        /// </summary>
//        /// <param name="client_id">The application ID to identify the source of the request.</param>
//        /// <param name="to_id">The user ID to get the follower list for.</param>
//        /// <param name="parameters">
//        /// A set of rest parameters to add to the request.
//        /// If specified, from_id and to_id are ignored.
//        /// </param>
//        /// <param name="settings">Settings to customize how the inputs, request, and response are handled.</param>
//        /// <returns>
//        /// Returns data that adheres to the <see cref="IHelixResponse{result_type}"/> interface.
//        /// <see cref="IHelixResponse{result_type}.result"/> contains the single page of a user's followers list.
//        /// </returns> 
//        /// <exception cref="ArgumentException">
//        /// Thrown if the client ID is null, empty, or contains only whitespace.
//        /// Thrown if <paramref name="to_id"/> is null, empty, or contains only whitespace.
//        /// </exception>
//        /// <exception cref="Exception">Thrown if an error occurred in an external assembly while assembling or executing a request, or while deserializing a response.</exception>
//        /// <exception cref="StatusException">Thrown if an error was returned by Twitch after executing the request.</exception>
//        /// <exception cref="RetryLimitReachedException">Thrown if the retry limit was reached.</exception>
//        public static IHelixResponse<FollowsDataPage<Follow>>
//        GetUserFollowersPage(string client_id, string to_id, FollowsParameters parameters, HelixRequestSettings settings = default)
//        {
//            IHelixResponse<FollowsDataPage<Follow>> response = GetUserFollowersPageAsync(client_id, to_id, parameters, settings).Result;

//            return response;
//        }

//        /// <summary>
//        /// Gets a user's complete follower list.
//        /// </summary>
//        /// <param name="client_id">The application ID to identify the source of the request.</param>
//        /// <param name="to_id">The user ID to get the follower list for.</param>
//        /// <param name="settings">Settings to customize how the inputs, request, and response are handled.</param>
//        /// <returns>
//        /// Returns data that adheres to the <see cref="IHelixResponse{result_type}"/> interface.
//        /// <see cref="IHelixResponse{result_type}.result"/> contains the user's complete follower list.
//        /// </returns>
//        /// <exception cref="ArgumentException">
//        /// Thrown if the client ID is null, empty, or contains only whitespace.
//        /// Thrown if <paramref name="to_id"/> is null, empty, or contains only whitespace.
//        /// </exception>
//        /// <exception cref="Exception">Thrown if an error occurred in an external assembly while assembling or executing a request, or while deserializing a response.</exception>
//        /// <exception cref="StatusException">Thrown if an error was returned by Twitch after executing the request.</exception>
//        /// <exception cref="RetryLimitReachedException">Thrown if the retry limit was reached.</exception>
//        public static IHelixResponse<FollowsDataPage<Follow>>
//        GetUserFollowers(string client_id, string to_id, HelixRequestSettings settings = default)
//        {
//            IHelixResponse<FollowsDataPage<Follow>> response = GetUserFollowersAsync(client_id, to_id, settings).Result;

//            return response;
//        }

//        /// <summary>
//        /// Gets a user's complete follower list.
//        /// </summary>
//        /// <param name="client_id">The application ID to identify the source of the request.</param>
//        /// <param name="to_id">The user ID to get the follower list for.</param>
//        /// <param name="parameters">
//        /// A set of rest parameters to add to the request.
//        /// If specified, from_id and to_id are ignored.
//        /// </param>
//        /// <param name="settings">Settings to customize how the inputs, request, and response are handled.</param>
//        /// <returns>
//        /// Returns data that adheres to the <see cref="IHelixResponse{result_type}"/> interface.
//        /// <see cref="IHelixResponse{result_type}.result"/> contains the user's complete follower list.
//        /// </returns>
//        /// <exception cref="ArgumentException">
//        /// Thrown if the client ID is null, empty, or contains only whitespace.
//        /// Thrown if <paramref name="to_id"/> is null, empty, or contains only whitespace.
//        /// </exception>
//        /// <exception cref="Exception">Thrown if an error occurred in an external assembly while assembling or executing a request, or while deserializing a response.</exception>
//        /// <exception cref="StatusException">Thrown if an error was returned by Twitch after executing the request.</exception>
//        /// <exception cref="RetryLimitReachedException">Thrown if the retry limit was reached.</exception>
//        public static IHelixResponse<FollowsDataPage<Follow>>
//        GetUserFollowers(string client_id, string to_id, FollowsParameters parameters, HelixRequestSettings settings = default)
//        {
//            IHelixResponse<FollowsDataPage<Follow>> response = GetUserFollowersAsync(client_id, to_id, parameters, settings).Result;

//            return response;
//        }

//        /// <summary>
//        /// Checks to see if a user is following another user.
//        /// </summary>
//        /// <param name="client_id">The application ID to identify the source of the request.</param>
//        /// <param name="from_id">The user ID to check if they are following another user.</param>
//        /// <param name="to_id">The user ID to check if another user is following them.</param>
//        /// <param name="settings">Settings to customize how the inputs, request, and response are handled.</param>
//        /// <returns>
//        /// Returns data that adheres to the <see cref="IHelixResponse{result_type}"/> interface.
//        /// <see cref="IHelixResponse{result_type}.result"/> is set true if from_id is following to_id, otherwise false.
//        /// </returns>        
//        /// <exception cref="ArgumentException">
//        /// Thrown if the client ID is null, empty, or contains only whitespace.
//        /// Thrown if either from_id and to_id are null, empty, or contains only whitespace.
//        /// </exception>
//        /// <exception cref="Exception">Thrown if an error occurred in an external assembly while assembling or executing a request, or while deserializing a response.</exception>
//        /// <exception cref="StatusException">Thrown if an error was returned by Twitch after executing the request.</exception>
//        /// <exception cref="RetryLimitReachedException">Thrown if the retry limit was reached.</exception>
//        public static IHelixResponse<bool>
//        IsUserFollowing(string client_id, string from_id, string to_id, HelixRequestSettings settings = default)
//        {
//            IHelixResponse<bool> is_following = IsUserFollowingAsync(client_id, from_id, to_id, settings).Result;

//            return is_following;
//        }

//        /// <summary>
//        /// Gets the relationship between two users, or a single page of a user's following/follower list.
//        /// </summary>
//        /// <param name="client_id">The application ID to identify the source of the request.</param>
//        /// <param name="parameters">
//        /// <para>A set of rest parameters to add to the request.</para>
//        /// <para>A from_id or to_id must be specified.</para>
//        /// </param>
//        /// <param name="settings">Settings to customize how the inputs, request, and response are handled.</param>
//        /// <returns>
//        /// Returns data that adheres to the <see cref="IHelixResponse{result_type}"/> interface.
//        /// <see cref="IHelixResponse{result_type}.result"/> contains the user relationship page, or a single page of the following/follower list of one user.
//        /// <exception cref="ArgumentNullException">Thrown if parameters is null.</exception>
//        /// <exception cref="ArgumentException">
//        /// Thrown if the client ID is null, empty, or contains only whitespace.
//        /// Thrown if both from_id and to_id are null, empty, or contains only whitespace.
//        /// </exception>
//        /// <exception cref="Exception">Thrown if an error occurred in an external assembly while assembling or executing a request, or while deserializing a response.</exception>
//        /// <exception cref="StatusException">Thrown if an error was returned by Twitch after executing the request.</exception>
//        /// <exception cref="RetryLimitReachedException">Thrown if the retry limit was reached.</exception>
//        public static IHelixResponse<FollowsDataPage<Follow>>
//        GetUserRelationshipPage(string client_id, FollowsParameters parameters, HelixRequestSettings settings = default)
//        {
//            IHelixResponse<FollowsDataPage<Follow>> response = GetUserRelationshipPageAsync(client_id, parameters, settings).Result;

//            return response;
//        }

//        /// <summary>
//        /// Gets the relationship between two users, or a user's complete following/follower list.
//        /// </summary>
//        /// <param name="client_id">The application ID to identify the source of the request.</param>
//        /// <param name="parameters">
//        /// <para>A set of rest parameters to add to the request.</para>
//        /// <para>A from_id or to_id must be specified.</para>
//        /// </param>
//        /// <param name="settings">Settings to customize how the inputs, request, and response are handled.</param>
//        /// <returns>
//        /// Returns data that adheres to the <see cref="IHelixResponse{result_type}"/> interface.
//        /// <see cref="IHelixResponse{result_type}.result"/> contains the user relationship, or the complete following/follower list of one user.
//        /// </returns>
//        /// <exception cref="ArgumentNullException">Thrown if parameters is null.</exception>
//        /// <exception cref="ArgumentException">
//        /// Thrown if the client ID is null, empty, or contains only whitespace.
//        /// Thrown if both from_id and to_id are null, empty, or contains only whitespace.
//        /// </exception>
//        /// <exception cref="Exception">Thrown if an error occurred in an external assembly while assembling or executing a request, or while deserializing a response.</exception>
//        /// <exception cref="StatusException">Thrown if an error was returned by Twitch after executing the request.</exception>
//        /// <exception cref="RetryLimitReachedException">Thrown if the retry limit was reached.</exception>
//        public static IHelixResponse<FollowsDataPage<Follow>>
//        GetUserRelationship(string client_id, FollowsParameters parameters, HelixRequestSettings settings = default)
//        {
//            IHelixResponse<FollowsDataPage<Follow>> response = GetUserRelationshipAsync(client_id, parameters, settings).Result;

//            return response;
//        }

//        #endregion
//    }
//}
