using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace libXNADeveloperConsole
{
    /// <summary>
    /// A delegate for a console command that the user will type in.
    /// </summary>
    /// <param name="args">The arguments typed after the command name</param>
    /// <param name="loggingQueue">A logging queue for outputting information back to the console</param>
    /// <returns>The error status code of the command</returns>
    public delegate int ConsoleCommandDelegate(string args, LimitedMessageQueue loggingQueue);

    /// <summary>
    /// An interface for a console command that the user will type in.
    /// </summary>
    public interface IConsoleCommand
    {
        /// <summary>
        /// The Name of the Command as the User Will Type it In.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// The Delegate that is called when the user types this command in.
        /// </summary>
        ConsoleCommandDelegate Function { get; }
    }
}
