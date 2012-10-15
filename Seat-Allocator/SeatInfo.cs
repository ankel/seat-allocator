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
        public int[,] adjScore;
        public int[,] oppScore;

        /// <summary>
        /// Display seating info onto console
        /// </summary>
        public void Display()
        {
            Console.WriteLine(size);
            for (int i = 1; i <= size; ++i)
            {
                for (int j = 1; j <= size; ++j)
                {
                    Console.Write(h[i, j] + " ");
                }
                Console.WriteLine();
            }
        }

        /// <summary>
        /// Read seating information from file
        /// </summary>
        /// <param name="path">file to read from</param>
        public void ReadFile(string path)
        {
            System.IO.StreamReader file = new System.IO.StreamReader(path);
            size = Convert.ToInt32(file.ReadLine());

            h = new int[size + 1,size + 1];         // additional space for heuristic information

            string[] s;
            for (int i = 1; i <= size; ++i)
            {
                s = file.ReadLine().Split(' ');
                if (s.Length != size)
                    throw new Exception("File malformat at line " + (i + 2));
                for (int j = 1; j <= size; ++j)
                {
                    h[i, j] = Convert.ToInt32(s[j - 1]);
                }
            }
            file.Close();
            AdjacentGen();
            OppositeGen();
        }
        /// <summary>
        /// print current seating configuration into a file
        /// </summary>
        /// <param name="path">output file path</param>
        public void PrintFile(string path)
        {
            System.IO.StreamWriter file = new System.IO.StreamWriter(path);

            file.WriteLine(this.size);

            for (int i = 1; i <= size; ++i)
            {
                for (int j = 1; j <= size; ++j)
                {
                    file.Write(h[i, j] + " ");
                }
                file.WriteLine();
            }
            file.Close();
        }

        /// <summary>
        /// UNTESTED - automatically generate a seating info using size and OVERIDE the current seating info
        /// </summary>
        /// <param name="Size">new size</param>
        public void Gen(int Size)
        {
            if (h != null)
            {
                throw new Overiding();
            }

            this.size = Size;
            h = new int[size + 1, size + 1];
            Random rand = new Random();

            for (int i = 1; i <= size; ++i)
            {
                for (int j = 1; j <= size; ++j)
                {
                    h[i, j] = rand.Next(-20, 20);
                }
            }
            AdjacentGen();
            OppositeGen();
        }

        /// <summary>
        /// Check if a given num is of a female or a male. Throw exception if number is less than 1 or bigger than size
        /// </summary>
        /// <param name="num">number to check</param>
        /// <returns>1 if female, -1 if male</returns>
        public int IsF(int num)
        {
            if (1 <= num && num <= (this.size / 2))
            {
                return 1;
            }
            else if (this.size / 2 < num && num <= this.size)
            {
                return -1;
            }
            else
            {
                throw new Exception("Number is incorrect");
            }
        }

        /// <summary>
        /// Check the adjacent pair score between (seat) and (seat-1)
        /// </summary>
        /// <param name="seat">the seat to look at</param>
        /// <param name="chart">the seating chart</param>
        /// <returns>score of the pair</returns>
        public int AdjacentScore(int seat, int[] chart)
        {
            int current = chart[seat];
            int adjacent = chart[seat - 1];
            int score = (IsF(current) * IsF(adjacent)) < 0 ? 1 : 0;
            score += h[current, adjacent] + h[adjacent, current];
            return score;
        }

        /// <summary>
        /// Check the opposite pair score between (seat) and (size / 2 + seat)
        /// </summary>
        /// <param name="seat">the seat to look at</param>
        /// <param name="chart">the seating chart</param>
        /// <returns>score of the pair</returns>
        public int OppositeScore(int seat, int[] chart)
        {
            int current = chart[seat];
            int opposite = chart[size / 2 + seat];
            int score = (IsF(current) * IsF(opposite)) < 0 ? 2 : 0;
            score += h[current, opposite] + h[opposite, current];
            return score;
        }

        /// <summary>
        /// Generate the adjacent score array
        /// </summary>
        private void AdjacentGen()
        {
            if (size == 0)
            {
                throw new Exception("Size not initialized");
            }

            adjScore = new int[size + 1, size + 1];
            for (int i = 1; i <= size; ++i)
            {
                for (int j = 1; j < size; ++j)
                {
                    int score = (IsF(i) * IsF(j)) < 0 ? 1 : 0;
                    score += h[i, j] + h[j, i];
                    adjScore[i, j] = adjScore[j, i] = score;
                }
            }
        }

        private void OppositeGen()
        {
            if (size == 0)
            {
                throw new Exception("Size not initialized");
            }

            oppScore = new int[size + 1, size + 1];
            for (int i = 1; i <= size; ++i)
            {
                for (int j = 1; j <= size; ++j)
                {
                    int score = (IsF(i) * IsF(j)) < 0 ? 2 : 0;
                    score += h[i, j] + h[j, i];
                    oppScore[i, j] = oppScore[j, i] = score;
                }
            }
        }

        /// <summary>
        /// Grade a seating chart based on current size and h
        /// </summary>
        /// <param name="chart">the seating chart</param>
        /// <returns>total score</returns>
        public int Score(int[] chart)
        {
            int score = oppScore[chart[1],chart[size / 2]];
            for (int i = 2; i <= size / 2; ++i)
            {
                score += adjScore[chart[i], chart[i - 1]] + adjScore[chart[size / 2 + i], chart[size / 2 + i - 1]];
                score += oppScore[i, size / 2 + i];
            }
            return score;
        }
    }
}
