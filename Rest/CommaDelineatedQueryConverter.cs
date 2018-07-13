namespace
TwitchNet.Rest
{
    /// <summary>
    /// Converts the elements of an array/list or the set flags of a bitfield enum into a comma deliniated string.
    /// The delineated string is then added to the <see cref="RestRequest"/> as a single query parameter.
    /// </summary>
    public class
    CommaDelineatedQueryConverter : DelineatedQueryConverter
    {
        public CommaDelineatedQueryConverter()
        {
            delineator = ",";
        }
    }
}
