using System;
using TerminalEmulator;
using TerminalEmulator.Classes;
using TerminalEmulator.Interfaces;

namespace EchoPlugin
{
    public class EchoPlugin : Plugin
    {
        public override string pAN { get; set; }
        public override string pluginName { get; } = "Echo";
        public override string pluginVersion { get; } = "1.0";
        public override Program theProgram { get; set; }

        public override void onEnable()
        {
            this.theProgram.cmdExecuter.registerCommand("echo", this);
        }

        public override void onDisable()
        {
            this.theProgram.cmdExecuter.unregisterCommand("echo");
        }

        public override void onExecute(string cmdName, string[] cmdArgs, bool anyArgs)
        {
            if (cmdName.Equals("echo"))
            {
                Console.WriteLine((anyArgs) ? string.Join((char)0x20, cmdArgs) : "No message was provided.");
            }
        }

        public override void onHelp()
        {
            Console.WriteLine("echo <text> - Displays a string to the screen");
        }
    }
}
