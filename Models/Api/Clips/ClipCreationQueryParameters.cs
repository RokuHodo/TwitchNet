namespace
TwitchNet.Models.Api.Clips
{
    public class
    ClipCreationQueryParameters
    {
        private string _broadcaster_id;

        /// <summary>
        /// The user ID from which the clip will be made.
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
