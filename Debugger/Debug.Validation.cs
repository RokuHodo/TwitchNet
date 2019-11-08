// standard namespaces
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

// project namespaces
using TwitchNet.Extensions;
using TwitchNet.Clients.Irc;

namespace
TwitchNet.Debugger
{
    internal static partial class
    Debug
    {
        #region Member validation

        [Conditional("DEBUG")]
        public static void
        ValidateMembers(object obj)
        {   
            if (obj.IsNullOrDefault())
            {
                return;
            }

            Type type = obj.GetType();

            if (type.IsArray || type.IsList())
            {
                IList list = obj as IList;
                foreach (object element in list)
                {
                    ValidateMembers(element);
                }
            }

            MemberInfo[] members = type.GetMembers<ValidateMemberAttribute>(); 
            if (!members.IsValid())
            {
                return;
            }

            foreach(MemberInfo member in members)
            {
                if (!member.TryGetAttributes(out ValidateMemberAttribute[] attributes))
                {
                    return;
                }

                foreach (ValidateMemberAttribute attribute in attributes)
                {
                    ValidateMember(obj, member, attribute);
                }
            }
        }

        [Conditional("DEBUG")]
        public static void
        ValidateMember(object obj, MemberInfo member, ValidateMemberAttribute attribute)
        {
            if(!TryCastMember(obj, member, out string name, out Type type, out object value))
            {
                return;
            }

            if (value.IsNull())
            {
                return;
            }

            if (type.IsClass && attribute.check == Check.None)
            {
                ValidateMembers(value);
            }

            switch (attribute.check)
            {
                case Check.IsNull:
                {
                    ValidationHandler_IsNull(name, value, attribute);
                }
                break;

                case Check.IsNotNull:
                {
                    ValidationHandler_IsNotNull(name, value, attribute);
                }
                break;

                case Check.IsDefault:
                {
                    ValidationHandler_IsDefault(name, type, value, attribute);
                }
                break;

                case Check.IsNotDefault:
                {
                    ValidationHandler_IsNotDefault(name, type, value, attribute);
                }
                break;

                case Check.IsNullOrDefault:
                {
                    ValidationHandler_IsNullOrDefault(name, type, value, attribute);
                }
                break;

                case Check.IsNotNullOrDefault:
                {
                    ValidationHandler_IsNotNullOrDefault(name, type, value, attribute);
                }
                break;

                case Check.IsValid:
                {
                    ValidationHandler_IsValid(obj, name, type, value, attribute);
                }
                break;

                case Check.IsInvalid:
                {
                    ValidationHandler_IsInvalid(obj, name, type, value, attribute);
                }
                break;

                case Check.IsEqualTo:
                {
                    ValidationHandler_IsEqualTo(name, value, attribute);
                }
                break;

                case Check.IsNotEqualTo:
                {
                    ValidationHandler_IsNotEqualTo(name, value, attribute);
                }
                break;

                case Check.RegexIsMatch:
                {
                    ValidationHandler_RegexIsMatch(name, value, attribute);
                }
                break;

                case Check.RegexNoMatch:
                {
                    ValidationHandler_RegexNoMatch(name, value, attribute);
                }
                break;

                case Check.TagsExtra:
                case Check.TagsMissing:
                {
                    ValidationHandler_Tags(obj, name, type, value, attribute);
                }
                break;

                case Check.None:
                default:
                {
                        
                }
                break;
            }
        }

        #endregion

        #region Member validation handlers

        private static void
        ValidationHandler_IsNull(string name, object value, ValidateMemberAttribute attribute)
        {
            if (value.IsNull())
            {
                return;
            }

            WriteError(attribute.level, name.WrapQuotes() + " must be null.", attribute.caller, attribute.source, attribute.line);
        }

        private static void
        ValidationHandler_IsNotNull(string name, object value, ValidateMemberAttribute attribute)
        {
            if (!value.IsNull())
            {
                return;
            }

            WriteError(attribute.level, name.WrapQuotes() + " cannot be null.", attribute.caller, attribute.source, attribute.line);
        }

        private static void
        ValidationHandler_IsDefault(string name, Type type, object value, ValidateMemberAttribute attribute)
        {
            if (value.IsDefault())
            {
                return;
            }

            WriteError(attribute.level, name.WrapQuotes() + " must be equal to the default value for " + type.Name + ", " + type.GetDefaultValue().ToString().WrapQuotes() + ".", attribute.caller, attribute.source, attribute.line);
        }

        private static void
        ValidationHandler_IsNotDefault(string name, Type type, object value, ValidateMemberAttribute attribute)
        {
            if (!value.IsDefault())
            {
                return;    
            }

            WriteError(attribute.level, name.WrapQuotes() + " cannot be equal to the default value for " + type.Name + ", " + type.GetDefaultValue().ToString().WrapQuotes() + ".", attribute.caller, attribute.source, attribute.line);
        }

        private static void
        ValidationHandler_IsNullOrDefault(string name, Type type, object value, ValidateMemberAttribute attribute)
        {
            if (value.IsNullOrDefault())
            {
                return;
            }

            WriteError(attribute.level, name.WrapQuotes() + " must be null or equal to the default value for " + type.Name + ", " + type.GetDefaultValue().ToString().WrapQuotes() + ".", attribute.caller, attribute.source, attribute.line);
        }

        private static void
        ValidationHandler_IsNotNullOrDefault(string name, Type type, object value, ValidateMemberAttribute attribute)
        {
            if (!value.IsNullOrDefault())
            {
                return;
            }

            WriteError(attribute.level, name.WrapQuotes() + " cannot be null or equal to the default value for " + type.Name + ", " + type.GetDefaultValue().ToString().WrapQuotes() + ".", attribute.caller, attribute.source, attribute.line);
        }

        private static void
        ValidationHandler_IsValid(object obj, string name, Type type, object value, ValidateMemberAttribute attribute)
        {
            if (type.IsNull() || value.IsNull())
            {
                return;
            }

            if (type == typeof(string))
            {
                string temp = value.ToString();
                if (!temp.IsValid())
                {
                    WriteError(attribute.level, name.WrapQuotes() + " is null, empty, or only contains whitespace.", attribute.caller, attribute.source, attribute.line);
                }
            }
            else
            {
                Type[] interfaces = type.GetInterfaces();
                if (interfaces.Contains(typeof(IList)))
                {
                    IList temp = value as IList;
                    if (temp.IsNull() || temp.Count == 0)
                    {
                        WriteError(attribute.level, name.WrapQuotes() + " is null or an empty array.", attribute.caller, attribute.source, attribute.line);
                    }
                }
                else if (interfaces.Contains(typeof(IDictionary)))
                {
                    IDictionary temp = value as IDictionary;
                    if (temp.IsNull() || temp.Keys.Count == 0)
                    {
                        WriteError(attribute.level, name.WrapQuotes() + " is null or an empty dictionary.", attribute.caller, attribute.source, attribute.line);
                    }
                }
            }
        }

        private static void
        ValidationHandler_IsInvalid(object obj, string name, Type type, object value, ValidateMemberAttribute attribute)
        {
            if (type == typeof(string))
            {
                string temp = value.ToString();
                if (temp.IsValid())
                {
                    WriteError(attribute.level, name.WrapQuotes() + " must be null, empty, or only contains whitespace.", attribute.caller, attribute.source, attribute.line);
                }
            }
            else
            {
                Type[] interfaces = type.GetInterfaces();
                if (interfaces.Contains(typeof(IList)))
                {
                    IList temp = value as IList;
                    if (!temp.IsNull() || temp.Count > 0)
                    {
                        WriteError(attribute.level, name.WrapQuotes() + " must be null or an empty array.", attribute.caller, attribute.source, attribute.line);
                    }
                }
                else if (interfaces.Contains(typeof(IDictionary)))
                {
                    IDictionary temp = value as IDictionary;
                    if (!temp.IsNull() || temp.Keys.Count > 0)
                    {
                        WriteError(attribute.level, name.WrapQuotes() + " must be null or an empty dictionary.", attribute.caller, attribute.source, attribute.line);
                    }
                }
            }
        }

        private static void
        ValidationHandler_IsEqualTo(string name, object value, ValidateMemberAttribute attribute)
        {
            if (value.Equals(attribute.compare_to))
            {
                return;
            }

            string compare_from = value.IsNull() ? "null" : value.ToString();
            string compare_to = attribute.compare_to.IsNull() ? "null" : attribute.compare_to.ToString();

            string message = name.WrapQuotes() + " is not equal to " + compare_to.WrapQuotes() + "." + Environment.NewLine +
                             "> Value: " + compare_from;
            WriteError(attribute.level, message, attribute.caller, attribute.source, attribute.line);
        }

        private static void
        ValidationHandler_IsNotEqualTo(string name, object value, ValidateMemberAttribute attribute)
        {
            if (!value.Equals(attribute.compare_to))
            {
                return;
            }

            string compare_from = value.IsNull() ? "null" : value.ToString();
            WriteError(attribute.level, name.WrapQuotes() + " cannot be equal to " + attribute.compare_to.ToString().WrapQuotes() + ".", attribute.caller, attribute.source, attribute.line);
        }

        private static void
        ValidationHandler_RegexIsMatch(string name, object value, ValidateMemberAttribute attribute)
        {
            string pattern = attribute.compare_to.ToString();
            if (pattern.IsNull())
            {
                WriteError(ErrorLevel.Major, "Failed to validate " + name.WrapQuotes() + " via regex. The regex pattern is null.", attribute.caller, attribute.source, attribute.line);

                return;
            }

            if (value.IsNull())
            {
                WriteError(ErrorLevel.Major, "Failed to validate " + name.WrapQuotes() + " via regex. The regex input is null.", attribute.caller, attribute.source, attribute.line);

                return;
            }

            Regex regex = new Regex(attribute.compare_to.ToString());
            if (regex.IsMatch(value.ToString()))
            {
                return;
            }

            WriteError(attribute.level, name.WrapQuotes() + " did not match the regex " + pattern.WrapQuotes(), attribute.caller, attribute.source, attribute.line);
        }

        private static void
        ValidationHandler_RegexNoMatch(string name, object value, ValidateMemberAttribute attribute)
        {
            string pattern = attribute.compare_to.ToString();
            if (pattern.IsNull())
            {
                WriteError(ErrorLevel.Major, "Failed to validate " + name.WrapQuotes() + " via regex. The regex pattern is null.", attribute.caller, attribute.source, attribute.line);

                return;
            }

            if (value.IsNull())
            {
                WriteError(ErrorLevel.Major, "Failed to validate " + name.WrapQuotes() + " via regex. The regex input is null.", attribute.caller, attribute.source, attribute.line);

                return;
            }

            Regex regex = new Regex(attribute.compare_to.ToString());
            if (!regex.IsMatch(value.ToString()))
            {
                return;
            }

            WriteError(attribute.level, name.WrapQuotes() + " cannot match the regex " + pattern.WrapQuotes(), attribute.caller, attribute.source, attribute.line);
        }

        private static void
        ValidationHandler_Tags(object obj, string name, Type type, object value, ValidateMemberAttribute attribute)
        {
            if (obj.IsNull() || value.IsNull())
            {
                return;
            }

            IrcMessage message = GetIrcMessage(obj);
            if (message.IsNull() || !message.tags_exist)
            {
                return;
            }

            MemberInfo[] irc_tags = value.GetType().GetMembers<IrcTagAttribute>();
            if (!irc_tags.IsValid())
            {
                return;
            }

            List<string> processed_tags_names = new List<string>();
            foreach (MemberInfo member in irc_tags)
            {
                processed_tags_names.Add(member.GetAttribute<IrcTagAttribute>().tag);
            }

            // Check for any missing tags
            if (attribute.check == Check.TagsMissing && TryGetMissingTags(message, processed_tags_names, out string[] missing_tag_names))
            {
                string error = "Tags are missing and not being parsed in " + name + Environment.NewLine +
                               "> Missing tags: " + string.Join(",", missing_tag_names);
                WriteError(ErrorLevel.Major, error, attribute.caller, attribute.source, attribute.line);
            }

            // Check for any extra tags
            if (attribute.check == Check.TagsExtra && TryGetExtraTags(message, processed_tags_names, out string[] extra_tag_names))
            {
                string warning = "Tags are being parsed that don't exist in " + name + Environment.NewLine +
                                 "> Extra tags: " + string.Join(",", extra_tag_names);
                WriteWarning(ErrorLevel.Minor, warning, attribute.caller, attribute.source, attribute.line);
            }
        }

        private static IrcMessage
        GetIrcMessage(object obj)
        {
            MemberInfo _message = obj.GetType().GetMember("irc_message")[0];

            PropertyInfo property = _message as PropertyInfo;
            FieldInfo field = _message as FieldInfo;
            if (!property.IsNull() && property.PropertyType == typeof(IrcMessage))
            {
                return (IrcMessage)(property.GetValue(obj));
            }
            else if (!field.IsNull() && field.FieldType == typeof(IrcMessage))
            {
                return (IrcMessage)(field.GetValue(obj));
            }

            return default;
        }

        private static bool
        TryGetMissingTags(IrcMessage message, List<string> processed_tags_names, out string[] missing_tag_names)
        {
            if (!message.tags_exist)
            {
                missing_tag_names = new string[0];

                return false;
            }

            if (!processed_tags_names.IsValid())
            {
                missing_tag_names = message.tags.keys;

                return false;
            }

            List<string> missing = new List<string>();
            foreach (IrcTag tag in message.tags)
            {
                if (processed_tags_names.Contains(tag.key))
                {
                    continue;
                }

                missing.Add(tag.key);
            }

            missing_tag_names = missing.ToArray();

            return missing_tag_names.IsValid();
        }

        private static bool
        TryGetExtraTags(IrcMessage message, List<string> processed_tags_names, out string[] extra_tag_names)
        {
            if (!message.tags_exist)
            {
                extra_tag_names = message.tags.keys;

                return false;
            }

            if (!processed_tags_names.IsValid())
            {
                extra_tag_names = new string[0];

                return false;
            }

            List<string> extra = new List<string>();
            foreach (string tag in processed_tags_names)
            {
                if (message.tags.ContainsKey(tag))
                {
                    continue;
                }

                extra.Add(tag);
            }

            extra_tag_names = extra.ToArray();

            return extra_tag_names.IsValid();
        }

        #endregion

        #region Helpers

        /// <summary>
        /// Attempts to cast a member to a property or field.
        /// </summary>
        /// <param name="obj">The object that constains the member.</param>
        /// <param name="member">The member to cast.</param>
        /// <param name="name">The member's name.</param>
        /// <param name="type">The member's non-reflected type.</param>
        /// <param name="value">The member's value.</param>
        /// <returns>
        /// Returns true if the memebr was able to be cast to a property or member.
        /// Returns false otherwise.
        /// </returns>
        private static bool
        TryCastMember(object obj, MemberInfo member, out string name, out Type type, out object value)
        {
            name = string.Empty;
            type = null;
            value = null;

            if (member.IsNull())
            {
                return false;
            }

            name = member.Name;

            PropertyInfo property = member as PropertyInfo;
            FieldInfo field = member as FieldInfo;
            if (!property.IsNull())
            {
                type = property.PropertyType;
                value = property.GetValue(obj);

                return true;
            }
            else if (!field.IsNull())
            {
                type = field.FieldType;
                value = field.GetValue(obj);

                return true;
            }

            return false;
        }

        #endregion
    }
}
