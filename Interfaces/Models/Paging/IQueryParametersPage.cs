// project namespaces
using TwitchNet.Models.Paging;

namespace TwitchNet.Interfaces.Models.Paging
{
    public interface
    IQueryParametersPage
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
