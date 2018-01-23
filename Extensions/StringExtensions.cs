// project namespaces
using System;
using System.Collections.Generic;

using TwitchNet.Extensions;

namespace
TwitchNet.Extensions
{
    internal static class
    StringExtensions
    {
        #region String parsing

        /// <summary>
        /// Gets the text after a sub <see cref="char"/> in a <see cref="string"/>.
        /// </summary>
        /// <param name="str">The <see cref="string"/> to be parsed.</param>
        /// <param name="sub_char_from">The <see cref="char"/> to parse from.</param>
        /// <param name="start_index">How far into the <see cref="string"/> to start searching for the sub <see cref="char"/>.</param>
        /// <returns>
        /// Returns an empty <see cref="string"/> if the string or sub <see cref="char"/> are null, empty, or white space, or if the sub <see cref="char"/> could not be found.
        /// Returns the text after the sub <see cref="char"/> otherwise.
        /// </returns>
        /// <exception cref="ArgumentException">Thrown when the start index is less than 0.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown is the start index is less than 0 or greater than the length of the string minus the length of the sub string.</exception>
        public static string
        TextAfter(this string str, char sub_char_from, int start_index = 0)
        {
            string result = str.TextAfter(sub_char_from.ToString(), start_index);

            return result;
        }


        /// <summary>
        /// Gets the text after a sub <see cref="string"/> in a <see cref="string"/>.
        /// </summary>
        /// <param name="str">The <see cref="string"/> to be parsed.</param>
        /// <param name="sub_str_from">The <see cref="string"/> to parse from.</param>
        /// <param name="start_index">How far into the <see cref="string"/> to start searching for the sub <see cref="char"/>.</param>
        /// <returns>
        /// Returns an empty <see cref="string"/> if the string or sub <see cref="string"/> are null, empty, or white space, or if the sub <see cref="string"/> could not be found.
        /// Returns the text after the sub <see cref="string"/> otherwise.
        /// </returns>
        /// <exception cref="ArgumentException">Thrown when the start index is less than 0.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown is the start index is less than 0 or greater than the length of the string minus the length of the sub string.</exception>
        public static string
        TextAfter(this string str, string sub_str_from, int start_index = 0)
        {
            string result = string.Empty;

            if (!str.IsValid() || sub_str_from.IsNull())
            {
                return result;
            }

            if(start_index < 0)
            {
                throw new ArgumentException(nameof(start_index), nameof(start_index) + " must be greater than or equal to 0");
            }

            int maximum = sub_str_from.Length - 1;
            if (!start_index.IsInRange(0, maximum))
            {
                throw new ArgumentOutOfRangeException(nameof(start_index), start_index, nameof(start_index) + " must be between 0 and " + maximum + " (length of the string minus 1)");
            }

            int index_of = str.IndexOf(sub_str_from, start_index, StringComparison.Ordinal);
            if (index_of < 0)
            {
                return result;
            }

            index_of += sub_str_from.Length;
            result = str.Substring(index_of);

            return result;
        }

        /// <summary>
        /// Gets the text before a sub <see cref="char"/> in a <see cref="string"/>.
        /// </summary>
        /// <param name="str">The <see cref="string"/> to be parsed.</param>
        /// <param name="sub_char">The <see cref="char"/> to parse up to.</param>
        /// <param name="start_index">How far into the <see cref="string"/> to start searching for the sub <see cref="char"/>.</param>
        /// <returns>
        /// Returns an empty <see cref="TreeRotation"/> if the string or sub <see cref="char"/> are null, emptys or white space, or if the sub <see cref="char"/> could not be found.
        /// Returns the text before the sub <see cref="char"/> otherwise.
        /// </returns>
        /// <exception cref="ArgumentException">Thrown when the start index is less than 0.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown is the start index is less than 0 or greater than the length of the string minus the length of the sub string.</exception>
        public static string
        TextBefore(this string str, char sub_char, int start_index = 0)
        {
            string result = str.TextBefore(sub_char.ToString(), start_index);

            return result;
        }

        /// <summary>
        /// Gets the text before a sub <see cref="string"/> in a <see cref="string"/>.
        /// </summary>
        /// <param name="str">The <see cref="string"/> to be parsed.</param>
        /// <param name="sub_str">The <see cref="string"/> to parse up to.</param>
        /// <param name="start_index">How far into the <see cref="string"/> to start searching for the sub <see cref="string"/>.</param>
        /// <returns>
        /// Returns an empty <see cref="string"/> if the string or sub <see cref="char"/> are null, emptys or white space, or if the sub <see cref="string"/> could not be found.
        /// Returns the text before the sub <see cref="string"/> otherwise.
        /// </returns>
        /// <exception cref="ArgumentException">Thrown when the start index is less than 0.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown is the start index is less than 0 or greater than the length of the string minus the length of the sub string.</exception>
        public static string
        TextBefore(this string str, string sub_str, int start_index = 0)
        {
            string result = string.Empty;

            if (!str.IsValid() || sub_str.IsNull())
            {
                return result;
            }

            if (start_index < 0)
            {
                throw new ArgumentException(nameof(start_index), nameof(start_index) + " must be greater than or equal to 0");
            }

            int maximum = str.Length - 1;
            if (!start_index.IsInRange(0, maximum))
            {
                throw new ArgumentOutOfRangeException(nameof(start_index), start_index, nameof(start_index) + " must be between 0 and " + maximum + " (length of the string minus 1)");
            }

            int index_of = str.IndexOf(sub_str, start_index, StringComparison.Ordinal);
            if (index_of < 0)
            {
                return result;
            }

            result = str.Substring(0, index_of);

            return result;
        }

        /// <summary>
        /// Gets the text between two sub <see cref="char"/>s in a <see cref="string"/>.
        /// </summary>
        /// <param name="str">The <see cref="string"/> to be parsed.</param>
        /// <param name="sub_char_from">The <see cref="char"/> to parse from.</param>
        /// <param name="sub_char_to">The <see cref="char"/> to parse up to.</param>
        /// <param name="start_index">How far into the <see cref="string"/> to start searching for the first sub <see cref="char"/>.</param>
        /// <returns>
        /// Returns an empty <see cref="string"/> if the <see cref="string"/> or either sub <see cref="char"/>s are null, emptys or white space, or if either sub <see cref="char"/>s could not be found.
        /// Returns the text between the sub <see cref="char"/>s otherwise.
        /// </returns>
        /// <exception cref="ArgumentException">Thrown when the start index is less than 0.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown is the start index is less than 0 or greater than the length of the string minus the length of the sub string.</exception>
        public static string
        TextBetween(this string str, char sub_char_from, char sub_char_to, int start_index = 0)
        {
            string result = str.TextBetween(sub_char_from.ToString(), sub_char_to.ToString(), start_index);

            return result;
        }

        /// <summary>
        /// Gets the text between a sub <see cref="char"/> and a sub <see cref="string"/> in a <see cref="string"/>.
        /// </summary>
        /// <param name="str">The <see cref="string"/> to be parsed.</param>
        /// <param name="sub_char_from">The <see cref="char"/> to parse from.</param>
        /// <param name="sub_str_to">The <see cref="string"/> to parse up to.</param>
        /// <param name="start_index">How far into the <see cref="string"/> to start searching for the sub <see cref="char"/>.</param>
        /// <returns>
        /// Returns an empty <see cref="string"/> if the <see cref="string"/>, sub <see cref="char"/>, or sub <see cref="string"/> are null, emptys or white space, or if either the sub <see cref="char"/> or sub <see cref="string"/> could not be found.
        /// Returns the text between the sub <see cref="char"/> and sub <see cref="string"/> otherwise.
        /// </returns>
        /// <exception cref="ArgumentException">Thrown when the start index is less than 0.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown is the start index is less than 0 or greater than the length of the string minus the length of the sub string.</exception>
        public static string
        TextBetween(this string str, char sub_char_from, string sub_str_to, int start_index = 0)
        {
            string result = str.TextBetween(sub_char_from.ToString(), sub_str_to, start_index);

            return result;
        }

        /// <summary>
        /// Gets the text between a sub <see cref="string"/> and a sub <see cref="char"/> in a <see cref="string"/>.
        /// </summary>
        /// <param name="str">The <see cref="string"/> to be parsed.</param>
        /// <param name="sub_str_from">The <see cref="string"/> to parse from.</param>
        /// <param name="sub_char_to">The <see cref="char"/> to parse up to.</param>
        /// <param name="start_index">How far into the <see cref="string"/> to start searching for the sub <see cref="string"/>.</param>
        /// <returns>
        /// Returns an empty <see cref="string"/> if the <see cref="string"/>, sub <see cref="string"/>, or sub <see cref="char"/> are null, emptys or white space, or if either the sub <see cref="string"/> or sub <see cref="char"/> could not be found.
        /// Returns the text between the sub <see cref="string"/> and sub <see cref="char"/> otherwise.
        /// </returns>
        /// <exception cref="ArgumentException">Thrown when the start index is less than 0.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown is the start index is less than 0 or greater than the length of the string minus the length of the sub string.</exception>
        public static string
        TextBetween(this string str, string sub_str_from, char sub_char_to, int start_index = 0)
        {
            string result = str.TextBetween(sub_str_from, sub_char_to.ToString(), start_index);

            return result;
        }

        /// <summary>
        /// Gets the text between two sub <see cref="char"/>s in a <see cref="string"/>.
        /// </summary>
        /// <param name="str">The <see cref="string"/> to be parsed.</param>
        /// <param name="sub_str_from">The <see cref="string"/> to parse from.</param>
        /// <param name="sub_str_to">The <see cref="string"/> to parse up to.</param>
        /// <param name="start_index">How far into the <see cref="string"/> to start searching for the first sub <see cref="string"/>.</param>
        /// <returns>
        /// Returns an empty <see cref="string"/> if the <see cref="string"/> or either sub <see cref="string"/>s are null, emptys or white space, or if either sub <see cref="string"/>s could not be found.
        /// Returns the text between the sub <see cref="string"/>s otherwise.
        /// </returns>
        /// <exception cref="ArgumentException">Thrown when the start index is less than 0.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown is the start index is less than 0 or greater than the length of the string minus the length of the sub string.</exception>
        public static string
        TextBetween(this string str, string sub_str_from, string sub_str_to, int start_index = 0)
        {
            string result = string.Empty;

            if (!str.IsValid() || sub_str_from.IsNull() || sub_str_to.IsNull())
            {
                return result;
            }

            if (start_index < 0)
            {
                throw new ArgumentException(nameof(start_index), nameof(start_index) + " must be greater than or equal to 0");
            }

            int maximum = str.Length - 1;
            if (!start_index.IsInRange(0, maximum))
            {
                throw new ArgumentOutOfRangeException(nameof(start_index), start_index, nameof(start_index) + " must be between 0 and " + maximum + " (length of the string minus 1)");
            }

            int parse_start_index = str.IndexOf(sub_str_from, start_index, StringComparison.Ordinal);
            if (parse_start_index < 0)
            {
                return result;
            }

            int parse_end_index = str.IndexOf(sub_str_to, parse_start_index + sub_str_from.Length, StringComparison.Ordinal);
            if (parse_end_index < 0)
            {
                return result;
            }

            parse_start_index += sub_str_from.Length;
            result = str.Substring(parse_start_index, parse_end_index - parse_start_index);

            return result;
        }

        /// <summary>
        /// Converts a <see cref="string"/> into an <see cref="Array"/> of a specified type.
        /// </summary>
        /// <typeparam name="type">The type of the returned <see cref="Array"/>.</typeparam>
        /// <param name="str">The <see cref="string"/> to be parsed.</param>
        /// <param name="separator">An <see cref="char"/> that represents a point to separate the string into elemnts.</param>
        /// <returns>
        /// Returns a default <see cref="Array"/> of the specified type is no <see cref="string"/> elements could be converted.
        /// Returns an <see cref="Array"/> of a specified type with the successfully converted <see cref="string"/> elements otherwise.
        /// </returns>
        public static type[] StringToArray<type>(this string str, char separator, StringSplitOptions options = StringSplitOptions.None)
        {
            return str.StringToArray<type>(new char[] { separator }, options);
        }

        /// <summary>
        /// Converts a <see cref="string"/> into an <see cref="Array"/> of a specified type.
        /// </summary>
        /// <typeparam name="type">The type of the returned <see cref="Array"/>.</typeparam>
        /// <param name="str">The <see cref="string"/> to be parsed.</param>
        /// <param name="separator">An <see cref="Array"/> of <see cref="char"/>s that represent points to separate the string into elemnts.</param>
        /// <returns>
        /// Returns a default <see cref="Array"/> of the specified type is no <see cref="string"/> elements could be converted.
        /// Returns an <see cref="Array"/> of a specified type with the successfully converted <see cref="string"/> elements otherwise.
        /// </returns>
        public static type[] StringToArray<type>(this string str, char[] separators, StringSplitOptions options = StringSplitOptions.None)
        {
            if (!str.IsValid())
            {
                return default(type[]);
            }

            List<type> result = new List<type>();

            string[] array = str.Split(separators, options);
            foreach (string element in array)
            {
                if (!element.CanCovertTo(typeof(type)))
                {
                    continue;
                }

                result.Add((type)Convert.ChangeType(element, typeof(type)));
            }

            return result.IsValid() ? result.ToArray() : default(type[]);
        }

        #endregion
    }
}
