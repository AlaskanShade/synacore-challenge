using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Synacore
{
    class Program
    {
        static void Main(string[] args)
        {
            // Initialize machine and run
            var m = new Machine(@"..\..\..\synacor.bin");
            //m.QueueCommands(File.ReadAllText(@"..\..\..\commands.txt").Replace("\r", ""));
            m.Run(File.ReadAllText(@"..\..\..\commands.txt").Replace("\r", ""));
            File.WriteAllText(@"..\..\..\output.txt", m.Output);
            // Finalize
            Console.ReadKey();
        }
    }
}
