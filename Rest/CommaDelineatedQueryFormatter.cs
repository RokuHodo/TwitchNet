namespace
TwitchNet.Rest
{
    /// <summary>
    /// Formats the elements of an array/list or the set flags of a bitfield enum into a comma deliniated string.
    /// The delineated string is then added to the <see cref="RestRequest"/> as a single query parameter.
    /// </summary>
    public class
    CommaDelineatedQueryFormatter : DelineatedQueryFormatter
    {
        public CommaDelineatedQueryFormatter()
        {
            delineator = ",";
        }
    }
}
