namespace
TwitchNet.Rest
{
    public enum
    QueryParameterType
    {
        /// <summary>
        /// <para>Auto adds the query parameter based on the query parameter's object type.</para>
        /// <para>
        /// If the parameter's object type implements the <see cref="IList"/> interface or is a bitfield enum, each parameter will be added as <see cref="ListSingleValues"/>.
        /// If the parameter's object type is anything else, the query parameter will be added as <see cref="Single"/>.
        /// </para>
        /// </summary>
        Auto = 0,

        /// <summary>
        /// <para>Adds the query parameter as a single value.</para>
        /// <para>If the object's type implements the <see cref="IList"/> interface or is a bitfield enum, each parameter will be added as <see cref="Auto"/>.</para>
        /// </summary>
        Single,

        /// <summary>
        /// <para>Each query parameter value will be formatted into a single string, with a single space between each value.</para>
        /// <para>
        /// Valid for any object type that implements the <see cref="IList"/> interface or is a bitfield enum.
        /// If the object's type is not vlaid, the query parameter will be added as <see cref="Auto"/>.
        /// </para>
        /// </summary>
        ListSpaceSeparated,

        /// <summary>
        /// <para>Each query parameter value will be formatted into a single string, with a single comma space between each value.</para>
        /// <para>
        /// Valid for any object type that implements the <see cref="IList"/> interface or is a bitfield enum.
        /// If the object's type is not vlaid, the query parameter will be added as <see cref="Auto"/>.
        /// </para>
        /// </summary>
        ListCommaSeparated,

        /// <summary>
        /// <para>Each query parameter value will be added as separated parameters.</para>
        /// <para>
        /// Valid for any object type that implements the <see cref="IList"/> interface or is a bitfield enum.
        /// If the object's type is not vlaid, the query parameter will be added as <see cref="Auto"/>.
        /// </para>
        /// </summary>
        ListSingleValues,
    }
}
