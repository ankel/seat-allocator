using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Seat_Allocator
{
    class Program
    {

        static int[] chart;
        static int score;

        /// <summary>
        /// Performce complete search based on info given by si to find the optimal configuration
        /// </summary>
        /// <param name="si">Seat info given</param>
        static void CompleteSearch(SeatInfo si)
        {
            int[] temp = new int[si.size + 1];

            CompleteSearchChart(si, temp, 1);
        }

        /// <summary>
        /// Recursive tree traverse used in complete search
        /// </summary>
        /// <param name="si">SeatInfo contains seating information</param>
        /// <param name="temp">current (can be incomplete) temporary seating chart</param>
        /// <param name="p">current position in chart</param>
        private static void CompleteSearchChart(SeatInfo si, int[] temp, int p)
        {
            if (p > si.size)
            {
                if (si.Score(temp) > score)
                {
                    score = si.Score(temp);
                    chart = new int[temp.Length];
                    for (int i = 0; i < temp.Length; ++i)
                        chart[i] = temp[i];
                }
                return;
            }

            for (int i = 1; i <= si.size; ++i)
            {
                if (Check(temp, p, i))
                {
                    temp[p] = i;
                    CompleteSearchChart(si, temp, p + 1);
                }
            }
        }

        /// <summary>
        /// Check if a person is already assigned a seat or not within a seating chart up to a certain position
        /// </summary>
        /// <param name="chart">current seating chart (can be incomplete)</param>
        /// <param name="p">current considering position</param>
        /// <param name="i">the person</param>
        /// <returns>true if did not assigned a seat, false if did</returns>
        private static bool Check(int[] chart, int p, int i)
        {
            for (int j = 1; j < p; ++j)
            {
                if (chart[j] == i)
                    return false;
            }
            return true;
        }

        /// <summary>
        /// Display seating chart
        /// </summary>
        /// <param name="chart">the chart</param>
        /// <param name="score">the chart's score</param>
        static void DisplaySeatingChart(int[] chart, int score)
        {
            Console.WriteLine(score);

            for (int i = 1; i < chart.Length; ++i)
            {
                Console.WriteLine(chart[i] + " " + i);
            }
        }

        static void StochasticSearch(SeatInfo si)
        {
            DateTime due = DateTime.Now.AddMinutes(1);

            int[] temp;
            int temptScore;

            while (DateTime.Now <= due)
            {
                temp = AssignRandomSeat(si.size);
                temptScore = si.Score(temp);
                bool changed = false;

                for (int i = 1; i <= si.size; ++i)
                {
                    for (int j = i + 1; j <= si.size; ++j)
                    {
                        int t = temp[i];

                    }
                }
            }
        }

        private static int[] AssignRandomSeat(int p)
        {
            int[] ret = new int[p + 1];
            int j;
            Random rand = new Random();
            for (int i = 1; i <= p; ++i)
            {
                do
                {
                    j = rand.Next(1, p + 1);
                } while (!Check(ret, i, j));
                ret[i] = j;
            }
            return ret;
        }

        static void Main(string[] args)
        {
            DateTime start = DateTime.Now;

            if (args.Length == 0)
            {
                Console.WriteLine("Argument: ");
                Console.WriteLine("g <num> <file>: generate a random seating info with <num> guests into <file>");
                Console.WriteLine("c <file>: complete space state search using seating info from <file>");
                Console.WriteLine("s <file>: stochastic search using seating info from <file>");
                return;
            }

            SeatInfo si = new SeatInfo();
            //si.Gen(15);
            //si.Display();
            //si.ReadFile(@"..\..\TextFile1.txt");
            //Console.Write("Complete searching...");
            //CompleteSearch(si);
            //Console.WriteLine("finished!");
            //DisplaySeatingChart(chart, score);
            //Console.WriteLine("Run time: " + (DateTime.Now - start));

            int[] a = AssignRandomSeat(10);
            for (int i = 1; i <= 10; ++i)
            {
                Console.Write(a[i] + ", ");
            }

            Console.ReadLine();
        }
    }
}
