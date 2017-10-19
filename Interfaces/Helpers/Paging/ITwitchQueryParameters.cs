﻿// project namespaces
using TwitchNet.Helpers.Paging;

namespace TwitchNet.Interfaces.Helpers.Paging
{
    public interface
    ITwitchQueryParameters
    {
        /// <summary>
        /// Maximum number of objects to return.
        /// </summary>
        [QueryParameter("first")]
        ushort first { get; set; }

        /// <summary>
        /// The cursor that tells the server where to start fetching the next set of results, in a multi-page response.
        /// </summary>
        [QueryParameter("after", false)]
        string after { get; set; }
    }
}