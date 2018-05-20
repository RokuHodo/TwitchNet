// project namespaces
using System;
using System.Collections.Generic;

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
        TextAfter(this string str, char char_from, int start_index = 0)
        {
            string result = string.Empty;

            if (!str.IsValid() || char_from.IsNull())
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

            int char_from_index = str.IndexOf(char_from, start_index);
            if (char_from_index < 0)
            {
                return result;
            }

            result = str.Substring(char_from_index + 1);

            return result;
        }


        /// <summary>
        /// Gets the text after a sub <see cref="string"/> in a <see cref="string"/>.
        /// </summary>
        /// <param name="str">The <see cref="string"/> to be parsed.</param>
        /// <param name="str_from">The <see cref="string"/> to parse from.</param>
        /// <param name="start_index">How far into the <see cref="string"/> to start searching for the sub <see cref="char"/>.</param>
        /// <returns>
        /// Returns an empty <see cref="string"/> if the string or sub <see cref="string"/> are null, empty, or white space, or if the sub <see cref="string"/> could not be found.
        /// Returns the text after the sub <see cref="string"/> otherwise.
        /// </returns>
        /// <exception cref="ArgumentException">Thrown when the start index is less than 0.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown is the start index is less than 0 or greater than the length of the string minus the length of the sub string.</exception>
        public static string
        TextAfter(this string str, string str_from, int start_index = 0)
        {
            string result = string.Empty;

            if (!str.IsValid() || str_from.IsNull())
            {
                return result;
            }

            if(start_index < 0)
            {
                throw new ArgumentException(nameof(start_index), nameof(start_index) + " must be greater than or equal to 0");
            }

            int maximum = str_from.Length - 1;
            if (!start_index.IsInRange(0, maximum))
            {
                throw new ArgumentOutOfRangeException(nameof(start_index), start_index, nameof(start_index) + " must be between 0 and " + maximum + " (length of the string minus 1)");
            }

            int str_from_index = str.IndexOf(str_from, start_index, StringComparison.Ordinal);
            if (str_from_index < 0)
            {
                return result;
            }

            str_from_index += str_from.Length;
            result = str.Substring(str_from_index);

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
        TextBefore(this string str, char char_to, int start_index = 0)
        {
            string result = string.Empty;

            if (!str.IsValid() || char_to.IsNull())
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

            int char_to_index = str.IndexOf(char_to, start_index);
            if (char_to_index < 0)
            {
                return result;
            }

            result = str.Substring(0, char_to_index);

            return result;
        }

        /// <summary>
        /// Gets the text before a sub <see cref="string"/> in a <see cref="string"/>.
        /// </summary>
        /// <param name="str">The <see cref="string"/> to be parsed.</param>
        /// <param name="str_to">The <see cref="string"/> to parse up to.</param>
        /// <param name="start_index">How far into the <see cref="string"/> to start searching for the sub <see cref="string"/>.</param>
        /// <returns>
        /// Returns an empty <see cref="string"/> if the string or sub <see cref="char"/> are null, emptys or white space, or if the sub <see cref="string"/> could not be found.
        /// Returns the text before the sub <see cref="string"/> otherwise.
        /// </returns>
        /// <exception cref="ArgumentException">Thrown when the start index is less than 0.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown is the start index is less than 0 or greater than the length of the string minus the length of the sub string.</exception>
        public static string
        TextBefore(this string str, string str_to, int start_index = 0)
        {
            string result = string.Empty;

            if (!str.IsValid() || str_to.IsNull())
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

            int str_to_index = str.IndexOf(str_to, start_index, StringComparison.Ordinal);
            if (str_to_index < 0)
            {
                return result;
            }

            result = str.Substring(0, str_to_index);

            return result;
        }

        /// <summary>
        /// Gets the text between two sub <see cref="char"/>s in a <see cref="string"/>.
        /// </summary>
        /// <param name="str">The <see cref="string"/> to be parsed.</param>
        /// <param name="char_from">The <see cref="char"/> to parse from.</param>
        /// <param name="char_to">The <see cref="char"/> to parse up to.</param>
        /// <param name="start_index">How far into the <see cref="string"/> to start searching for the first sub <see cref="char"/>.</param>
        /// <returns>
        /// Returns an empty <see cref="string"/> if the <see cref="string"/> or either sub <see cref="char"/>s are null, emptys or white space, or if either sub <see cref="char"/>s could not be found.
        /// Returns the text between the sub <see cref="char"/>s otherwise.
        /// </returns>
        /// <exception cref="ArgumentException">Thrown when the start index is less than 0.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown is the start index is less than 0 or greater than the length of the string minus the length of the sub string.</exception>
        public static string
        TextBetween(this string str, char char_from, char char_to, int start_index = 0)
        {
            string result = string.Empty;

            if (!str.IsValid() || char_from.IsNull() || char_to.IsNull())
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

            int char_from_index = str.IndexOf(char_from, start_index);
            if (char_from_index < 0)
            {
                return result;
            }

            int char_to_index = str.IndexOf(char_to, char_from_index + 1);
            if (char_to_index < 0)
            {
                return result;
            }

            result = str.Substring(char_from_index + 1, char_to_index - char_from_index - 1);

            return result;
        }

        /// <summary>
        /// Gets the text between two sub <see cref="char"/>s in a <see cref="string"/>.
        /// </summary>
        /// <param name="str">The <see cref="string"/> to be parsed.</param>
        /// <param name="str_from">The <see cref="string"/> to parse from.</param>
        /// <param name="str_to">The <see cref="string"/> to parse up to.</param>
        /// <param name="start_index">How far into the <see cref="string"/> to start searching for the first sub <see cref="string"/>.</param>
        /// <returns>
        /// Returns an empty <see cref="string"/> if the <see cref="string"/> or either sub <see cref="string"/>s are null, emptys or white space, or if either sub <see cref="string"/>s could not be found.
        /// Returns the text between the sub <see cref="string"/>s otherwise.
        /// </returns>
        /// <exception cref="ArgumentException">Thrown when the start index is less than 0.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown is the start index is less than 0 or greater than the length of the string minus the length of the sub string.</exception>
        public static string
        TextBetween(this string str, string str_from, string str_to, int start_index = 0)
        {
            string result = string.Empty;

            if (!str.IsValid() || str_from.IsNull() || str_to.IsNull())
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

            int str_from_index = str.IndexOf(str_from, start_index, StringComparison.Ordinal);
            if (str_from_index < 0)
            {
                return result;
            }

            int str_to_index = str.IndexOf(str_to, str_from_index + str_from.Length, StringComparison.Ordinal);
            if (str_to_index < 0)
            {
                return result;
            }

            str_from_index += str_from.Length;
            result = str.Substring(str_from_index, str_to_index - str_from_index);

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
        public static type[]
        StringToArray<type>(this string str, char separator, StringSplitOptions options = StringSplitOptions.None)
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
        public static type[]
        StringToArray<type>(this string str, char[] separators, StringSplitOptions options = StringSplitOptions.None)
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

        /// <summary>
        /// Wraps a string in quote.
        /// </summary>
        /// <param name="str">The string to wrap.</param>
        /// <returns>Returns a string wrapped in quotes.</returns>
        public static string
        WrapQuotes(this string str)
        {
            if (str.IsNull())
            {
                return "\"\"";
            }

            return "\"" + str + "\"";

        }

        /// <summary>
        /// Wraps a string in quote.
        /// </summary>
        /// <param name="character">The string to wrap.</param>
        /// <returns>Returns a string wrapped in quotes.</returns>
        public static string
        WrapQuotes(this char character)
        {
            if (character.IsNull())
            {
                return "\"\"";
            }

            return "\"" + character + "\"";

        }

        #endregion
    }
}
