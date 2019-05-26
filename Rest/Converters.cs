// standard namespaces
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Globalization;

// project namespaces
using TwitchNet.Extensions;
using TwitchNet.Helpers.Json;
using TwitchNet.Utilities;

namespace
TwitchNet.Rest
{
    #region Converter cache

    public static class
    RestConverterCache
    {
        internal static readonly ConcurrentDictionary<Type, RestParameterConverter> CONVERTER_CACHE = new ConcurrentDictionary<Type, RestParameterConverter>();

        public static RestParameterConverter
        GetOradd(Type type)
        {
            if (!typeof(RestParameterConverter).IsAssignableFrom(type))
            {
                return null;
            }

            RestParameterConverter converter = CONVERTER_CACHE.GetOrAdd(type, AddRestConverter);

            return converter;
        }

        private static RestParameterConverter
        AddRestConverter(Type type)
        {
            return (RestParameterConverter)Activator.CreateInstance(type);
        }
    }

    #endregion

    #region Body converters

    /// <summary>
    /// The default converter for request body parameters.
    /// Adds the value of serializable object as a Json body to a <see cref="RestRequest"/>.
    /// </summary>
    public class
    DefaultBodyConverter : RestParameterConverter
    {
        /// <summary>
        /// Adds the value of serializable object as a Json body to a <see cref="RestRequest"/>.
        /// </summary>
        /// <param name="request">The request to be executed.</param>
        /// <param name="paramerter">The rest parameter to be converted and added.</param>
        public override void
        AddParameter(RestRequest request, RestParameter parameter)
        {
            if (parameter.value.IsNull())
            {
                return;
            }

            request.json_serialzier = new JsonSerializer();
            request.SetBody(parameter.value);
        }

        /// <summary>
        /// Determines if the member marked with <see cref="RestParameterConverter"/> can be converted.
        /// </summary>
        /// <param name="paramerter">The rest parameter to be converted and added.</param>
        /// Returns false if the parameter type is not <see cref="HttpParameterType.Body"/>.
        /// Returns false if the parameter value is null.
        /// Returns true otherwise.
        public override bool
        CanConvert(RestParameter parameter)
        {
            if (parameter.parameter_type != HttpParameterType.Body)
            {
                return false;
            }

            if (parameter.value.IsNull())
            {
                return false;
            }

            return true;
        }
    }

    #endregion

    #region Query converters

    /// <summary>
    /// The default converter for query string parameters.
    /// Adds value types and strings to the <see cref="RestRequest"/> as a query parameter.
    /// </summary>
    public class
    DefaultQueryConverter : RestParameterConverter
    {
        /// <summary>
        /// Adds value types and strings to the <see cref="RestRequest"/> as a query parameter.
        /// </summary>
        /// <param name="request">The request to be executed.</param>
        /// <param name="paramerter">The rest parameter to be converted and added.</param>
        public override void
        AddParameter(RestRequest request, RestParameter parameter)
        {
            string result = string.Empty;
            if (parameter.member_type.IsEnum)
            {
                if (!EnumUtil.TryGetName(parameter.member_type, parameter.value, out result))
                {
                    return;
                }
            }
            else
            {
                result = parameter.value.ToString();
            }

            request.AddQueryParameter(parameter.name, result);

            return;
        }

        /// <summary>
        /// Determines if the member marked with <see cref="RestParameterConverter"/> can be converted.
        /// </summary>
        /// <param name="paramerter">The rest parameter to be converted and added.</param>
        /// <returns>
        /// Returns false if the parameter type is not <see cref="HttpParameterType.Query"/>.
        /// Returns false if the parameter name is null, empty, or only contains whitespace.
        /// Returns false if the parameter value is null.
        /// Returns false if the member type is not a value type, enum, or string.
        /// Returns true otherwise.
        /// </returns>
        public override bool
        CanConvert(RestParameter parameter)
        {
            if (parameter.parameter_type != HttpParameterType.Query)
            {
                return false;
            }

            if (!parameter.name.IsValid() || parameter.value.IsNull())
            {
                return false;
            }

            if (!parameter.member_type.IsValueType && !parameter.member_type.IsEnum && parameter.member_type != typeof(string))
            {
                return false;
            }

            return true;
        }
    }

    /// <summary>
    /// Converts the elements of an array/list or the set flags of a bitfield enum into a space deliniated string.
    /// The delineated string is then added to the <see cref="RestRequest"/> as a single query parameter.
    /// </summary>
    public class
    SpaceDelineatedQueryConverter : DelineatedQueryConverter
    {
        public SpaceDelineatedQueryConverter()
        {
            delineator = " ";
        }
    }

    /// <summary>
    /// <para>
    /// Converts the elements of an array/list or the set flags of a bitfield enum into a deliniated string.
    /// The delineated string is then added to the <see cref="RestRequest"/> as a single query parameter.
    /// </para>
    /// <para>By default, the delineator is empty.</para>
    /// </summary>
    public class
    DelineatedQueryConverter : RestParameterConverter
    {
        /// <summary>
        /// The query parameter value delineator (separator).
        /// </summary>
        protected string delineator = string.Empty;

        /// <summary>
        /// Converts the elements of an array/list or the set flags of a bitfield enum into a deliniated string.
        /// The delineated string is then added to the <see cref="RestRequest"/> as a single query parameter.
        /// </summary>
        /// <param name="request">The request to be executed.</param>
        /// <param name="paramerter">The rest parameter to be converted and added.</param>
        public override void
        AddParameter(RestRequest request, RestParameter parameter)
        {
            if (delineator.IsNull())
            {
                return;
            }

            if (parameter.member_type.IsEnum)
            {
                if (!EnumUtil.TryGetFlagNames(parameter.member_type, parameter.value, out string[] names))
                {
                    return;
                }

                string value = string.Join(delineator, names);
                request.AddQueryParameter(parameter.name, value);
            }
            else
            {
                IList list = parameter.value as IList;
                if (list.IsNull() || list.Count == 0)
                {
                    return;
                }

                Type element_type = parameter.member_type.IsList() ? parameter.member_type.GetGenericArguments()[0] : parameter.member_type.GetElementType();
                if (!element_type.IsValueType && element_type != typeof(string) && !element_type.IsEnum)
                {
                    return;
                }

                if (parameter.member_type.IsEnum)
                {
                    List<string> temp = new List<string>();

                    bool is_flags = parameter.member_type.HasAttribute<FlagsAttribute>();

                    foreach (object element in temp)
                    {
                        if (element.IsNull())
                        {
                            continue;
                        }

                        if (is_flags)
                        {
                            if (EnumUtil.TryGetFlagNames(element_type, element, out string[] names) && names.IsValid())
                            {
                                temp.AddRange(names);
                            }
                        }
                        else if (EnumUtil.TryGetName(element_type, element, out string _name) && _name.IsValid())
                        {
                            temp.Add(_name);
                        }
                    }

                    if (!temp.IsValid())
                    {
                        return;
                    }

                    string value = string.Join(delineator, temp);
                    request.AddQueryParameter(parameter.name, value);
                }
                else
                {
                    string value = string.Join(delineator, list);
                    request.AddQueryParameter(parameter.name, value);
                }
            }
        }

        /// <summary>
        /// Determines if the member marked with <see cref="RestParameterConverter"/> can be converted.
        /// </summary>
        /// <param name="paramerter">The rest parameter to be converted and added.</param>
        /// <returns>
        /// Returns false if the parameter type is not <see cref="HttpParameterType.Query"/>.
        /// Returns false if the parameter name is null, empty, or only contains whitespace.
        /// Returns false if the parameter value is null.
        /// Returns false if the member type is not an array/list of value types or a bitfield enum.
        /// Returns true otherwise.
        /// </returns>
        public override bool
        CanConvert(RestParameter parameter)
        {
            if (parameter.parameter_type != HttpParameterType.Query)
            {
                return false;
            }

            if (!parameter.name.IsValid() || parameter.value.IsNull())
            {
                return false;
            }

            if (!parameter.member_type.IsArray && !parameter.member_type.IsList() && !(parameter.member_type.IsEnum && parameter.member_type.HasAttribute<FlagsAttribute>()))
            {
                return false;
            }

            return true;
        }
    }

    /// <summary>
    /// Adds the elements of an array/list or the set flags of a bitfield enum to the <see cref="RestRequest"/> as separate query parameters.
    /// </summary>
    public class
    SeparateQueryConverter : RestParameterConverter
    {
        /// <summary>
        /// Adds the elements of an array/list or the set flags of a bitfield enum to the <see cref="RestRequest"/> as separate query parameters.
        /// </summary>
        /// <param name="request">The request to be executed.</param>
        /// <param name="paramerter">The rest parameter to be converted and added.</param>
        public override void
        AddParameter(RestRequest request, RestParameter parameter)
        {
            if (parameter.member_type.IsEnum)
            {
                if (!EnumUtil.TryGetFlagNames(parameter.member_type, parameter.value, out string[] names))
                {
                    return;
                }

                foreach (string element in names)
                {
                    if (element.IsNull())
                    {
                        continue;
                    }

                    request.AddQueryParameter(parameter.name, element);
                }
            }
            else
            {
                IList list = parameter.value as IList;
                if (list.IsNull() || list.Count == 0)
                {
                    return;
                }

                // Make sure the collection are value types or strings.
                Type element_type = parameter.member_type.IsList() ? parameter.member_type.GetGenericArguments()[0] : parameter.member_type.GetElementType();
                if (!element_type.IsValueType && element_type != typeof(string) && !element_type.IsEnum)
                {
                    return;
                }

                if (parameter.member_type.IsEnum)
                {
                    bool is_flags = parameter.member_type.HasAttribute<FlagsAttribute>();

                    foreach (object element in list)
                    {
                        if (element.IsNull())
                        {
                            continue;
                        }

                        if (is_flags)
                        {
                            parameter.value = element;
                            AddParameter(request, parameter);
                        }
                        else if (EnumUtil.TryGetName(element_type, element, out string value) && !value.IsNull())
                        {
                            request.AddQueryParameter(parameter.name, value);
                        }
                    }
                }
                else
                {
                    foreach (object element in list)
                    {
                        if (element.IsNull())
                        {
                            continue;
                        }

                        request.AddQueryParameter(parameter.name, element.ToString());
                    }
                }
            }
        }

        /// <summary>
        /// Determines if the member marked with <see cref="RestParameterConverter"/> can be converted.
        /// </summary>
        /// <param name="paramerter">The rest parameter to be converted and added.</param>
        /// <returns>
        /// Returns false if the parameter type is not <see cref="HttpParameterType.Query"/>.
        /// Returns false if the parameter name is null, empty, or only contains whitespace.
        /// Returns false if the parameter value is null.
        /// Returns false if the member type is not an array/list of value types or a bitfield enum.
        /// Returns true otherwise.
        /// </returns>
        public override bool
        CanConvert(RestParameter parameter)
        {
            if (parameter.parameter_type != HttpParameterType.Query)
            {
                return false;
            }

            if (!parameter.name.IsValid() || parameter.value.IsNull())
            {
                return false;
            }

            if (!parameter.member_type.IsArray && !parameter.member_type.IsList() && !(parameter.member_type.IsEnum && parameter.member_type.HasAttribute<FlagsAttribute>()))
            {
                return false;
            }

            return true;
        }
    }

    /// <summary>
    /// Converts a <see cref="DateTime"/> value to a RFC 3339 compliant string and adds it to the <see cref="RestRequest"/> as a query parameter.
    /// </summary>
    public class
    RFC3339QueryConverter : RestParameterConverter
    {
        /// <summary>
        /// Converts a <see cref="DateTime"/> value to a RFC 3339 compliant string and adds it to the <see cref="RestRequest"/> as a query parameter.
        /// </summary>
        /// <param name="request">The request to be executed.</param>
        /// <param name="paramerter">The rest parameter to be converted and added.</param>
        public override void
        AddParameter(RestRequest request, RestParameter parameter)
        {
            // We don't need to special case encode '+' here since it's taken care of when the request is built.
            string rfc_3339 = ((DateTime)parameter.value).ToString("yyyy-MM-dd'T'HH:mm:ss.fffzzz", DateTimeFormatInfo.InvariantInfo);

            request.AddQueryParameter(parameter.name, rfc_3339);
        }

        /// <summary>
        /// Determines if the member marked with <see cref="RestParameterConverter"/> can be converted.
        /// </summary>
        /// <param name="paramerter">The rest parameter to be converted and added.</param>
        /// <returns>
        /// Returns false if the parameter type is not <see cref="HttpParameterType.Query"/>.
        /// Returns false if the parameter name is null, empty, or only contains whitespace.
        /// Returns false if the parameter value is null.
        /// Returns false if the member type is not <see cref="DateTime"/>.
        /// Returns true otherwise.
        /// </returns>
        public override bool
        CanConvert(RestParameter parameter)
        {
            if (parameter.parameter_type != HttpParameterType.Query)
            {
                return false;
            }

            if (!parameter.name.IsValid() || parameter.value.IsNull())
            {
                return false;
            }

            if (parameter.member_type != typeof(DateTime))
            {
                return false;
            }

            return true;
        }
    }

    #endregion

    #region Abstract converter

    public abstract class
    RestParameterConverter
    {
        /// <summary>
        /// Converts the value of a member marked with <see cref="RestParameterAttribute"/> and adds it to the <see cref="RestRequest"/>.
        /// </summary>
        /// <param name="request">The request to be executed.</param>
        /// <param name="paramerter">The rest parameter to be converted and added.</param>
        public abstract void
        AddParameter(RestRequest request, RestParameter paramerter);

        /// <summary>
        /// Determines if the member marked with <see cref="RestParameterConverter"/> can be converted.
        /// The intended use for this is primarily for universal checks.
        /// </summary>
        /// <param name="paramerter">The rest parameter to be converted and added.</param>
        /// <returns>
        /// Returns true if the parameter can be converted.
        /// Returns false otherwise.
        /// </returns>
        public abstract bool
        CanConvert(RestParameter paramerter);
    }

    #endregion
}

