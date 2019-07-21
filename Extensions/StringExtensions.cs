// project namespaces
using System;

namespace
TwitchNet.Extensions
{
    internal static class
    StringExtensions
    {
        #region Parsing

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

        #endregion

        #region Formatting

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

        /// <summary>
        /// Nulls the string if it is invalid.
        /// </summary>
        /// <param name="str">The string to potentially null.</param>
        /// <returns>
        /// Returns the string if it is valid.
        /// Returns null otherwise.
        /// </returns>
        public static string
        NullIfInvalid(this string str)
        {
            return str.IsValid() ? str : null;
        }

        #endregion
    }
}
