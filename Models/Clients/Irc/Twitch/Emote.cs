// standard namspaces
using System;
using System.Collections.Generic;

// project namespaces
using TwitchNet.Extensions;

namespace
TwitchNet.Models.Clients.Irc.Twitch
{

    public struct
    Emote
    {
        /// <summary>
        /// <para>The emote id.</para>
        /// <para>The id is empty if the tag is invalid.</para>
        /// </summary>
        string id;

        /// <summary>
        /// <para>The character index range(s) where the emote was used.</para>
        /// <para>
        /// The array is empty is the tag is invalid.
        /// Indexing starts at the begining of the actual message and ignores the use of '\001ACTION'.
        /// </para>
        /// </summary>
        Range[] ranges;

        public Emote(string pair)
        {
            id = string.Empty;
            ranges = new Range[0];

            if (pair.IsValid())
            {
                id = pair.TextBefore(':');

                List<Range> ranges_list = new List<Range>();

                string[] _ranges = pair.TextAfter(':').StringToArray<string>(',');
                foreach(string _range in _ranges)
                {
                    Range range = new Range(_range);
                    ranges_list.Add(range);
                }

                ranges = ranges_list.ToArray();
            }
        }
    }

    public struct
    Range
    {
        /// <summary>
        /// <para>The index in the message where the first emote character is located.</para>
        /// <para>
        /// Set to -1 when the index could not be parsed.
        /// Indexing starts at the begining of the actual message and ignores the use of '\001ACTION'.
        /// </para>
        /// </summary>
        short index_start;

        /// <summary>
        /// <para>The index in the message where the last emote character is located.</para>
        /// <para>
        /// Set to -1 when the index could not be parsed.
        /// Indexing starts at the begining of the actual message and ignores the use of '\001ACTION'.
        /// </para>
        /// </summary>
        short index_end;

        public Range(string range_pair)
        {
            if(!Int16.TryParse(range_pair.TextBefore('-'), out index_start))
            {
                index_start = -1;
            }

            if(Int16.TryParse(range_pair.TextAfter('-'), out index_end))
            {
                index_end = -1;
            }
        }
    }
}
