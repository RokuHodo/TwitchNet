// standard namespaces
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;

// project namespaces
using TwitchNet.Debugger;
using TwitchNet.Rest;
using TwitchNet.Rest.Api;
using TwitchNet.Rest.OAuth;
using TwitchNet.Rest.OAuth.Validate;
using TwitchNet.Extensions;

// imported .dll's
using Newtonsoft.Json;

namespace
TwitchNet.Utilities
{
    public static partial class
    RestUtil
    {        
        //public static async Task<RestInfo<result_type>>
        //TraceExecuteAsync<data_type, result_type>(RestInfo<result_type> info, IPagingParameters parameters)
        //where result_type : DataPage<data_type>, IDataPage<data_type>, new()
        //{
        //    IRestResponse<result_type> response = new RestResponse<result_type>();

        //    result_type total_data = new result_type();
        //    total_data.data = new List<data_type>();

        //    List<data_type> data = new List<data_type>();

        //    if (parameters.IsNull())
        //    {
        //        parameters = new PagingParameters();
        //    }

        //    bool requesting = true;
        //    do
        //    {
        //        info = await ExecuteAsync(info);

        //        if (info.response.Data.data.IsValid())
        //        {
        //            foreach (data_type element in info.response.Data.data)
        //            {
        //                data.Add(element);
        //            }
        //        }                

        //        requesting = info.response.Data.data.IsValid() && info.response.Data.pagination.cursor.IsValid();
        //        if (requesting)
        //        {
        //            // NOTE: This is a temporary fix and will only work with Helix.
        //            info.request = info.request.AddOrUpdateParameter("after", info.response.Data.pagination.cursor, ParameterType.QueryString);
        //        }
        //        else
        //        {
        //            // TODO: Clean up
        //            total_data = info.response.Data;
        //            total_data.data = data;

        //            response = info.response;
        //            response.Data = total_data;

        //            info.response = response;
        //        }
        //    }
        //    while (requesting);

        //    return info;
        //}       
    }
}