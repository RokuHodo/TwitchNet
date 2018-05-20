// standard namespaces
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;

// project namespaces
using TwitchNet.Enums.Debugger;
using TwitchNet.Extensions;
using TwitchNet.Models.Clients.Irc;

namespace
TwitchNet.Debugger
{
    internal static partial class
    Debug
    {
        #region Member validation

        public static void
        ValidateMembers(object obj)
        {
            string obj_name = obj.GetType().Name.WrapQuotes();
            if (obj.IsNullOrDefault())
            {
                WriteError(ErrorLevel.Major, "Failed to validate the members for the object " + obj_name + ". The object is null or default.");

                return;
            }

            MemberInfo[] members_enter      = obj.GetType().GetMembers().ExcludeMembers(typeof(ValidateTagAttribute));
            MemberInfo[] members_validate   = members_enter.FilterMembers(typeof(ValidateMemberAttribute));

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

            ValidateNestedMembers(obj, members_enter);
        }

        private static void
        ValidateNestedMembers(object obj, MemberInfo[] members)
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

        private static void
        ValidateMember(object parent, MemberInfo member)
        {
            if (!member.TryGetAttributes(out ValidateMemberAttribute[] attributes))
            {
                return;
            }

            foreach(ValidateMemberAttribute attribute in attributes)
            {
                ValidateMember(parent, member, attribute);
            }
        }

        private static void
        ValidateMember(object parent, MemberInfo member, ValidateMemberAttribute attribute)
        {
            if (attribute.IsNull())
            {
                return;
            }

            if(!TryCastMember(parent, member, out string name, out Type type, out object value))
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
                    ValidationHandler_IsValid(parent, name, type, value, attribute);
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

                case Check.Tags:
                {
                    ValidationHandler_Tags(parent, name, type, value, attribute);
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
        ValidationHandler_IsValid(object obj, string name, Type type, object value, ValidateMemberAttribute attribute)
        {
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
        ValidationHandler_Tags(object parent, string name, Type type, object value, ValidateMemberAttribute attribute)
        {
            IrcMessage irc_message = null;

            string target = "irc_message";
            MemberInfo[] members = parent.GetType().GetMember(target);
            foreach(MemberInfo member in members)
            {
                PropertyInfo property = member as PropertyInfo;
                FieldInfo field = member as FieldInfo;
                if (!property.IsNull() && property.PropertyType == typeof(IrcMessage))
                {
                    irc_message = (IrcMessage)(property.GetValue(parent));
                }
                else if (!field.IsNull() && field.FieldType == typeof(IrcMessage))
                {
                    irc_message = (IrcMessage)(field.GetValue(parent));
                }

                if (!irc_message.IsNull())
                {
                    break;
                }
            }

            if (irc_message.IsNull())
            {
                WriteError(ErrorLevel.Major, "Failed to validate the object's IRC tags. No member with the name " + target.WrapQuotes() + " and type " + nameof(IrcMessage).WrapQuotes() + " was found to compare against.");

                return;
            }            

            ValidateTags(value, irc_message, true, attribute.caller, attribute.source, attribute.line);
        }

        #endregion

        #region Tag validation

        internal static void
        ValidateTags(object obj, IrcMessage irc_message, bool validate_members = false, [CallerMemberName] string caller = "", [CallerFilePath] string source = "", [CallerLineNumber] int line = -1)
        {
            string obj_name = obj.GetType().Name.WrapQuotes();
            if (obj.IsNull())
            {
                WriteError(ErrorLevel.Major, "Failed to validate the IRC tags for " + obj_name + ". The object is null.");

                return;
            }

            if (validate_members)
            {
                ValidateMembers(obj);
            }

            if (irc_message.IsNull())
            {
                WriteError(ErrorLevel.Major, "Failed to validate the IRC tags for " + obj_name + ". " + nameof(irc_message).WrapQuotes() + " is null.");

                return;
            }            

            if (!irc_message.tags.IsValid())
            {
                WriteWarning(ErrorLevel.Minor, "Failed to validate the IRC tags for " + obj_name + ". " + nameof(irc_message).WrapQuotes() + " does not contain tags.");

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
            if (TryGetMissingTags(irc_message, processed_tags_names, out string[] missing_tag_names))
            {
                string message = "Tags are missing and not being parsed in " + obj_name + Environment.NewLine +
                                 "> Missing tags: " + string.Join(",", missing_tag_names);
                WriteError(ErrorLevel.Major, message, caller, source, line);
            }

            // Check for any extra tags
            if (TryGetExtraTags(irc_message, processed_tags_names, out string[] extra_tag_names))
            {
                string message = "Tags are being parsed that don't exist in " + obj_name + Environment.NewLine +
                                 "> Extra tags: " + string.Join(",", extra_tag_names);
                WriteWarning(ErrorLevel.Minor, message, caller, source, line);
            }

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

            ValidateNestedMembers(obj, members_enter.ToArray());
        }


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

        #region Helpers

        private static bool
        TryCastMember(object parent, MemberInfo member, out string name, out Type type, out object value)
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
                value = property.GetValue(parent);

                return true;
            }
            else if (!field.IsNull())
            {
                type = field.FieldType;
                value = field.GetValue(parent);

                return true;
            }

            return false;
        }

        #endregion

        #endregion
    }
}
