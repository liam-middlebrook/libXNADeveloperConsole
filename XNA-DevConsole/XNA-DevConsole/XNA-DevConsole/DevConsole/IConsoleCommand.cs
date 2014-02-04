using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XNA_DevConsole.DevConsole
{
    delegate int ConsoleCommandDelegate(string args);

    interface IConsoleCommand
    {
        string Name { get; }

        ConsoleCommandDelegate Function { get; }
    }
}
