using System;
using System.Collections.Generic;
using System.Text;

#pragma warning disable
namespace TerminalEmulator.Interfaces
{
    /// <summary>
    /// A simple plugin interface
    /// </summary>
    [Obsolete("It is not recommended to use the raw interface, please use the Plugin class instead.")]
    public interface IPlugin
    {
        /// <summary>
        /// DO NOT CHANGE THIS
        /// </summary>
        string pAN { get; set; }
        /// <summary>
        /// The plugin's name
        /// </summary>
        string pluginName { get; }
        /// <summary>
        /// The plugin's version
        /// </summary>
        string pluginVersion { get; }
        /// <summary>
        /// The program instance the plugin is running on
        /// </summary>
        Program theProgram { get; set; }

        /// <summary>
        /// Function called when the plugin is enabled
        /// </summary>
        void onEnable();
        /// <summary>
        /// Function called when the plugin is disabled
        /// </summary>
        void onDisable();

        /// <summary>
        /// Function called when an registered command is executed
        /// </summary>
        /// <param name="cmdName">Executed command's name</param>
        /// <param name="cmdArgs">Executed command's arguments</param>
        /// <param name="anyArgs">If there are any arguments</param>
        void onExecute(string cmdName, string[] cmdArgs, bool anyArgs);
        /// <summary>
        /// Function called when the "help" command is executed<para/>
        /// Example usage: Console.WriteLine("example - Command used to show an example message");
        /// </summary>
        void onHelp();
    }
}
