// standard namespaces
using System;
using System.Collections;
using System.Reflection;
using System.Runtime.CompilerServices;

// project namespaces
using TwitchNet.Enums.Debug;
using TwitchNet.Extensions;

namespace
TwitchNet.Debug
{
    internal static class
    Log
    {
        #region Fields

        private static LogLevel _level = LogLevel.All;

        #endregion

        #region Properties

        /// <summary>
        /// A bitfield that determines what will be printed out when running.
        /// If the library is not in debug mode, nothing will be printed out.
        /// </summary>
        public static LogLevel level
        {
            get
            {
                return _level;
            }
            set
            {
#if !DEBUG
                _level = LogLevel.None;
#else
                _level = value;
#endif
            }
        }

        #endregion

        #region Printing

        public static void
        PrintLine(string value)
        {
            PrintLine(ConsoleColor.Gray, TimeStamp.None, value);
        }

        public static void
        PrintLine(TimeStamp stamp, string value)
        {
            PrintLine(ConsoleColor.Gray, stamp, value);
        }

        public static void
        PrintLine(ConsoleColor color, string value)
        {
            PrintLine(color, TimeStamp.None, value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void
        PrintLine(ConsoleColor color, TimeStamp stamp, string value)
        {
            PrintLine(color, stamp, value, null);
        }

        public static void
        PrintLine(string format, params object[] parameters)
        {
            PrintLine(ConsoleColor.Gray, TimeStamp.None, format, parameters);
        }

        public static void
        PrintLine(TimeStamp stamp, string format, params object[] parameters)
        {
            PrintLine(ConsoleColor.Gray, stamp, format, parameters);
        }

        public static void
        PrintLine(ConsoleColor color, string format, params object[] parameters)
        {
            PrintLine(color, TimeStamp.None, format, parameters);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void
        PrintLine(ConsoleColor color, TimeStamp stamp, string format, params object[] parameters)
        {
            DebugPrintLine(LogLevel.Detailed, color, stamp, format, parameters);
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void
        DebugPrintLine(LogLevel level, ConsoleColor color, TimeStamp stamp, string format, params object[] parametrs)
        {
            if (!format.IsValid())
            {
                return;
            }

            // TODO: Replace this with manual bitwise since this is *really* slow.
            if (level == LogLevel.None || !level.HasFlag(level))
            {
                return;
            }

            string value = GetTimeStampString(stamp);
            if (value.IsValid())
            {
                format = value + " " + format;
            }

            if(color != ConsoleColor.Gray)
            {
                Console.ForegroundColor = color;
            }

            if (parametrs.IsValid())
            {
                Console.WriteLine(format, parametrs);
            }
            else
            {
                Console.WriteLine(format);
            }

            if (color != ConsoleColor.Gray)
            {
                Console.ResetColor();
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static string
        GetTimeStampString(TimeStamp stamp)
        {
            string time = string.Empty;

            switch (stamp)
            {
                case TimeStamp.None:
                {
                    time = string.Empty;
                }
                break;

                case TimeStamp.DateLong:
                {
                    time = "[ " + DateTime.Now.ToLongDateString() + " ]";
                }
                break;

                case TimeStamp.DateShort:
                {
                    time = "[ " + DateTime.Now.ToShortDateString() + " ]";
                }
                break;

                case TimeStamp.TimeLong:
                {
                    time = "[ " + DateTime.Now.ToLongTimeString() + " ]";
                }
                break;

                case TimeStamp.TimeShort:
                {
                    time = "[ " + DateTime.Now.ToShortTimeString() + " ]";
                }
                break;

                case TimeStamp.Default:
                default:
                {
                    time = "[ " + DateTime.Now.ToString() + " ]";
                }
                break;
            }

            return time;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void
        BlankLine()
        {
            Console.WriteLine();
        }

        #endregion

        #region Object dumping

        /// <summary>
        /// Prints all properties and fields of an object and all sub objects.
        /// </summary>        
        public static void
        PrintObject(object obj)
        {
            PrintObject(string.Empty, obj);
        }

        /// <summary>
        /// Prints all properties and fields of an object and all sub objects with a specified prefix.
        /// </summary>
        public static void
        PrintObject(string label, object obj)
        {
            if (obj == null || obj is ValueType || obj is DateTime || obj is string)
            {
                string value = GetObjectValueString(obj);
                // TODO: Reimplement all printing functions 
                // PrintLineColumns(label, value);
            }
            else if (obj is IEnumerable)
            {
                foreach (object element in (IEnumerable)obj)
                {
                    PrintObject(label, element);
                }
            }
            else
            {
                MemberInfo[] members = obj.GetType().GetMembers(BindingFlags.Public | BindingFlags.Instance);

                foreach (MemberInfo member in members)
                {
                    FieldInfo field_info = member as FieldInfo;
                    PropertyInfo property_info = member as PropertyInfo;

                    if (field_info != null || property_info != null)
                    {
                        // get the type of the member
                        Type obj_member_type = field_info != null ? field_info.FieldType : property_info.PropertyType;

                        // get the value of the member as an object
                        object obj_member_value = field_info != null ? field_info.GetValue(obj) : property_info.GetValue(obj, null);
                        string obj_member_value_string = GetObjectValueString(obj_member_value);

                        // there is nothing to print, the object has sub properties/fields to print
                        if (obj_member_value_string == string.Empty)
                        {
                            BlankLine();
                            // Header(member.Name);
                            PrintObject(string.Empty, obj_member_value);
                        }
                        else
                        {
                            // PrintLineColumns(member.Name, obj_member_value_string);
                        }
                    }
                }
            }
        }

        private static string
        GetObjectValueString(object obj)
        {
            string value = string.Empty;

            // get the string version of the value to print
            if (obj is DateTime)
            {
                value = ((DateTime)obj).ToLongDateString();
            }
            else if (obj is ValueType || obj is string)
            {
                value = obj.ToString();
            }
            else if (obj == null)
            {
                value = "null";
            }

            return value;
        }

        #endregion
    }
}
