using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Seat_Allocator
{
    /// <summary>
    /// AI class PSU Fall 2012
    /// Teacher: Bart Massey
    /// Student: Binh Tran
    /// 
    /// Seat allocating problem. Solution implemented both complete search and local search (with random factor)
    /// Unless specify otherwise, program will try the most suitable mode.
    /// 
    /// Copyleft, no right reserved. Feel free to do anything with it.
    /// </summary>
    class Program
    {

        static int[] chart;
        static int score;
        static bool debug = false;

        /// <summary>
        /// Perform a complete search based on info given by si to find the optimal configuration
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

        /// <summary>
        /// Perform a local search with random move 50% of the time
        /// </summary>
        /// <param name="si">Seating info used to grade the seating scheme</param>
        static void StochasticSearch(SeatInfo si)
        {
            DateTime due = DateTime.Now.AddMinutes(1);

            int[] temp = AssignRandomSeat(si.size);
            int tempScore = si.Score(temp);
            DateTime t = DateTime.Now;
            int n = 0;      //how many time a move results in a suboptimal score / no good move is found
            
            while (t <= due)
            {
                if (n == 0 || t.Millisecond % 2 == 0) // perform the "good" move if the last good-move attempt wasn't stuck
                {

                    GoodMove(si, ref temp, ref tempScore);
                    if (tempScore > score)
                    {
                        Array.Copy(temp, chart, si.size + 1);
                        score = tempScore;
                        n = 0;
                    }
                    else
                    {
                        ++n;
                    }
                }
                else
                {
                    RandomMove(si, ref temp, ref tempScore);

                    if (tempScore > score)
                    {
                        Array.Copy(temp, chart, si.size + 1);
                        score = tempScore;
                        n = 0;
                    }
                    else
                    {
                        ++n;
                    }
                }

                if (n > 100) // stuck for the last 100 moves then restart
                {
                    temp = AssignRandomSeat(si.size);
                    tempScore = si.Score(temp);
                }

                t = DateTime.Now;
            }
        }

        /// <summary>
        /// Perform a random move
        /// </summary>
        /// <param name="si">Seating info to grade the new scheme</param>
        /// <param name="temp">current seating scheme</param>
        /// <param name="tempScore">current score</param>
        private static void RandomMove(SeatInfo si, ref int[] temp, ref int tempScore)
        {
            Random rand = new Random();

            int i = rand.Next(si.size) + 1;
            int j;
            do
            {
                j = rand.Next(si.size) + 1;
            } while (i == j);
            Swap(ref temp, i, j);
            tempScore = si.Score(temp);
        }

        /// <summary>
        /// Perform a good move: look for the neighbour with the highest score
        /// </summary>
        /// <param name="si">seating info to grade the scheme</param>
        /// <param name="temp">current seating scheme</param>
        /// <param name="tempScore">current score</param>
        private static void GoodMove(SeatInfo si, ref int[] temp, ref int tempScore)
        {
            int seatToSwap1 = 0, seatToSwap2 = 0;

            for (int i = 1; i <= si.size; ++i)
            {
                for (int j = i + 1; j <= si.size; ++j)
                {
                    Swap(ref temp, i, j);
                    if (si.Score(temp) > tempScore)
                    {
                        seatToSwap1 = i;
                        seatToSwap2 = j;
                        tempScore = si.Score(temp);
                    }
                    Swap(ref temp, i, j);
                }
            }
            if (seatToSwap1 != 0 && seatToSwap2 != 0)
            {
                Swap(ref temp, seatToSwap1, seatToSwap2);
            }
            tempScore = si.Score(temp);
        }

        /// <summary>
        /// Swap 2 elements of an array
        /// </summary>
        /// <param name="temp">the array</param>
        /// <param name="i">1st position</param>
        /// <param name="j">2nd position</param>
        private static void Swap(ref int[] temp, int i, int j)
        {
            int t = temp[i];
            temp[i] = temp[j];
            temp[j] = t;
        }

        /// <summary>
        /// Assign random people to random seat to generate a random seating scheme
        /// </summary>
        /// <param name="p">number to total people</param>
        /// <returns>an array represent the random seating scheme</returns>
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
                Console.WriteLine("c <file>: complete space state search using seating info from <file>");
                Console.WriteLine("s <file>: stochastic search using seating info from <file>");
                Console.WriteLine("a <file>: auto mode. Will try the most suitable search method");
                return;
            }

            SeatInfo si = new SeatInfo();
            si.ReadFile(args[1]);
            chart = new int[si.size + 1];
            score = 0;

            switch (args[0])
            {
                case "c":
                    Console.Write("Complete search...");
                    CompleteSearch(si);
                    Console.WriteLine(" finished!");
                    break;
                case "s":
                    Console.Write("Local search... ");
                    StochasticSearch(si);
                    Console.WriteLine(" finished!");
                    break;
                case "a":
                    if (si.size <= 11)
                    {
                        Console.Write("Complete search...");
                        CompleteSearch(si);
                        Console.WriteLine(" finished!");
                    }
                    else
                    {
                        Console.Write("Local search... ");
                        StochasticSearch(si);
                        Console.WriteLine(" finished!");
                    }
                    break;
                default:
                    Console.WriteLine("Argument: ");
                    Console.WriteLine("c <file>: complete space state search using seating info from <file>");
                    Console.WriteLine("s <file>: stochastic search using seating info from <file>");
                    Console.WriteLine("a <file>: auto mode. Will try the most suitable search method");
                    return;
            }

            if (debug)
            {
                si.Display();
            }
            Console.WriteLine(" finished!");
            DisplaySeatingChart(chart, score);
            Console.WriteLine("Run time: " + (DateTime.Now - start));

            //int[] a = AssignRandomSeat(10);
            //for (int i = 1; i <= 10; ++i)
            //{
            //    Console.Write(a[i] + ", ");
            //}

            Console.ReadLine();
        }
    }
}
