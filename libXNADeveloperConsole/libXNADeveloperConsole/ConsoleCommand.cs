using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace libXNADeveloperConsole
{
    public class ConsoleCommand : IConsoleCommand
    {
        private string name;

        private ConsoleCommandDelegate function;

        public string Name { get { return name; } }

        public ConsoleCommandDelegate Function { get { return function; } }

        public ConsoleCommand(string name, ConsoleCommandDelegate function)
        {
            this.name = name;
            this.function = function;
        }
    }
}
