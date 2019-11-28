// standard namespaces
using System;

// project namespaces
using TwitchNet.Clients.Irc;
using TwitchNet.Extensions;
using TwitchNet.Utilities;

// imported .dll's
using Newtonsoft.Json;

namespace
TwitchNet.Helpers.Json
{
    internal class
    EnumConverter : JsonConverter
    {
        /// <summary>
        /// Whether this JsonConverter can read JSON.
        /// </summary>
        public override bool
        CanRead => true;

        /// <summary>
        /// Whether this JsonConverter can write JSON.
        /// </summary>
        public override bool
        CanWrite => true;

        /// <summary>
        /// Converts a JSON object to a <see cref="TimeSpan"/> object.
        /// </summary>
        /// <param name="reader">The <see cref="JsonReader"/> that reads incoming serialized data.</param>
        /// <param name="object_type">The type of the JSON object being converted.</param>
        /// <param name="existing_value">The existing value of object being read.</param>
        /// <param name="serializer">The calling serializer.</param>
        /// <returns>Returns a <see cref="TimeSpan"/> object converted from the JSON object.</returns>
        public override object
        ReadJson(JsonReader reader, Type object_type, object existing_value, Newtonsoft.Json.JsonSerializer serializer)
        {
            string name = reader.TokenType == JsonToken.Null ? string.Empty : reader.Value.ToString();

            #if DEBUG

            // It's okay if it crashes dureing testing, that's a good thing.
            // If it crashes in a release build, that would be a bad thing.
            object value = EnumUtil.Parse(object_type, name);

            #else

            EnumUtil.TryParse(object_type, name, out object value);

            #endif

            return value;
        }

        /// <summary>
        /// Writes the JSON representation of the object.
        /// </summary>
        /// <param name="writer">The <see cref="JsonWriter"/> to write to.</param>
        /// <param name="value">The value of the object to serialize</param>
        /// <param name="serializer">The calling serializer.</param>m>
        public override void
        WriteJson(JsonWriter writer, object value, Newtonsoft.Json.JsonSerializer serializer)
        {
            if (value.IsNull())
            {
                writer.WriteNull();
            }

            Enum enum_value = (Enum)value;

            string string_value = EnumUtil.GetName(enum_value.GetType(), enum_value);

            writer.WriteValue(string_value);
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
            return object_type.GetTrueType().IsEnum;
        }
    }

    internal class
    UnixEpochSecondsConverter : JsonConverter
    {
        /// <summary>
        /// Whether this JsonConverter can read JSON.
        /// </summary>
        public override bool
        CanRead => true;

        /// <summary>
        /// Whether this JsonConverter can write JSON.
        /// </summary>
        public override bool
        CanWrite => false;

        /// <summary>
        /// Converts a JSON object to a <see cref="DateTime"/> from a unix epoch value.
        /// </summary>
        /// <param name="reader">The <see cref="JsonReader"/> that reads incoming serialized data.</param>
        /// <param name="object_type">The type of the JSON object being converted.</param>
        /// <param name="existing_value">The existing value of object being read.</param>
        /// <param name="serializer">The calling serializer.</param>
        /// <returns>Returns a <see cref="TimeSpan"/> object converted from the JSON object.</returns>
        public override object
        ReadJson(JsonReader reader, Type object_type, object existing_value, Newtonsoft.Json.JsonSerializer serializer)
        {
            string name = reader.TokenType == JsonToken.Null ? string.Empty : reader.Value.ToString();
            if (!name.HasContent())
            {
                return DateTime.MinValue;
            }

            DateTime value = ((long)reader.Value).FromUnixEpochSeconds();

            return value;
        }

        public override void
        WriteJson(JsonWriter writer, object value, Newtonsoft.Json.JsonSerializer serializer)
        {

        }

        public override bool
        CanConvert(Type object_type)
        {
            return object_type == typeof(int) || object_type == typeof(uint) || object_type == typeof(long) || object_type == typeof(ulong);
        }
    }

    internal class
    VideoDurationConverter : JsonConverter
    {
        /// <summary>
        /// Whether this JsonConverter can read JSON.
        /// </summary>
        public override bool
        CanRead => true;

        /// <summary>
        /// Whether this JsonConverter can write JSON.
        /// </summary>
        public override bool
        CanWrite => false;

        /// <summary>
        /// Converts a JSON object to a <see cref="TimeSpan"/> object.
        /// </summary>
        /// <param name="reader">The <see cref="JsonReader"/> that reads incoming serialized data.</param>
        /// <param name="object_type">The type of the JSON object being converted.</param>
        /// <param name="existing_value">The existing value of object being read.</param>
        /// <param name="serializer">The calling serializer.</param>
        /// <returns>Returns a <see cref="TimeSpan"/> object converted from the JSON object.</returns>
        public override object
        ReadJson(JsonReader reader, Type object_type, object existing_value, Newtonsoft.Json.JsonSerializer serializer)
        {
            string value = reader.Value.ToString();

            // attempt to match HH:MM:SS format
            if (!TimeSpan.TryParse(value, out TimeSpan time_span))
            {
                // attempt to match 00h00m00s format
                TwitchIrcUtil.TryConvertToVideoLength(value, out time_span);
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
        WriteJson(JsonWriter writer, object value, Newtonsoft.Json.JsonSerializer serializer)
        {
            throw new NotImplementedException("Custom JsonWriter for converter " + nameof(VideoDurationConverter) + " not implemented because it is marked as read only. This writer will be skipped when called.");
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
