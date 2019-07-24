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

        /// <summary>
        /// Recursively validates the members of an object marked with <see cref="ValidateMemberAttribute"/>.
        /// Any member whose type is marked with <see cref="ValidateObjectAttribute"/> will also be recursively validated.
        /// </summary>
        /// <param name="obj">The object to validate.</param>
        [Conditional("DEBUG")]
        public static void
        ValidateMembers(object obj)
        {
            string obj_name = obj.GetType().Name.WrapQuotes();
            if (obj.IsNullOrDefault())
            {
                WriteError(ErrorLevel.Major, "Failed to validate the members for the object " + obj_name + ". The object is null or default.");

                return;
            }

            MemberInfo[] members_enter      = obj.GetType().GetMembers().ExcludeMembersByAttribute(typeof(ValidateTagAttribute));
            MemberInfo[] members_validate   = members_enter.FilterMembersByAttribute(typeof(ValidateMemberAttribute));

            if (!members_validate.IsValid())
            {
                WriteWarning(ErrorLevel.Minor, "Failed to validate the members for the object " + obj_name + ". No members are marked with the " + nameof(ValidateMemberAttribute).WrapQuotes() + " attribute.");
            }
            else
            {
                foreach(MemberInfo member in members_validate)
                {
                    ValidateMember(obj, member);
                }
            }

            ValidateMembers(obj, members_enter);
        }

        /// <summary>
        /// Scans the specified members of an object that may <see cref="ValidateObjectAttribute"/>.
        /// Any member marked with <see cref="ValidateObjectAttribute"/> will then be validated using <see cref="ValidateMembers(object)"/>.
        /// </summary>
        /// <param name="obj">The object that constains the specified members.</param>
        /// <param name="members">The members to scan.</param>
        [Conditional("DEBUG")]
        public static void
        ValidateMembers(object obj, MemberInfo[] members)
        {
            if (!members.IsValid())
            {
                return;
            }

            foreach (MemberInfo member in members)
            {
                if (!TryCastMember(obj, member, out string name, out Type type, out object value))
                {
                    continue;
                }

                // This will fail if an type is an array or a list, regardless if it is marked with the attribute.
                // In that case we need to check the element type to see if it's a type worth caring about.
                if (type.HasAttribute<ValidateObjectAttribute>())
                {
                    ValidateMembers(value);
                }
                else if ((type.IsArray || type.IsList()) && type.GetElementType().HasAttribute<ValidateObjectAttribute>())
                {
                    IList temp = value as IList;
                    foreach (object element in temp)
                    {
                        ValidateMembers(element);
                    }
                }
            }
        }

        /// <summary>
        /// Validates a member marked with <see cref="ValidateMemberAttribute"/>.
        /// </summary>
        /// <param name="obj">The object that constains the specified member.</param>
        /// <param name="member">The member to validate./</param>
        [Conditional("DEBUG")]
        public static void
        ValidateMember(object obj, MemberInfo member)
        {
            if (!member.TryGetAttributes(out ValidateMemberAttribute[] attributes))
            {
                return;
            }

            foreach(ValidateMemberAttribute attribute in attributes)
            {
                ValidateMember(obj, member, attribute);
            }
        }

        /// <summary>
        /// Validates a member marked with <see cref="ValidateMemberAttribute"/> using the specified <see cref="ValidateMemberAttribute"/>.
        /// </summary>
        /// <param name="obj">The object that constains the specified member.</param>
        /// <param name="member">The member to validate.</param>
        /// <param name="attribute">The attribute containing the validation information.</param>
        [Conditional("DEBUG")]
        public static void
        ValidateMember(object obj, MemberInfo member, ValidateMemberAttribute attribute)
        {
            if (attribute.IsNull())
            {
                return;
            }

            if(!TryCastMember(obj, member, out string name, out Type type, out object value))
            {
                return;
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

                case Check.Tags:
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

        /// <summary>
        /// The validation handler for the check, <see cref="Check.IsNull"/>.
        /// </summary>
        /// <param name="name">The member's name.</param>
        /// <param name="value">The value of the member.</param>
        /// <param name="attribute">The assiciated validation attribute.</param>
        private static void
        ValidationHandler_IsNull(string name, object value, ValidateMemberAttribute attribute)
        {
            if (value.IsNull())
            {
                return;
            }

            WriteError(attribute.level, name.WrapQuotes() + " must be null.", attribute.caller, attribute.source, attribute.line);
        }

        /// <summary>
        /// The validation handler for the check, <see cref="Check.IsNotNull"/>.
        /// </summary>
        /// <param name="name">The member's name.</param>
        /// <param name="value">The value of the member.</param>
        /// <param name="attribute">The assiciated validation attribute.</param>
        private static void
        ValidationHandler_IsNotNull(string name, object value, ValidateMemberAttribute attribute)
        {
            if (!value.IsNull())
            {
                return;
            }

            WriteError(attribute.level, name.WrapQuotes() + " cannot be null.", attribute.caller, attribute.source, attribute.line);
        }

        /// <summary>
        /// The validation handler for the check, <see cref="Check.IsDefault"/>.
        /// </summary>
        /// <param name="name">The member's name.</param>
        /// <param name="type">The member's non-reflected type.</param>
        /// <param name="value">The value of the member.</param>
        /// <param name="attribute">The assiciated validation attribute.</param>
        private static void
        ValidationHandler_IsDefault(string name, Type type, object value, ValidateMemberAttribute attribute)
        {
            if (value.IsDefault())
            {
                return;
            }

            WriteError(attribute.level, name.WrapQuotes() + " must be equal to the default value for " + type.Name + ", " + type.GetDefaultValue().ToString().WrapQuotes() + ".", attribute.caller, attribute.source, attribute.line);
        }

        /// <summary>
        /// The validation handler for the check, <see cref="Check.IsNotDefault"/>.
        /// </summary>
        /// <param name="name">The member's name.</param>
        /// <param name="type">The member's non-reflected type.</param>
        /// <param name="value">The value of the member.</param>
        /// <param name="attribute">The assiciated validation attribute.</param>
        private static void
        ValidationHandler_IsNotDefault(string name, Type type, object value, ValidateMemberAttribute attribute)
        {
            if (!value.IsDefault())
            {
                return;    
            }

            WriteError(attribute.level, name.WrapQuotes() + " cannot be equal to the default value for " + type.Name + ", " + type.GetDefaultValue().ToString().WrapQuotes() + ".", attribute.caller, attribute.source, attribute.line);
        }

        /// <summary>
        /// The validation handler for the check, <see cref="Check.IsNullOrDefault"/>.
        /// </summary>
        /// <param name="name">The member's name.</param>
        /// <param name="type">The member's non-reflected type.</param>
        /// <param name="value">The value of the member.</param>
        /// <param name="attribute">The assiciated validation attribute.</param>
        private static void
        ValidationHandler_IsNullOrDefault(string name, Type type, object value, ValidateMemberAttribute attribute)
        {
            if (value.IsNullOrDefault())
            {
                return;
            }

            WriteError(attribute.level, name.WrapQuotes() + " must be null or equal to the default value for " + type.Name + ", " + type.GetDefaultValue().ToString().WrapQuotes() + ".", attribute.caller, attribute.source, attribute.line);
        }

        /// <summary>
        /// The validation handler for the check, <see cref="Check.IsNotNullOrDefault"/>.
        /// </summary>
        /// <param name="name">The member's name.</param>
        /// <param name="type">The member's non-reflected type.</param>
        /// <param name="value">The value of the member.</param>
        /// <param name="attribute">The assiciated validation attribute.</param>
        private static void
        ValidationHandler_IsNotNullOrDefault(string name, Type type, object value, ValidateMemberAttribute attribute)
        {
            if (!value.IsNullOrDefault())
            {
                return;
            }

            WriteError(attribute.level, name.WrapQuotes() + " cannot be null or equal to the default value for " + type.Name + ", " + type.GetDefaultValue().ToString().WrapQuotes() + ".", attribute.caller, attribute.source, attribute.line);
        }

        /// <summary>
        /// The validation handler for the check, <see cref="Check.IsValid"/>.
        /// </summary>
        /// <param name="obj">The object that contains the member.</param>
        /// <param name="name">The member's name.</param>
        /// <param name="type">The member's non-reflected type.</param>
        /// <param name="value">The value of the member.</param>
        /// <param name="attribute">The assiciated validation attribute.</param>
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

        /// <summary>
        /// The validation handler for the check, <see cref="Check.IsInvalid"/>.
        /// </summary>
        /// <param name="obj">The object that contains the member.</param>
        /// <param name="name">The member's name.</param>
        /// <param name="type">The member's non-reflected type.</param>
        /// <param name="value">The value of the member.</param>
        /// <param name="attribute">The assiciated validation attribute.</param>
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

        /// <summary>
        /// The validation handler for the check, <see cref="Check.IsEqualTo"/>.
        /// </summary>
        /// <param name="name">The member's name.</param>
        /// <param name="value">The value of the member.</param>
        /// <param name="attribute">The assiciated validation attribute.</param>
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

        /// <summary>
        /// The validation handler for the check, <see cref="Check.IsNotEqualTo"/>.
        /// </summary>
        /// <param name="name">The member's name.</param>
        /// <param name="value">The value of the member.</param>
        /// <param name="attribute">The assiciated validation attribute.</param>
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

        /// <summary>
        /// The validation handler for the check, <see cref="Check.RegexIsMatch"/>.
        /// </summary>
        /// <param name="name">The member's name.</param>
        /// <param name="value">The value of the member.</param>
        /// <param name="attribute">The assiciated validation attribute.</param>
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

        /// <summary>
        /// The validation handler for the check, <see cref="Check.RegexNoMatch"/>.
        /// </summary>
        /// <param name="name">The member's name.</param>
        /// <param name="value">The value of the member.</param>
        /// <param name="attribute">The assiciated validation attribute.</param>
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

        /// <summary>
        /// The validation handler for the check, <see cref="Check.Tags"/>.
        /// </summary>
        /// <param name="obj">The object that contains the member.</param>
        /// <param name="name">The member's name.</param>
        /// <param name="type">The member's non-reflected type.</param>
        /// <param name="value">The value of the member.</param>
        /// <param name="attribute">The assiciated validation attribute.</param>
        private static void
        ValidationHandler_Tags(object obj, string name, Type type, object value, ValidateMemberAttribute attribute)
        {
            IrcMessage irc_message = default;

            string target = "irc_message";
            MemberInfo[] members = obj.GetType().GetMember(target);
            foreach(MemberInfo member in members)
            {
                PropertyInfo property = member as PropertyInfo;
                FieldInfo field = member as FieldInfo;
                if (!property.IsNull() && property.PropertyType == typeof(IrcMessage))
                {
                    irc_message = (IrcMessage)(property.GetValue(obj));
                }
                else if (!field.IsNull() && field.FieldType == typeof(IrcMessage))
                {
                    irc_message = (IrcMessage)(field.GetValue(obj));
                }

                if (!irc_message.IsNull())
                {
                    break;
                }
            }

            if (irc_message.IsNullOrDefault())
            {
                WriteError(ErrorLevel.Major, "Failed to validate the object's IRC tags. No member with the name " + target.WrapQuotes() + " and type " + nameof(IrcMessage).WrapQuotes() + " was found to compare against.");

                return;
            }            

            ValidateTags(value, irc_message, true, attribute.caller, attribute.source, attribute.line);
        }

        #endregion

        #region Tag validation

        /// <summary>
        /// Validates the members of an object marked with <see cref="ValidateTagAttribute"/>.
        /// Members marked with <see cref="ValidateTagAttribute"/> are then validated normally if the associated tag exists within the <see cref="IrcMessage"/>.
        /// </summary>
        /// <param name="obj">The object to validate.</param>
        /// <param name="message">The IRC message containing the existing tags.</param>
        /// <param name="validate_normal_members">Whether or not to also validate members marked with <see cref="ValidateMemberAttribute"/>.</param>
        /// <param name="caller">The caller of the method.</param>
        /// <param name="source">The source file of the caller.</param>
        /// <param name="line">The line of the caller.</param>
        [Conditional("DEBUG")]
        public static void
        ValidateTags(object obj, IrcMessage message, bool validate_normal_members = false, [CallerMemberName] string caller = "", [CallerFilePath] string source = "", [CallerLineNumber] int line = -1)
        {
            if (obj.IsNull())
            {
                WriteError(ErrorLevel.Major, "Failed to validate the IRC tags for " + obj.ToString() + ". The object is null.");

                return;
            }

            string obj_name = obj.GetType().Name.WrapQuotes();

            if (validate_normal_members)
            {
                ValidateMembers(obj);
            }

            if (message.IsNull())
            {
                WriteError(ErrorLevel.Major, "Failed to validate the IRC tags for " + obj_name + ". " + nameof(message).WrapQuotes() + " is null.");

                return;
            }            

            if (!message.tags.IsValid())
            {
                WriteWarning(ErrorLevel.Minor, "Failed to validate the IRC tags for " + obj_name + ". " + nameof(message).WrapQuotes() + " does not contain tags.");

                return;
            }

            MemberInfo[] members = obj.GetType().GetMembers<ValidateTagAttribute>();
            if (!members.IsValid())
            {
                WriteWarning(ErrorLevel.Minor, "Failed to validate the IRC tags for " + obj_name + ". No members are marked with the " + nameof(ValidateTagAttribute).WrapQuotes() + " attribute.");

                return;
            }

            List<ValidateTagAttribute> attributes = new List<ValidateTagAttribute>();
            List<string> processed_tags_names = new List<string>();
            foreach(MemberInfo member in members)
            {
                ValidateTagAttribute attribute = member.GetAttribute<ValidateTagAttribute>();
                attributes.Add(attribute);
                processed_tags_names.Add(attribute.tag);
            }

            // Check for any missing tags
            if (TryGetMissingTags(message, processed_tags_names, out string[] missing_tag_names))
            {
                string error = "Tags are missing and not being parsed in " + obj_name + Environment.NewLine +
                               "> Missing tags: " + string.Join(",", missing_tag_names);
                WriteError(ErrorLevel.Major, error, caller, source, line);
            }

            // Check for any extra tags
            if (TryGetExtraTags(message, processed_tags_names, out string[] extra_tag_names))
            {
                string warning = "Tags are being parsed that don't exist in " + obj_name + Environment.NewLine +
                                 "> Extra tags: " + string.Join(",", extra_tag_names);
                WriteWarning(ErrorLevel.Minor, warning, caller, source, line);
            }

            // Perform validation checks on tags that exist and were processed
            List<MemberInfo> members_enter = new List<MemberInfo>();
            for(int index = 0; index < members.Length; index++)
            {
                if (extra_tag_names.Contains(attributes[index].tag))
                {
                    continue;
                }

                members_enter.Add(members[index]);

                ValidateMember(obj, members[index], attributes[index]);
            }

            ValidateMembers(obj, members_enter.ToArray());
        }

        /// <summary>
        /// Attempts to get tags that are included in the <see cref="IrcMessage"/> but were not processed by members marked with <see cref="ValidateTagAttribute"/>.
        /// </summary>
        /// <param name="message">The IRC message containing the existing tags.</param>
        /// <param name="processed_tags_names">The list of tags that were attempted to be processed.</param>
        /// <param name="missing_tag_names">
        /// <para>The tags that were included in the <see cref="IrcMessage"/> but were not processed.</para>
        /// <para>Set to an empty array if the <see cref="IrcMessage"/> contains no tags or no tags were processed.</para>
        /// </param>
        /// <returns>
        /// Returns true if at least one tag was not processed in the <see cref="IrcMessage"/>.
        /// Returns false otherwise.
        /// </returns>
        private static bool
        TryGetMissingTags(IrcMessage message, List<string> processed_tags_names, out string[] missing_tag_names)
        {
            if (!message.tags.IsValid())
            {
                missing_tag_names = new string[0];

                return false;
            }

            if (!processed_tags_names.IsValid())
            {
                missing_tag_names = message.tags.Keys.ToArray();

                return false;
            }

            List<string> missing = new List<string>();
            foreach (string key in message.tags.Keys)
            {
                if (processed_tags_names.Contains(key))
                {
                    continue;
                }

                missing.Add(key);
            }

            missing_tag_names = missing.ToArray();

            return missing_tag_names.IsValid();
        }

        /// <summary>
        /// Attempts to get tags that were processed by members marked with <see cref="ValidateTagAttribute"/> but are not included in the <see cref="IrcMessage"/>.
        /// </summary>
        /// <param name="message">The IRC message containing the existing tags.</param>
        /// <param name="processed_tags_names">The list of tags that were attempted to be processed.</param>
        /// <param name="extra_tag_names">
        /// <para>The tags that were processed but not included in the <see cref="IrcMessage"/>.</para>
        /// <para>Set to an empty array if no tags were processed or no extra tags were processed.</para>
        /// </param>
        /// <returns>
        /// Returns true if at least one tag was processed that does not exist in the <see cref="IrcMessage"/>.
        /// Returns false otherwise.
        /// </returns>
        private static bool
        TryGetExtraTags(IrcMessage message, List<string> processed_tags_names, out string[] extra_tag_names)
        {
            if (!message.tags.IsValid())
            {
                extra_tag_names = message.tags.Keys.ToArray();

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
                if (message.tags.Keys.Contains(tag))
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
