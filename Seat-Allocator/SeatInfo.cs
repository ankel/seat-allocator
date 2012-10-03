using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Seat_Allocator
{
    class SeatInfo
    {
        public int size;
        public int[,] h;

        public void Display()
        {
            Console.WriteLine(size);
            for (int i = 0; i < size; ++i)
            {
                for (int j = 0; j < size; ++j)
                {
                    Console.Write(h[i, j] + " ");
                }
                Console.WriteLine();
            }
        }

        public void ReadFile(string path)
        {
            System.IO.StreamReader file = new System.IO.StreamReader(path);
            size = Convert.ToInt32(file.ReadLine());

            h = new int[size,size];

            string[] s;
            for (int i = 0; i < size; ++i)
            {
                s = file.ReadLine().Split(' ');
                if (s.Length != size)
                    throw new Exception("File malformat at line " + (i + 2));
                for (int j = 0; j < size; ++j)
                {
                    h[i, j] = Convert.ToInt32(s[j]);
                }
            }
        }
    }
}
