namespace
TwitchNet.Debugger
{
    public enum 
    Check
    {
        None = 0,

        IsNull,

        IsNotNull,

        IsDefault,

        IsNotDefault,

        IsNullOrDefault,

        IsNotNullOrDefault,

        IsValid,

        IsInvalid,

        IsEqualTo,

        IsNotEqualTo,        

        RegexIsMatch,

        RegexNoMatch,

        TagsMissing,

        TagsExtra,
    }
}
