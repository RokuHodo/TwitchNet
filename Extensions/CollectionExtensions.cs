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
        Sanitize(this List<string> list)
        {
            if (!list.IsValid())
            {
                return list;
            }

            list = list.Distinct().ToList();
            list.RemoveAll(str => !str.IsValid());

            return list;
        }
    }
}
