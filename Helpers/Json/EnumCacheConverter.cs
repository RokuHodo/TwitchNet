// standard namespaces
using System;

// project namespaces
using TwitchNet.Extensions;
using TwitchNet.Utilities;

// imported .dll's
using Newtonsoft.Json;

namespace
TwitchNet.Helpers.Json
{
    internal class
    EnumCacheConverter : JsonConverter
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
        ReadJson(JsonReader reader, Type object_type, object existing_value, JsonSerializer serializer)
        {
            string name = reader.TokenType == JsonToken.Null ? string.Empty : reader.Value.ToString();

            object value = EnumUtil.Parse(object_type, name);

            return value;
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
            Type type = object_type.IsNullable() ? Nullable.GetUnderlyingType(object_type) : object_type;

            return type.IsEnum;
        }
    }
}
