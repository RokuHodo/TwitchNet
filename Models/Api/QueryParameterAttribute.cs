﻿// standard namespaces
using System;

namespace
TwitchNet.Models.Api
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false)]
    internal sealed class
    QueryParameterAttribute : Attribute
    {
        #region Properties

        /// <summary>
        /// Whether or not to make the query parameter value lower case.
        /// </summary>
        public bool                 to_lower            { get; set; }

        /// <summary>
        /// The name of the query parameter.
        /// </summary>
        public string               query_name          { get; set; }

        #endregion

        #region Constructors

        public QueryParameterAttribute(string query_name, bool to_lower = true)
        {
            this.query_name = query_name;
            this.to_lower = to_lower;
        }

        #endregion
    }
}
