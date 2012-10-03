using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Seat_Allocator
{
    class Overiding : Exception{
        public Overiding() : base()
        {
        }

        public Overiding(string s) : base(s)
        {
        }
    }

    class SeatInfo
    {
        public int size;
        public int[,] h;
        public int[] chart;

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

            h = new int[size + 1,size + 1];

            string[] s;
            for (int i = 1; i <= size; ++i)
            {
                s = file.ReadLine().Split(' ');
                if (s.Length != size)
                    throw new Exception("File malformat at line " + (i + 2));
                for (int j = 1; j <= size; ++j)
                {
                    h[i, j] = Convert.ToInt32(s[j]);
                }
            }
            file.Close();
        }

        public void PrintFile(string path)
        {
            System.IO.StreamWriter file = new System.IO.StreamWriter(path);

            file.WriteLine(this.size);

            for (int i = 0; i < size; ++i)
            {
                for (int j = 0; j < size; ++j)
                {
                    file.Write(h[i, j] + " ");
                }
                file.WriteLine();
            }
            file.Close();
        }

        public void Gen(int Size, string path, bool overide)
        {
            if (h != null && overide == false)
            {
                throw new Overiding();
            }

            this.size = Size;
            h = new int[size, size];
            Random rand = new Random();

            for (int i = 0; i < size; ++i)
            {
                for (int j = 0; j < size; ++j)
                {
                    h[i, j] = rand.Next(-20, 20);
                }
            }
        }

        public int IsF(int num)
        {
            if (1 <= num && num <= (this.size / 2))
            {
                return 1;
            }
            if (this.size / 2 < num && num <= this.size)
            {
                return -1;
            }
        }

        /// <summary>
        /// Check the adjacent pair score between (seat) and (seat-1)
        /// </summary>
        /// <param name="seat"> the seat to look at</param>
        /// <returns>score of the pair</returns>
        public int AdjacentScore(int seat)
        {
            int current = chart[seat];
            int adjacent = chart[seat - 1];
            int score = (IsF(current) * IsF(adjacent)) < 0 ? 1 : 0;
            score += h[current, adjacent] + h[adjacent, current];
            return score;
        }

        public int OppositeScore(int seat)
        {
            int current = chart[seat];
            int opposite = chart[size / 2 + seat];
            int score = (IsF(current) * IsF(opposite)) < 0 ? 2 : 0;
            score += h[current, opposite] + h[opposite, current];
            return score;
        }

        public int Score()
        {
            int score = 0;
            int first = 1;
            int firstOpposite = size / 2 + 1;
            if (IsF(chart[first]) * IsF(firstOpposite) < 0)
            {
                score += 2;
            }
            score += h[chart[first], chart[firstOpposite]] + h[chart[firstOpposite], chart[first]];

            for (int i = 2; i < firstOpposite; ++i)
            {

            }


            return score;
        }
    }
}
