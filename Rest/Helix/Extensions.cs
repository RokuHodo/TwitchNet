// standard nsamespaces
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

// project namespaces
using TwitchNet.Helpers.Json;

// imported .dll's
using Newtonsoft.Json;

namespace
TwitchNet.Rest.Helix
{
    #region /extensions/transactions

    public class
    ExtensionTransactionsParameters : PagingParameters, IPagingParameters
    {
        /// <summary>
        /// The ID of the extension.
        /// </summary>
        [QueryParameter("extension_id")]
        public virtual string extension_id { get; set; }

        /// <summary>
        /// A list of transaction ID's to loop up, up to 100.
        /// </summary>
        [QueryParameter("id", typeof(SeparateQueryConverter))]
        public virtual List<string> ids { get; set; }

        public ExtensionTransactionsParameters()
        {
            ids = new List<string>();
        }
    }

    public class
    ExtensionTransaction
    {
        /// <summary>
        /// The user ID of the channel the transaction occurred in.
        /// </summary>
        [JsonProperty("broadcaster_id")]
        public virtual string broadcaster_id { get; set; }

        /// <summary>
        /// The display namer of the channel the transaction occurred in.
        /// </summary>
        [JsonProperty("broadcaster_name")]
        public virtual string broadcaster_name { get; set; }

        /// <summary>
        /// The ID of the extension.
        /// </summary>
        [JsonProperty("id")]
        public virtual string id { get; set; }

        /// <summary>
        /// The product data associated with the transaction.
        /// </summary>
        [JsonProperty("product_data")]
        public virtual ProductData product_data { get; set; }

        /// <summary>
        /// The type of product that was purchased with the transaction.
        /// </summary>
        [JsonProperty("product_type")]
        public virtual ProductType product_type { get; set; }

        /// <summary>
        /// When the transaction occurred.
        /// </summary>
        [JsonProperty("timestamp")]
        public virtual DateTime timestamp { get; set; }

        /// <summary>
        /// The ID of the user who performed the transaction.
        /// </summary>
        [JsonProperty("user_id")]
        public virtual string user_id { get; set; }

        /// <summary>
        /// The display name of the user who performed the transaction.
        /// </summary>
        [JsonProperty("user_name")]
        public virtual string user_name { get; set; }        
    }

    public class
    ProductData
    {
        /// <summary>
        /// The required cost to acquire the product.
        /// </summary>
        [JsonProperty("cost")]
        public virtual TransactionCost cost { get; set; }        

        /// <summary>
        /// Whether or not the data was sent over the extension pubsub to all instances of the extension.
        /// </summary>
        [JsonProperty("broadcast")]
        public virtual bool broadcast { get; set; }

        /// <summary>
        ///  The product display name.
        /// </summary>
        [JsonProperty("displayName")]
        public virtual string displayName { get; set; }

        /// <summary>
        /// The extension domain.
        /// Should be set to 'twitch.ext' + the extension ID.
        /// </summary>
        [JsonProperty("domain")]
        public virtual string domain { get; set; }

        /// <summary>
        /// Not currently used by Twitcn since only unexpired products can be purchased.
        /// </summary>
        [JsonProperty("expiration")]
        public virtual string expiration { get; set; }

        /// <summary>
        /// Whether or not the product is currently in development.
        /// </summary>
        [JsonProperty("inDevelopment")]
        public virtual bool inDevelopment { get; set; }

        /// <summary>
        /// A unique identifier for the product across the extension.
        /// </summary>
        [JsonProperty("sku")]
        public virtual string sku { get; set; }        
    }

    public class
    TransactionCost
    {
        /// <summary>
        /// The number of required bits to acquire the product.
        /// </summary>
        [JsonProperty("amount")]
        public virtual int amount { get; set; }

        /// <summary>
        /// The transaction currency type.
        /// Always bits.
        /// </summary>
        [JsonProperty("type")]
        public virtual CostType type { get; set; }
    }

    [JsonConverter(typeof(EnumConverter))]
    public enum
    ProductType
    {
        /// <summary>
        /// Unsupported or unkown product type.
        /// </summary>
        [EnumMember(Value = "")]
        Other = 0,

        /// <summary>
        /// <para>BITS_IN_EXTENSION</para>
        /// <para>A user used bits in an extension to complete the transaction.</para>
        /// </summary>
        [EnumMember(Value = "BITS_IN_EXTENSION")]
        BitsInExtension
    }

    public enum
    CostType
    {
        /// <summary>
        /// Unsupported or unkown cost type.
        /// </summary>
        [EnumMember(Value = "")]
        Other = 0,

        /// <summary>
        /// Bits.
        /// </summary>
        [EnumMember(Value = "bits")]
        Bits
    }

    #endregion
}