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
            SeatInfo si = new SeatInfo();
            si.ReadFile(args[1]);
            si.Display();
            Console.ReadLine();
        }
    }
}
