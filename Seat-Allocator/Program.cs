using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Seat_Allocator
{
    class Program
    {
        static int[,] h;
        static int total;
        static void Main(string[] args)
        {
            System.IO.StreamReader file = new System.IO.StreamReader(args[1]);
            total = Convert.ToInt32(file.ReadLine());

            h = new int[total, total];
            string[] s;
            for (int i = 0; i < total; ++i)
            {
                s = file.ReadLine().Split(' ');
                for (int j = 0; j < total; ++j)
                {
                    h[i, j] = Convert.ToInt32(s[j]);
                }
            }

            Console.WriteLine(total);
            for (int i = 0; i < total; ++i)
            {
                for (int j = 0; j < total; ++j)
                {
                    Console.Write(h[i, j] + " ");
                }
                Console.WriteLine();
            }
            Console.ReadLine();
        }
    }
}
