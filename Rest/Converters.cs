using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TwitchNet.Extensions;
using TwitchNet.Helpers.Json;
using TwitchNet.Utilities;

namespace TwitchNet.Rest
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
        /// <param name="request">
        /// <para>The request to be executed.</para>
        /// <para>The request will never be null and will always be instantiated.</para>
        /// </param>
        /// <param name="paramerter">The rest parameter to be converted.</param>
        /// <param name="member_type">
        /// <para>The type of the member marked with <see cref="RestParameterConverter"/>.</para>
        /// <para>
        /// If the type of the member is nullable, this type is the underlying type.
        /// The member type will never be null and always be instantiated.
        /// </para>
        /// </param>
        /// <returns>Returns the rest request to be executed.</returns>
        public override void
        AddParameter(RestRequest request, RestParameter parameter, Type member_type)
        {
            request.json_serialzier = new JsonSerializer();
            request.SetBody(parameter.value);
        }

        /// <summary>
        /// Determines if the member marked with <see cref="RestParameterConverter"/> can be converted.
        /// </summary>
        /// <param name="paramerter">The rest parameter to be converted.</param>
        /// <param name="member_type">
        /// <para>The type of the member marked with <see cref="RestParameterConverter"/>.</para>
        /// <para>
        /// If the type of the member is nullable, this type is the underlying type.
        /// The member type will never be null and always be instantiated.
        /// </para>
        /// </param>
        /// <returns>
        /// Returns true if the <see cref="Parameter.Type"/> is equal to <see cref="ParameterType.RequestBody"/>.
        /// Returns false otherwise.
        /// </returns>
        public override bool
        CanConvert(RestParameter parameter, Type member_type)
        {
            return parameter.type == HttpParameterType.Body;
        }
    }

    #endregion

    #region Query converters

    /// <summary>
    /// The default converter for query string parameters.
    /// Adds value types and strings to the <see cref="RestRequest"/> as query parameters.
    /// </summary>
    public class
    DefaultQueryConverter : RestParameterConverter
    {
        /// <summary>
        /// Adds value types and strings to the <see cref="RestRequest"/> as query parameters.
        /// </summary>
        /// <param name="request">
        /// <para>The request to be executed.</para>
        /// <para>The request will never be null and will always be instantiated.</para>
        /// </param>
        /// <param name="paramerter">The rest parameter to be converted.</param>
        /// <param name="member_type">
        /// <para>The type of the member marked with <see cref="RestParameterConverter"/>.</para>
        /// <para>
        /// If the type of the member is nullable, this type is the underlying type.
        /// The member type will never be null and always be instantiated.
        /// </para>
        /// </param>
        /// <returns>Returns the rest request to be executed.</returns>
        public override void
        AddParameter(RestRequest request, RestParameter parameter, Type member_type)
        {
            if (!parameter.name.IsValid() || parameter.value.IsNull())
            {
                return;
            }

            string result = string.Empty;
            if (member_type.IsEnum)
            {
                EnumUtil.TryGetName(member_type, parameter.value, out result);
            }
            else
            {
                result = parameter.value.ToString();
            }

            if (!result.IsValid())
            {
                return;
            }

            request.AddQueryParameter(parameter.name, result);

            return;
        }

        /// <summary>
        /// Determines if the member marked with <see cref="RestParameterConverter"/> can be converted.
        /// </summary>
        /// <param name="paramerter">The rest parameter to be converted.</param>
        /// <param name="member_type">
        /// <para>The type of the member marked with <see cref="RestParameterConverter"/>.</para>
        /// <para>
        /// If the type of the member is nullable, this type is the underlying type.
        /// The member type will never be null and always be instantiated.
        /// </para>
        /// </param>
        /// <returns>
        /// Returns true if the <see cref="Parameter.Type"/> is equal to <see cref="ParameterType.QueryString"/> and the member is a value type, enum, or string.
        /// Returns false otherwise.
        /// </returns>
        public override bool
        CanConvert(RestParameter parameter, Type member_type)
        {
            return parameter.type == HttpParameterType.Query && (member_type.IsEnum || member_type.IsValueType || member_type == typeof(string));
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
    /// <para>By default, the  delineator is null and needs to set before the converter can be used.</para>
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
        /// <param name="request">
        /// <para>The request to be executed.</para>
        /// <para>The request will never be null and will always be instantiated.</para>
        /// </param>
        /// <param name="paramerter">The rest parameter to be converted.</param>
        /// <param name="member_type">
        /// <para>The type of the member marked with <see cref="RestParameterConverter"/>.</para>
        /// <para>
        /// If the type of the member is nullable, this type is the underlying type.
        /// The member type will never be null and always be instantiated.
        /// </para>
        /// </param>
        /// <returns>Returns the rest request to be executed.</returns>
        public override void
        AddParameter(RestRequest request, RestParameter parameter, Type member_type)
        {
            if (!parameter.name.IsValid() || parameter.value.IsNull())
            {
                return;
            }

            if (delineator.IsNull())
            {
                return;
            }

            if (member_type.IsEnum)
            {
                if (!EnumUtil.TryGetFlagNames(member_type, parameter.value, out string[] names))
                {
                    return;
                }

                if (!names.IsValid())
                {
                    return;
                }

                string value = string.Join(delineator, names);
                request.AddQueryParameter(parameter.name, value);
            }
            else
            {
                IList list = parameter.value as IList;
                if (list.Count == 0)
                {
                    return;
                }

                Type element_type = member_type.IsList() ? member_type.GetGenericArguments()[0] : member_type.GetElementType();
                if (!element_type.IsValueType && element_type != typeof(string) && !element_type.IsEnum)
                {
                    return;
                }

                if (member_type.IsEnum)
                {
                    List<string> temp = new List<string>();

                    bool is_flags = member_type.HasAttribute<FlagsAttribute>();

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
                    if (!value.IsValid())
                    {
                        return;
                    }

                    request.AddQueryParameter(parameter.name, value);
                }
                else
                {
                    string value = string.Join(delineator, list);
                    if (!value.IsValid())
                    {
                        return;
                    }

                    request.AddQueryParameter(parameter.name, value);
                }
            }
        }

        /// <summary>
        /// Determines if the member marked with <see cref="RestParameterConverter"/> can be converted.
        /// </summary>
        /// <param name="paramerter">The rest parameter to be converted.</param>
        /// <param name="member_type">
        /// <para>The type of the member marked with <see cref="RestParameterConverter"/>.</para>
        /// <para>
        /// If the type of the member is nullable, this type is the underlying type.
        /// The member type will never be null and always be instantiated.
        /// </para>
        /// </param>
        /// <returns>
        /// Returns true if the <see cref="Parameter.Type"/> is equal to <see cref="ParameterType.QueryString"/> and the member is an array/list of value types or a bitfield enum.
        /// Returns false otherwise.
        /// </returns>
        public override bool
        CanConvert(RestParameter parameter, Type member_type)
        {
            return parameter.type == HttpParameterType.Query && (member_type.IsArray || member_type.IsList() || (member_type.IsEnum && member_type.HasAttribute<FlagsAttribute>()));
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
        /// <param name="request">
        /// <para>The request to be executed.</para>
        /// <para>The request will never be null and will always be instantiated.</para>
        /// </param>
        /// <param name="paramerter">The rest parameter to be converted.</param>
        /// <param name="member_type">
        /// <para>The type of the member marked with <see cref="RestParameterConverter"/>.</para>
        /// <para>
        /// If the type of the member is nullable, this type is the underlying type.
        /// The member type will never be null and always be instantiated.
        /// </para>
        /// </param>
        /// <returns>Returns the rest request to be executed.</returns>
        public override void
        AddParameter(RestRequest request, RestParameter parameter, Type member_type)
        {
            if (!parameter.name.IsValid() || parameter.value.IsNull())
            {
                return;
            }

            if (member_type.IsEnum)
            {
                if (!EnumUtil.TryGetFlagNames(member_type, parameter.value, out string[] names))
                {
                    return;
                }

                foreach (string element in names)
                {
                    if (!element.IsValid())
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

                Type element_type = member_type.IsList() ? member_type.GetGenericArguments()[0] : member_type.GetElementType();
                if (!element_type.IsValueType && element_type != typeof(string) && !element_type.IsEnum)
                {
                    return;
                }

                if (member_type.IsEnum)
                {
                    bool is_flags = member_type.HasAttribute<FlagsAttribute>();

                    foreach (object element in list)
                    {
                        if (element.IsNull())
                        {
                            continue;
                        }

                        if (is_flags)
                        {
                            parameter.value = element;
                            AddParameter(request, parameter, element_type);
                        }
                        else if (EnumUtil.TryGetName(element_type, element, out string value) && value.IsValid())
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

                        string value = element.ToString();
                        if (!value.IsValid())
                        {
                            continue;
                        }

                        request.AddQueryParameter(parameter.name, value);
                    }
                }
            }
        }

        /// <summary>
        /// Determines if the member marked with <see cref="RestParameterConverter"/> can be converted.
        /// </summary>
        /// <param name="paramerter">The rest parameter to be converted.</param>
        /// <param name="member_type">
        /// <para>The type of the member marked with <see cref="RestParameterConverter"/>.</para>
        /// <para>
        /// If the type of the member is nullable, this type is the underlying type.
        /// The member type will never be null and always be instantiated.
        /// </para>
        /// </param>
        /// <returns>
        /// Returns true if the <see cref="Parameter.Type"/> is equal to <see cref="ParameterType.QueryString"/> and the member is an array/list of value types or a bitfield enum.
        /// Returns false otherwise.
        /// </returns>
        public override bool
        CanConvert(RestParameter parameter, Type member_type)
        {
            return parameter.type == HttpParameterType.Query && (member_type.IsArray || member_type.IsList() || (member_type.IsEnum && member_type.HasAttribute<FlagsAttribute>()));
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
        /// <param name="request">
        /// <para>The request to be executed.</para>
        /// <para>The request will never be null and will always be instantiated.</para>
        /// </param>
        /// <param name="paramerter">The rest parameter to be converted.</param>
        /// <param name="member_type">
        /// <para>The type of the member marked with <see cref="RestParameterConverter"/>.</para>
        /// <para>
        /// If the type of the member is nullable, this type is the underlying type.
        /// The member type will never be null and always be instantiated.
        /// </para>
        /// </param>
        /// <returns>Returns the rest request to be executed.</returns>
        public override void
        AddParameter(RestRequest request, RestParameter parameter, Type member_type)
        {
            if (!parameter.name.IsValid() || parameter.value.IsNull())
            {
                return;
            }

            string rfc_3339 = ((DateTime)parameter.value).ToString("yyyy-MM-dd'T'HH:mm:ss.fffzzz", DateTimeFormatInfo.InvariantInfo);

            request.AddQueryParameter(parameter.name, rfc_3339);
        }

        /// <summary>
        /// Determines if the member marked with <see cref="RestParameterConverter"/> can be converted.
        /// </summary>
        /// <param name="paramerter">The rest parameter to be converted.</param>
        /// <param name="member_type">
        /// <para>The type of the member marked with <see cref="RestParameterConverter"/>.</para>
        /// <para>
        /// If the type of the member is nullable, this type is the underlying type.
        /// The member type will never be null and always be instantiated.
        /// </para>
        /// </param>
        /// <returns>
        /// Returns true if the <see cref="Parameter.Type"/> is equal to <see cref="ParameterType.QueryString"/> and the member is a <see cref="DateTime"/> value.
        /// Returns false otherwise.
        /// </returns>
        public override bool
        CanConvert(RestParameter parameter, Type member_type)
        {
            return parameter.type == HttpParameterType.Query && member_type == typeof(DateTime);
        }
    }

    #endregion

    public abstract class
    RestParameterConverter
    {
        /// <summary>
        /// Converts the value of a member marked with <see cref="RestParameterAttribute"/> and adds it to the <see cref="RestRequest"/>.
        /// </summary>
        /// <param name="request">
        /// <para>The request to be executed.</para>
        /// <para>The request will never be null and will always be instantiated.</para>
        /// </param>
        /// <param name="paramerter">The rest parameter to be converted.</param>
        /// <param name="member_type">
        /// <para>The type of the member marked with <see cref="RestParameterConverter"/>.</para>
        /// <para>
        /// If the type of the member is nullable, this type is the underlying type.
        /// The member type will never be null and always be instantiated.
        /// </para>
        /// </param>
        /// <returns>Returns the rest request to be executed.</returns>
        public abstract void
        AddParameter(RestRequest request, RestParameter paramerter, Type member_type);

        /// <summary>
        /// Determines if the member marked with <see cref="RestParameterConverter"/> can be converted.
        /// </summary>
        /// <param name="paramerter">The rest parameter to be converted.</param>
        /// <param name="member_type">
        /// <para>The type of the member marked with <see cref="RestParameterConverter"/>.</para>
        /// <para>
        /// If the type of the member is nullable, this type is the underlying type.
        /// The member type will never be null and always be instantiated.
        /// </para>
        /// </param>
        /// <returns>
        /// Returns true if the member can be converted.
        /// Returns false otherwise.
        /// </returns>
        public abstract bool
        CanConvert(RestParameter paramerter, Type member_type);
    }
}

