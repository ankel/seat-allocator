using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Seat_Allocator
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("Argument: ");
                Console.WriteLine("g <num> <file>: generate a random seating info with <num> guests into <file>");
                Console.WriteLine("c <file>: complete space state search using seating info from <file>");
                Console.WriteLine("s <file>: stochastic search using seating info from <file>");
                return;
            }

            SeatInfo si = new SeatInfo();
            si.ReadFile(args[1]);
            si.Display();
            Console.ReadLine();
        }
    }
}
