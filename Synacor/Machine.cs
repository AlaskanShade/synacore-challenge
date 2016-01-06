using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Synacore
{
    public class Machine
    {
        public ushort[] Memory { get; private set; }
        public ushort[] Register { get; private set; }
        public Stack<ushort> Stack { get; private set; }
        private uint Current;

        #region Command parameters
        private ushort A
        {
            get
            {
                return GetValue(Memory[Current + 1]);
            }
        }

        private ushort B
        {
            get
            {
                return GetValue(Memory[Current + 2]);
            }
        }

        private ushort C
        {
            get
            {
                return GetValue(Memory[Current + 3]);
            }
        }

        private ushort GetValue(ushort input)
        {
            if (input > 32767) return Register[input - 32768];
            return input;
        }
        #endregion

        public Machine(ushort[] instructions)
        {
            // 16 bit memory with 15-bit address
            Memory = new ushort[32767];
            // 8 registers
            Register = new ushort[8];
            // unbounded stack
            Stack = new Stack<ushort>();
            Memory = instructions;
        }

        public void Run()
        {
            while (Step()) ;
        }

        private bool Step()
        {
            switch (Memory[Current])
            {
                case 0: // halt
                    //Trace.WriteLine(String.Format("{0:X}: halt", Current * 2));
                    Console.WriteLine("Halted");
                    return false;
                case 1: // set register a to b
                    //Trace.WriteLine(String.Format("{0:X}: set {1} to {2}", Current * 2, A, B));
                    Register[Memory[Current + 1] % 32768] = B;
                    Current += 3;
                    return true;
                case 2: // push a onto stack
                    Stack.Push(A);
                    Current += 2;
                    return true;
                case 3: // pop into a
                    Register[Memory[Current + 1] % 32768] = Stack.Pop();
                    Current += 2;
                    return true;
                case 4: // eq: a = b == c ? 1 : 0
                    Register[Memory[Current + 1] % 32768] = (ushort)(B == C ? 1 : 0);
                    Current += 4;
                    return true;
                case 5: // gt: a = b > c ? 1 : 0
                    Register[Memory[Current + 1] % 32768] = (ushort)(B > C ? 1 : 0);
                    Current += 4;
                    return true;
                case 6: // jmp to a
                    //Trace.WriteLine(String.Format("{0:X}: jmp {1}", Current * 2, A));
                    Current = A;
                    return true;
                case 7: // jt: jump to b if a is non-zero
                    //Trace.WriteLine(String.Format("{0:X}: jt {1} {2:X}", Current * 2, A, B * 2));
                    if (A != 0) Current = B;
                    else Current += 3;
                    return true;
                case 8: // jf: jump to b if a is zero
                    //Trace.WriteLine(String.Format("{0:X}: jf {1} {2:X}", Current * 2, A, B * 2));
                    if (A == 0) Current = B;
                    else Current += 3;
                    return true;
                case 9: // add: a = b + c (% 32768)
                    Register[Memory[Current + 1] % 32768] = (ushort)((ushort)(B + C) % 32768);
                    Current += 4;
                    return true;
                case 10: // mult: a = b * c (% 32768)
                    Register[Memory[Current + 1] % 32768] = (ushort)((ushort)(B * C) % 32768);
                    Current += 4;
                    return true;
                case 11: // mod: a = b % c
                    Register[Memory[Current + 1] % 32768] = (ushort)((ushort)(B % C) % 32768);
                    Current += 4;
                    return true;
                case 12: // and: a = b & c
                    Register[Memory[Current + 1] % 32768] = (ushort)((ushort)(B & C) % 32768);
                    Current += 4;
                    return true;
                case 13: // or: a = b | c
                    Register[Memory[Current + 1] % 32768] = (ushort)((ushort)(B | C) % 32768);
                    Current += 4;
                    return true;
                case 14: // not: a = ~b
                    Register[Memory[Current + 1] % 32768] = (ushort)((ushort)~B % 32768);
                    Current += 3;
                    return true;
                case 15: // rmem: read memory at b into a
                    Register[Memory[Current + 1] % 32768] = Memory[B];
                    Current += 3;
                    return true;
                case 16: // wmem: write b into memory at a
                    Memory[A] = B;
                    Current += 3;
                    return true;
                case 17: // call: push next address to stack and jump to a
                    Stack.Push((ushort)(Current + 2));
                    Current = A;
                    return true;
                case 18: // ret: pop stack and jump
                    if (Stack.Count == 0) Console.WriteLine("Halted due to empty stack");
                    else Current = Stack.Pop();
                    return true;
                case 19: // out: write ascii code a to output
                    Console.Write((char)A);
                    Current += 2;
                    return true;
                case 20: // in: read character to a
                    ConsoleKeyInfo k;
                    //do
                    //{
                        k = Console.ReadKey();
                        Register[Memory[Current + 1] % 32768] = k.KeyChar;
                    //} while (k.Key != ConsoleKey.Enter);
                    return true;
                case 21: // noop
                    Current++;
                    return true;
                default:
                    throw new ApplicationException("Invalid instruction: " + Memory[Current]);
            }
        }
    }
}
