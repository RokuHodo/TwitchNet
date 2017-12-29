namespace
TwitchNet.Models.Api.Clips
{
    public class
    ClipCreationQueryParameters
    {
        private string _broadcaster_id;

        /// <summary>
        /// The ID of the clip being queried.
        /// </summary>
        [QueryParameter("broadcaster_id")]
        public string broadcaster_id
        {
            get
            {
                return _broadcaster_id;
            }
            set
            {
                _broadcaster_id = value;
            }
        }

        public ClipCreationQueryParameters()
        {

        }
    }
}
