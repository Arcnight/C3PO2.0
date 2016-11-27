using System;

using Serilog;

namespace C3PO.Utilities
{
    public static class Logging
    {
        [Flags]
        public enum LoggingTypes
        {
            Console,
            Database,
            RollingFile
        }

        public static ILogger CreateLogger()
        {
            var loggerConfig = CreateConsoleConfig();

            return loggerConfig.CreateLogger();
        }

        static LoggerConfiguration CreateConsoleConfig()
        {
            return new LoggerConfiguration().WriteTo.ColoredConsole();
        }
    }
}
