﻿// standard nsamespaces
using System.Collections.Generic;
using System.Runtime.Serialization;

// project namespaces
using TwitchNet.Helpers.Json;

// imported .dll's
using Newtonsoft.Json;

namespace
TwitchNet.Rest.Api.Entitlements
{
    public class
    EntitlementsCodeParameters
    {
        /// <summary>
        /// The ID of the user that will recieve the entitlement associated with the code.
        /// </summary>
        [QueryParameter("user_id")]
        public virtual string user_id { get; set; }

        /// <summary>
        /// <para>A list of codes, up to 20.</para>
        /// <para>All elements that are null, empty, or contain only whitespace are filtered out and all duplicate elements are removed before calculating the final count.</para>
        /// </summary>
        [QueryParameter("code", typeof(SeparateQueryConverter))]
        public virtual List<string> codes { get; set; }

        public EntitlementsCodeParameters()
        {
            codes = new List<string>();
        }
    }

    public class
    CodeStatus
    {
        /// <summary>
        /// The ID of the user that will recieve the entitlement associated with the code.
        /// </summary>
        [QueryParameter("user_id")]
        public virtual string user_id { get; set; }

        /// <summary>
        /// <para>A list of codes, up to 20.</para>
        /// <para>All elements that are null, empty, or contain only whitespace are filtered out and all duplicate elements are removed before calculating the final count.</para>
        /// </summary>
        [QueryParameter("status")]
        public virtual EntitlementCodeStatus status { get; set; }
    }

    [JsonConverter(typeof(EnumConverter))]
    public enum
    EntitlementCodeStatus
    {
        /// <summary>
        /// Unsupported or unkown code status.
        /// </summary>
        [EnumMember(Value = "")]
        Other = 0,

        /// <summary>
        /// <para>SUCCESSFULLY_REDEEMED</para>
        /// <para>The code was successfully redeemed to the authenticated user’s account.</para>
        /// <para>This status will only be valid when redeeming a code.</para>
        /// </summary>
        [EnumMember(Value = "SUCCESSFULLY_REDEEMED")]
        SuccessfullyRedeemed,

        /// <summary>
        /// <para>ALREADY_CLAIMED</para>
        /// <para>The code has already been claimed by another user.</para>
        /// </summary>
        [EnumMember(Value = "ALREADY_CLAIMED")]
        AlreadyClaimed,

        /// <summary>
        /// <para>EXPIRED</para>
        /// <para>The code has expired and can no longer be claimed.</para>
        /// </summary>
        [EnumMember(Value = "EXPIRED")]
        Expired,

        /// <summary>
        /// <para>USER_NOT_ELIGIBLE</para>
        /// <para>The user is not eligible to redeem this code.</para>
        /// </summary>
        [EnumMember(Value = "USER_NOT_ELIGIBLE")]
        UserNotEligible,

        /// <summary>
        /// <para>NOT_FOUND</para>
        /// <para>The code is not valid and/or does not exist in the database.</para>
        /// </summary>
        [EnumMember(Value = "NOT_FOUND")]
        NotFound,

        /// <summary>
        /// <para>INACTIVE</para>
        /// <para>The code is not currently active.</para>
        /// </summary>
        [EnumMember(Value = "INACTIVE")]
        Inactive,

        /// <summary>
        /// <para>UNUSED</para>
        /// <para>The code has not been claimed.</para>
        /// <para>This status will only be valid when checking the status of a code.</para>
        /// </summary>
        [EnumMember(Value = "UNUSED")]
        Unused,

        /// <summary>
        /// <para>INCORRECT_FORMAT</para>
        /// <para>The code was not properly formatted.</para>
        /// </summary>
        [EnumMember(Value = "INCORRECT_FORMAT")]
        IncorrectFormat,

        /// <summary>
        /// <para>INTERNAL_ERROR</para>
        /// <para>Twitch encountered an internal and/or unknown failure handling this code.</para>
        /// </summary>
        [EnumMember(Value = "INTERNAL_ERROR")]
        InternalError,
    }
}
