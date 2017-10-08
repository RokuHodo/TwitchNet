// project namespaces
using System;

namespace TwitchNet.Extensions
{
    public static class StringExtensions
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
        public static string TextAfter(this string str, char sub_char_from, int start_index = 0)
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
        public static string TextAfter(this string str, string sub_str_from, int start_index = 0)
        {
            string result = string.Empty;

            if (!str.isValid() || !sub_str_from.isValid())
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

            int index_of = str.IndexOf(sub_str_from, start_index);
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
        public static string TextBefore(this string str, char sub_char, int start_index = 0)
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
        public static string TextBefore(this string str, string sub_str, int start_index = 0)
        {
            string result = string.Empty;

            if (!str.isValid() || !sub_str.isValid())
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

            int index_of = str.IndexOf(sub_str, start_index);
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
        public static string TextBetween(this string str, char sub_char_from, char sub_char_to, int start_index = 0)
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
        public static string TextBetween(this string str, char sub_char_from, string sub_str_to, int start_index = 0)
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
        public static string TextBetween(this string str, string sub_str_from, char sub_char_to, int start_index = 0)
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
        public static string TextBetween(this string str, string sub_str_from, string sub_str_to, int start_index = 0)
        {
            string result = string.Empty;

            if (!str.isValid() || !sub_str_from.isValid() || !sub_str_to.isValid())
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

            int parse_start_index = str.IndexOf(sub_str_from, start_index);
            if (parse_start_index < 0)
            {
                return result;
            }

            int parse_end_index = str.IndexOf(sub_str_to, parse_start_index + sub_str_from.Length);
            if (parse_end_index < 0)
            {
                return result;
            }

            parse_start_index += sub_str_from.Length;
            result = str.Substring(parse_start_index, parse_end_index - parse_start_index);

            return result;
        }

        #endregion
    }
}
