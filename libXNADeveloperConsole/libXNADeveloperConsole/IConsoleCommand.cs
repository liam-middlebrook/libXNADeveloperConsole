using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace libXNADeveloperConsole
{
    public delegate int ConsoleCommandDelegate(string args, LimitedMessageQueue loggingQueue);

    public interface IConsoleCommand
    {
        string Name { get; }

        ConsoleCommandDelegate Function { get; }
    }
}
