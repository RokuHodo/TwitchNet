﻿// imported .dll's
using Newtonsoft.Json;

namespace
TwitchNet.Rest.Api.Entitlements
{
    public class
    EntitlementUploadUrl
    {
        /// <summary>
        /// The URL where you will upload the manifest file.
        /// This is the URL of a pre-signed S3 bucket.
        /// Lease time: 15 minutes.
        /// </summary>
        [JsonProperty("url")]
        public string url { get ; protected set; }
    }
}