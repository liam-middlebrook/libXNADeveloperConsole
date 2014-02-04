using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XNA_DevConsole.DevConsole
{
    class ConsoleCommand : IConsoleCommand
    {
        string name;

        ConsoleCommandDelegate function;

        public string Name { get { return name; } }

        public ConsoleCommandDelegate Function { get { return function; } }

        public ConsoleCommand(string name, ConsoleCommandDelegate function)
        {
            this.name = name;
            this.function = function;
        }
    }
}
