namespace
TwitchNet.Models.Api.Clips
{
    public class
    ClipQueryParameters
    {
        private string _id;

        /// <summary>
        /// The ID of the clip being queried.
        /// </summary>
        [QueryParameter("id")]
        public string id
        {
            get
            {
                return _id;
            }
            set
            {
                _id = value;
            }
        }

        public ClipQueryParameters()
        {

        }
    }
}
