// standard namespaces
using System.Threading.Tasks;

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
        #region Fields

        private static readonly string              client_id_default               = string.Empty;

        private const           ApiRequestSettings  api_request_settings_default    = default(ApiRequestSettings);

        #endregion

        #region Entitlements

        /// <summary>
        /// <para>Asynchronously creates a URL where you can upload a manifest file and notify users that they have an entitlement.</para>
        /// <para>Required Authorization: Application Access Token</para>
        /// </summary>
        /// <param name="application_token">The application access token used to determine whose description to update and authorize the request.</param>
        /// <param name="query_parameters">A set of query parameters to customize the request.</param>
        /// <param name="api_request_settings">A set up customizable settings to handle diferent status codes.</param>
        /// <returns>Returns data that adheres to the <see cref="IApiResponse{type}"/> interface.</returns>
        public static async Task<IApiResponse<Url>>
        CreateEntitlementGrantsUploadUrlAsync(string application_token, EntitlementQueryParameters query_parameters, ApiRequestSettings api_request_settings = api_request_settings_default)
        {
            IApiResponse<Url> url = await CreateEntitlementGrantsUploadUrlAsync(application_token, client_id_default, query_parameters, api_request_settings);

            return url;
        }

        /// <summary>
        /// <para>Asynchronously creates a URL where you can upload a manifest file and notify users that they have an entitlement.</para>
        /// <para>Required Authorization: Application Access Token</para>
        /// </summary>
        /// <param name="application_token">The application access token used to determine whose description to update and authorize the request.</param>
        /// <param name="client_id">The Client ID to identify the application making the request.</param>
        /// <param name="query_parameters">A set of query parameters to customize the request.</param>
        /// <param name="api_request_settings">A set up customizable settings to handle diferent status codes.</param>
        /// <returns>Returns data that adheres to the <see cref="IApiResponse{type}"/> interface.</returns>
        public static async Task<IApiResponse<Url>>
        CreateEntitlementGrantsUploadUrlAsync(string application_token, string client_id, EntitlementQueryParameters query_parameters, ApiRequestSettings api_request_settings = api_request_settings_default)
        {
            IApiResponse<Url> url = await TwitchApiInternal.CreateEntitlementGrantsUploadUrlAsync(application_token, client_id, query_parameters, api_request_settings);

            return url;
        }

        #endregion

        #region Games

        /// <summary>
        /// Asynchronously gets information about a list of games.
        /// </summary>
        /// <param name="oauth_token">The OAuth token used to determine whose description to update and authorize the request.</param>
        /// <param name="query_parameters">A set of query parameters to customize the request.</param>
        /// <param name="api_request_settings">A set up customizable settings to handle diferent status codes.</param>
        /// <returns>Returns data that adheres to the <see cref="IApiResponse{type}"/> interface.</returns>
        public static async Task<IApiResponse<Game>>
        GetGamesAsync(string oauth_token, GamesQueryParameters query_parameters, ApiRequestSettings api_request_settings = api_request_settings_default)
        {
            IApiResponse<Game> games = await TwitchApiInternal.GetGamesAsync(oauth_token, client_id_default, query_parameters, api_request_settings);

            return games;
        }

        /// <summary>
        /// Asynchronously gets information about a list of games.
        /// </summary>
        /// <param name="oauth_token">The OAuth token used to determine whose description to update and authorize the request.</param>
        /// <param name="client_id">The Client ID to identify the application making the request.</param>
        /// <param name="query_parameters">A set of query parameters to customize the request.</param>
        /// <param name="api_request_settings">A set up customizable settings to handle diferent status codes.</param>
        /// <returns>Returns data that adheres to the <see cref="IApiResponse{type}"/> interface.</returns>
        public static async Task<IApiResponse<Game>>
        GetGamesAsync(string oauth_token, string client_id, GamesQueryParameters query_parameters, ApiRequestSettings api_request_settings = api_request_settings_default)
        {
            IApiResponse<Game> games = await TwitchApiInternal.GetGamesAsync(oauth_token, client_id, query_parameters, api_request_settings);

            return games;
        }

        #endregion

        #region Streams

        /// <summary>
        /// Asynchronously gets a single page of streams.
        /// </summary>
        /// <param name="oauth_token">The OAuth token to authorize the request.</param>
        /// <param name="api_request_settings">A set up customizable settings to handle diferent status codes.</param>
        /// <returns>Returns data that adheres to the <see cref="IApiResponsePage{type}"/> interface.</returns>
        public static async Task<IApiResponsePage<Stream>>
        GetStreamsPageAsync(string oauth_token, ApiRequestSettings api_request_settings = api_request_settings_default)
        {
            IApiResponsePage<Stream> streams = await GetStreamsPageAsync(oauth_token, default(StreamsQueryParameters), api_request_settings);

            return streams;
        }

        /// <summary>
        /// Asynchronously gets a single page of streams.
        /// </summary>
        /// <param name="oauth_token">The OAuth token used to determine whose description to update and authorize the request.</param>
        /// <param name="query_parameters">A set of query parameters to customize the request.</param>
        /// <param name="api_request_settings">A set up customizable settings to handle diferent status codes.</param>
        /// <returns>Returns data that adheres to the <see cref="IApiResponsePage{type}"/> interface.</returns>
        public static async Task<IApiResponsePage<Stream>>
        GetStreamsPageAsync(string oauth_token, StreamsQueryParameters query_parameters, ApiRequestSettings api_request_settings = api_request_settings_default)
        {
            IApiResponsePage<Stream> streams = await GetStreamsPageAsync(oauth_token, client_id_default, query_parameters, api_request_settings);

            return streams;
        }

        /// <summary>
        /// Asynchronously gets a single page of streams.
        /// </summary>
        /// <param name="oauth_token">The OAuth token used to determine whose description to update and authorize the request.</param>
        /// <param name="client_id">The Client ID to identify the application making the request.</param>
        /// <param name="api_request_settings">A set up customizable settings to handle diferent status codes.</param>
        /// <returns>Returns data that adheres to the <see cref="IApiResponsePage{type}"/> interface.</returns>
        public static async Task<IApiResponsePage<Stream>>
        GetStreamsPageAsync(string oauth_token, string client_id, ApiRequestSettings api_request_settings = api_request_settings_default)
        {
            IApiResponsePage<Stream> streams = await GetStreamsPageAsync(oauth_token, client_id, default(StreamsQueryParameters), api_request_settings);

            return streams;
        }

        /// <summary>
        /// Asynchronously gets a single page of streams.
        /// </summary>
        /// <param name="oauth_token">The OAuth token used to determine whose description to update and authorize the request.</param>
        /// <param name="client_id">The Client ID to identify the application making the request.</param>
        /// <param name="query_parameters">A set of query parameters to customize the request.</param>
        /// <param name="api_request_settings">A set up customizable settings to handle diferent status codes.</param>
        /// <returns>Returns data that adheres to the <see cref="IApiResponsePage{type}"/> interface.</returns>
        public static async Task<IApiResponsePage<Stream>>
        GetStreamsPageAsync(string oauth_token,string client_id, StreamsQueryParameters query_parameters, ApiRequestSettings api_request_settings = api_request_settings_default)
        {
            IApiResponsePage<Stream> streams = await TwitchApiInternal.GetStreamsPageAsync(oauth_token, client_id, query_parameters, api_request_settings);

            return streams;
        }

        /// <summary>
        /// Asynchronously gets a complete list of streams.
        /// </summary>
        /// <param name="oauth_token">The OAuth token to authorize the request.</param>
        /// <param name="api_request_settings">A set up customizable settings to handle diferent status codes.</param>
        /// <returns>Returns data that adheres to the <see cref="IApiResponse{type}"/> interface.</returns>
        public static async Task<IApiResponse<Stream>>
        GetStreamsAsync(string oauth_token, ApiRequestSettings api_request_settings = api_request_settings_default)
        {
            IApiResponse<Stream> streams = await GetStreamsAsync(oauth_token, default(StreamsQueryParameters), api_request_settings);

            return streams;
        }

        /// <summary>
        /// Asynchronously gets a complete list of streams.
        /// </summary>
        /// <param name="oauth_token">The OAuth token to authorize the request.</param>
        /// <param name="query_parameters">A set of query parameters to customize the request.</param>
        /// <param name="api_request_settings">A set up customizable settings to handle diferent status codes.</param>
        /// <returns>Returns data that adheres to the <see cref="IApiResponse{type}"/> interface.</returns>
        public static async Task<IApiResponse<Stream>>
        GetStreamsAsync(string oauth_token, StreamsQueryParameters query_parameters, ApiRequestSettings api_request_settings = api_request_settings_default)
        {
            IApiResponse<Stream> streams = await GetStreamsAsync(oauth_token, client_id_default, query_parameters, api_request_settings);

            return streams;
        }

        /// <summary>
        /// Asynchronously gets a complete list of streams.
        /// </summary>
        /// <param name="oauth_token">The OAuth token used to determine whose description to update and authorize the request.</param>
        /// <param name="client_id">The Client ID to identify the application making the request.</param>
        /// <param name="api_request_settings">A set up customizable settings to handle diferent status codes.</param>
        /// <returns>Returns data that adheres to the <see cref="IApiResponse{type}"/> interface.</returns>
        public static async Task<IApiResponse<Stream>>
        GetStreamsAsync(string oauth_token, string client_id, ApiRequestSettings api_request_settings = api_request_settings_default)
        {
            IApiResponse<Stream> streams = await GetStreamsAsync(oauth_token, client_id, default(StreamsQueryParameters), api_request_settings);

            return streams;
        }

        /// <summary>
        /// Asynchronously gets a complete list of streams.
        /// </summary>
        /// <param name="oauth_token">The OAuth token used to determine whose description to update and authorize the request.</param>
        /// <param name="client_id">The Client ID to identify the application making the request.</param>
        /// <param name="query_parameters">A set of query parameters to customize the request.</param>
        /// <param name="api_request_settings">A set up customizable settings to handle diferent status codes.</param>
        /// <returns>Returns data that adheres to the <see cref="IApiResponse{type}"/> interface.</returns>
        public static async Task<IApiResponse<Stream>>
        GetStreamsAsync(string oauth_token, string client_id, StreamsQueryParameters query_parameters, ApiRequestSettings api_request_settings = api_request_settings_default)
        {
            IApiResponse<Stream> streams = await TwitchApiInternal.GetStreamsAsync(oauth_token, client_id, query_parameters, api_request_settings);

            return streams;
        }

        /// <summary>
        /// Asynchronously gets a single page of metadata about streams playing either Overwatch or Hearthstone.
        /// </summary>
        /// <param name="oauth_token">The OAuth token to authorize the request.</param>
        /// <param name="api_request_settings">A set up customizable settings to handle diferent status codes.</param>
        /// <returns>Returns data that adheres to the <see cref="IApiResponsePage{type}"/> interface.</returns>
        public static async Task<IApiResponsePage<Metadata>>
        GetStreamsMetadataPageAsync(string oauth_token, ApiRequestSettings api_request_settings = api_request_settings_default)
        {
            IApiResponsePage<Metadata> metadata = await GetStreamsMetadataPageAsync(oauth_token, default(StreamsQueryParameters), api_request_settings);

            return metadata;
        }

        /// <summary>
        /// Asynchronously gets a single page of metadata about streams playing either Overwatch or Hearthstone.
        /// </summary>
        /// <param name="oauth_token">The OAuth token to authorize the request.</param>
        /// <param name="query_parameters">A set of query parameters to customize the request.</param>
        /// <param name="api_request_settings">A set up customizable settings to handle diferent status codes.</param>
        /// <returns>Returns data that adheres to the <see cref="IApiResponsePage{type}"/> interface.</returns>
        public static async Task<IApiResponsePage<Metadata>>
        GetStreamsMetadataPageAsync(string oauth_token, StreamsQueryParameters query_parameters, ApiRequestSettings api_request_settings = api_request_settings_default)
        {
            IApiResponsePage<Metadata> metadata = await GetStreamsMetadataPageAsync(oauth_token, client_id_default, query_parameters, api_request_settings);

            return metadata;
        }

        /// <summary>
        /// Asynchronously gets a single page of metadata about streams playing either Overwatch or Hearthstone.
        /// </summary>
        /// <param name="oauth_token">The OAuth token to authorize the request.</param>
        /// <param name="client_id">The Client ID to identify the application making the request.</param>
        /// <param name="api_request_settings">A set up customizable settings to handle diferent status codes.</param>
        /// <returns>Returns data that adheres to the <see cref="IApiResponsePage{type}"/> interface.</returns>
        public static async Task<IApiResponsePage<Metadata>>
        GetStreamsMetadataPageAsync(string oauth_token, string client_id, ApiRequestSettings api_request_settings = api_request_settings_default)
        {
            IApiResponsePage<Metadata> metadata = await GetStreamsMetadataPageAsync(oauth_token, client_id, default(StreamsQueryParameters), api_request_settings);

            return metadata;
        }

        /// <summary>
        /// Asynchronously gets a single page of metadata about streams playing either Overwatch or Hearthstone.
        /// </summary>
        /// <param name="oauth_token">The OAuth token to authorize the request.</param>
        /// <param name="client_id">The Client ID to identify the application making the request.</param>
        /// <param name="query_parameters">A set of query parameters to customize the request.</param>
        /// <param name="api_request_settings">A set up customizable settings to handle diferent status codes.</param>
        /// <returns>Returns data that adheres to the <see cref="IApiResponsePage{type}"/> interface.</returns>
        public static async Task<IApiResponsePage<Metadata>>
        GetStreamsMetadataPageAsync(string oauth_token, string client_id, StreamsQueryParameters query_parameters, ApiRequestSettings api_request_settings = api_request_settings_default)
        {
            IApiResponsePage<Metadata> metadata = await TwitchApiInternal.GetStreamsMetadataPageAsync(oauth_token, client_id, query_parameters, api_request_settings);

            return metadata;
        }

        /// <summary>
        /// Asynchronously gets a complete list of metadata about streams playing either Overwatch or Hearthstone.
        /// </summary>
        /// <param name="oauth_token">The OAuth token to authorize the request.</param>
        /// <param name="api_request_settings">A set up customizable settings to handle diferent status codes.</param>
        /// <returns>Returns data that adheres to the <see cref="IApiResponse{type}"/> interface.</returns>
        public static async Task<IApiResponse<Metadata>>
        GetStreamsMetadataAsync(string oauth_token, ApiRequestSettings api_request_settings = api_request_settings_default)
        {
            IApiResponse<Metadata> metadata = await GetStreamsMetadataAsync(oauth_token, default(StreamsQueryParameters), api_request_settings);

            return metadata;
        }

        /// <summary>
        /// Asynchronously gets a complete list of metadata about streams playing either Overwatch or Hearthstone.
        /// </summary>
        /// <param name="oauth_token">The OAuth token to authorize the request.</param>
        /// <param name="query_parameters">A set of query parameters to customize the request.</param>
        /// <param name="api_request_settings">A set up customizable settings to handle diferent status codes.</param>
        /// <returns>Returns data that adheres to the <see cref="IApiResponse{type}"/> interface.</returns>
        public static async Task<IApiResponse<Metadata>>
        GetStreamsMetadataAsync(string oauth_token, StreamsQueryParameters query_parameters, ApiRequestSettings api_request_settings = api_request_settings_default)
        {
            IApiResponse<Metadata> metadata = await GetStreamsMetadataAsync(oauth_token, client_id_default, query_parameters, api_request_settings);

            return metadata;
        }

        /// <summary>
        /// Asynchronously gets a complete list of metadata about streams playing either Overwatch or Hearthstone.
        /// </summary>
        /// <param name="oauth_token">The OAuth token to authorize the request.</param>
        /// <param name="client_id">The Client ID to identify the application making the request.</param>
        /// <param name="api_request_settings">A set up customizable settings to handle diferent status codes.</param>
        /// <returns>Returns data that adheres to the <see cref="IApiResponse{type}"/> interface.</returns>
        public static async Task<IApiResponse<Metadata>>
        GetStreamsMetadataAsync(string oauth_token, string client_id, ApiRequestSettings api_request_settings = api_request_settings_default)
        {
            IApiResponse<Metadata> metadata = await GetStreamsMetadataAsync(oauth_token, client_id, default(StreamsQueryParameters), api_request_settings);

            return metadata;
        }

        /// <summary>
        /// Asynchronously gets a complete list of metadata about streams playing either Overwatch or Hearthstone.
        /// </summary>
        /// <param name="oauth_token">The OAuth token to authorize the request.</param>
        /// <param name="client_id">The Client ID to identify the application making the request.</param>
        /// <param name="query_parameters">A set of query parameters to customize the request.</param>
        /// <param name="api_request_settings">A set up customizable settings to handle diferent status codes.</param>
        /// <returns>Returns data that adheres to the <see cref="IApiResponse{type}"/> interface.</returns>
        public static async Task<IApiResponse<Metadata>>
        GetStreamsMetadataAsync(string oauth_token, string client_id, StreamsQueryParameters query_parameters, ApiRequestSettings api_request_settings = api_request_settings_default)
        {
            IApiResponse<Metadata> metadata = await TwitchApiInternal.GetStreamsMetadataAsync(oauth_token, client_id, query_parameters, api_request_settings);

            return metadata;
        }

        #endregion

        #region Users

        /// <summary>
        /// <para>Asynchronously gets the information of the user looked up by the provided OAuth token.</para>
        /// <para>
        /// Optional Scope: 'user:read:email'.
        /// If provided, the user's email is included in the response.
        /// </para>
        /// </summary>
        /// <param name="oauth_token">The OAuth token to authorize the request.</param>
        /// <param name="api_request_settings">A set up customizable settings to handle diferent status codes.</param>
        /// <returns>Returns data that adheres to the <see cref="IApiResponse{type}"/> interface.</returns>
        public static async Task<IApiResponse<User>>
        GetUserAsync(string oauth_token, ApiRequestSettings api_request_settings = api_request_settings_default)
        {
            IApiResponse<User> users = await GetUserAsync(oauth_token, client_id_default, api_request_settings);

            return users;
        }

        /// <summary>
        /// <para>Asynchronously gets the information of the user looked up by the provided OAuth token.</para>
        /// <para>
        /// Optional Scope: 'user:read:email'.
        /// If provided, the user's email is included in the response.
        /// </para>
        /// </summary>
        /// <param name="oauth_token">The OAuth token to authorize the request.</param>
        /// <param name="client_id">The Client ID to identify the application making the request.</param>
        /// <param name="api_request_settings">A set up customizable settings to handle diferent status codes.</param>
        /// <returns>Returns data that adheres to the <see cref="IApiResponse{type}"/> interface.</returns>
        public static async Task<IApiResponse<User>>
        GetUserAsync(string oauth_token, string client_id, ApiRequestSettings api_request_settings = api_request_settings_default)
        {
            IApiResponse<User> users = await GetUsersAsync(oauth_token, client_id, null, api_request_settings);

            return users;
        }

        /// <summary>
        /// <para>
        /// Asynchronously gets the information of one or more users by their id or login.
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
        public static async Task<IApiResponse<User>>
        GetUsersAsync(string oauth_token, UsersQueryParameters query_parameters, ApiRequestSettings api_request_settings = api_request_settings_default)
        {
            IApiResponse<User> users = await GetUsersAsync(oauth_token, client_id_default, query_parameters, api_request_settings);

            return users;
        }

        /// <summary>
        /// <para>
        /// Asynchronously gets the information of one or more users by their id or login.
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
        public static async Task<IApiResponse<User>>
        GetUsersAsync(string oauth_token, string client_id, UsersQueryParameters query_parameters, ApiRequestSettings api_request_settings = api_request_settings_default)
        {
            IApiResponse<User> users = await TwitchApiInternal.GetUsersAsync(oauth_token, client_id, query_parameters, api_request_settings);

            return users;
        }

        /// <summary>
        /// Asynchronously gets the relationship between two users.
        /// </summary>
        /// <param name="oauth_token">The OAuth token to authorize the request.</param>
        /// <param name="from_id">The user to compare from.</param>
        /// <param name="to_id">The user to compare to.</param>
        /// <param name="api_request_settings">A set up customizable settings to handle diferent status codes.</param>
        /// <returns>Returns data that adheres to the <see cref="IApiResponsePage{type}"/> interface.</returns>
        public static async Task<IApiResponsePage<Follow>>
        GetUserRelationshipAsync(string oauth_token, string from_id, string to_id, ApiRequestSettings api_request_settings = api_request_settings_default)
        {
            IApiResponsePage<Follow> relationship = await GetUserRelationshipAsync(oauth_token, client_id_default, from_id, to_id, api_request_settings);

            return relationship;
        }

        /// <summary>
        /// Asynchronously gets the relationship between two users.
        /// </summary>
        /// <param name="oauth_token">The OAuth token to authorize the request.</param>
        /// <param name="client_id">The Client ID to identify the application making the request.</param>
        /// <param name="from_id">The user to compare from.</param>
        /// <param name="to_id">The user to compare to.</param>
        /// <param name="api_request_settings">A set up customizable settings to handle diferent status codes.</param>
        /// <returns>Returns data that adheres to the <see cref="IApiResponsePage{type}"/> interface.</returns>
        public static async Task<IApiResponsePage<Follow>>
        GetUserRelationshipAsync(string oauth_token, string client_id, string from_id, string to_id, ApiRequestSettings api_request_settings = api_request_settings_default)
        {
            IApiResponsePage<Follow> relationship = await TwitchApiInternal.GetUserRelationshipPageAsync(oauth_token, client_id, from_id, to_id, default(FollowsQueryParameters), api_request_settings);

            return relationship;
        }

        /// <summary>
        /// Asynchronously checks to see if <paramref name="from_id"/> is following <paramref name="to_id"/>.
        /// </summary>
        /// <param name="oauth_token">The OAuth token to authorize the request.</param>
        /// <param name="from_id">The user to compare from.</param>
        /// <param name="to_id">The user to compare to.</param>
        /// <param name="api_request_settings">A set up customizable settings to handle diferent status codes.</param>
        /// <returns>Returns data that adheres to the <see cref="IApiResponseValue{type}"/> interface.</returns>
        public static async Task<IApiResponseValue<bool>>
        IsUserFollowingAsync(string oauth_token, string from_id, string to_id, ApiRequestSettings api_request_settings = api_request_settings_default)
        {
            IApiResponseValue<bool> is_following = await IsUserFollowingAsync(oauth_token, client_id_default, from_id, to_id, api_request_settings);

            return is_following;
        }

        /// <summary>
        /// Asynchronously checks to see if <paramref name="from_id"/> is following <paramref name="to_id"/>.
        /// </summary>
        /// <param name="oauth_token">The OAuth token to authorize the request.</param>
        /// <param name="client_id">The Client ID to identify the application making the request.</param>
        /// <param name="from_id">The user to compare from.</param>
        /// <param name="to_id">The user to compare to.</param>
        /// <param name="api_request_settings">A set up customizable settings to handle diferent status codes.</param>
        /// <returns>Returns data that adheres to the <see cref="IApiResponse{type}"/> interface.</returns>
        public static async Task<IApiResponseValue<bool>>
        IsUserFollowingAsync(string oauth_token, string client_id, string from_id, string to_id, ApiRequestSettings api_request_settings = api_request_settings_default)
        {
            IApiResponseValue<bool> is_following = await TwitchApiInternal.IsUserFollowingAsync(oauth_token, client_id, from_id, to_id, api_request_settings);

            return is_following;
        }

        /// <summary>
        /// Asynchronously gets a single page of a user's following list.
        /// </summary>
        /// <param name="oauth_token">The OAuth token to authorize the request.</param>
        /// <param name="from_id">The user to get the following list from.</param>
        /// <param name="api_request_settings">A set up customizable settings to handle diferent status codes.</param>
        /// <returns>Returns data that adheres to the <see cref="IApiResponse{type}"/> interface.</returns>
        public static async Task<IApiResponsePage<Follow>>
        GetUserFollowingPageAsync(string oauth_token, string from_id, ApiRequestSettings api_request_settings = api_request_settings_default)
        {
            IApiResponsePage<Follow> following = await GetUserFollowingPageAsync(oauth_token, from_id, default(FollowsQueryParameters), api_request_settings);

            return following;
        }

        /// <summary>
        /// Asynchronously gets a single page of a user's following list.
        /// </summary>
        /// <param name="oauth_token">The OAuth token to authorize the request.</param>
        /// <param name="from_id">The user to get the following list from.</param>
        /// <param name="query_parameters">
        /// A set of query parameters to customize the request.
        /// The <code>from_id</code> and <code>to_id</code> properties in the <paramref name="query_parameters"/> are ignored if specified.
        /// </param>
        /// <param name="api_request_settings">A set up customizable settings to handle diferent status codes.</param>
        /// <returns>Returns data that adheres to the <see
        public static async Task<IApiResponsePage<Follow>>
        GetUserFollowingPageAsync(string oauth_token, string from_id, FollowsQueryParameters query_parameters, ApiRequestSettings api_request_settings = api_request_settings_default)
        {
            IApiResponsePage<Follow> following = await GetUserFollowingPageAsync(oauth_token, client_id_default, from_id, query_parameters, api_request_settings);

            return following;
        }

        /// <summary>
        /// Asynchronously gets a single page of a user's following list.
        /// </summary>
        /// <param name="oauth_token">The OAuth token to authorize the request.</param>
        /// <param name="client_id">The Client ID to identify the application making the request.</param>
        /// <param name="from_id">The user to get the following list from.</param>
        /// <param name="api_request_settings">A set up customizable settings to handle diferent status codes.</param>
        /// <returns>Returns data that adheres to the <see cref="IApiResponse{type}"/> interface.</returns>
        public static async Task<IApiResponsePage<Follow>>
        GetUserFollowingPageAsync(string oauth_token, string client_id, string from_id, ApiRequestSettings api_request_settings = api_request_settings_default)
        {
            IApiResponsePage<Follow> following = await GetUserFollowingPageAsync(oauth_token, client_id, from_id, default(FollowsQueryParameters), api_request_settings);

            return following;
        }

        /// <summary>
        /// Asynchronously gets a single page of a user's following list.
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
        public static async Task<IApiResponsePage<Follow>>
        GetUserFollowingPageAsync(string oauth_token, string client_id, string from_id, FollowsQueryParameters query_parameters, ApiRequestSettings api_request_settings = api_request_settings_default)
        {
            IApiResponsePage<Follow> following = await TwitchApiInternal.GetUserRelationshipPageAsync(oauth_token, client_id, from_id, string.Empty, query_parameters, api_request_settings);

            return following;
        }

        /// <summary>
        /// Asynchronously gets a user's following list.
        /// </summary>
        /// <param name="oauth_token">The OAuth token to authorize the request.</param>
        /// <param name="from_id">The user to get the following list from.</param>
        /// <param name="api_request_settings">A set up customizable settings to handle diferent status codes.</param>
        /// <returns>Returns data that adheres to the <see cref="IApiResponse{type}"/> interface.</returns>
        public static async Task<IApiResponse<Follow>>
        GetUserFollowingAsync(string oauth_token, string from_id, ApiRequestSettings api_request_settings = api_request_settings_default)
        {
            IApiResponse<Follow> following = await GetUserFollowingAsync(oauth_token, client_id_default, from_id, api_request_settings);

            return following;
        }

        /// <summary>
        /// Asynchronously gets a user's following list.
        /// </summary>
        /// <param name="oauth_token">The OAuth token to authorize the request.</param>
        /// <param name="client_id">The Client ID to identify the application making the request.</param>
        /// <param name="from_id">The user to get the following list from.</param>
        /// <param name="api_request_settings">A set up customizable settings to handle diferent status codes.</param>
        /// <returns>Returns data that adheres to the <see cref="IApiResponse{type}"/> interface.</returns>
        public static async Task<IApiResponse<Follow>>
        GetUserFollowingAsync(string oauth_token, string client_id, string from_id, ApiRequestSettings api_request_settings = api_request_settings_default)
        {
            IApiResponse<Follow> following = await TwitchApiInternal.GetUserRelationshipAsync(oauth_token, client_id, from_id, string.Empty, default(FollowsQueryParameters), api_request_settings);

            return following;
        }

        /// <summary>
        /// Asynchronously gets a single paged of a user's followers list.
        /// </summary>
        /// <param name="oauth_token">The OAuth token to authorize the request.</param>
        /// <param name="to_id">The user to get the followers for.</param>
        /// <param name="api_request_settings">A set up customizable settings to handle diferent status codes.</param>
        /// <returns>Returns data that adheres to the <see cref="IApiResponse{type}"/> interface.</returns>
        public static async Task<IApiResponsePage<Follow>>
        GetUserFollowersPageAsync(string oauth_token, string to_id, ApiRequestSettings api_request_settings = api_request_settings_default)
        {
            IApiResponsePage<Follow> followers = await GetUserFollowersPageAsync(oauth_token, to_id, default(FollowsQueryParameters), api_request_settings);

            return followers;
        }

        /// <summary>
        /// Asynchronously gets a single paged of a user's followers list.
        /// </summary>
        /// <param name="oauth_token">The OAuth token to authorize the request.</param>
        /// <param name="to_id">The user to get the followers for.</param>
        /// <param name="query_parameters">
        /// A set of query parameters to customize the request.
        /// The <code>from_id</code> and <code>to_id</code> properties in the <paramref name="query_parameters"/> are ignored if specified.
        /// </param>
        /// <param name="api_request_settings">A set up customizable settings to handle diferent status codes.</param>
        /// <returns>Returns data that adheres to the <see cref="IApiResponse{type}"/> interface.</returns>
        public static async Task<IApiResponsePage<Follow>>
        GetUserFollowersPageAsync(string oauth_token, string to_id, FollowsQueryParameters query_parameters, ApiRequestSettings api_request_settings = api_request_settings_default)
        {
            IApiResponsePage<Follow> followers = await GetUserFollowersPageAsync(oauth_token, client_id_default, to_id, query_parameters);

            return followers;
        }

        /// <summary>
        /// Asynchronously gets a single paged of a user's followers list.
        /// </summary>
        /// <param name="oauth_token">The OAuth token to authorize the request.</param>
        /// <param name="client_id">The Client ID to identify the application making the request.</param>
        /// <param name="to_id">The user to get the followers for.</param>
        /// <param name="api_request_settings">A set up customizable settings to handle diferent status codes.</param>
        /// <returns>Returns data that adheres to the <see cref="IApiResponse{type}"/> interface.</returns>
        public static async Task<IApiResponsePage<Follow>>
        GetUserFollowersPageAsync(string oauth_token, string client_id, string to_id, ApiRequestSettings api_request_settings = api_request_settings_default)
        {
            IApiResponsePage<Follow> followers = await GetUserFollowersPageAsync(oauth_token, client_id, to_id, default(FollowsQueryParameters), api_request_settings);
            return followers;
        }

        /// <summary>
        /// Asynchronously gets a single paged of a user's followers list.
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
        public static async Task<IApiResponsePage<Follow>>
        GetUserFollowersPageAsync(string oauth_token, string client_id, string to_id, FollowsQueryParameters query_parameters, ApiRequestSettings api_request_settings = api_request_settings_default)
        {
            IApiResponsePage<Follow> followers = await TwitchApiInternal.GetUserRelationshipPageAsync(oauth_token, client_id, string.Empty, to_id, query_parameters, api_request_settings);
            return followers;
        }

        /// <summary>
        /// Asynchronously gets a user's followers list.
        /// </summary>
        /// <param name="oauth_token">The OAuth token to authorize the request.</param>
        /// <param name="to_id">The user to get the followers for.</param>
        /// <param name="api_request_settings">A set up customizable settings to handle diferent status codes.</param>
        /// <returns>Returns data that adheres to the <see cref="IApiResponse{type}"/> interface.</returns>
        public static async Task<IApiResponse<Follow>>
        GetUserFollowersAsync(string oauth_token, string to_id, ApiRequestSettings api_request_settings = api_request_settings_default)
        {
            IApiResponse<Follow> followers = await GetUserFollowersAsync(oauth_token, client_id_default, to_id, api_request_settings);

            return followers;
        }

        /// <summary>
        /// Asynchronously gets a user's followers list.
        /// </summary>
        /// <param name="oauth_token">The OAuth token to authorize the request.</param>
        /// <param name="client_id">The Client ID to identify the application making the request.</param>
        /// <param name="to_id">The user to get the followers for.</param>
        /// <param name="api_request_settings">A set up customizable settings to handle diferent status codes.</param>
        /// <returns>Returns data that adheres to the <see cref="IApiResponse{type}"/> interface.</returns>
        public static async Task<IApiResponse<Follow>>
        GetUserFollowersAsync(string oauth_token, string client_id, string to_id, ApiRequestSettings api_request_settings = api_request_settings_default)
        {
            IApiResponse<Follow> followers = await TwitchApiInternal.GetUserRelationshipAsync(oauth_token, client_id, string.Empty, to_id, default(FollowsQueryParameters), api_request_settings);

            return followers;
        }

        /// <summary>
        /// <para>Asynchronously sets the description of a user specified by the OAuth token provided.</para>
        /// <para>Required Scope: 'user:edit'</para>
        /// </summary>
        /// <param name="oauth_token">The OAuth token used to determine whose description to update and authorize the request.</param>
        /// <param name="description">The new description to set.</param>
        /// <param name="api_request_settings">A set up customizable settings to handle diferent status codes.</param>
        /// <returns>Returns data that adheres to the <see cref="IApiResponse{type}"/> interface.</returns>
        public static async Task<IApiResponseValue<bool>>
        SetUserDescriptionAsync(string oauth_token, string description, ApiRequestSettings api_request_settings = api_request_settings_default)
        {
            IApiResponseValue<bool> success = await SetUserDescriptionAsync(oauth_token, client_id_default, description, api_request_settings);

            return success;
        }

        /// <summary>
        /// <para>Asynchronously sets the description of a user specified by the OAuth token provided.</para>
        /// <para>Required Scope: 'user:edit'</para>
        /// </summary>
        /// <param name="oauth_token">The OAuth token used to determine whose description to update and authorize the request.</param>
        /// <param name="client_id">The Client ID to identify the application making the request.</param>
        /// <param name="description">The new description to set.</param>
        /// <param name="api_request_settings">A set up customizable settings to handle diferent status codes.</param>
        /// <returns>Returns data that adheres to the <see cref="IApiResponse{type}"/> interface.</returns>
        public static async Task<IApiResponseValue<bool>>
        SetUserDescriptionAsync(string oauth_token, string client_id, string description, ApiRequestSettings api_request_settings = api_request_settings_default)
        {
            IApiResponseValue<bool> success = await TwitchApiInternal.SetUserDescriptionAsync(oauth_token, client_id, description, api_request_settings);

            return success;
        }

        #endregion

        #region Videos

        /// <summary>
        /// Asynchronously gets a single page of information on one or more videos.
        /// </summary>
        /// <param name="oauth_token">The OAuth token used to determine whose description to update and authorize the request.</param>
        /// <param name="query_parameters">A set of query parameters to customize the request.</param>
        /// <param name="api_request_settings">A set up customizable settings to handle diferent status codes.</param>
        /// <returns>Returns data that adheres to the <see cref="IApiResponsePage{type}"/> interface.</returns>
        public static async Task<IApiResponsePage<Video>>
        GetVideosPageAsync(string oauth_token, VideosQueryParameters query_parameters, ApiRequestSettings api_request_settings = api_request_settings_default)
        {
            IApiResponsePage<Video> videos = await GetVideosPageAsync(oauth_token, client_id_default, query_parameters, api_request_settings);

            return videos;
        }

        /// <summary>
        /// Asynchronously gets a single page of information on one or more videos.
        /// </summary>
        /// <param name="oauth_token">The OAuth token used to determine whose description to update and authorize the request.</param>
        /// <param name="client_id">The Client ID to identify the application making the request.</param>
        /// <param name="query_parameters">A set of query parameters to customize the request.</param>
        /// <param name="api_request_settings">A set up customizable settings to handle diferent status codes.</param>
        /// <returns>Returns data that adheres to the <see cref="IApiResponsePage{type}"/> interface.</returns>
        public static async Task<IApiResponsePage<Video>>
        GetVideosPageAsync(string oauth_token, string client_id, VideosQueryParameters query_parameters, ApiRequestSettings api_request_settings = api_request_settings_default)
        {
            IApiResponsePage<Video> videos = await TwitchApiInternal.GetVideosPageAsync(oauth_token, client_id, query_parameters, api_request_settings);

            return videos;
        }

        /// <summary>
        /// Asynchronously gets a complete list of information on one or more videos.
        /// </summary>
        /// <param name="oauth_token">The OAuth token used to determine whose description to update and authorize the request.</param>
        /// <param name="query_parameters">A set of query parameters to customize the request.</param>
        /// <param name="api_request_settings">A set up customizable settings to handle diferent status codes.</param>
        /// <returns>Returns data that adheres to the <see cref="IApiResponse{type}"/> interface.</returns>
        public static async Task<IApiResponse<Video>>
        GetVideosAsync(string oauth_token, VideosQueryParameters query_parameters, ApiRequestSettings api_request_settings = api_request_settings_default)
        {
            IApiResponse<Video> videos = await GetVideosAsync(oauth_token, client_id_default, query_parameters, api_request_settings);

            return videos;
        }

        /// <summary>
        /// Asynchronously gets a complete list of information on one or more videos.
        /// </summary>
        /// <param name="oauth_token">The OAuth token used to determine whose description to update and authorize the request.</param>
        /// <param name="client_id">The Client ID to identify the application making the request.</param>
        /// <param name="query_parameters">A set of query parameters to customize the request.</param>
        /// <param name="api_request_settings">A set up customizable settings to handle diferent status codes.</param>
        /// <returns>Returns data that adheres to the <see cref="IApiResponse{type}"/> interface.</returns>
        public static async Task<IApiResponse<Video>>
        GetVideosAsync(string oauth_token, string client_id, VideosQueryParameters query_parameters, ApiRequestSettings api_request_settings = api_request_settings_default)
        {
            IApiResponse<Video> videos = await TwitchApiInternal.GetVideosAsync(oauth_token, client_id, query_parameters, api_request_settings);

            return videos;
        }

        #endregion
    }
}