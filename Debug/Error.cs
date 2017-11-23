namespace
TwitchNet.Debug
{
    internal static class
    Error
    {
        #region Normal logging messages

        public static string NORMAL_NULL = "Value cannot be null.";
        public static string NORMAL_TASK_FAULTED = "Task has faulted.";
        public static string NORMAL_EXCEPTION = "Compiler exception";
        public static string NORMAL_CONVERT = "Could not convert the object to desired type.";
        public static string NORMAL_OUT_OF_BOUNDS = "Index is out of bounds.";
        public static string NORMAL_DESERIALIZE = "Failed to deserialize the object.";
        public static string NORMAL_SERIALIZE = "Failed to serialize the object.";

        #endregion

        #region Exception messages

        public static string EXCEPTION_ARGUMENT_EMPTY = "Value is empty or whitespace.";
        public static string EXCEPTION_ARGUMENT_TWITCH_NAME = "The Twitch NICK must start with a letter, contain only letters, numbers, or underscores, and must be between 3 and 25 characters (inclusive).";
        public static string EXCEPTION_ARGUMENT_TWITCH_Bearer = "The Twitch Bearer token must be prefixed with \"Bearer:\".";

        #endregion
    }
}
