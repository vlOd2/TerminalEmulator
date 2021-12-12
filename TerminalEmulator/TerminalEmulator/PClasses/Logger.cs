using System;
using System.Collections.Generic;
using System.Text;

#pragma warning disable
namespace TerminalEmulator
{
    /// <summary>
    /// Enum used to specify the logger level
    /// </summary>
    public enum Level
    {
        INFO,
        WARN,
        ERROR,
        FATAL
    }

    public class Logger
    {
        /// <summary>
        /// Default logger level
        /// </summary>
        public Level defaultLevel;
        /// <summary>
        /// Get all of the logged messages
        /// </summary>
        public readonly List<string> loggedMessages = new List<string>();

        /// <summary>
        /// A class used to log to the console
        /// </summary>
        /// <param name="defaultLevel">Default level</param>
        public Logger(Level defaultLevel = Level.INFO)
        {
            this.defaultLevel = defaultLevel;
        }

        /// <summary>
        /// Log to the console with the default level
        /// </summary>
        /// <param name="logMessage">Log message</param>
        public void Log(string logMessage)
        {
            loggedMessages.Add(logMessage);

            Console.WriteLine(
                $"{DateTime.Now.ToString("HH:mm:ss")} [{defaultLevel.ToString()}] {logMessage}"
            );
        }

        /// <summary>
        /// Log to the console with a choosen level
        /// </summary>
        /// <param name="logLevel">Log level</param>
        /// <param name="logMessage">Log message</param>
        public void Log(Level logLevel, string logMessage)
        {
            loggedMessages.Add(logMessage);

            Console.WriteLine(
                $"{DateTime.Now.ToString("HH:mm:ss")} [{logLevel.ToString()}] {logMessage}"
            );
        }
    }
}
