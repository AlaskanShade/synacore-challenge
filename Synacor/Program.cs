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
            // load instructions into an array
            var instructionPath = @"..\..\..\challenge.bin";
            byte[] buffer = File.ReadAllBytes(instructionPath);
            ushort[] instructions = new ushort[buffer.Length / 2];
            Buffer.BlockCopy(buffer, 0, instructions, 0, buffer.Length);
            // Initialize machine and run
            var m = new Machine(instructions);
            m.Run();
            // Finalize
            Console.ReadKey();
        }
    }
}
