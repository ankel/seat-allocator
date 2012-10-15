using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Seat_Allocator
{
    class SeatingChart
    {
        public int[] chart;
        public int score;
        public int size;
        SeatInfo seatInfo;

        public SeatingChart(SeatInfo si)
        {
            seatInfo = si;
            this.size = si.size;
            chart = new int[size + 1];
        }

        public void InitializeChart()
        {
            for (int i = 1; i <= size; ++i)
            {
                chart[i] = i;
            }
            score = seatInfo.Score(chart);
        }

        public void SmallTweak()
        {
            Random rand = new Random();
            int i = rand.Next(size) + 1, j;
            do
            {
                j = rand.Next(size) + 1;
            } while (i == j);
            int temp = chart[i];
            chart[i] = chart[j];
            chart[j] = temp;
            score = seatInfo.Score(chart);
        }

        public void RandomChart()
        {
            InitializeChart();
            Random rand = new Random();
            int j;
            for (int i = 1; i <= size; ++i)
            {
                do
                {
                    j = rand.Next(size) + 1;
                } while (i == j);

                int t = chart[i];
                chart[i] = chart[j];
                chart[j] = t;
            }
            score = seatInfo.Score(chart);
        }

        public static void Copy(SeatingChart src, out SeatingChart desc)
        {
            desc = new SeatingChart(src.seatInfo);
            Array.Copy(src.chart, desc.chart, src.size + 1);
            desc.score = src.score;
            desc.score = src.score;
        }

        public string ToString()
        {
            string str = string.Empty;
            for (int i = 1; i <= size; ++i)
            {
                str += chart[i] + " ";
            }
            str += "score: " + score;
            return str;
        }

        public void Display()
        {
            Console.WriteLine(score);
            for (int i = 1; i <= size; ++i)
            {
                Console.Write(chart[i] + " " + i);
            }
        }
    }
}
