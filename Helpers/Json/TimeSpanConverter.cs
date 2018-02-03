﻿// standard namespaces
using System;
using System.Text.RegularExpressions;

// imported .dll's
using Newtonsoft.Json;

namespace
TwitchNet.Helpers.Json
{
    internal class
    TimeSpanConverter : JsonConverter
    {
        /// <summary>
        /// Whether this JsonConverter can read JSON.
        /// </summary>
        public override bool
        CanRead
        {
            get
            {
                return true;
            }
        }

        /// <summary>
        /// Whether this JsonConverter can write JSON.
        /// </summary>
        public override bool
        CanWrite
        {
            get
            {
                return false;
            }
        }                

        /// <summary>
        /// Converts a JSON object to a <see cref="TimeSpan"/> object.
        /// </summary>
        /// <param name="reader">The <see cref="JsonReader"/> that reads incoming serialized data.</param>
        /// <param name="object_type">The type of the JSON object being converted.</param>
        /// <param name="existing_value">The existing value of object being read.</param>
        /// <param name="serializer">The calling serializer.</param>
        /// <returns>Returns a <see cref="TimeSpan"/> object converted from the JSON object.</returns>
        public override object
        ReadJson(JsonReader reader, Type object_type, object existing_value, JsonSerializer serializer)
        {
            string value = reader.Value.ToString();
            // attempt to match HH:MM:SS format used by TimeSpan
            if(!TimeSpan.TryParse(value, out TimeSpan time_span))
            {
                // attempt to match 00h00m00s format
                Regex regex = new Regex("^(?:(?:(?:(?<days>\\d{1,})d)?(?<hours>\\d{1,2})h)?(?<minutes>\\d{1,2})m)?(?<seconds>\\d{1,2})s$");
                Match match = regex.Match(value);
                if (match.Success)
                {
                    Int32.TryParse(match.Groups["days"].Value, out int days);
                    Int32.TryParse(match.Groups["hours"].Value, out int hours);
                    Int32.TryParse(match.Groups["minutes"].Value, out int minutes);
                    Int32.TryParse(match.Groups["seconds"].Value, out int seconds);

                    time_span = new TimeSpan(days, hours, minutes, seconds);
                }
                else
                {
                    throw new ArgumentException("Failed to convert " + nameof(value) + " from " + object_type.Name + " to " + typeof(TimeSpan).Name, value);
                }
            }

            return time_span;
        }

        /// <summary>
        /// Writes the JSON representation of the object.
        /// </summary>
        /// <param name="writer">The <see cref="JsonWriter"/> to write to.</param>
        /// <param name="value">The value of the object to serialize</param>
        /// <param name="serializer">The calling serializer.</param>m>
        public override void
        WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException("Custom JsonWriter for converter " + nameof(TimeSpanConverter) + " not implemented because it is marked as read only. This writer will be skipped when called.");
        }

        /// <summary>
        /// Checks to see if the object can be converted to <see cref="TimeSpan"/>.
        /// </summary>
        /// <param name="object_type">The type of the member.</param>
        /// <returns>
        /// Returns true if the type is equal to <see cref="TimeSpan"/>.
        /// Returns false otherwise.
        /// </returns>
        public override bool
        CanConvert(Type object_type)
        {
            return typeof(TimeSpan) == object_type;
        }
    }
}
