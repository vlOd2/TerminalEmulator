using System;
using System.Collections.Generic;
using System.Text;
using TerminalEmulator.Interfaces;

#pragma warning disable
namespace PlugmanPlugin
{
    public class CommandHandler
    {
        private IPlugin thePlugin;

        public CommandHandler(IPlugin thePlugin)
        {
            this.thePlugin = thePlugin;
        }

        public void displayHelp()
        {
            Console.WriteLine("plugman help - Displays help");
            Console.WriteLine("plugman load <name> - Load plugin");
            Console.WriteLine("plugman unload <name> - Unload plugin");
            Console.WriteLine("plugman reload <name> - Reload plugin");
            Console.WriteLine("plugman loadall - Loads all plugins");
            Console.WriteLine("plugman unloadall - Unloads all plugins");
        }

        public void runSubCommand(string subCommand, string[] subCommandData)
        {
            if (subCommand == "help")
            {
                displayHelp();
            }
            else if (subCommand == "load")
            {
                this.thePlugin.theProgram.pluginManager.loadPlugin(subCommandData[0]);
            }
            else if (subCommand == "unload")
            {
                this.thePlugin.theProgram.pluginManager.unloadPlugin(this.thePlugin.theProgram.pluginManager.getPlugin(subCommandData[0]));
            }
            else if (subCommand == "reload")
            {
                this.thePlugin.theProgram.pluginManager.unloadPlugin(this.thePlugin.theProgram.pluginManager.getPlugin(subCommandData[0]));
                this.thePlugin.theProgram.pluginManager.loadPlugin(subCommandData[0]);
            }
            else if (subCommand == "loadall")
            {
                this.thePlugin.theProgram.pluginManager.loadPlugins();
            }
            else if (subCommand == "unloadall")
            {
                this.thePlugin.theProgram.pluginManager.unloadPlugins();
            }
            else
            {
                Console.WriteLine("Maybe try \"plugman help\"?");
            }
        }
    }
}
