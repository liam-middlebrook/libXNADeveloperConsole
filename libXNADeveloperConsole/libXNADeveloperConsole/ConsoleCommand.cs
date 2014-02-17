using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace libXNADeveloperConsole
{
    /// <summary>
    /// An implementation of IConsoleCommand.
    /// </summary>
    public class ConsoleCommand : IConsoleCommand
    {
        private string name;

        private ConsoleCommandDelegate function;

        /// <summary>
        /// The Name the the console command will be addressed by in code.
        /// </summary>
        public string Name { get { return name; } }

        /// <summary>
        /// The delegate that is called when the user types in this command
        /// </summary>
        public ConsoleCommandDelegate Function { get { return function; } }

        /// <summary>
        /// Creates a new Instance of Console Command
        /// </summary>
        /// <param name="name">The Name the the console command will be addressed by in code.</param>
        /// <param name="function">The delegate that is called when the user types in this command</param>
        public ConsoleCommand(string name, ConsoleCommandDelegate function)
        {
            this.name = name;
            this.function = function;
        }
    }
}
