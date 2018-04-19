namespace
TwitchNet.Models.Api
{
    public struct
    QueryParameter
    {
        /// <summary>
        /// The string value to be added as a query parameter.
        /// </summary>
        public string name;

        /// <summary>
        /// The string value to be added as a query parameter.
        /// </summary>
        public string value;

        /// <summary>
        /// Creates a new instance of the <see cref="QueryParameter"/> struct;
        /// </summary>
        /// <param name="name">The name of the query parameter.</param>
        /// <param name="value">The value of the query parameter.</param>
        public QueryParameter(string name, string value)
        {
            this.name   = name;
            this.value  = value;
        }
    }
}
