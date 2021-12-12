using System;
using System.Collections.Generic;
using System.Text;
using TerminalEmulator.Interfaces;

#pragma warning disable
namespace TerminalEmulator.Classes
{
    /// <summary>
    /// A wrapped IPlugin class, great for beginners
    /// </summary>
    public class Plugin : IPlugin
    {
        /// <summary>
        /// DO NOT CHANGE THIS
        /// </summary>
        public virtual string pAN { get; set; }
        /// <summary>
        /// The plugin's name
        /// </summary>
        public virtual string pluginName { get; } = "My Plugin";
        /// <summary>
        /// The plugin's version
        /// </summary>
        public virtual string pluginVersion { get; } = "My Version";
        /// <summary>
        /// The program instance the plugin is running on
        /// </summary>
        public virtual Program theProgram { get; set; }

        /// <summary>
        /// Function called when the plugin is enabled
        /// </summary>
        public virtual void onEnable()
        {
            return;
        }

        /// <summary>
        /// Function called when the plugin is disabled
        /// </summary>
        public virtual void onDisable()
        {
            return;
        }

        /// <summary>
        /// Function called when an registered command is executed
        /// </summary>
        /// <param name="cmdName">Executed command's name</param>
        /// <param name="cmdArgs">Executed command's arguments</param>
        /// <param name="anyArgs">If there are any arguments</param>
        public virtual void onExecute(string cmdName, string[] cmdArgs, bool anyArgs)
        {
            return;
        }

        /// <summary>
        /// Function called when the "help" command is executed<para/>
        /// Example usage: Console.WriteLine("example - Command used to show an example message");
        /// </summary>
        public virtual void onHelp()
        {
            return;
        }
    }
}
