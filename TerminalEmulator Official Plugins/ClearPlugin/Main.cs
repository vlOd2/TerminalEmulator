using System;
using TerminalEmulator;
using TerminalEmulator.Classes;
using TerminalEmulator.Interfaces;

namespace ClearPlugin
{
    public class ClearPlugin : Plugin
    {
        public override string pAN { get; set; }
        public override string pluginName { get; } = "Clear";
        public override string pluginVersion { get; } = "1.0";
        public override Program theProgram { get; set; }

        public override void onEnable()
        {
            this.theProgram.cmdExecuter.registerCommand("clear", this);
        }

        public override void onDisable()
        {
            this.theProgram.cmdExecuter.unregisterCommand("clear");
        }

        public override void onExecute(string cmdName, string[] cmdArgs, bool anyArgs)
        {
            if (cmdName.Equals("clear"))
            {
                Console.Clear();
            }
        }

        public override void onHelp()
        {
            Console.WriteLine("clear - Clears the screen");
        }
    }
}
