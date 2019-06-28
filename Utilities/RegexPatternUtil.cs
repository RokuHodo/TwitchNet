namespace TwitchNet.Utilities
{
    internal static class
    RegexPatternUtil
    {
        public const string UUID = "[a-fA-F0-9]{8}-[a-fA-F0-9]{4}-[a-fA-F0-9]{4}-[a-fA-F0-9]{4}-[a-fA-F0-9]{12}";

        public const string ENTITLEMENT_CODE = "^[a-zA-Z0-9]{5}-?[a-zA-Z0-9]{5}-?[a-zA-Z0-9]{5}$";
    }
}
