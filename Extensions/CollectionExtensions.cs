// standard namespaces
using System.Collections.Generic;
using System.Linq;

namespace TwitchNet.Extensions
{
    public static class
    CollectionExtensions
    {
        /// <summary>
        /// Removes all duplicate elements, and all elements that are null, empty, or only contain whitespace.
        /// </summary>
        /// <param name="list">The list to sanitize.</param>
        /// <returns>Returns the sanitized list.</returns>
        public static List<string>
        RemoveInvalidAndDuplicateValues(this List<string> list)
        {
            if (!list.IsValid())
            {
                return list;
            }

            list.RemoveAll(str => !str.IsValid());
            list = list.Distinct().ToList();

            return list;
        }

        /// <summary>
        /// Removes all null entries from the dictionary.
        /// </summary>
        /// <typeparam name="key_type">The dictionary key type.</typeparam>
        /// <typeparam name="value_type">The dictionary value type.</typeparam>
        /// <param name="dictionary">The dictionary to sanitize.</param>
        /// <returns>Returns the sanitized dictionary.</returns>
        public static Dictionary<key_type, value_type>
        RemoveNullValues<key_type, value_type>(this Dictionary<key_type, value_type> dictionary)
        {
            if (!dictionary.IsValid())
            {
                return dictionary;
            }

            Dictionary<key_type, value_type> result = new Dictionary<key_type, value_type>(dictionary.Count);

            foreach(KeyValuePair<key_type, value_type> pair in dictionary)
            {
                if (pair.Value.IsNull())
                {
                    continue;
                }

                result.Add(pair.Key, pair.Value);
            }

            dictionary = result;

            return dictionary;
        }
    }
}
