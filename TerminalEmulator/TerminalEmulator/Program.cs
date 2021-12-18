using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using TerminalEmulator.Interfaces;

#pragma warning disable
namespace TerminalEmulator
{
    public class Program
    {
        /// <summary>
        /// The current instance of the program
        /// </summary>
        public static Program thisProgram;

        /// <summary>
        /// The entry point of the program
        /// </summary>
        /// <param name="args">Program args</param>
        static void Main(string[] args)
        {
            Program.thisProgram = new Program();
            Program.thisProgram.Main2();
        }

        /// <summary>
        /// The terminal input symbol
        /// </summary>
        public readonly char terminalSymbol = '>';
        /// <summary>
        /// The terminal logger, used for logs
        /// </summary>
        public readonly Logger cLogger = new Logger();
        /// <summary>
        /// The plugin manager for the plugins
        /// </summary>
        public PluginManager pluginManager;
        /// <summary>
        /// The command executer
        /// </summary>
        public CommandExecuter cmdExecuter;
        /// <summary>
        /// This bool determines if the program loop should be running
        /// </summary>
        public bool isRunning = false;
        /// <summary>
        /// This bool indicates if the program has panicked, changing the value doesn't have any effect
        /// </summary>
        public bool isPanicked = false;

        /// <summary>
        /// Displays program information
        /// </summary>
        private void writeInfo()
        {
            Console.WriteLine("");
            Console.WriteLine("---------------------");
            Console.WriteLine("  Terminal Emulator");
            Console.WriteLine("---------------------");
            Console.WriteLine("An expandable terminal-like program.");
            Console.WriteLine("");
        }

        /// <summary>
        /// Handles any error that has occured
        /// </summary>
        /// <param name="errorAction">The action that was attempted</param>
        /// <param name="errorMessage">The error message for the action</param>
        /// <param name="errorException">The exception that has occured</param>
        /// <param name="displayException">If the exception should be displayed</param>
        public void handleError(string errorAction, 
            string errorMessage, 
            Exception errorException = null, 
            bool displayException = false)
        {
            this.cLogger.Log(Level.ERROR, $"Could not perform the action \"{errorAction}\": {errorMessage}");

            if (displayException)
                this.cLogger.Log(Level.ERROR, errorException.ToString());
        }

        /// <summary>
        /// The main function of terminal emulator
        /// </summary>
        private void Main2()
        {
            // Create the variables
            this.pluginManager = new PluginManager();
            this.cmdExecuter = new CommandExecuter(cLogger);

            // Create the plugins directory (if it doesn't exist)
            if (!Directory.Exists(this.pluginManager.pluginsDir))
            {
                Directory.CreateDirectory(this.pluginManager.pluginsDir);
            }

            // Load the plugins (if any)
            this.pluginManager.loadPlugins();

            // Set isRunning to true
            isRunning = true;

            // Write program information
            this.writeInfo();

            // Program loop
            while (isRunning)
            {
                // Write the terminal symbol
                Console.Write($"{this.terminalSymbol.ToString()} ");

                // Get the raw user input
                string rawUserInput = Console.ReadLine();

                // Execute the command
                this.cmdExecuter.executeCommand(rawUserInput);
            }
        }

        /// <summary>
        /// Causes the program to panic. After this function is called, the program will hang
        /// </summary>
        /// <param name="panicMessage">The message used in the panic</param>
        /// <param name="panicCode">The code used in the panic</param>
        /// <param name="stackTraceException">The exception used as a stacktrace (Optional)</param>
        [Obsolete("Usage not recommended! " +
            "For errors, log to the screen using cLogger. " +
            "For closing the program use the shutdown() method instead.")]
        public void panic(string panicMessage, int panicCode, Exception stackTraceException = null) 
        {
            // Set the program to not be running
            this.isRunning = false;

            // Set the panicked value to be true
            this.isPanicked = true;

            // Calls onPanic() on every plugin
            foreach (IPlugin loadedPlugin in this.pluginManager.loadedPlugins) 
            {
                loadedPlugin.onPanic();
            }

            // Assign default exception if null
            if (stackTraceException == null) 
            {
                stackTraceException = new Exception(panicMessage);
            }

            // Show panic message
            Console.Clear();
            Console.WriteLine($"[PANIC] THE PROGRAM HAS PANICKED");
            Console.WriteLine($"[PANIC] THE PROGRAM EXECUTION HAS BEEN HALTED");
            Console.WriteLine($"[PANIC]");
            Console.WriteLine($"[PANIC] 0x{panicCode.ToString().PadLeft(9, '0').Substring(0, 9)}");
            Console.WriteLine(
                $"[PANIC] " +
                $"{stackTraceException.ToString().Replace(Environment.NewLine, $"{Environment.NewLine}[PANIC]")}"
            );
            Console.WriteLine($"[PANIC]");
            Console.WriteLine($"[PANIC] {panicMessage.ToUpper().Replace(Environment.NewLine, $"{Environment.NewLine}[PANIC]")}");

            // Hang the program
            while (true) 
            {
                continue;
            }
        }

        /// <summary>
        /// Cleanly exits the program
        /// </summary>
        public void shutdown() 
        {
            // Unloads all the plugin
            this.pluginManager.unloadPlugins();

            // Set the program status to not be loaded 
            this.isRunning = false;

            // Quit the program
            Environment.Exit(0);
        }
    }
}
