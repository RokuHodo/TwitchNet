// standard namespaces
using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;

// project namespaces
using TwitchNet.Debugger;
using TwitchNet.Extensions;
using TwitchNet.Rest.Api.Streams;

namespace
TwitchNet.Utilities
{
    public static class
    EnumUtil
    {
        private static readonly ConcurrentDictionary<Type, EnumTypeCache> CACHE = new ConcurrentDictionary<Type, EnumTypeCache>();

        #region Structs

        private struct
        EnumResult
        {
            /// <summary>
            /// The enum name result.
            /// </summary>
            public string name;

            public string[] names;

            /// <summary>
            /// The enum value result.
            /// </summary>
            public object value;

            public object[] values;

            /// <summary>
            /// The detailed inner exception.
            /// </summary>
            public Exception inner_exception;

            /// <summary>
            /// Throws an general <see cref="Exception"/> with a more detailed inner exception that was encountered.
            /// </summary>
            /// <param name="message">The general exception message</param>
            public void
            Throw(string message)
            {
                Exception exception = new Exception(message, inner_exception);

                throw exception;
            }

            /// <summary>
            /// Throws an general <see cref="ArgumentException"/> with a more detailed inner exception that was encountered.
            /// </summary>
            /// <param name="message">The general exception message</param>
            public void
            Throw(string message, string param_name)
            {
                ArgumentException exception = new ArgumentException(message, param_name, inner_exception);

                throw exception;
            }
        }

        public readonly struct
        EnumTypeCache
        {
            #region Fields

            /// <summary>
            /// The enum's type.
            /// </summary>
            public readonly Type type;

            /// <summary>
            /// The Enums's underlying type code.
            /// </summary>
            public readonly TypeCode type_code;

            /// <summary>
            /// Whether or not the enum has the <see cref="FlagsAttribute"/> and is a collection of flags.
            /// </summary>
            public readonly bool is_flags;

            /// <summary>
            /// The enum's default value.
            /// </summary>
            public readonly object default_value;

            /// <summary>
            /// The native (unresolved) enum names.
            /// </summary>
            public readonly string[] names;

            /// <summary>
            /// The native (unresolved) enum values.
            /// </summary>
            public readonly Array values;

            /// <summary>
            /// <para>The resolved enum names.</para>
            /// <para>The names extraced from the <see cref="EnumMemberAttribute"/>, otherwise the native names.</para>
            /// </summary>
            public readonly string[] resolved_names;

            /// <summary>
            /// The resolved enum values converted to <see cref="UInt64"/>.
            /// </summary>
            public readonly ulong[] resolved_values;

            #endregion

            #region Constructors

            /// <summary>
            /// <para>Creates a new instance of the <see cref="EnumTypeCache"/> struct.</para>
            /// <para>Builds the cache for this enum.</para>
            /// </summary>
            /// <param name="type">The enum's type.</param>
            /// <exception cref="ArgumentNullException">Thrown if the <paramref name="type"/> is null.</exception>
            /// <exception cref="ArgumentException">
            /// Thrown if the <paramref name="type"/> is not an enum.
            /// </exception>
            public EnumTypeCache(Type type)
            {
                ExceptionUtil.ThrowIfNull(type, nameof(type));
                if (!type.IsEnum)
                {
                    throw new NotSupportedException("Type " + type.Name.WrapQuotes() + " is not an enum.");
                }

                this.type = type;
                type_code = Type.GetTypeCode(type);

                is_flags = type.IsDefined(typeof(FlagsAttribute), false);

                default_value = type.GetDefaultValue();

                names = Enum.GetNames(type);
                values = Enum.GetValues(type);

                resolved_names = new string[names.Length];
                resolved_values = new ulong[names.Length];

                for (long index = 0; index < names.Length; ++index)
                {
                    object value = values.GetValue(index);
                    TryToUInt64(type_code, value, out resolved_values[index]);

                    FieldInfo field = type.GetField(names[index], BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
                    resolved_names[index] = field.TryGetAttribute(out EnumMemberAttribute attribute) ? attribute.Value : names[index];
                }
            }

            #endregion

            #region Methods

            /// <summary>
            /// <para>Gets the name of the specified enum's value.</para>
            /// <para>Supports bitfield enum values.</para>
            /// </summary>
            /// <param name="value">The enum value to get the name of.</param>
            /// <returns>
            /// Returns resolved name of the enum value.
            /// </returns>
            /// <exception cref="ArgumentNullException">Thrown if the specified value is null.</exception>
            /// <exception cref="ArgumentException">
            /// Thrown if no names or values exist in the enum.
            /// Thrown if the specified value cannot be converted to a UInt64.
            /// Thrown if no flags were set or one or more flags in the specified value could not be converted if the enum is a bitfield enum.
            /// Thrown if the specified value could not be found in the enum type.
            /// </exception>
            public string
            GetName(object value)
            {
                EnumResult enum_result = new EnumResult();

                if (!TryGetNameInternal(value, ref enum_result))
                {
                    enum_result.Throw("The specified value could not be converted into an equivalent enum name of type " + type.Name.WrapQuotes() + ".");
                }

                return enum_result.name;
            }

            /// <summary>
            /// <para>Attempts to get the name of the specified enum's value.</para>
            /// <para>Supports bitfield enum values.</para>
            /// </summary>
            /// <param name="value">The enum value to get the name of.</param>
            /// <param name="result">
            /// Set to the resolved name, if successful.
            /// Set to null otherwise.
            /// </param>
            /// <returns>
            /// Returns true if the name was successfully retrieved.
            /// Returns false otherwise.
            /// </returns>
            public bool
            TryGetName(object value, out string result)
            {
                EnumResult enum_result = new EnumResult();

                bool success = TryGetNameInternal(value, ref enum_result);
                result = enum_result.name;

                return success;
            }

            /// <summary>
            /// <para>Attempts to get the name of the specified enum's value.</para>
            /// <para>Supports bitfield enum values.</para>
            /// </summary>
            /// <param name="value">The enum value to get the name of.</param>
            /// <param name="enum_result">The internal enum result used for error handling.</param>
            /// <returns>
            /// Returns true if the name was successfully retrieved.
            /// Returns false otherwise.
            /// </returns>
            private bool
            TryGetNameInternal(object value, ref EnumResult enum_result)
            {
                enum_result.name = null;

                if (!PassesCommonChecks(value, ref enum_result))
                {
                    return false;
                }

                if (!TryToUInt64(type_code, value, out ulong _value))
                {
                    enum_result.inner_exception = new ArgumentException("Failed to convert specified value " + value.ToString().WrapQuotes() + " to UInt64.");

                    return false;
                }

                if (is_flags)
                {
                    enum_result.name = FormatFlagsAsName(_value);
                    if (!enum_result.name.IsNull())
                    {
                        return true;
                    }

                    enum_result.inner_exception = new ArgumentException("Failed to convert one or more flags in the specified value into a name for the enum type " + type.Name.WrapQuotes() + ".", nameof(value));
                }
                else
                {
                    int index = Array.BinarySearch(resolved_values, _value);
                    if (index > -1)
                    {
                        enum_result.name = resolved_names[index];

                        return true;
                    }

                    enum_result.inner_exception = new ArgumentException("Could not find the specified value " + value.ToString().WrapQuotes() + " in the enum type " + type.Name.WrapQuotes() + ".", nameof(value));
                }

                return false;
            }

            /// <summary>
            /// Gets the names of the individual flags set in a bitfield enum.
            /// </summary>
            /// <param name="value">The bitfield enum value to get the names of.</param>
            /// <returns>Returns the names of the individual flags that are set in the specified bitfield enum value.</returns>
            /// <exception cref="ArgumentNullException">Thrown if the specified value is null.</exception>
            /// <exception cref="ArgumentException">
            /// Thrown if no names or values exist in the enum.
            /// Thrown if the specified value cannot be converted to a UInt64.
            /// Thrown if the enum is not a bitfield.
            /// Thrown if no flags were set or one or more flags in the specified value could not be converted.
            /// </exception>
            public string[]
            GetFlagNames(object value)
            {
                EnumResult enum_result = new EnumResult();

                if (!TryGetFlagNamesInternal(value, ref enum_result))
                {
                    enum_result.Throw("The names of the individual flags set in the bitfield enum value of type " + type.Name.WrapQuotes() + " could not be obtained.");
                }

                return enum_result.names;
            }

            /// <summary>
            /// Attempts to get the names of the individual flags set in a bitfield enum.
            /// </summary>
            /// <param name="value">The bitfield enum value to get the names of.</param>
            /// <param name="result">The names of the individual flags that are set in the specified bitfield enum value.</param>
            /// <returns>
            /// Returns true if all set flags could be found and were resolved into names.
            /// Returns false otherwise.
            /// </returns>
            public bool
            TryGetFlagNames(object value, out string[] result)
            {
                EnumResult enum_result = new EnumResult();

                bool success = TryGetFlagNamesInternal(value, ref enum_result);
                result = enum_result.names;

                return success;
            }

            /// <summary>
            /// Attempts to get the names of the individual flags set in a bitfield enum value.
            /// </summary>
            /// <param name="value">The bitfield enum value to get the names of.</param>
            /// <param name="enum_result">The internal enum result used for error handling.</param>
            /// <returns>
            /// Returns true if all set flags could be found and were resolved into names.
            /// Returns false otherwise.
            /// </returns>
            private bool
            TryGetFlagNamesInternal(object value, ref EnumResult enum_result)
            {
                enum_result.names = new string[0];

                if (!PassesCommonChecks(value, ref enum_result))
                {
                    return false;
                }

                if (!TryToUInt64(type_code, value, out ulong _value))
                {
                    enum_result.inner_exception = new ArgumentException("Failed to convert specified value " + value.ToString().WrapQuotes() + " to UInt64.");

                    return false;
                }

                if (!is_flags)
                {
                    enum_result.inner_exception = new ArgumentException("The enum type " + type.ToString().WrapQuotes() + " is not a bitfield enum.");

                    return false;
                }          

                IList<int> indicies = GetFlagIndicies(_value);
                if (!indicies.IsValid())
                {
                    enum_result.inner_exception = new ArgumentException("No flags were set or one or more flags in the specified value " + value.ToString().WrapQuotes() + " could not be found.");

                    return false;
                }

                List<string> _names = new List<string>();
                foreach (int index in indicies)
                {
                    _names.Add(resolved_names[index]);
                }

                enum_result.names = _names.ToArray();

                return true;
            }

            /// <summary>
            /// Gets the values of the individual flags set in a bitfield enum.
            /// </summary>
            /// <param name="value">The bitfield enum value to get the names of.</param>
            /// <returns>Returns the values of the individual flags that are set in the specified bitfield enum value.</returns>
            /// <exception cref="ArgumentNullException">Thrown if the specified value is null.</exception>
            /// <exception cref="ArgumentException">
            /// Thrown if no names or values exist in the enum.
            /// Thrown if the specified value cannot be converted to a UInt64.
            /// Thrown if the enum is not a bitfield.
            /// Thrown if no flags were set or one or more flags in the specified value could not be converted.
            /// </exception>
            public object[]
            GetFlagValues(object value)
            {
                EnumResult enum_result = new EnumResult();

                if (!TryGetFlagValuesInternal(value, ref enum_result))
                {
                    enum_result.Throw("The values of the individual flags set in the bitfield enum value of type " + type.Name.WrapQuotes() + " could not be obtained.");
                }

                return enum_result.values;
            }

            /// <summary>
            /// Attempts to get the values of the individual flags set in a bitfield enum.
            /// </summary>
            /// <param name="value">The bitfield enum value to get the names of.</param>
            /// <param name="result">The values of the individual flags that are set in the specified bitfield enum value.</param>
            /// <returns>
            /// Returns true if all set flags could be found and were resolved into values.
            /// Returns false otherwise.
            /// </returns>
            public bool
            TryGetFlagValues(object value, out object[] result)
            {
                EnumResult enum_result = new EnumResult();

                bool success = TryGetFlagValuesInternal(value, ref enum_result);
                result = enum_result.values;

                return success;
            }

            /// <summary>
            /// Attempts to get the values of the individual flags set in a bitfield enum value.
            /// </summary>
            /// <param name="value">The bitfield enum value to get the names of.</param>
            /// <param name="enum_result">The internal enum result used for error handling.</param>
            /// <returns>
            /// Returns true if all set flags could be found and were resolved into values.
            /// Returns false otherwise.
            /// </returns>
            private bool
            TryGetFlagValuesInternal(object value, ref EnumResult enum_result)
            {
                if (!PassesCommonChecks(value, ref enum_result))
                {
                    return false;
                }

                if (!TryToUInt64(type_code, value, out ulong _value))
                {
                    enum_result.inner_exception = new ArgumentException("Failed to convert specified value " + value.ToString().WrapQuotes() + " to UInt64.");

                    return false;
                }

                if (!is_flags)
                {
                    enum_result.inner_exception = new ArgumentException("The enum type " + type.ToString().WrapQuotes() + " is not a bitfield enum.");

                    return false;
                }

                IList<int> indicies = GetFlagIndicies(_value);
                if (!indicies.IsValid())
                {
                    enum_result.inner_exception = new ArgumentException("No flags were set or one or more flags in the specified value " + value.ToString().WrapQuotes() + " could not be found.");

                    return false;
                }

                List<object> _flags = new List<object>();
                foreach (int index in indicies)
                {
                    _flags.Add(Enum.ToObject(type, resolved_values[index]));
                }

                enum_result.values = _flags.ToArray();

                return true;
            }

            /// <summary>
            /// <para>Converts a string representation of an enum value into the equivalent constant enum value.</para>
            /// <para>Supports bitfield formatted strings.</para>
            /// </summary>
            /// <param name="value">
            /// <para>The string to convert.</para>
            /// <para>This can either be the resolved or native (unresolved) name.</para>
            /// </param>
            /// <returns>Returns the equivalent constant enum value of its string representation.</returns>
            /// <exception cref="ArgumentNullException">Thrown if the specified value is null.</exception>
            /// <exception cref="ArgumentException">
            /// Thrown if no names or values exist in the enum.
            /// Thrown if the specified value is not a set of flags and could not be matched by name or value.
            /// Thrown if the specified value is a set of flags and could not be matched by name, value, or individual flags.
            /// </exception>
            public object
            Parse(string value)
            {
                return Parse(value, false);
            }

            /// <summary>
            /// <para>Converts a string representation of an enum value into the equivalent constant enum value.</para>
            /// <para>Supports bitfield formatted strings.</para>
            /// </summary>
            /// <param name="value">
            /// <para>The string to convert.</para>
            /// <para>This can either be the resolved or native (unresolved) name.</para>
            /// </param>
            /// <param name="ignore_case">Whether or not to ignore case when parsing.</param>
            /// <returns>Returns the equivalent constant enum value of its string representation.</returns>
            /// <exception cref="ArgumentNullException">Thrown if the specified value is null.</exception>
            /// <exception cref="ArgumentException">
            /// Thrown if no names or values exist in the enum.
            /// Thrown if the specified value is not a set of flags and could not be matched by name or value.
            /// Thrown if the specified value is a set of flags and could not be matched by name, value, or individual flags.
            /// </exception>
            public object
            Parse(string value, bool ignore_case)
            {
                EnumResult enum_result = new EnumResult();

                if (!TryParseInternal(value, ignore_case, ref enum_result))
                {
                    enum_result.Throw("Could not parse the specified value into an enum value of type " + type.Name.WrapQuotes() + ".");
                }

                return enum_result.value;
            }

            /// <summary>
            /// <para>Attempts to convert a string representation of an enum value into the equivalent constant enum value.</para>
            /// <para>Supports bitfield formatted strings.</para>
            /// </summary>
            /// <param name="value">
            /// <para>The string to convert.</para>
            /// <para>This can either be the resolved or native (unresolved) name.</para>
            /// </param>
            /// <param name="result">
            /// Set to the equivalent constant enum value, if successful.
            /// Set to the enum's default value otherwise.
            /// </param>
            /// <returns>
            /// Returns true if the string value was successfully parsed.
            /// Returns false otherwise.
            /// </returns>
            public bool
            TryParse(string value, out object result)
            {
                return TryParse(value, false, out result);
            }

            /// <summary>
            /// <para>Attempts to convert a string representation of an enum value into the equivalent constant enum value.</para>
            /// <para>Supports bitfield formatted strings.</para>
            /// </summary>
            /// <param name="value">
            /// <para>The string to convert.</para>
            /// <para>This can either be the resolved or native (unresolved) name.</para>
            /// </param>
            /// <param name="ignore_case">Whether or not to ignore case when parsing.</param>
            /// <param name="result">
            /// Set to the equivalent constant enum value, if successful.
            /// Set to the enum's default value otherwise.
            /// </param>
            /// <returns>
            /// Returns true if the string value was successfully parsed.
            /// Returns false otherwise.
            /// </returns>
            public bool
            TryParse(string value, bool ignore_case, out object result)
            {
                EnumResult enum_result = new EnumResult();

                bool success = TryParseInternal(value, ignore_case, ref enum_result);
                result = enum_result.value;

                return success;
            }

            /// <summary>
            /// <para>Attempts to convert a string representation of an enum value into the equivalent constant enum value.</para>
            /// <para>Supports bitfield formatted strings.</para>
            /// </summary>
            /// <param name="value">
            /// <para>The string to convert.</para>
            /// <para>This can either be the resolved or native (unresolved) name.</para>
            /// </param>
            /// <param name="ignore_case">Whether or not to ignore case when parsing.</param>
            /// <param name="enum_result">
            /// Set to the equivalent constant enum value, if successful.
            /// Set to the enum's default value otherwise.
            /// </param>
            /// <returns>
            /// Returns true if the string value was successfully parsed.
            /// Returns false otherwise.
            /// </returns>
            private bool
            TryParseInternal(string value, bool ignore_case, ref EnumResult enum_result)
            {
                enum_result.value = default_value;

                if (!PassesCommonChecks(value, ref enum_result))
                {
                    return false;
                }

                StringComparison comparison = ignore_case ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal;

                // Attempt 1: Try and match the value exactly against the resolved and unresolved names.
                int index = FindValueIndexByName(value, comparison);
                if (index != -1)
                {
                    enum_result.value = Enum.ToObject(type, resolved_values[index]);

                    return true;
                }

                // Attemp 2: Check to see if it's a number, and if it is, compare it against the resolved values for a match.
                value = value.Trim();
                if (UInt64.TryParse(value, out ulong _result))
                {
                    enum_result.value = Enum.ToObject(type, _result);

                    return true;
                }

                // Specified value could not be found via name or vlaue matching. Failed parsing.
                if (!is_flags)
                {
                    enum_result.inner_exception = new ArgumentException("Failed to parse specified value " + value.WrapQuotes() + " into an enum value of type " + type.Name.WrapQuotes() + " by name and value matching.", nameof(value));

                    return false;
                }

                // Attempt 3: Parse the string as individual flags.
                ulong bitfield_result = 0;

                string[] elements = value.Split(',');
                foreach (string element in elements)
                {
                    index = FindValueIndexByName(element.Trim(), comparison);
                    if (index != -1)
                    {
                        bitfield_result |= resolved_values[index];

                        continue;
                    }
                    else
                    {
                        break;
                    }
                }

                // All individual flags found valid matches. Success.
                if (index != -1)
                {
                    enum_result.value = Enum.ToObject(type, bitfield_result);

                    return true;
                }
                // Specified value could not be parsed for individual flags or any above condition. Failed parsing.
                else
                {
                    enum_result.inner_exception = new ArgumentException("Failed to parse specified value " + value.WrapQuotes() + " into an enum value of type " + type.Name.WrapQuotes() + " by name, value, and flag matching.", nameof(value));
                }

                return false;
            }

            #endregion

            #region Helpers                        

            /// <summary>
            /// Formats the <see cref="UInt64"/> equivalent of an enum value into a bitfield formatted string.
            /// </summary>
            /// <param name="value">The equivalent of an enum value.</param>
            /// <returns>
            /// Returns the resolved bitfield formatted string if successful.
            /// Returns the resolved default enum value if no flags were set in the specified value.
            /// Returns null otherwise.
            /// </returns>
            private string
            FormatFlagsAsName(ulong value)
            {
                string result = null;

                // Case 1: The value had no set flags to begin with. "Failed" conversion.
                if (value == 0)
                {
                    return resolved_values.IsValid() && resolved_values[0] == 0 ? resolved_names[0] : result;
                }

                IList<int> indicies = GetFlagIndicies(value);
                // Case 2: At least one flag in the value could not be found in the resolved enum values and could not be fully converted. Failed/incomplete converison.
                if (!indicies.IsValid())
                {
                    return result;
                }

                bool first_flag = true;

                StringBuilder _result = new StringBuilder();
                foreach(int index in indicies)
                {
                    if (!first_flag)
                    {
                        _result.Insert(0, ", ");
                    }
                    _result.Insert(0, resolved_names[index]);

                    first_flag = false;
                }

                result = _result.ToString();

                return result;
            }

            /// <summary>
            /// Gets the flag indicies of the enum value.
            /// </summary>
            /// <param name="value">The equivalent of an enum value.</param>
            /// <returns>
            /// Returns the list of the flag indicies of the equivalent enum value.
            /// If no flags were set or not all flags could be found, the list count is 0.
            /// </returns>
            public IList<int>
            GetFlagIndicies(ulong value)
            {
                IList<int> indicies = new List<int>();

                // Case 1: The value had no set flags to begin with or is not a bitfield enum. "Failed" conversion.
                if (value == 0 || !is_flags)
                {
                    return indicies;
                }

                // Since enums are sorted, search for flags in reverse order so we don't start with "0".
                int index = resolved_values.Length - 1;
                while (index >= 0)
                {
                    // We reached the end of all possible resolved flags or the default value.
                    // All possible flags that could be accounted for have already been accounted for.
                    if (index == 0 && resolved_values[index] == 0)
                    {
                        break;
                    }

                    if ((value & resolved_values[index]) == resolved_values[index])
                    {
                        // Flip the bit to show that the flag was successfully accounted for.
                        value ^= resolved_values[index];

                        indicies.Add(index);
                    }

                    index--;
                }

                // Case 2: At least one flag in the value could not be found in the resolved enum values and could not be fully converted. Failed/incomplete converison.
                if (value != 0)
                {
                    indicies.Clear();
                }

                // Case 3: All flags in the value were found in the resolved enum values. Successful converison.
                // Do nothing.

                return indicies;
            }

            /// <summary>
            /// Finds the index of the enum value in the resolved value array that corresponds to the specified enum name.
            /// </summary>
            /// <param name="name">The resolved or native (unresolved) enum name to search for.</param>
            /// <param name="comparison">One of the enumeration values that specifies the rules to use in the comparison.</param>
            /// <returns>
            /// Returns the index of the enum value in the reolved value array if a match was found.
            /// Returns -1 otherwise.
            /// </returns>
            private int
            FindValueIndexByName(string name, StringComparison comparison)
            {
                for (int index = 0; index < resolved_names.Length; ++index)
                {
                    if (string.Compare(resolved_names[index], name, comparison) == 0)
                    {
                        return index;
                    }
                }

                for (int index = 0; index < names.Length; ++index)
                {
                    if (string.Compare(names[index], name, comparison) == 0)
                    {
                        return index;
                    }
                }

                return -1;
            }

            private bool
            PassesCommonChecks(object value, ref EnumResult enum_result)
            {
                if (value.IsNull())
                {
                    enum_result.inner_exception = new ArgumentNullException(nameof(value));

                    return false;
                }

                if (!resolved_names.IsValid())
                {
                    enum_result.inner_exception = new ArgumentException("No names exist for the enum type " + type.Name.WrapQuotes() + ".");

                    return false;
                }

                if (!resolved_values.IsValid())
                {
                    enum_result.inner_exception = new ArgumentException("No values exist for the enum type " + type.Name.WrapQuotes() + ".");

                    return false;
                }

                return true;
            }

            #endregion
        }

        #endregion

        #region Benchmarks

        public static void
        RunBenchmarks()
        {
            BenchmarkRun run1 = new BenchmarkRun();
            run1.action = Benchmark_FromStreamLanguage_Single;
            run1.iterations = 1_000_000;
            run1.name = "Benchmark_FromStreamLanguage_Single";

            BenchmarkRun run2 = new BenchmarkRun();
            run2.action = Benchmark_FromStreamLanguage_Flags;
            run2.iterations = 1_000_000;
            run2.name = "Benchmark_FromStreamLanguage_Flags";

            BenchmarkRun run3 = new BenchmarkRun();
            run3.action = Benchmark_ToStreamLanguage_Single;
            run3.iterations = 1_000_000;
            run3.name = "Benchmark_ToStreamLanguage_Single";

            Benchmark benchmark = new Benchmark();
            benchmark.Add(run1);
            benchmark.Add(run2);
            benchmark.Add(run3);
            benchmark.Execute();
        }

        private static void
        Benchmark_FromStreamLanguage_Single()
        {
            StreamLanguage language = StreamLanguage.EnGb;
            EnumTypeCache cache = CACHE.GetOrAdd(typeof(StreamLanguage), CreateCache);

            cache.TryGetName(language, out string name);
        }

        private static void
        Benchmark_FromStreamLanguage_Flags()
        {
            StreamLanguage language = StreamLanguage.EnGb | StreamLanguage.Ar | StreamLanguage.ZhTw | (StreamLanguage)(1 << 58);
            EnumTypeCache cache = CACHE.GetOrAdd(typeof(StreamLanguage), CreateCache);

            cache.TryGetName(language, out string name);
        }

        private static void
        Benchmark_ToStreamLanguage_Single()
        {
            string name = "en-gb";
            EnumTypeCache cache = CACHE.GetOrAdd(typeof(StreamLanguage), CreateCache);

            cache.TryParse(name, out object value);
            StreamLanguage language = (StreamLanguage)value;
        }

        #endregion

        #region Converters

        /// <summary>
        /// <para>Gets the name of the specified enum's value.</para>
        /// <para>Supports bitfield enum values.</para>
        /// </summary>
        /// <typeparam name="enum_type">
        /// The enum's generic type.
        /// Restricted to a struct.
        /// </typeparam>
        /// <param name="value">The enum value to get the name of.</param>
        /// <returns>Returns resolved name of the enum value.</returns>
        /// <exception cref="ArgumentException">
        /// Thrown if the type is not an enum.
        /// Thrown if no names or values exist in the enum type.
        /// Thrown if the specified value cannot be converted to a UInt64.
        /// Thrown if one or more flags in the specified value could not be converted if the enum is a bitfiled.
        /// Thrown if the specified value could not be found in the enum type.
        /// </exception>
        public static string
        GetName<enum_type>(enum_type value)
        where enum_type : struct
        {
            return GetName(typeof(enum_type), value);
        }

        /// <summary>
        /// <para>Gets the name of the specified enum's value.</para>
        /// <para>Supports bitfield enum values.</para>
        /// </summary>
        /// <param name="type">The enum's type.</param>
        /// <param name="value">The enum value to get the name of.</param>
        /// <returns>Returns resolved name of the enum value.</returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown if the type is null.
        /// Thrown if the specified value is null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// Thrown if the type is not an enum.
        /// Thrown if no names or values exist in the enum type.
        /// Thrown if the specified value cannot be converted to a UInt64.
        /// Thrown if one or more flags in the specified value could not be converted if the enum is a bitfield.
        /// Thrown if the specified value could not be found in the enum type.
        /// </exception>
        public static string
        GetName(Type type, object value)
        {
            type = type.GetTrueType();
            EnumTypeCache cache = GetOrAddCache(type);

            return cache.GetName(value);
        }

        /// <summary>
        /// <para>Attempts to get the name of the specified enum's value.</para>
        /// <para>Supports bitfield enum values.</para>
        /// </summary>
        /// <typeparam name="enum_type">
        /// The enum's generic type.
        /// Restricted to a struct.
        /// </typeparam>
        /// <param name="value">The enum value to get the name of.</param>
        /// <param name="result">
        /// Set to the resolved name, if successful.
        /// Set to null otherwise.
        /// </param>
        /// <returns>
        /// Returns true if the name was successfully retrieved.
        /// Returns false otherwise.
        /// </returns>
        public static bool
        TryGetName<enum_type>(enum_type value, out string result)
        where enum_type : struct
        {
            return TryGetName(typeof(enum_type), value, out result);
        }

        /// <summary>
        /// <para>Attempts to get the name of the specified enum's value.</para>
        /// <para>Supports bitfield enum values.</para>
        /// </summary>
        /// <param name="type">The enum's type.</param>
        /// <param name="value">The enum value to get the name of.</param>
        /// <param name="result">
        /// Set to the resolved name, if successful.
        /// Set to null otherwise.
        /// </param>
        /// <returns>
        /// Returns true if the name was successfully retrieved.
        /// Returns false otherwise.
        /// </returns>
        public static bool
        TryGetName(Type type, object value, out string result)
        {
            type = type.GetTrueType();
            if (type.IsNull() || !type.IsEnum)
            {
                result = null;

                return false;
            }

            EnumTypeCache cache = GetOrAddCache(type);

            return cache.TryGetName(value, out result);
        }

        /// <summary>
        /// Gets the names of the individual flags set in a bitfield enum.
        /// </summary>
        /// <typeparam name="enum_type">
        /// The enum's generic type.
        /// Restricted to a struct.
        /// </typeparam>
        /// <param name="value">The bitfield enum value to get the names of.</param>
        /// <returns>Returns the names of the individual flags that are set in the specified bitfield enum value</returns>
        /// <exception cref="ArgumentException">
        /// Thrown if the type is not an enum.
        /// Thrown if no names or values exist in the enum.
        /// Thrown if the specified value cannot be converted to a UInt64.
        /// Thrown if the enum is not a bitfield.
        /// Thrown if no flags were set or one or more flags in the specified value could not be converted.
        /// </exception>
        public static string[]
        GetFlagNames<enum_type>(enum_type value)
        where enum_type : struct
        {
            return GetFlagNames(typeof(enum_type), value);
        }

        /// <summary>
        /// Gets the names of the individual flags set in a bitfield enum.
        /// </summary>
        /// <param name="type">The enum's type.</param>
        /// <param name="value">The bitfield enum value to get the names of.</param>
        /// <returns>Returns the names of the individual flags that are set in the specified bitfield enum value</returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown if the type is null.
        /// Thrown if the specified value is null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// Thrown if the type is not an enum.
        /// Thrown if no names or values exist in the enum.
        /// Thrown if the specified value cannot be converted to a UInt64.
        /// Thrown if the enum is not a bitfield.
        /// Thrown if no flags were set or one or more flags in the specified value could not be converted.
        /// </exception>
        public static string[]
        GetFlagNames(Type type, object value)
        {
            type = type.GetTrueType();
            EnumTypeCache cache = GetOrAddCache(type);

            return cache.GetFlagNames(value);
        }

        /// <summary>
        /// Attempts to get the names of the individual flags set in a bitfield enum.
        /// </summary>
        /// <typeparam name="enum_type">
        /// The enum's generic type.
        /// Restricted to a struct.
        /// </typeparam>
        /// <param name="value">The bitfield enum value to get the names of.</param>
        /// <param name="result">
        /// Set to the names of the individual flags that are set in the specified bitfield enum value, if successful.
        /// Set to an empty array otherwise.
        /// </param>
        /// <returns>
        /// Returns true if all set flags could be found and were resolved into names.
        /// Returns false otherwise.
        /// </returns>
        public static bool
        TryGetFlagNames<enum_type>(enum_type value, out string[] result)
        where enum_type : struct
        {
            return TryGetFlagNames(typeof(enum_type), value, out result);
        }

        /// <summary>
        /// Attempts to get the names of the individual flags set in a bitfield enum.
        /// </summary>
        /// <param name="type">The enum's type.</param>
        /// <param name="value">The bitfield enum value to get the names of.</param>
        /// <param name="result">
        /// Set to the names of the individual flags that are set in the specified bitfield enum value, if successful.
        /// Set to an empty array otherwise.
        /// </param>
        /// <returns>
        /// Returns true if all set flags could be found and were resolved into names.
        /// Returns false otherwise.
        /// </returns>
        public static bool
        TryGetFlagNames(Type type, object value, out string[] result)
        {
            type = type.GetTrueType();
            if (type.IsNull() || !type.IsEnum || !type.HasAttribute<FlagsAttribute>())
            {
                result = new string[0];

                return false;
            }

            EnumTypeCache cache = GetOrAddCache(type);

            return cache.TryGetFlagNames(value, out result);
        }

        /// <summary>
        /// Gets the values of the individual flags set in a bitfield enum.
        /// </summary>
        /// <typeparam name="enum_type">
        /// The enum's generic type.
        /// Restricted to a struct.
        /// </typeparam>
        /// <param name="value">The bitfield enum value to get the names of.</param>
        /// <returns>The values of the individual flags that are set in the specified bitfield enum value.</returns>
        /// <exception cref="ArgumentNullException">Thrown if the specified value is null.</exception>
        /// <exception cref="ArgumentException">
        /// Thrown if the type is not an enum.
        /// Thrown if no names or values exist in the enum.
        /// Thrown if the specified value cannot be converted to a UInt64.
        /// Thrown if the enum is not a bitfield.
        /// Thrown if no flags were set or one or more flags in the specified value could not be converted.
        /// </exception>
        public static enum_type[]
        GetFlagValues<enum_type>(object value)
        where enum_type : struct
        {
            object[] _result = GetFlagValues(typeof(enum_type), value);

            enum_type[] result = new enum_type[_result.Length];
            for (int index = 0; index < result.Length; ++index)
            {
                result[index] = (enum_type)_result[index];
            }

            return result;
        }

        /// <summary>
        /// Gets the values of the individual flags set in a bitfield enum.
        /// </summary>
        /// <param name="type">The enum's type.</param>
        /// <param name="value">The bitfield enum value to get the names of.</param>
        /// <returns>The values of the individual flags that are set in the specified bitfield enum value.</returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown if the type is null.
        /// Thrown if the specified value is null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// Thrown if the type is not an enum.
        /// Thrown if no names or values exist in the enum.
        /// Thrown if the specified value cannot be converted to a UInt64.
        /// Thrown if the enum is not a bitfield.
        /// Thrown if no flags were set or one or more flags in the specified value could not be converted.
        /// </exception>
        public static object[]
        GetFlagValues(Type type, object value)
        {
            type = type.GetTrueType();
            EnumTypeCache cache = GetOrAddCache(type);

            return cache.GetFlagValues(value);
        }

        /// <summary>
        /// Attempts to get the values of the individual flags set in a bitfield enum.
        /// </summary>
        /// <typeparam name="enum_type">
        /// The enum's generic type.
        /// Restricted to a struct.
        /// </typeparam>
        /// <param name="value">The bitfield enum value to get the names of.</param>
        /// <param name="result">
        /// Set to the values of the individual flags that are set in the specified bitfield enum value, if successful.
        /// Set to an empty array otherwise.
        /// </param>
        /// <returns>
        /// Returns true if all set flags could be found and were resolved into values.
        /// Returns false otherwise.
        /// </returns>
        public static bool
        TryGetFlagValues<enum_type>(object value, out enum_type[] result)
        where enum_type : struct
        {
            bool success = TryGetFlagValues(typeof(enum_type), value, out object[] _result);

            result = new enum_type[_result.Length];
            for(int index = 0; index < result.Length; ++index)
            {
                result[index] = (enum_type)_result[index];
            }

            return success;
        }

        /// <summary>
        /// Attempts to get the values of the individual flags set in a bitfield enum.
        /// </summary>
        /// <param name="type">The enum's type.</param>
        /// <param name="value">The bitfield enum value to get the names of.</param>
        /// <param name="result">The values of the individual flags that are set in the specified bitfield enum value.</param>
        /// <returns>
        /// Returns true if all set flags could be found and were resolved into values.
        /// Returns false otherwise.
        /// </returns>
        public static bool
        TryGetFlagValues(Type type, object value, out object[] result)
        {
            type = type.GetTrueType();
            if (type.IsNull() || !type.IsEnum || !type.HasAttribute<FlagsAttribute>())
            {
                result = new object[0];

                return false;
            }

            EnumTypeCache cache = GetOrAddCache(type);

            return cache.TryGetFlagValues(value, out result);
        }

        /// <summary>
        /// <para>Converts a string representation of an enum value into the equivalent constant enum value.</para>
        /// <para>Supports bitfield formatted strings.</para>
        /// </summary>
        /// <typeparam name="enum_type">
        /// The enum's generic type.
        /// Restricted to a struct.
        /// </typeparam>
        /// <param name="value">
        /// <para>The string to convert.</para>
        /// <para>This can either be the resolved or native (unresolved) name.</para>
        /// </param>        
        /// <returns>Returns the equivalent constant enum value of its string representation.</returns>
        /// <exception cref="ArgumentNullException">Thrown if the specified value is null.</exception>
        /// <exception cref="ArgumentException">
        /// Thrown if the type is not an enum.
        /// Thrown if no names or values exist in the enum.
        /// Thrown if the specified value is not a set of flags and could not be matched by name or value.
        /// Thrown if the specified value is a set of flags and could not be matched by name, value, or individual flags.
        /// </exception>
        public static enum_type
        Parse<enum_type>(string value)
        where enum_type : struct
        {
            return (enum_type)Parse(typeof(enum_type), value);
        }

        /// <summary>
        /// <para>Converts a string representation of an enum value into the equivalent constant enum value.</para>
        /// <para>Supports bitfield formatted strings.</para>
        /// </summary>
        /// <param name="type">The enum's type.</param>
        /// <param name="value">
        /// <para>The string to convert.</para>
        /// <para>This can either be the resolved or native (unresolved) name.</para>
        /// </param>        
        /// <returns>Returns the equivalent constant enum value of its string representation.</returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown if the type is null.
        /// Thrown if the specified value is null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// Thrown if the type is not an enum.
        /// Thrown if no names or values exist in the enum.
        /// Thrown if the specified value is not a set of flags and could not be matched by name or value.
        /// Thrown if the specified value is a set of flags and could not be matched by name, value, or individual flags.
        /// </exception>
        public static object
        Parse(Type type, string value)
        {
            return Parse(type, value, false);
        }

        /// <summary>
        /// <para>Converts a string representation of an enum value into the equivalent constant enum value.</para>
        /// <para>Supports bitfield formatted strings.</para>
        /// </summary>
        /// <typeparam name="enum_type">
        /// The enum's generic type.
        /// Restricted to a struct.
        /// </typeparam>
        /// <param name="value">
        /// <para>The string to convert.</para>
        /// <para>This can either be the resolved or native (unresolved) name.</para>
        /// </param>
        /// <param name="ignore_case">Whether or not to ignore case when parsing.</param>
        /// <returns>Returns the equivalent constant enum value of its string representation.</returns>
        /// <exception cref="ArgumentNullException">Thrown if the specified value is null.</exception>
        /// <exception cref="ArgumentException">
        /// Thrown if the type is not an enum.
        /// Thrown if no names or values exist in the enum.
        /// Thrown if the specified value is not a set of flags and could not be matched by name or value.
        /// Thrown if the specified value is a set of flags and could not be matched by name, value, or individual flags.
        /// </exception>
        public static enum_type
        Parse<enum_type>(string value, bool ignore_case)
        where enum_type : struct
        {
            return (enum_type)Parse(typeof(enum_type), value, ignore_case);
        }

        /// <summary>
        /// <para>Converts a string representation of an enum value into the equivalent constant enum value.</para>
        /// <para>Supports bitfield formatted strings.</para>
        /// </summary>
        /// <param name="type">The enum's type.</param>
        /// <param name="value">
        /// <para>The string to convert.</para>
        /// <para>This can either be the resolved or native (unresolved) name.</para>
        /// </param>
        /// <param name="ignore_case">Whether or not to ignore case when parsing.</param>
        /// <returns>Returns the equivalent constant enum value of its string representation.</returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown if the type is null.
        /// Thrown if the specified value is null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// Thrown if the type is not an enum.
        /// Thrown if no names or values exist in the enum.
        /// Thrown if the specified value is not a set of flags and could not be matched by name or value.
        /// Thrown if the specified value is a set of flags and could not be matched by name, value, or individual flags.
        /// </exception>
        public static object
        Parse(Type type, string value, bool ignore_case)
        {
            type = type.GetTrueType();
            EnumTypeCache cache = GetOrAddCache(type);

            return cache.Parse(value, ignore_case);
        }

        /// <summary>
        /// <para>Attempts to convert a string representation of an enum value into the equivalent constant enum value.</para>
        /// <para>Supports bitfield formatted strings.</para>
        /// </summary>
        /// <typeparam name="enum_type">
        /// The enum's generic type.
        /// Restricted to a struct.
        /// </typeparam>
        /// <param name="value">
        /// <para>The string to convert.</para>
        /// <para>This can either be the resolved or native (unresolved) name.</para>
        /// </param>
        /// <param name="result">
        /// Set to null if the type is null or if the type is not an enum.
        /// Set to the equivalent constant enum value if the conversion was successful.
        /// Set to the enum's default value otherwise.
        /// </param>
        /// <returns>
        /// Returns true if the string value was successfully parsed.
        /// Returns false otherwise.
        /// </returns>
        public static bool
        TryParse<enum_type>(string value, out enum_type result)
        where enum_type : struct
        {
            bool success = TryParse(typeof(enum_type), value, out object _result);
            result = (enum_type)_result;

            return success;
        }

        /// <summary>
        /// <para>Attempts to convert a string representation of an enum value into the equivalent constant enum value.</para>
        /// <para>Supports bitfield formatted strings.</para>
        /// </summary>
        /// <param name="type">The enum's type.</param>
        /// <param name="value">
        /// <para>The string to convert.</para>
        /// <para>This can either be the resolved or native (unresolved) name.</para>
        /// </param>
        /// <param name="result">
        /// Set to null if the type is null or if the type is not an enum.
        /// Set to the equivalent constant enum value if the conversion was successful.
        /// Set to the enum's default value otherwise.
        /// </param>
        /// <returns>
        /// Returns true if the string value was successfully parsed.
        /// Returns false otherwise.
        /// </returns>
        public static bool
        TryParse(Type type, string value, out object result)
        {
            return TryParse(type, value, false, out result);
        }

        /// <summary>
        /// <para>Attempts to convert a string representation of an enum value into the equivalent constant enum value.</para>
        /// <para>Supports bitfield formatted strings.</para>
        /// </summary>
        /// <typeparam name="enum_type">
        /// The enum's generic type.
        /// Restricted to a struct.
        /// </typeparam>
        /// <param name="value">
        /// <para>The string to convert.</para>
        /// <para>This can either be the resolved or native (unresolved) name.</para>
        /// </param>
        /// <param name="ignore_case">Whether or not to ignore case when parsing.</param>
        /// <param name="result">
        /// Set to null if the type is null or if the type is not an enum.
        /// Set to the equivalent constant enum value if the conversion was successful.
        /// Set to the enum's default value otherwise.
        /// </param>
        /// <returns>
        /// Returns true if the string value was successfully parsed.
        /// Returns false otherwise.
        /// </returns>
        public static bool
        TryParse<enum_type>(string value, bool ignore_case, out enum_type result)
        where enum_type : struct
        {
            bool success = TryParse(typeof(enum_type), value, ignore_case, out object _result);
            result = (enum_type)_result;

            return success;
        }

        /// <summary>
        /// <para>Attempts to convert a string representation of an enum value into the equivalent constant enum value.</para>
        /// <para>Supports bitfield formatted strings.</para>
        /// </summary>
        /// <param name="type">The enum's type.</param>
        /// <param name="value">
        /// <para>The string to convert.</para>
        /// <para>This can either be the resolved or native (unresolved) name.</para>
        /// </param>
        /// <param name="ignore_case">Whether or not to ignore case when parsing.</param>
        /// <param name="result">
        /// Set to null if the type is null or if the type is not an enum.
        /// Set to the equivalent constant enum value if the conversion was successful.
        /// Set to the enum's default value otherwise.
        /// </param>
        /// <returns>
        /// Returns true if the string value was successfully parsed.
        /// Returns false otherwise.
        /// </returns>
        public static bool
        TryParse(Type type, string value, bool ignore_case, out object result)
        {
            type = type.GetTrueType();
            if (type.IsNull() || !type.IsEnum)
            {
                result = type.GetDefaultValue();

                return false;
            }

            EnumTypeCache cache = GetOrAddCache(type);

            return cache.TryParse(value, ignore_case, out result);
        }

        /// <summary>
        /// Gets the enum cache for the specified type.
        /// If no cache exists for the specified enum type, one is created.
        /// </summary>
        /// <typeparam name="enum_type">
        /// The enum's type as a generic parameter.
        /// Restricted to a struct.
        /// </typeparam>
        /// <returns></returns>
        /// <exception cref="ArgumentException">Thrown if the <typeparamref name="enum_type"/> is not an enum.</exception>
        public static EnumTypeCache
        GetOrAddCache<enum_type>()
        where enum_type : struct
        {
            EnumTypeCache cache = GetOrAddCache(typeof(enum_type));

            return cache;
        }

        /// <summary>
        /// Gets the enum cache for the specified type.
        /// If no cache exists for the specified enum type, one is created.
        /// </summary>
        /// <param name="type">The enum's type.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">Thrown if the <paramref name="type"/> is null.</exception>
        /// <exception cref="ArgumentException">Thrown if the <paramref name="type"/> is not an enum.</exception>
        public static EnumTypeCache
        GetOrAddCache(Type type)
        {
            EnumTypeCache cache = CACHE.GetOrAdd(type, CreateCache);

            return cache;
        }

        #endregion

        #region Helpers        

        /// <summary>
        /// Creates a cache for the specified enum type and adds it to the master cache.
        /// </summary>
        /// <param name="type">The enum type.</param>
        /// <returns>Returns the cache for the enum type</returns>
        /// <exception cref="ArgumentNullException">Thrown if the <paramref name="type"/> is null.</exception>
        /// <exception cref="ArgumentException">
        /// Thrown if the <paramref name="type"/> is not an enum.
        /// </exception>
        private static EnumTypeCache
        CreateCache(Type type)
        {
            return new EnumTypeCache(type);
        }

        /// <summary>
        /// Converts an any of the valid <see cref="Enum"/> types to a <see cref="UInt64"/>.
        /// </summary>
        /// <param name="code">The type to convert the object to.</param>
        /// <param name="value">The object to convert.</param>
        /// <param name="result">The <see cref="UInt64"/> equivalent of the specified value.</param>
        /// <returns>
        /// Returns true if the specified value was successfully converted.
        /// Returns false otherwise.
        /// </returns>
        private static bool
        TryToUInt64(TypeCode code, object value, out ulong result)
        {
            result = 0;

            // Any negative numbers will be wrapped.
            switch (code)
            {
                case TypeCode.SByte:
                {
                    result = (ulong)(sbyte)value;

                    return true;
                }

                case TypeCode.Byte:
                {
                    result = (byte)value;

                    return true;
                }

                case TypeCode.Boolean:
                {
                    result = Convert.ToByte((bool)value);

                    return true;
                }

                case TypeCode.Int16:
                {
                    result = (ulong)(short)value;

                    return true;
                }

                case TypeCode.UInt16:
                {
                    result = (ushort)value;

                    return true;
                }

                case TypeCode.Char:
                {
                    result = (char)value;

                    return true;
                }

                case TypeCode.UInt32:
                {
                    result = (uint)value;

                    return true;
                }

                case TypeCode.Int32:
                {
                    result = (ulong)(int)value;

                    return true;
                }

                case TypeCode.UInt64:
                {
                    result = (ulong)value;

                    return true;
                }

                case TypeCode.Int64:
                {
                    result = (ulong)(long)value;

                    return true;
                }
            }

            return false;
        }

        #endregion
    }
}