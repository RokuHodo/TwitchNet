namespace
TwitchNet.Interfaces.Api
{
    public interface
    IApiResponseValue<type> : IApiResponse
    {

        /// <summary>
        /// The value of the result. 
        /// </summary>
        type result { get; }
    }
}
