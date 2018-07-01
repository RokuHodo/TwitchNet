// standard namespaces
using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;

// project namespaces
using TwitchNet.Extensions;

namespace
TwitchNet.Debugger
{
    internal static partial class
    Debug
    {
        #region Fields

        /// <summary>
        /// The path to the Log.txt file.
        /// </summary>
        private static readonly string LOG_PATH = Environment.CurrentDirectory + "/../../../Log.txt";

        /// <summary>
        /// Determines what will be printed. Bitfield enum.
        /// </summary>
        public static DebugLevel print_filter       = DebugLevel.All;

        /// <summary>
        /// <para>Determines what errors or warnings will be printed. Bitfield enum.</para>
        /// <para>A sub filter to <see cref="print_filter"/>.</para>
        /// </summary>
        public static ErrorLevel print_error_filter = ErrorLevel.All;

        /// <summary>
        /// Determines what will be logged. Bitfield enum.
        /// </summary>
        public static DebugLevel log_filter         = DebugLevel.Warning | DebugLevel.Error;

        /// <summary>
        /// <para>Determines what errors or warnings will be logged. Bitfield enum.</para>
        /// <para>A sub filter to <see cref="log_filter"/>.</para>
        /// </summary>
        public static ErrorLevel log_error_filter   = ErrorLevel.All;

        #endregion

        #region General writing

        /// <summary>
        /// Writes a blank line to the output stream.
        /// </summary>
        [Conditional("DEBUG")]
        public static void
        WriteLine()
        {
            WriteLine("");
        }

        /// <summary>
        /// Writes a string to the output stream.
        /// </summary>
        /// <param name="value">The text to write.</param>
        [Conditional("DEBUG")]
        public static void
        WriteLine(string value)
        {            
            WriteLine(ConsoleColor.Gray, TimeStamp.None, value);
        }

        /// <summary>
        /// Writes a string to the output stream.
        /// </summary>
        /// <param name="stamp">The time stamp format.</param>
        /// <param name="value">The text to write.</param>
        [Conditional("DEBUG")]
        public static void
        WriteLine(TimeStamp stamp, string value)
        {
            WriteLine(ConsoleColor.Gray, stamp, value);
        }

        /// <summary>
        /// Writes a string to the output stream.
        /// </summary>
        /// <param name="color">The color of the text.</param>
        /// <param name="value">The text to write.</param>
        [Conditional("DEBUG")]
        public static void
        WriteLine(ConsoleColor color, string value)
        {
            WriteLine(color, TimeStamp.None, value);
        }

        /// <summary>
        /// Writes a string to the output stream.
        /// </summary>
        /// <param name="color">The color of the text.</param>
        /// <param name="stamp">The time stamp format.</param>
        /// <param name="value">The text to write.</param>
        [Conditional("DEBUG")]
        public static void
        WriteLine(ConsoleColor color, TimeStamp stamp, string value)
        {
            WriteLine(color, stamp, value, null);
        }

        /// <summary>
        /// Writes a string to the output stream using the specified format.
        /// </summary>
        /// <param name="format">A composite format string.</param>
        /// <param name="parameters">the objects to format.</param>
        [Conditional("DEBUG")]
        public static void
        WriteLine(string format, params object[] parameters)
        {
            WriteLine(ConsoleColor.Gray, TimeStamp.None, format, parameters);
        }

        /// <summary>
        /// Writes a string to the output stream using the specified format.
        /// </summary>
        /// <param name="stamp">The time stamp format.</param>
        /// <param name="format">A composite format string.</param>
        /// <param name="parameters">the objects to format.</param>
        [Conditional("DEBUG")]
        public static void
        WriteLine(TimeStamp stamp, string format, params object[] parameters)
        {
            WriteLine(ConsoleColor.Gray, stamp, format, parameters);
        }

        /// <summary>
        /// Writes a string to the output stream using the specified format.
        /// </summary>
        /// <param name="color">The color of the text.</param>
        /// <param name="format">A composite format string.</param>
        /// <param name="parameters">the objects to format.</param>
        public static void
        WriteLine(ConsoleColor color, string format, params object[] parameters)
        {
            WriteLine(color, TimeStamp.None, format, parameters);
        }

        /// <summary>
        /// Writes a string to the output stream using the specified format.
        /// </summary>
        /// <param name="color">The color of the text.</param>
        /// <param name="stamp">The time stamp format.</param>
        /// <param name="format">A composite format string.</param>
        /// <param name="parameters">the objects to format.</param>
        [Conditional("DEBUG")]
        public static void
        WriteLine(ConsoleColor color, TimeStamp stamp, string format, params object[] parameters)
        {
            _WriteLine(DebugLevel.General, ErrorLevel.None, color, stamp, format, parameters);
        }

        #endregion

        #region Debug writing

        /// <summary>
        /// Writes debug information to the output stream.
        /// </summary>
        /// <param name="value">The text to write.</param>
        [Conditional("DEBUG")]
        public static void
        WriteInfo(string value)
        {
            string message = "[ INFO ]" + Environment.NewLine +
                             "> " + value;
            _WriteLine(DebugLevel.Info, ErrorLevel.None, ConsoleColor.Gray, TimeStamp.TimeShort, message);
        }

        /// <summary>
        /// Writes a debug warning to the output stream.
        /// </summary>
        /// <param name="error_level">The severity of the warning.</param>
        /// <param name="value">The text to write.</param>
        /// <param name="caller">The caller of the method.</param>
        /// <param name="source">The source file the caller is located in.</param>
        /// <param name="line">The line of the caller.</param>
        [Conditional("DEBUG")]
        public static void
        WriteWarning(ErrorLevel error_level, string value, [CallerMemberName] string caller = "", [CallerFilePath] string source = "", [CallerLineNumber] int line = -1)
        {
            StackTrace trace = new StackTrace();
            StackFrame frame = trace.GetFrame(1);

            string message = "[ WARNING ] :: " + error_level.ToString() + Environment.NewLine +
                             "> Caller:  " + caller                     + Environment.NewLine +
                             "> Source:  " + source                     + Environment.NewLine +
                             "> Line:    " + line                       + Environment.NewLine +
                             "> Message: " + value;
            _WriteLine(DebugLevel.Warning, error_level, ConsoleColor.Yellow, TimeStamp.TimeShort, message);
        }

        /// <summary>
        /// Writes a debug error to the output stream.
        /// </summary>
        /// <param name="error_level">The severity of the error.</param>
        /// <param name="value">The text to write.</param>
        /// <param name="caller">The caller of the method.</param>
        /// <param name="source">The source file the caller is located in.</param>
        /// <param name="line">The line of the caller.</param>
        [Conditional("DEBUG")]
        public static void
        WriteError(ErrorLevel error_level, string value, [CallerMemberName] string caller = "", [CallerFilePath] string source = "", [CallerLineNumber] int line = -1)
        {            
            StackTrace trace = new StackTrace();
            StackFrame frame = trace.GetFrame(1);

            string message = "[ ERROR ] :: " + error_level.ToString()   + Environment.NewLine +
                             "> Caller:  " + caller                     + Environment.NewLine +
                             "> Source:  " + source                     + Environment.NewLine +
                             "> Line:    " + line                       + Environment.NewLine +
                             "> Message: " + value;
            _WriteLine(DebugLevel.Error, error_level, ConsoleColor.Red, TimeStamp.TimeShort, message);
        }

        #endregion

        #region Internal writing

        /// <summary>
        /// Writes a string to the output stream using the specified format.
        /// </summary>
        /// <param name="write_level">The type of message.</param>
        /// <param name="error_level">The severity of the message if it an error or warning.</param>
        /// <param name="color">The color of the text.</param>
        /// <param name="stamp">The time stamp format.</param>
        /// <param name="format">A composite format string.</param>
        /// <param name="parameters">the objects to format.</param>
        [Conditional("DEBUG")]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void
        _WriteLine(DebugLevel write_level, ErrorLevel error_level, ConsoleColor color, TimeStamp stamp, string format, params object[] parametrs)
        {
            string value = GetTimeStampString(stamp);
            if (value.IsValid())
            {
                format = value + " " + format;
            }
            string message = parametrs.IsValid() ? string.Format(format, parametrs) : format;

            PrintMessage(write_level, error_level, color, message);
            LogMessage(write_level, error_level, message);
        }

        /// <summary>
        /// Gets the string equivelant from the <see cref="TimeStamp"/> format.
        /// </summary>
        /// <param name="stamp">The time stamp format.</param>
        /// <returns>Returns a time stamp corresponding to the <see cref="TimeStamp"/> format.</returns>
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

        /// <summary>
        /// Writes a string to the commanmd prompt window.
        /// </summary>
        /// <param name="write_level">The type of message.</param>
        /// <param name="error_level">The severity of the message if it an error or warning.</param>
        /// <param name="color">The color of the text.</param>
        /// <param name="message">The message to write.</param>
        private static void
        PrintMessage(DebugLevel write_level, ErrorLevel error_level, ConsoleColor color, string message)
        {
            if (print_filter == DebugLevel.None)
            {
                return;
            }

            if ((print_filter & write_level) != write_level)
            {
                return;
            }

            if (write_level == DebugLevel.Error || write_level == DebugLevel.Warning)
            {
                if((print_error_filter & error_level) != error_level)
                {
                    return;
                }
            }

            if (color != ConsoleColor.Gray)
            {
                Console.ForegroundColor = color;
            }

            Console.WriteLine(message);

            if (color != ConsoleColor.Gray)
            {
                Console.ResetColor();
            }
        }

        /// <summary>
        /// Writes a string to the log file.
        /// </summary>
        /// <param name="write_level">The type of message.</param>
        /// <param name="error_level">The severity of the message if it an error or warning.</param>
        /// <param name="message">The message to write.</param>
        private static void
        LogMessage(DebugLevel write_level, ErrorLevel error_level, string message)
        {
            if (log_filter == DebugLevel.None)
            {
                return;
            }

            if ((log_filter & write_level) != write_level)
            {
                return;
            }

            if (write_level == DebugLevel.Error || write_level == DebugLevel.Warning)
            {
                if ((log_error_filter & error_level) != error_level)
                {
                    return;
                }
            }

            if (!File.Exists(LOG_PATH))
            {
                File.Create(LOG_PATH);
            }

            using (StreamWriter wrtier = File.AppendText(LOG_PATH))
            {
                wrtier.WriteLine(message);
            }
        }

        #endregion
    }
}
