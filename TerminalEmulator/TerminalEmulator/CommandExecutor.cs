using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TerminalEmulator.Interfaces;

#pragma warning disable
namespace TerminalEmulator
{
    public class CommandExecuter
    {
        /// <summary>
        /// The registered system commands (Cannot be registered)
        /// </summary>
        public readonly string[] systemCommands = new string[] { "help", "exit", "reload", "panic" };
        /// <summary>
        /// The terminal logger, used for logs
        /// </summary>
        public Logger cLogger;
        /// <summary>
        /// The registered commands
        /// </summary>
        public Dictionary<string, IPlugin> registeredCommands = new Dictionary<string, IPlugin>();
        /// <summary>
        /// Bool that allows commands to be executed
        /// </summary>
        public bool commandsEnabled = true;

        /// <summary>
        /// A class used to handle commands
        /// </summary>
        public CommandExecuter(Logger cLogger)
        {
            this.cLogger = cLogger;
        }

        /// <summary>
        /// Register a new command from a plugin
        /// </summary>
        /// <param name="cmdName">Command name</param>
        /// <param name="cmdPlugin">Command plugin</param>
        public void registerCommand(string cmdName, IPlugin cmdPlugin)
        {
            // Check if the command already exists
            if (!this.registeredCommands.ContainsKey(cmdName))
            {
                // Add the new command
                this.registeredCommands.Add(cmdName, cmdPlugin);
            }
            // If so, cancel call
            else
            {
                return;
            }
        }

        /// <summary>
        /// Unregister an existing command
        /// </summary>
        /// <param name="cmdName">Command name</param>
        public void unregisterCommand(string cmdName)
        {
            // Check if the command does exist
            if (this.registeredCommands.ContainsKey(cmdName))
            {
                // Go thru each registered command
                foreach (KeyValuePair<string, IPlugin> registeredCmd in this.registeredCommands.ToList())
                {
                    // Check if the registered command is the same as the requested one
                    if (registeredCmd.Key == cmdName)
                    {
                        // If so, remove the requested command
                        this.registeredCommands.Remove(registeredCmd.Key);
                    }
                }
            }
            // If not, cancel call
            else
            {
                return;
            }
        }

        /// <summary>
        /// Execute a command
        /// </summary>
        /// <param name="rawUserInput">Raw user input</param>
        /// <returns>Command result</returns>
        public bool executeCommand(string rawUserInput)
        {
            // Check if commands are not enabled
            if (!this.commandsEnabled)
            {
                Console.WriteLine("Commands are disabled!");
                return false;
            }

            // Split the raw user input
            string[] splittedUserCmd = rawUserInput.Split((char)0x20);

            // Parse the splitted user input
            string userCmd = splittedUserCmd[0];
            string[] userArgs = splittedUserCmd.Skip(1).ToArray();

            // Check if there are any arguments
            bool anyArgs = (userArgs.Length < 1) ? false : true;

            // Check if the command is a system command
            if (this.systemCommands.Contains(userCmd))
            {
                this.executeSystemCommand(userCmd);
                return true;
            }

            // Go thru each registered command
            foreach (KeyValuePair<string, IPlugin> registeredCmd in this.registeredCommands)
            {
                // Check if the user command is the same as the registered one
                if (registeredCmd.Key == userCmd)
                {
                    // If so, call execute on the associated plugin
                    try
                    {
                        registeredCmd.Value.onExecute(userCmd, userArgs, anyArgs);
                    }
                    catch (Exception ex)
                    {
                        Program.thisProgram.handleError(
                            "onExecute", 
                            "An unhandled exception has occured.",
                            ex, 
                            true
                        );
                    }

                    return true;
                }
            }

            Console.WriteLine("Invalid command. Type \"help\" to show available commands.");
            return false;
        }

        /// <summary>
        /// Execute a system command
        /// </summary>
        /// <param name="systemCommand">System command</param>
        private void executeSystemCommand(string systemCommand)
        {
            // Check if the command is "help"
            if (systemCommand == this.systemCommands[0])
            {
                Console.WriteLine("--------");
                Console.WriteLine("  Help");
                Console.WriteLine("--------");
                Console.WriteLine("");
                Console.WriteLine("System commands:");
                Console.WriteLine("");
                Console.WriteLine("help - Displays help information");
                Console.WriteLine("exit - Quits the application");
                Console.WriteLine("reload - Reloads all plugins");
                Console.WriteLine("panic - Panics the program (Intended for debug usage)");
                Console.WriteLine("");
                Console.WriteLine("Plugin commands:");
                Console.WriteLine("");

                foreach (IPlugin loadedPlugin in Program.thisProgram.pluginManager.loadedPlugins)
                {
                    try
                    {
                        loadedPlugin.onHelp();
                    }
                    catch (Exception ex)
                    {
                        Program.thisProgram.handleError(
                            "onHelp",
                            "An unhandled exception has occured.",
                            ex,
                            true
                        );
                    }
                }
            }
            // Check if the command is "exit"
            else if (systemCommand == this.systemCommands[1])
            {
                Program.thisProgram.shutdown();
            }
            // Check if the command is "reload"
            else if (systemCommand == this.systemCommands[2])
            {
                Program.thisProgram.pluginManager.unloadPlugins();
                Program.thisProgram.pluginManager.loadPlugins();
            }
            // Check if the command is "panic"
            else if (systemCommand == this.systemCommands[3]) 
            {
                try 
                {
                    int patch = 0;
                    int boost = 10 / patch;
                }
                catch (Exception ex) 
                {
                    Program.thisProgram.panic(ex.Message, ex.HResult, ex);
                }
                //Program.thisProgram.panic("Manually triggered panic", 000000000);
            }
            else
            {
                return;
            }
        }
    }
}
