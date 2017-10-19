// standard namespaces
using System;
using System.Collections;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;

//project namespaces
using TwitchNet.Enums.Debug;
using TwitchNet.Extensions;

namespace TwitchNet.Debug
{
    internal static class Log
    {
        #region Success, error, and warning messages

        /// <summary>
        /// A bitfield that determines what will be printed out when running.
        /// </summary>
        public static LogLevel level = LogLevel.All;

        public static void Success(params string[] lines)
        {
            Success(TimeStamp.None, lines);
        }

        public static void Success(TimeStamp stamp, params string[] lines)
        {
            DebugHeader(LogLevel.Success, ConsoleColor.Green, stamp, "[ Success ]", lines);
        }

        public static void Error(params string[] lines)
        {
            Error(TimeStamp.None, lines);
        }

        public static void Error(TimeStamp stamp, params string[] lines)
        {
            DebugHeader(LogLevel.Errors, ConsoleColor.Red, stamp, "[ Error ]", lines);
        }

        public static void Warning(params string[] lines)
        {
            Warning(TimeStamp.None, lines);
        }

        public static void Warning(TimeStamp stamp, params string[] lines)
        {
            DebugHeader(LogLevel.Warnings, ConsoleColor.Yellow, stamp, "[ Warning ]", lines);
        }

        #endregion

        #region Notifies and headers

        public static void Header(string header, params string[] lines)
        {
            Header(TimeStamp.None, header, lines);
        }

        public static void Header(TimeStamp stamp, string header, params string[] lines)
        {
            DebugHeader(LogLevel.Headers, ConsoleColor.Cyan, stamp, header, lines);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void DebugHeader(LogLevel flag, ConsoleColor color, TimeStamp stamp, string header, params string[] lines)
        {
            string[] print = new string[lines.Length + 1];
            print[0] = header;
            Array.Copy(lines, 0, print, 1, lines.Length);

            DebugPrintLine(flag, color, stamp, print);
        }

        #endregion

        #region Printing

        #region No carriage return

        public static void Print(params string[] words)
        {
            Print(ConsoleColor.Gray, TimeStamp.None, words);
        }

        public static void Print(ConsoleColor color, params string[] words)
        {
            Print(color, TimeStamp.None, words);
        }

        public static void Print(TimeStamp stamp, params string[] words)
        {
            Print(ConsoleColor.Gray, stamp, words);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Print(ConsoleColor color, TimeStamp stamp, params string[] words)
        {
            DebugPrint(LogLevel.Detailed, color, stamp, words);
        }

        #endregion

        #region Carriage return

        public static void PrintLine(params string[] lines)
        {
            PrintLine(ConsoleColor.Gray, TimeStamp.None, lines);
        }

        public static void PrintLine(ConsoleColor color, params string[] lines)
        {
            PrintLine(color, TimeStamp.None, lines);
        }

        public static void PrintLine(TimeStamp stamp, params string[] lines)
        {
            PrintLine(ConsoleColor.Gray, stamp, lines);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void PrintLine(ConsoleColor color, TimeStamp stamp, params string[] lines)
        {
            DebugPrintLine(LogLevel.Detailed, color, stamp, lines);
        }

        public static void PrintLineColumns(string label, string text)
        {
            PrintLineColumns(ConsoleColor.Gray, TimeStamp.None, label, text);
        }

        public static void PrintLineColumns(ConsoleColor color, string label, string text)
        {
            PrintLineColumns(color, TimeStamp.None, label, text);
        }

        public static void PrintLineColumns(TimeStamp stamp, string label, string text)
        {
            PrintLineColumns(ConsoleColor.Gray, stamp, label, text);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void PrintLineColumns(ConsoleColor color, TimeStamp stamp, string label, string text)
        {
            string message = string.Format("{0,-20} {1,-20}", label, text);

            PrintLine(color, stamp, message);
        }

        #endregion

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void DebugPrintLine(LogLevel flag, ConsoleColor color, TimeStamp stamp, params string[] lines)
        {
            PrintBuilder(flag, color, stamp, true, lines);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void DebugPrint(LogLevel flag, ConsoleColor color, TimeStamp stamp, params string[] words)
        {
            PrintBuilder(flag, color, stamp, false, words);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void PrintBuilder(LogLevel flag, ConsoleColor color, TimeStamp stamp, bool carriage_return, params string[] lines)
        {
            if (!lines.IsValid())
            {
                return;
            }

            //HasFlag(DebugLevel.None) always evaluates to true since None = 0, need to test this case separatelty
            if (level == LogLevel.None)
            {
                return;
            }

            if (!level.HasFlag(flag) && !level.HasFlag(LogLevel.All))
            {
                return;
            }

            string time = GetTimeStampString(stamp);

            StringBuilder builder = new StringBuilder();
            builder.Append(time);

            foreach (string line in lines)
            {
                if (!line.IsValid())
                {
                    continue;
                }

                if (carriage_return)
                {
                    builder.AppendLine(line);
                }
                else
                {
                    builder.Append(lines);
                }
            }

            Console.ForegroundColor = color;
            Console.Write(builder);
            Console.ResetColor();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static string GetTimeStampString(TimeStamp stamp)
        {
            string time = "[ {0} ] ";

            switch (stamp)
            {
                case TimeStamp.None:
                    {
                        time = string.Empty;
                    }
                    break;
                case TimeStamp.DateLong:
                    {
                        time = string.Format(time, DateTime.Now.ToLongDateString());
                    }
                    break;
                case TimeStamp.DateShort:
                    {
                        time = string.Format(time, DateTime.Now.ToShortDateString());
                    }
                    break;
                case TimeStamp.TimeLong:
                    {
                        time = string.Format(time, DateTime.Now.ToLongTimeString());
                    }
                    break;
                case TimeStamp.TimeShort:
                    {
                        time = string.Format(time, DateTime.Now.ToShortTimeString());
                    }
                    break;
                case TimeStamp.Default:
                default:
                    {
                        time = string.Format(time, DateTime.Now.ToString());
                    }
                    break;
            }

            return time;
        }

        /// <summary>
        /// Prints a blank line to the command line.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void BlankLine()
        {
            Console.WriteLine();
        }

        #endregion        

        #region Object dumping

        /// <summary>
        /// Prints all properties and fields of an object and all sub objects.
        /// </summary>        
        public static void PrintObject(object obj)
        {
            PrintObject(string.Empty, obj);
        }

        /// <summary>
        /// Prints all properties and fields of an object and all sub objects with a specified prefix.
        /// </summary>
        public static void PrintObject(string label, object obj)
        {
            if (obj == null || obj is ValueType || obj is DateTime || obj is string)
            {
                string value = GetObjectValueString(obj);
                PrintLineColumns(label, value);
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
                            Header(member.Name);
                            PrintObject(string.Empty, obj_member_value);
                        }
                        else
                        {
                            PrintLineColumns(member.Name, obj_member_value_string);
                        }
                    }
                }
            }
        }

        private static string GetObjectValueString(object obj)
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

        #region Formatting

        public static string FormatColumns(object column_1, object column_2, int column_1_align = -20, int column_2_align = -20)
        {
            return string.Format("{0," + column_1_align + "} {1," + column_2_align + "}", column_1, column_2);
        }

        #endregion
    }
}
