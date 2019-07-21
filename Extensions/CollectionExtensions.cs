// standard namespaces
using System.Collections.Generic;
using System.Linq;

namespace TwitchNet.Extensions
{
    public static class
    CollectionExtensions
    {
        /// <summary>
        /// Removes all duplicate elements, and All elements that are null, empty, or contain only whitespace.
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

            //list.RemoveAll(str => !str.IsValid());
            //list = list.Distinct().ToList();

            HashSet<string> hash = new HashSet<string>();
            foreach(string element in list)
            {
                if (!element.HasContent())
                {
                    continue;
                }

                hash.Add(element);
            }

            return hash.ToList();
        }

        public static List<int>
        GetNoContentIndicies(this List<string> list)
        {
            if (!list.IsValid())
            {
                return new List<int>();
            }

            List<int> indicies = new List<int>(list.Count);
            for (int index = 0; index < list.Count; ++index)
            {
                if (list[index].HasContent())
                {
                    continue;
                }

                indicies.Add(index);
            }

            indicies.TrimExcess();

            return indicies;
        }

        public static List<string>
        GetDuplicateElements(this List<string> list)
        {
            if (!list.IsValid())
            {
                return new List<string>();
            }

            HashSet<string> hash = new HashSet<string>();
            HashSet<string> duplicates = new HashSet<string>();

            for (int index = 0; index < list.Count; ++index)
            {
                if (hash.Add(list[index]))
                {
                    continue;
                }

                duplicates.Add(list[index]);
            }

            duplicates.TrimExcess();

            return duplicates.ToList();
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
