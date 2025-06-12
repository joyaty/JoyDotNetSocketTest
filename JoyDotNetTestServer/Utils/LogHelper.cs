
namespace Joy.Util
{
    public enum EnumLogLevel
    {
        DEBUG,
        WARNING,
        ERROR,
    }

    public class LogHelper
    {
        public static void Debug(string message)
        {
            string messageWithTime = StringFormatUtil.Format("[{0}] {1}", System.DateTime.Now.ToString(), message);
            InternalLog(EnumLogLevel.DEBUG, messageWithTime);
        }

        public static void Debug<T>(string format, T param)
        {
            string formatWithTime = StringFormatUtil.Format("[{0}] {1}", System.DateTime.Now.ToString(), format);
            InternalLog(EnumLogLevel.DEBUG, StringFormatUtil.Format(formatWithTime, param));
        }

        public static void Debug<T1, T2>(string format, T1 param1, T2 param2)
        {
            string formatWithTime = StringFormatUtil.Format("[{0}] {1}", System.DateTime.Now.ToString(), format);
            InternalLog(EnumLogLevel.DEBUG, StringFormatUtil.Format(formatWithTime, param1, param2));
        }

        public static void Debug<T1, T2, T3>(string format, T1 param1, T2 param2, T3 param3)
        {
            string formatWithTime = StringFormatUtil.Format("[{0}] {1}", System.DateTime.Now.ToString(), format);
            InternalLog(EnumLogLevel.DEBUG, StringFormatUtil.Format(formatWithTime, param1, param2, param3));
        }

        public static void Debug<T1, T2, T3, T4>(string format, T1 param1, T2 param2, T3 param3, T4 param4)
        {
            string formatWithTime = StringFormatUtil.Format("[{0}] {1}", System.DateTime.Now.ToString(), format);
            InternalLog(EnumLogLevel.DEBUG, StringFormatUtil.Format(formatWithTime, param1, param2, param3, param4));
        }

        public static void Debug<T1, T2, T3, T4, T5>(string format, T1 param1, T2 param2, T3 param3, T4 param4, T5 param5)
        {
            string formatWithTime = StringFormatUtil.Format("[{0}] {1}", System.DateTime.Now.ToString(), format);
            InternalLog(EnumLogLevel.DEBUG, StringFormatUtil.Format(formatWithTime, param1, param2, param3, param4, param5));
        }

        public static void Debug<T1, T2, T3, T4, T5, T6>(string format, T1 param1, T2 param2, T3 param3, T4 param4, T5 param5, T6 param6)
        {
            string formatWithTime = StringFormatUtil.Format("[{0}] {1}", System.DateTime.Now.ToString(), format);
            InternalLog(EnumLogLevel.DEBUG, StringFormatUtil.Format(formatWithTime, param1, param2, param3, param4, param5, param6));
        }

        public static void Debug<T1, T2, T3, T4, T5, T6, T7>(string format, T1 param1, T2 param2, T3 param3, T4 param4, T5 param5, T6 param6, T7 param7)
        {
            string formatWithTime = StringFormatUtil.Format("[{0}] {1}", System.DateTime.Now.ToString(), format);
            InternalLog(EnumLogLevel.DEBUG, StringFormatUtil.Format(formatWithTime, param1, param2, param3, param4, param5, param6, param7));
        }

        public static void Debug<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(string format, T1 param1, T2 param2, T3 param3, T4 param4, T5 param5, T6 param6, T7 param7, T8 param8, T9 param9, T10 param10, T11 param11)
        {
            string formatWithTime = StringFormatUtil.Format("[{0}] {1}", System.DateTime.Now.ToString(), format);
            InternalLog(EnumLogLevel.DEBUG, StringFormatUtil.Format(formatWithTime, param1, param2, param3, param4, param5, param6, param7, param8, param9, param10, param11));
        }

        public static void Debug(string format, params object[] args)
        {
            string formatWithTime = StringFormatUtil.Format("[{0}] {1}", System.DateTime.Now.ToString(), format);
            InternalLog(EnumLogLevel.DEBUG, StringFormatUtil.Format(formatWithTime, args));
        }

        public static void Warning(string message)
        {
            string messageWithTime = StringFormatUtil.Format("[{0}] {1}", System.DateTime.Now.ToString(), message);
            InternalLog(EnumLogLevel.WARNING, messageWithTime);
        }

        public static void Warning<T>(string format, T param)
        {
            string formatWithTime = StringFormatUtil.Format("[{0}] {1}", System.DateTime.Now.ToString(), format);
            InternalLog(EnumLogLevel.WARNING, StringFormatUtil.Format(formatWithTime, param));
        }

        public static void Warning<T1, T2>(string format, T1 param1, T2 param2)
        {
            string formatWithTime = StringFormatUtil.Format("[{0}] {1}", System.DateTime.Now.ToString(), format);
            InternalLog(EnumLogLevel.WARNING, StringFormatUtil.Format(formatWithTime, param1, param2));
        }

        public static void Warning<T1, T2, T3>(string format, T1 param1, T2 param2, T3 param3)
        {
            string formatWithTime = StringFormatUtil.Format("[{0}] {1}", System.DateTime.Now.ToString(), format);
            InternalLog(EnumLogLevel.WARNING, StringFormatUtil.Format(formatWithTime, param1, param2, param3));
        }

        public static void Warning<T1, T2, T3, T4>(string format, T1 param1, T2 param2, T3 param3, T4 param4)
        {
            string formatWithTime = StringFormatUtil.Format("[{0}] {1}", System.DateTime.Now.ToString(), format);
            InternalLog(EnumLogLevel.WARNING, StringFormatUtil.Format(formatWithTime, param1, param2, param3, param4));
        }

        public static void Warning<T1, T2, T3, T4, T5>(string format, T1 param1, T2 param2, T3 param3, T4 param4, T5 param5)
        {
            string formatWithTime = StringFormatUtil.Format("[{0}] {1}", System.DateTime.Now.ToString(), format);
            InternalLog(EnumLogLevel.WARNING, StringFormatUtil.Format(formatWithTime, param1, param2, param3, param4, param5));
        }

        public static void Warning(string format, params object[] args)
        {
            string formatWithTime = StringFormatUtil.Format("[{0}] {1}", System.DateTime.Now.ToString(), format);
            InternalLog(EnumLogLevel.WARNING, StringFormatUtil.Format(formatWithTime, args));
        }

        public static void Error(string message)
        {
            string messageWithTime = StringFormatUtil.Format("[{0}] {1}", System.DateTime.Now.ToString(), message);
            InternalLog(EnumLogLevel.ERROR, messageWithTime);
        }

        public static void Error<T>(string format, T param)
        {
            string formatWithTime = StringFormatUtil.Format("[{0}] {1}", System.DateTime.Now.ToString(), format);
            InternalLog(EnumLogLevel.ERROR, StringFormatUtil.Format(formatWithTime, param));
        }

        public static void Error<T1, T2>(string format, T1 param1, T2 param2)
        {
            string formatWithTime = StringFormatUtil.Format("[{0}] {1}", System.DateTime.Now.ToString(), format);
            InternalLog(EnumLogLevel.ERROR, StringFormatUtil.Format(formatWithTime, param1, param2));
        }

        public static void Error<T1, T2, T3>(string format, T1 param1, T2 param2, T3 param3)
        {
            string formatWithTime = StringFormatUtil.Format("[{0}] {1}", System.DateTime.Now.ToString(), format);
            InternalLog(EnumLogLevel.ERROR, StringFormatUtil.Format(formatWithTime, param1, param2, param3));
        }

        public static void Error<T1, T2, T3, T4>(string format, T1 param1, T2 param2, T3 param3, T4 param4)
        {
            string formatWithTime = StringFormatUtil.Format("[{0}] {1}", System.DateTime.Now.ToString(), format);
            InternalLog(EnumLogLevel.ERROR, StringFormatUtil.Format(formatWithTime, param1, param2, param3, param4));
        }

        public static void Error<T1, T2, T3, T4, T5>(string format, T1 param1, T2 param2, T3 param3, T4 param4, T5 param5)
        {
            string formatWithTime = StringFormatUtil.Format("[{0}] {1}", System.DateTime.Now.ToString(), format);
            InternalLog(EnumLogLevel.ERROR, StringFormatUtil.Format(formatWithTime, param1, param2, param3, param4, param5));
        }

        public static void Error<T1, T2, T3, T4, T5, T6>(string format, T1 param1, T2 param2, T3 param3, T4 param4, T5 param5, T6 param6)
        {
            string formatWithTime = StringFormatUtil.Format("[{0}] {1}", System.DateTime.Now.ToString(), format);
            InternalLog(EnumLogLevel.ERROR, StringFormatUtil.Format(formatWithTime, param1, param2, param3, param4, param5, param6));
        }

        public static void Error<T1, T2, T3, T4, T5, T6, T7>(string format, T1 param1, T2 param2, T3 param3, T4 param4, T5 param5, T6 param6, T7 param7)
        {
            string formatWithTime = StringFormatUtil.Format("[{0}] {1}", System.DateTime.Now.ToString(), format);
            InternalLog(EnumLogLevel.ERROR, StringFormatUtil.Format(formatWithTime, param1, param2, param3, param4, param5, param6, param7));
        }

        public static void Error(string format, params object[] args)
        {
            string formatWithTime = StringFormatUtil.Format("[{0}] {1}", System.DateTime.Now.ToString(), format);
            InternalLog(EnumLogLevel.ERROR, StringFormatUtil.Format(formatWithTime, args));
        }

        public static void Assert(bool val, string msg = "")
        {
            if (!val)
            {
                InternalLog(EnumLogLevel.ERROR, msg);
            }
        }

        public static void Assert<T>(bool val, string strFormat, T param)
        {
            if (!val)
            {
                InternalLog(EnumLogLevel.ERROR, StringFormatUtil.Format(strFormat, param));
            }
        }

        public static void Assert<T1, T2>(bool val, string strFormat, T1 param1, T2 param2)
        {
            if (!val)
            {
                InternalLog(EnumLogLevel.ERROR, StringFormatUtil.Format(strFormat, param1, param2));
            }
        }

        public static void Assert<T1, T2, T3>(bool val, string strFormat, T1 param1, T2 param2, T3 param3)
        {
            if (!val)
            {
                InternalLog(EnumLogLevel.ERROR, StringFormatUtil.Format(strFormat, param1, param2, param3));
            }
        }

        public static void Assert<T1, T2, T3, T4>(bool val, string strFormat, T1 param1, T2 param2, T3 param3, T4 param4)
        {
            if (!val)
            {
                InternalLog(EnumLogLevel.ERROR, StringFormatUtil.Format(strFormat, param1, param2, param3, param4));
            }
        }

        public static void Assert<T1, T2, T3, T4, T5>(bool val, string strFormat, T1 param1, T2 param2, T3 param3, T4 param4, T5 param5)
        {
            if (!val)
            {
                InternalLog(EnumLogLevel.ERROR, StringFormatUtil.Format(strFormat, param1, param2, param3, param4, param5));
            }
        }

        /// <summary>
        /// 内部Log打印实现
        /// </summary>
        /// <param name="logLevel"></param>
        /// <param name="content"></param>
        private static void InternalLog(EnumLogLevel logLevel, string content)
        {
            switch (logLevel)
            {
                case EnumLogLevel.DEBUG:
                    Console.WriteLine(content);
                    break;
                case EnumLogLevel.WARNING:
                    Console.WriteLine(content);
                    break;
                case EnumLogLevel.ERROR:
                    Console.WriteLine(content);
                    break;
            }
        }
    }
}