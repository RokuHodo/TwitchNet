// standard namspaces
using System;
using System.Collections.Generic;

// project namespaces
using TwitchNet.Debugger;
using TwitchNet.Extensions;

namespace
TwitchNet.Clients.Irc.Twitch
{
    [ValidateObject]
    public class
    Emote
    {
        /// <summary>
        /// <para>The emote id.</para>
        /// <para>The id is empty if the tag is invalid.</para>
        /// </summary>
        [ValidateMember(Check.IsValid)]
        public string   id      { get; protected set; }

        /// <summary>
        /// <para>The character index range(s) where the emote was used.</para>
        /// <para>
        /// The array is empty is the tag is invalid.
        /// Indexing starts at the begining of the actual message and ignores the use of '\001ACTION'.
        /// </para>
        /// </summary>
        [ValidateMember(Check.IsValid)]
        public Range[]  ranges  { get; protected set; }

        /// <summary>
        /// Creates a new instance of the <see cref="Emote"/> class.
        /// </summary>
        /// <param name="pair">The emote tag pair to parse.</param>
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

    [ValidateObject]
    public class
    Range
    {
        /// <summary>
        /// <para>The index in the message where the first emote character is located.</para>
        /// <para>
        /// Set to -1 when the index could not be parsed.
        /// Indexing starts at the begining of the actual message and ignores the use of '\001ACTION'.
        /// </para>
        /// </summary>
        [ValidateMember(Check.IsNotEqualTo, -1)]
        public int index_start  { get; protected set; }

        /// <summary>
        /// <para>The index in the message where the last emote character is located.</para>
        /// <para>
        /// Set to -1 when the index could not be parsed.
        /// Indexing starts at the begining of the actual message and ignores the use of '\001ACTION'.
        /// </para>
        /// </summary>
        [ValidateMember(Check.IsNotEqualTo, -1)]
        public int index_end    { get; protected set; }

        /// <summary>
        /// Creates a new instance of the <see cref="Range"/> class.
        /// </summary>
        /// <param name="pair">The range tag value pair to parse.</param>
        public Range(string range_pair)
        {
            if(!Int32.TryParse(range_pair.TextBefore('-'), out int _index_start))
            {
                _index_start = -1;
            }
            index_start = _index_start;

            if (!Int32.TryParse(range_pair.TextAfter('-'), out int _index_end))
            {
                _index_end = -1;
            }
            index_end = _index_end;
        }
    }
}
