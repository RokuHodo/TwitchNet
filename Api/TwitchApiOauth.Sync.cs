// project namespaces
using TwitchNet.Interfaces.Api;
using TwitchNet.Models.Api;
using TwitchNet.Models.Api.Entitlements;
using TwitchNet.Models.Api.Games;
using TwitchNet.Models.Api.Streams;
using TwitchNet.Models.Api.Users;
using TwitchNet.Models.Api.Videos;

namespace TwitchNet.Api
{
    public static partial class
    TwitchApiOauth
    {
        #region Entitlements

        /// <summary>
        /// <para>Creates a URL where you can upload a manifest file and notify users that they have an entitlement.</para>
        /// <para>Required Authorization: Application Access Token</para>
        /// </summary>
        /// <param name="application_token">The application access token used to determine whose description to update and authorize the request.</param>
        /// <param name="query_parameters">A set of query parameters to customize the request.</param>
        /// <param name="api_request_settings">A set up customizable settings to handle diferent status codes.</param>
        /// <returns>Returns data that adheres to the <see cref="IApiResponse{type}"/> interface.</returns>
        public static IApiResponse<Url>
        CreateEntitlementGrantsUploadUrl(string application_token, EntitlementQueryParameters query_parameters, ApiRequestSettings api_request_settings = api_request_settings_default)
        {
            IApiResponse<Url> url = CreateEntitlementGrantsUploadUrlAsync(application_token, query_parameters, api_request_settings).Result;

            return url;
        }

        /// <summary>
        /// <para>Creates a URL where you can upload a manifest file and notify users that they have an entitlement.</para>
        /// <para>Required Authorization: Application Access Token</para>
        /// </summary>
        /// <param name="application_token">The application access token used to determine whose description to update and authorize the request.</param>
        /// <param name="client_id">The Client ID to identify the application making the request.</param>
        /// <param name="query_parameters">A set of query parameters to customize the request.</param>
        /// <param name="api_request_settings">A set up customizable settings to handle diferent status codes.</param>
        /// <returns>Returns data that adheres to the <see cref="IApiResponse{type}"/> interface.</returns>
        public static IApiResponse<Url>
        CreateEntitlementGrantsUploadUrl(string application_token, string client_id, EntitlementQueryParameters query_parameters, ApiRequestSettings api_request_settings = api_request_settings_default)
        {
            IApiResponse<Url> url = CreateEntitlementGrantsUploadUrlAsync(application_token, client_id, query_parameters, api_request_settings).Result;

            return url;
        }

        #endregion

        #region Games

        /// <summary>
        /// Gets information about a list of games.
        /// </summary>
        /// <param name="oauth_token">The OAuth token used to determine whose description to update and authorize the request.</param>
        /// <param name="query_parameters">A set of query parameters to customize the request.</param>
        /// <param name="api_request_settings">A set up customizable settings to handle diferent status codes.</param>
        /// <returns>Returns data that adheres to the <see cref="IApiResponse{type}"/> interface.</returns>
        public static IApiResponse<Game>
        GetGames(string oauth_token, GamesQueryParameters query_parameters, ApiRequestSettings api_request_settings = api_request_settings_default)
        {
            IApiResponse<Game> games = GetGamesAsync(oauth_token, query_parameters, api_request_settings).Result;

            return games;
        }

        /// <summary>
        /// Gets information about a list of games.
        /// </summary>
        /// <param name="oauth_token">The OAuth token used to determine whose description to update and authorize the request.</param>
        /// <param name="client_id">The Client ID to identify the application making the request.</param>
        /// <param name="query_parameters">A set of query parameters to customize the request.</param>
        /// <param name="api_request_settings">A set up customizable settings to handle diferent status codes.</param>
        /// <returns>Returns data that adheres to the <see cref="IApiResponse{type}"/> interface.</returns>
        public static IApiResponse<Game>
        GetGames(string oauth_token, string client_id, GamesQueryParameters query_parameters, ApiRequestSettings api_request_settings = api_request_settings_default)
        {
            IApiResponse<Game> games = GetGamesAsync(oauth_token, client_id, query_parameters, api_request_settings).Result;

            return games;
        }

        #endregion

        #region Streams

        /// <summary>
        /// Gets a single page of streams.
        /// </summary>
        /// <param name="oauth_token">The OAuth token to authorize the request.</param>
        /// <param name="api_request_settings">A set up customizable settings to handle diferent status codes.</param>
        /// <returns>Returns data that adheres to the <see cref="IApiResponsePage{type}"/> interface.</returns>
        public static IApiResponsePage<Stream>
        GetStreamsPage(string oauth_token, ApiRequestSettings api_request_settings = api_request_settings_default)
        {
            IApiResponsePage<Stream> streams = GetStreamsPageAsync(oauth_token, api_request_settings).Result;

            return streams;
        }

        /// <summary>
        /// Gets a single page of streams.
        /// </summary>
        /// <param name="oauth_token">The OAuth token used to determine whose description to update and authorize the request.</param>
        /// <param name="query_parameters">A set of query parameters to customize the request.</param>
        /// <param name="api_request_settings">A set up customizable settings to handle diferent status codes.</param>
        /// <returns>Returns data that adheres to the <see cref="IApiResponsePage{type}"/> interface.</returns>
        public static IApiResponsePage<Stream>
        GetStreamsPage(string oauth_token, StreamsQueryParameters query_parameters, ApiRequestSettings api_request_settings = api_request_settings_default)
        {
            IApiResponsePage<Stream> streams = GetStreamsPageAsync(oauth_token, query_parameters, api_request_settings).Result;

            return streams;
        }

        /// <summary>
        /// Gets a single page of streams.
        /// </summary>
        /// <param name="oauth_token">The OAuth token used to determine whose description to update and authorize the request.</param>
        /// <param name="client_id">The Client ID to identify the application making the request.</param>
        /// <param name="api_request_settings">A set up customizable settings to handle diferent status codes.</param>
        /// <returns>Returns data that adheres to the <see cref="IApiResponsePage{type}"/> interface.</returns>
        public static IApiResponsePage<Stream>
        GetStreamsPage(string oauth_token, string client_id, ApiRequestSettings api_request_settings = api_request_settings_default)
        {
            IApiResponsePage<Stream> streams = GetStreamsPageAsync(oauth_token, client_id, api_request_settings).Result;

            return streams;
        }

        /// <summary>
        /// Gets a single page of streams.
        /// </summary>
        /// <param name="oauth_token">The OAuth token used to determine whose description to update and authorize the request.</param>
        /// <param name="client_id">The Client ID to identify the application making the request.</param>
        /// <param name="query_parameters">A set of query parameters to customize the request.</param>
        /// <param name="api_request_settings">A set up customizable settings to handle diferent status codes.</param>
        /// <returns>Returns data that adheres to the <see cref="IApiResponsePage{type}"/> interface.</returns>
        public static IApiResponsePage<Stream>
        GetStreamsPage(string oauth_token, string client_id, StreamsQueryParameters query_parameters, ApiRequestSettings api_request_settings = api_request_settings_default)
        {
            IApiResponsePage<Stream> streams = GetStreamsPageAsync(oauth_token, client_id, query_parameters, api_request_settings).Result;

            return streams;
        }

        /// <summary>
        /// Gets a complete list of streams.
        /// </summary>
        /// <param name="oauth_token">The OAuth token to authorize the request.</param>
        /// <param name="api_request_settings">A set up customizable settings to handle diferent status codes.</param>
        /// <returns>Returns data that adheres to the <see cref="IApiResponse{type}"/> interface.</returns>
        public static IApiResponse<Stream>
        GetStreams(string oauth_token, ApiRequestSettings api_request_settings = api_request_settings_default)
        {
            IApiResponse<Stream> streams = GetStreamsAsync(oauth_token, api_request_settings).Result;

            return streams;
        }

        /// <summary>
        /// Gets a complete list of streams.
        /// </summary>
        /// <param name="oauth_token">The OAuth token to authorize the request.</param>
        /// <param name="query_parameters">A set of query parameters to customize the request.</param>
        /// <param name="api_request_settings">A set up customizable settings to handle diferent status codes.</param>
        /// <returns>Returns data that adheres to the <see cref="IApiResponse{type}"/> interface.</returns>
        public static IApiResponse<Stream>
        GetStreams(string oauth_token, StreamsQueryParameters query_parameters, ApiRequestSettings api_request_settings = api_request_settings_default)
        {
            IApiResponse<Stream> streams = GetStreamsAsync(oauth_token, query_parameters, api_request_settings).Result;

            return streams;
        }

        /// <summary>
        /// Gets a complete list of streams.
        /// </summary>
        /// <param name="oauth_token">The OAuth token used to determine whose description to update and authorize the request.</param>
        /// <param name="client_id">The Client ID to identify the application making the request.</param>
        /// <param name="api_request_settings">A set up customizable settings to handle diferent status codes.</param>
        /// <returns>Returns data that adheres to the <see cref="IApiResponse{type}"/> interface.</returns>
        public static IApiResponse<Stream>
        GetStreams(string oauth_token, string client_id, ApiRequestSettings api_request_settings = api_request_settings_default)
        {
            IApiResponse<Stream> streams = GetStreamsAsync(oauth_token, client_id, api_request_settings).Result;

            return streams;
        }

        /// <summary>
        /// Gets a complete list of streams.
        /// </summary>
        /// <param name="oauth_token">The OAuth token used to determine whose description to update and authorize the request.</param>
        /// <param name="client_id">The Client ID to identify the application making the request.</param>
        /// <param name="query_parameters">A set of query parameters to customize the request.</param>
        /// <param name="api_request_settings">A set up customizable settings to handle diferent status codes.</param>
        /// <returns>Returns data that adheres to the <see cref="IApiResponse{type}"/> interface.</returns>
        public static IApiResponse<Stream>
        GetStreams(string oauth_token, string client_id, StreamsQueryParameters query_parameters, ApiRequestSettings api_request_settings = api_request_settings_default)
        {
            IApiResponse<Stream> streams = GetStreamsAsync(oauth_token, client_id, query_parameters, api_request_settings).Result;

            return streams;
        }

        /// <summary>
        /// Gets a single page of metadata about streams playing either Overwatch or Hearthstone.
        /// </summary>
        /// <param name="oauth_token">The OAuth token to authorize the request.</param>
        /// <param name="api_request_settings">A set up customizable settings to handle diferent status codes.</param>
        /// <returns>Returns data that adheres to the <see cref="IApiResponsePage{type}"/> interface.</returns>
        public static IApiResponsePage<Metadata>
        GetStreamsMetadataPage(string oauth_token, ApiRequestSettings api_request_settings = api_request_settings_default)
        {
            IApiResponsePage<Metadata> metadata = GetStreamsMetadataPageAsync(oauth_token, api_request_settings).Result;

            return metadata;
        }

        /// <summary>
        /// Gets a single page of metadata about streams playing either Overwatch or Hearthstone.
        /// </summary>
        /// <param name="oauth_token">The OAuth token to authorize the request.</param>
        /// <param name="query_parameters">A set of query parameters to customize the request.</param>
        /// <param name="api_request_settings">A set up customizable settings to handle diferent status codes.</param>
        /// <returns>Returns data that adheres to the <see cref="IApiResponsePage{type}"/> interface.</returns>
        public static IApiResponsePage<Metadata>
        GetStreamsMetadataPage(string oauth_token, StreamsQueryParameters query_parameters, ApiRequestSettings api_request_settings = api_request_settings_default)
        {
            IApiResponsePage<Metadata> metadata = GetStreamsMetadataPageAsync(oauth_token, query_parameters, api_request_settings).Result;

            return metadata;
        }

        /// <summary>
        /// Gets a single page of metadata about streams playing either Overwatch or Hearthstone.
        /// </summary>
        /// <param name="oauth_token">The OAuth token to authorize the request.</param>
        /// <param name="client_id">The Client ID to identify the application making the request.</param>
        /// <param name="api_request_settings">A set up customizable settings to handle diferent status codes.</param>
        /// <returns>Returns data that adheres to the <see cref="IApiResponsePage{type}"/> interface.</returns>
        public static IApiResponsePage<Metadata>
        GetStreamsMetadataPage(string oauth_token, string client_id, ApiRequestSettings api_request_settings = api_request_settings_default)
        {
            IApiResponsePage<Metadata> metadata = GetStreamsMetadataPageAsync(oauth_token, client_id, api_request_settings).Result;

            return metadata;
        }

        /// <summary>
        /// Gets a single page of metadata about streams playing either Overwatch or Hearthstone.
        /// </summary>
        /// <param name="oauth_token">The OAuth token to authorize the request.</param>
        /// <param name="client_id">The Client ID to identify the application making the request.</param>
        /// <param name="query_parameters">A set of query parameters to customize the request.</param>
        /// <param name="api_request_settings">A set up customizable settings to handle diferent status codes.</param>
        /// <returns>Returns data that adheres to the <see cref="IApiResponsePage{type}"/> interface.</returns>
        public static IApiResponsePage<Metadata>
        GetStreamsMetadataPage(string oauth_token, string client_id, StreamsQueryParameters query_parameters, ApiRequestSettings api_request_settings = api_request_settings_default)
        {
            IApiResponsePage<Metadata> metadata = GetStreamsMetadataPageAsync(oauth_token, client_id, query_parameters, api_request_settings).Result;

            return metadata;
        }

        /// <summary>
        /// Gets a complete list of metadata about streams playing either Overwatch or Hearthstone.
        /// </summary>
        /// <param name="oauth_token">The OAuth token to authorize the request.</param>
        /// <param name="api_request_settings">A set up customizable settings to handle diferent status codes.</param>
        /// <returns>Returns data that adheres to the <see cref="IApiResponse{type}"/> interface.</returns>
        public static IApiResponse<Metadata>
        GetStreamsMetadata(string oauth_token, ApiRequestSettings api_request_settings = api_request_settings_default)
        {
            IApiResponse<Metadata> metadata = GetStreamsMetadataAsync(oauth_token, api_request_settings).Result;

            return metadata;
        }

        /// <summary>
        /// Gets a complete list of metadata about streams playing either Overwatch or Hearthstone.
        /// </summary>
        /// <param name="oauth_token">The OAuth token to authorize the request.</param>
        /// <param name="query_parameters">A set of query parameters to customize the request.</param>
        /// <param name="api_request_settings">A set up customizable settings to handle diferent status codes.</param>
        /// <returns>Returns data that adheres to the <see cref="IApiResponse{type}"/> interface.</returns>
        public static IApiResponse<Metadata>
        GetStreamsMetadata(string oauth_token, StreamsQueryParameters query_parameters, ApiRequestSettings api_request_settings = api_request_settings_default)
        {
            IApiResponse<Metadata> metadata = GetStreamsMetadataAsync(oauth_token, query_parameters, api_request_settings).Result;

            return metadata;
        }

        /// <summary>
        /// Gets a complete list of metadata about streams playing either Overwatch or Hearthstone.
        /// </summary>
        /// <param name="oauth_token">The OAuth token to authorize the request.</param>
        /// <param name="client_id">The Client ID to identify the application making the request.</param>
        /// <param name="api_request_settings">A set up customizable settings to handle diferent status codes.</param>
        /// <returns>Returns data that adheres to the <see cref="IApiResponse{type}"/> interface.</returns>
        public static IApiResponse<Metadata>
        GetStreamsMetadata(string oauth_token, string client_id, ApiRequestSettings api_request_settings = api_request_settings_default)
        {
            IApiResponse<Metadata> metadata = GetStreamsMetadataAsync(oauth_token, client_id, api_request_settings).Result;

            return metadata;
        }

        /// <summary>
        /// Gets a complete list of metadata about streams playing either Overwatch or Hearthstone.
        /// </summary>
        /// <param name="oauth_token">The OAuth token to authorize the request.</param>
        /// <param name="client_id">The Client ID to identify the application making the request.</param>
        /// <param name="query_parameters">A set of query parameters to customize the request.</param>
        /// <param name="api_request_settings">A set up customizable settings to handle diferent status codes.</param>
        /// <returns>Returns data that adheres to the <see cref="IApiResponse{type}"/> interface.</returns>
        public static IApiResponse<Metadata>
        GetStreamsMetadata(string oauth_token, string client_id, StreamsQueryParameters query_parameters, ApiRequestSettings api_request_settings = api_request_settings_default)
        {
            IApiResponse<Metadata> metadata = GetStreamsMetadataAsync(oauth_token, client_id, query_parameters, api_request_settings).Result;

            return metadata;
        }

        #endregion

        #region Users

        /// <summary>
        /// <para>Gets the information of the user looked up by the provided OAuth token.</para>
        /// <para>
        /// Optional Scope: 'user:read:email'.
        /// If provided, the user's email is included in the response.
        /// </para>
        /// </summary>
        /// <param name="oauth_token">The OAuth token to authorize the request.</param>
        /// <param name="api_request_settings">A set up customizable settings to handle diferent status codes.</param>
        /// <returns>Returns data that adheres to the <see cref="IApiResponse{type}"/> interface.</returns>
        public static IApiResponse<User>
        GetUser(string oauth_token, ApiRequestSettings api_request_settings = api_request_settings_default)
        {
            IApiResponse<User> users = GetUserAsync(oauth_token, api_request_settings).Result;

            return users;
        }

        /// <summary>
        /// <para>Gets the information of the user looked up by the provided OAuth token.</para>
        /// <para>
        /// Optional Scope: 'user:read:email'.
        /// If provided, the user's email is included in the response.
        /// </para>
        /// </summary>
        /// <param name="oauth_token">The OAuth token to authorize the request.</param>
        /// <param name="client_id">The Client ID to identify the application making the request.</param>
        /// <param name="api_request_settings">A set up customizable settings to handle diferent status codes.</param>
        /// <returns>Returns data that adheres to the <see cref="IApiResponse{type}"/> interface.</returns>
        public static IApiResponse<User>
        GetUser(string oauth_token, string client_id, ApiRequestSettings api_request_settings = api_request_settings_default)
        {
            IApiResponse<User> users = GetUserAsync(oauth_token, client_id, api_request_settings).Result;

            return users;
        }

        /// <summary>
        /// <para>
        /// Gets the information of one or more users by their id or login.
        /// If no <paramref name="ids"/> are specified, the user is looked up by if the OAuth token provided.
        /// </para>
        /// <para>
        /// Optional Scope: 'user:read:email'.
        /// If provided, the user's email is included in the response.
        /// </para>
        /// </summary>
        /// <param name="oauth_token">The OAuth token to authorize the request.</param>
        /// <param name="query_parameters">A set of query parameters to customize the request.</param>
        /// <param name="api_request_settings">A set up customizable settings to handle diferent status codes.</param>
        /// <returns>Returns data that adheres to the <see cref="IApiResponse{type}"/> interface.</returns>
        public static IApiResponse<User>
        GetUsers(string oauth_token, UsersQueryParameters query_parameters, ApiRequestSettings api_request_settings = api_request_settings_default)
        {
            IApiResponse<User> users = GetUsersAsync(oauth_token, query_parameters, api_request_settings).Result;

            return users;
        }

        /// <summary>
        /// <para>
        /// Gets the information of one or more users by their id or login.
        /// If no <paramref name="ids"/> are specified, the user is looked up by if the OAuth token provided.
        /// </para>
        /// <para>
        /// Optional Scope: 'user:read:email'.
        /// If provided, the user's email is included in the response.
        /// </para>
        /// </summary>
        /// <param name="oauth_token">The OAuth token to authorize the request.</param>
        /// <param name="client_id">The Client ID to identify the application making the request.</param>
        /// <param name="query_parameters">A set of query parameters to customize the request.</param>
        /// <param name="api_request_settings">A set up customizable settings to handle diferent status codes.</param>
        /// <returns>Returns data that adheres to the <see cref="IApiResponse{type}"/> interface.</returns>
        public static IApiResponse<User>
        GetUsers(string oauth_token, string client_id, UsersQueryParameters query_parameters, ApiRequestSettings api_request_settings = api_request_settings_default)
        {
            IApiResponse<User> users = GetUsersAsync(oauth_token, client_id, query_parameters, api_request_settings).Result;

            return users;
        }

        /// <summary>
        /// Gets the relationship between two users.
        /// </summary>
        /// <param name="oauth_token">The OAuth token to authorize the request.</param>
        /// <param name="from_id">The user to compare from.</param>
        /// <param name="to_id">The user to compare to.</param>
        /// <param name="api_request_settings">A set up customizable settings to handle diferent status codes.</param>
        /// <returns>Returns data that adheres to the <see cref="IApiResponsePage{type}"/> interface.</returns>
        public static IApiResponsePage<Follow>
        GetUserRelationship(string oauth_token, string from_id, string to_id, ApiRequestSettings api_request_settings = api_request_settings_default)
        {
            IApiResponsePage<Follow> relationship = GetUserRelationshipAsync(oauth_token, from_id, to_id, api_request_settings).Result;

            return relationship;
        }

        /// <summary>
        /// Gets the relationship between two users.
        /// </summary>
        /// <param name="oauth_token">The OAuth token to authorize the request.</param>
        /// <param name="client_id">The Client ID to identify the application making the request.</param>
        /// <param name="from_id">The user to compare from.</param>
        /// <param name="to_id">The user to compare to.</param>
        /// <param name="api_request_settings">A set up customizable settings to handle diferent status codes.</param>
        /// <returns>Returns data that adheres to the <see cref="IApiResponsePage{type}"/> interface.</returns>
        public static IApiResponsePage<Follow>
        GetUserRelationship(string oauth_token, string client_id, string from_id, string to_id, ApiRequestSettings api_request_settings = api_request_settings_default)
        {
            IApiResponsePage<Follow> relationship = GetUserRelationshipAsync(oauth_token, client_id, from_id, to_id, api_request_settings).Result;

            return relationship;
        }

        /// <summary>
        /// Checks to see if <paramref name="from_id"/> is following <paramref name="to_id"/>.
        /// </summary>
        /// <param name="oauth_token">The OAuth token to authorize the request.</param>
        /// <param name="from_id">The user to compare from.</param>
        /// <param name="to_id">The user to compare to.</param>
        /// <param name="api_request_settings">A set up customizable settings to handle diferent status codes.</param>
        /// <returns>Returns data that adheres to the <see cref="IApiResponseValue{type}"/> interface.</returns>
        public static IApiResponseValue<bool>
        IsUserFollowing(string oauth_token, string from_id, string to_id, ApiRequestSettings api_request_settings = api_request_settings_default)
        {
            IApiResponseValue<bool> is_following = IsUserFollowingAsync(oauth_token, from_id, to_id, api_request_settings).Result;

            return is_following;
        }

        /// <summary>
        /// Checks to see if <paramref name="from_id"/> is following <paramref name="to_id"/>.
        /// </summary>
        /// <param name="oauth_token">The OAuth token to authorize the request.</param>
        /// <param name="client_id">The Client ID to identify the application making the request.</param>
        /// <param name="from_id">The user to compare from.</param>
        /// <param name="to_id">The user to compare to.</param>
        /// <param name="api_request_settings">A set up customizable settings to handle diferent status codes.</param>
        /// <returns>Returns data that adheres to the <see cref="IApiResponse{type}"/> interface.</returns>
        public static IApiResponseValue<bool>
        IsUserFollowing(string oauth_token, string client_id, string from_id, string to_id, ApiRequestSettings api_request_settings = api_request_settings_default)
        {
            IApiResponseValue<bool> is_following = IsUserFollowingAsync(oauth_token, client_id, from_id, to_id, api_request_settings).Result;

            return is_following;
        }

        /// <summary>
        /// Gets a single page of a user's following list.
        /// </summary>
        /// <param name="oauth_token">The OAuth token to authorize the request.</param>
        /// <param name="from_id">The user to get the following list from.</param>
        /// <param name="api_request_settings">A set up customizable settings to handle diferent status codes.</param>
        /// <returns>Returns data that adheres to the <see cref="IApiResponse{type}"/> interface.</returns>
        public static IApiResponsePage<Follow>
        GetUserFollowingPage(string oauth_token, string from_id, ApiRequestSettings api_request_settings = api_request_settings_default)
        {
            IApiResponsePage<Follow> following = GetUserFollowingPageAsync(oauth_token, from_id, api_request_settings).Result;

            return following;
        }

        /// <summary>
        /// Gets a single page of a user's following list.
        /// </summary>
        /// <param name="oauth_token">The OAuth token to authorize the request.</param>
        /// <param name="from_id">The user to get the following list from.</param>
        /// <param name="query_parameters">
        /// A set of query parameters to customize the request.
        /// The <code>from_id</code> and <code>to_id</code> properties in the <paramref name="query_parameters"/> are ignored if specified.
        /// </param>
        /// <param name="api_request_settings">A set up customizable settings to handle diferent status codes.</param>
        /// <returns>Returns data that adheres to the <see
        public static IApiResponsePage<Follow>
        GetUserFollowingPage(string oauth_token, string from_id, FollowsQueryParameters query_parameters, ApiRequestSettings api_request_settings = api_request_settings_default)
        {
            IApiResponsePage<Follow> following = GetUserFollowingPageAsync(oauth_token, from_id, query_parameters, api_request_settings).Result;

            return following;
        }

        /// <summary>
        /// Gets a single page of a user's following list.
        /// </summary>
        /// <param name="oauth_token">The OAuth token to authorize the request.</param>
        /// <param name="client_id">The Client ID to identify the application making the request.</param>
        /// <param name="from_id">The user to get the following list from.</param>
        /// <param name="api_request_settings">A set up customizable settings to handle diferent status codes.</param>
        /// <returns>Returns data that adheres to the <see cref="IApiResponse{type}"/> interface.</returns>
        public static IApiResponsePage<Follow>
        GetUserFollowingPage(string oauth_token, string client_id, string from_id, ApiRequestSettings api_request_settings = api_request_settings_default)
        {
            IApiResponsePage<Follow> following = GetUserFollowingPageAsync(oauth_token, client_id, from_id, api_request_settings).Result;

            return following;
        }

        /// <summary>
        /// Gets a single page of a user's following list.
        /// </summary>
        /// <param name="oauth_token">The OAuth token to authorize the request.</param>
        /// <param name="client_id">The Client ID to identify the application making the request.</param>
        /// <param name="from_id">The user to get the following list from.</param>
        /// <param name="query_parameters">
        /// A set of query parameters to customize the request.
        /// The <code>from_id</code> and <code>to_id</code> properties in the <paramref name="query_parameters"/> are ignored if specified.
        /// </param>
        /// <param name="api_request_settings">A set up customizable settings to handle diferent status codes.</param>
        /// <returns>Returns data that adheres to the <see cref="IApiResponse{type}"/> interface.</returns>
        public static IApiResponsePage<Follow>
        GetUserFollowingPage(string oauth_token, string client_id, string from_id, FollowsQueryParameters query_parameters, ApiRequestSettings api_request_settings = api_request_settings_default)
        {
            IApiResponsePage<Follow> following = GetUserFollowingPageAsync(oauth_token, client_id, from_id, query_parameters, api_request_settings).Result;

            return following;
        }

        /// <summary>
        /// Gets a user's following list.
        /// </summary>
        /// <param name="oauth_token">The OAuth token to authorize the request.</param>
        /// <param name="from_id">The user to get the following list from.</param>
        /// <param name="api_request_settings">A set up customizable settings to handle diferent status codes.</param>
        /// <returns>Returns data that adheres to the <see cref="IApiResponse{type}"/> interface.</returns>
        public static IApiResponse<Follow>
        GetUserFollowing(string oauth_token, string from_id, ApiRequestSettings api_request_settings = api_request_settings_default)
        {
            IApiResponse<Follow> following = GetUserFollowingAsync(oauth_token, from_id, api_request_settings).Result;

            return following;
        }

        /// <summary>
        /// Gets a user's following list.
        /// </summary>
        /// <param name="oauth_token">The OAuth token to authorize the request.</param>
        /// <param name="client_id">The Client ID to identify the application making the request.</param>
        /// <param name="from_id">The user to get the following list from.</param>
        /// <param name="api_request_settings">A set up customizable settings to handle diferent status codes.</param>
        /// <returns>Returns data that adheres to the <see cref="IApiResponse{type}"/> interface.</returns>
        public static IApiResponse<Follow>
        GetUserFollowing(string oauth_token, string client_id, string from_id, ApiRequestSettings api_request_settings = api_request_settings_default)
        {
            IApiResponse<Follow> following = GetUserFollowingAsync(oauth_token, client_id, from_id, api_request_settings).Result;

            return following;
        }

        /// <summary>
        /// Gets a single paged of a user's followers list.
        /// </summary>
        /// <param name="oauth_token">The OAuth token to authorize the request.</param>
        /// <param name="to_id">The user to get the followers for.</param>
        /// <param name="api_request_settings">A set up customizable settings to handle diferent status codes.</param>
        /// <returns>Returns data that adheres to the <see cref="IApiResponse{type}"/> interface.</returns>
        public static IApiResponsePage<Follow>
        GetUserFollowersPage(string oauth_token, string to_id, ApiRequestSettings api_request_settings = api_request_settings_default)
        {
            IApiResponsePage<Follow> followers = GetUserFollowersPageAsync(oauth_token, to_id, api_request_settings).Result;

            return followers;
        }

        /// <summary>
        /// Gets a single paged of a user's followers list.
        /// </summary>
        /// <param name="oauth_token">The OAuth token to authorize the request.</param>
        /// <param name="to_id">The user to get the followers for.</param>
        /// <param name="query_parameters">
        /// A set of query parameters to customize the request.
        /// The <code>from_id</code> and <code>to_id</code> properties in the <paramref name="query_parameters"/> are ignored if specified.
        /// </param>
        /// <param name="api_request_settings">A set up customizable settings to handle diferent status codes.</param>
        /// <returns>Returns data that adheres to the <see cref="IApiResponse{type}"/> interface.</returns>
        public static IApiResponsePage<Follow>
        GetUserFollowersPage(string oauth_token, string to_id, FollowsQueryParameters query_parameters, ApiRequestSettings api_request_settings = api_request_settings_default)
        {
            IApiResponsePage<Follow> followers = GetUserFollowersPageAsync(oauth_token, to_id, query_parameters).Result;

            return followers;
        }

        /// <summary>
        /// Gets a single paged of a user's followers list.
        /// </summary>
        /// <param name="oauth_token">The OAuth token to authorize the request.</param>
        /// <param name="client_id">The Client ID to identify the application making the request.</param>
        /// <param name="to_id">The user to get the followers for.</param>
        /// <param name="api_request_settings">A set up customizable settings to handle diferent status codes.</param>
        /// <returns>Returns data that adheres to the <see cref="IApiResponse{type}"/> interface.</returns>
        public static IApiResponsePage<Follow>
        GetUserFollowersPage(string oauth_token, string client_id, string to_id, ApiRequestSettings api_request_settings = api_request_settings_default)
        {
            IApiResponsePage<Follow> followers = GetUserFollowersPageAsync(oauth_token, client_id, to_id, api_request_settings).Result;
            return followers;
        }

        /// <summary>
        /// Gets a single paged of a user's followers list.
        /// </summary>
        /// <param name="oauth_token">The OAuth token to authorize the request.</param>
        /// <param name="client_id">The Client ID to identify the application making the request.</param>
        /// <param name="to_id">The user to get the followers for.</param>
        /// <param name="query_parameters">
        /// A set of query parameters to customize the request.
        /// The <code>from_id</code> and <code>to_id</code> properties in the <paramref name="query_parameters"/> are ignored if specified.
        /// </param>
        /// <param name="api_request_settings">A set up customizable settings to handle diferent status codes.</param>
        /// <returns>Returns data that adheres to the <see cref="IApiResponse{type}"/> interface.</returns>
        public static IApiResponsePage<Follow>
        GetUserFollowersPage(string oauth_token, string client_id, string to_id, FollowsQueryParameters query_parameters, ApiRequestSettings api_request_settings = api_request_settings_default)
        {
            IApiResponsePage<Follow> followers = GetUserFollowersPageAsync(oauth_token, client_id, to_id, query_parameters, api_request_settings).Result;
            return followers;
        }

        /// <summary>
        /// Gets a user's followers list.
        /// </summary>
        /// <param name="oauth_token">The OAuth token to authorize the request.</param>
        /// <param name="to_id">The user to get the followers for.</param>
        /// <param name="api_request_settings">A set up customizable settings to handle diferent status codes.</param>
        /// <returns>Returns data that adheres to the <see cref="IApiResponse{type}"/> interface.</returns>
        public static IApiResponse<Follow>
        GetUserFollowers(string oauth_token, string to_id, ApiRequestSettings api_request_settings = api_request_settings_default)
        {
            IApiResponse<Follow> followers = GetUserFollowersAsync(oauth_token, to_id, api_request_settings).Result;

            return followers;
        }

        /// <summary>
        /// Gets a user's followers list.
        /// </summary>
        /// <param name="oauth_token">The OAuth token to authorize the request.</param>
        /// <param name="client_id">The Client ID to identify the application making the request.</param>
        /// <param name="to_id">The user to get the followers for.</param>
        /// <param name="api_request_settings">A set up customizable settings to handle diferent status codes.</param>
        /// <returns>Returns data that adheres to the <see cref="IApiResponse{type}"/> interface.</returns>
        public static IApiResponse<Follow>
        GetUserFollowers(string oauth_token, string client_id, string to_id, ApiRequestSettings api_request_settings = api_request_settings_default)
        {
            IApiResponse<Follow> followers = GetUserFollowersAsync(oauth_token, client_id, to_id, api_request_settings).Result;

            return followers;
        }

        /// <summary>
        /// <para>Sets the description of a user specified by the OAuth token provided.</para>
        /// <para>Required Scope: 'user:edit'</para>
        /// </summary>
        /// <param name="oauth_token">The OAuth token used to determine whose description to update and authorize the request.</param>
        /// <param name="description">The new description to set.</param>
        /// <param name="api_request_settings">A set up customizable settings to handle diferent status codes.</param>
        /// <returns>Returns data that adheres to the <see cref="IApiResponse{type}"/> interface.</returns>
        public static IApiResponseValue<bool>
        SetUserDescription(string oauth_token, string description, ApiRequestSettings api_request_settings = api_request_settings_default)
        {
            IApiResponseValue<bool> success = SetUserDescriptionAsync(oauth_token, description, api_request_settings).Result;

            return success;
        }

        /// <summary>
        /// <para>Sets the description of a user specified by the OAuth token provided.</para>
        /// <para>Required Scope: 'user:edit'</para>
        /// </summary>
        /// <param name="oauth_token">The OAuth token used to determine whose description to update and authorize the request.</param>
        /// <param name="client_id">The Client ID to identify the application making the request.</param>
        /// <param name="description">The new description to set.</param>
        /// <param name="api_request_settings">A set up customizable settings to handle diferent status codes.</param>
        /// <returns>Returns data that adheres to the <see cref="IApiResponse{type}"/> interface.</returns>
        public static IApiResponseValue<bool>
        SetUserDescription(string oauth_token, string client_id, string description, ApiRequestSettings api_request_settings = api_request_settings_default)
        {
            IApiResponseValue<bool> success = SetUserDescriptionAsync(oauth_token, client_id, description, api_request_settings).Result;

            return success;
        }

        #endregion

        #region Videos

        /// <summary>
        /// Gets a single page of information on one or more videos.
        /// </summary>
        /// <param name="oauth_token">The OAuth token used to determine whose description to update and authorize the request.</param>
        /// <param name="query_parameters">A set of query parameters to customize the request.</param>
        /// <param name="api_request_settings">A set up customizable settings to handle diferent status codes.</param>
        /// <returns>Returns data that adheres to the <see cref="IApiResponsePage{type}"/> interface.</returns>
        public static IApiResponsePage<Video>
        GetVideosPage(string oauth_token, VideosQueryParameters query_parameters, ApiRequestSettings api_request_settings = api_request_settings_default)
        {
            IApiResponsePage<Video> videos = GetVideosPageAsync(oauth_token, query_parameters, api_request_settings).Result;

            return videos;
        }

        /// <summary>
        /// Gets a single page of information on one or more videos.
        /// </summary>
        /// <param name="oauth_token">The OAuth token used to determine whose description to update and authorize the request.</param>
        /// <param name="client_id">The Client ID to identify the application making the request.</param>
        /// <param name="query_parameters">A set of query parameters to customize the request.</param>
        /// <param name="api_request_settings">A set up customizable settings to handle diferent status codes.</param>
        /// <returns>Returns data that adheres to the <see cref="IApiResponsePage{type}"/> interface.</returns>
        public static IApiResponsePage<Video>
        GetVideosPage(string oauth_token, string client_id, VideosQueryParameters query_parameters, ApiRequestSettings api_request_settings = api_request_settings_default)
        {
            IApiResponsePage<Video> videos = GetVideosPageAsync(oauth_token, client_id, query_parameters, api_request_settings).Result;

            return videos;
        }

        /// <summary>
        /// Gets a complete list of information on one or more videos.
        /// </summary>
        /// <param name="oauth_token">The OAuth token used to determine whose description to update and authorize the request.</param>
        /// <param name="query_parameters">A set of query parameters to customize the request.</param>
        /// <param name="api_request_settings">A set up customizable settings to handle diferent status codes.</param>
        /// <returns>Returns data that adheres to the <see cref="IApiResponse{type}"/> interface.</returns>
        public static IApiResponse<Video>
        GetVideos(string oauth_token, VideosQueryParameters query_parameters, ApiRequestSettings api_request_settings = api_request_settings_default)
        {
            IApiResponse<Video> videos = GetVideosAsync(oauth_token, query_parameters, api_request_settings).Result;

            return videos;
        }

        /// <summary>
        /// Gets a complete list of information on one or more videos.
        /// </summary>
        /// <param name="oauth_token">The OAuth token used to determine whose description to update and authorize the request.</param>
        /// <param name="client_id">The Client ID to identify the application making the request.</param>
        /// <param name="query_parameters">A set of query parameters to customize the request.</param>
        /// <param name="api_request_settings">A set up customizable settings to handle diferent status codes.</param>
        /// <returns>Returns data that adheres to the <see cref="IApiResponse{type}"/> interface.</returns>
        public static IApiResponse<Video>
        GetVideos(string oauth_token, string client_id, VideosQueryParameters query_parameters, ApiRequestSettings api_request_settings = api_request_settings_default)
        {
            IApiResponse<Video> videos = GetVideosAsync(oauth_token, client_id, query_parameters, api_request_settings).Result;

            return videos;
        }

        #endregion
    }
}
