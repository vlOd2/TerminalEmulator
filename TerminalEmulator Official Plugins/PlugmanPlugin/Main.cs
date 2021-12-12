using System;
using System.Linq;
using TerminalEmulator;
using TerminalEmulator.Classes;
using TerminalEmulator.Interfaces;

namespace PlugmanPlugin
{
    public class PlugmanPlugin : Plugin
    {
        public override string pAN { get; set; }
        public override string pluginName { get; } = "Plugman";
        public override string pluginVersion { get; } = "1.0";
        public override Program theProgram { get; set; }

        public CommandHandler cmdHandler;

        public override void onEnable()
        {
            this.cmdHandler = new CommandHandler(this);

            this.theProgram.cmdExecuter.registerCommand("plugman", this);
        }

        public override void onDisable()
        {
            this.cmdHandler = null;

            this.theProgram.cmdExecuter.unregisterCommand("plugman");
        }

        public override void onExecute(string cmdName, string[] cmdArgs, bool anyArgs)
        {
            if (cmdName.Equals("plugman"))
            {
                if (!anyArgs)
                {
                    Console.WriteLine("Maybe try \"plugman help\"?");
                    return;
                }

                this.cmdHandler.runSubCommand(cmdArgs[0], cmdArgs.Skip(1).ToArray());
            }
        }

        public override void onHelp()
        {
            Console.WriteLine("plugman - Manages plugins");
        }
    }
}
