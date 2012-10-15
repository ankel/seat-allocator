using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Seat_Allocator
{
    class Annealing
    {
        SeatInfo seatInfo;
        SeatingChart bestSoFar;

        private const int INITIAL_TEMP = 30;
        private const double FINAL_TEMP = 0.5;
        private const double ALPHA = 0.98;
        private const int STEPS_PER_CHANGE = 500;

        public Annealing(SeatInfo si)
        {
            seatInfo = si;
            bestSoFar = new SeatingChart(si);
            bestSoFar.InitializeChart();
        }

        public Annealing(string path)
        {
            seatInfo = new SeatInfo();
            seatInfo.ReadFile(path);
            bestSoFar = new SeatingChart(seatInfo);
            bestSoFar.RandomChart();
        }

        public SeatingChart Anneal()
        {
            DateTime due = DateTime.Now.AddMinutes(1);
            System.IO.StreamWriter debug = new System.IO.StreamWriter(@"..\..\debug.txt");
            SeatingChart current = null;

            while (DateTime.Now <= due)
            {
                double temp = INITIAL_TEMP;
                if (current == null)
                {
                    current = new SeatingChart(seatInfo);
                    current.RandomChart();
                }
                if (bestSoFar.score > current.score)
                {
                    SeatingChart.Copy(bestSoFar, out current);
                }
                else
                {
                    SeatingChart.Copy(current, out bestSoFar);
                }

                while (temp > FINAL_TEMP)
                {
                    //Console.WriteLine("Temp = " + temp);
                    for (int i = 0; i < STEPS_PER_CHANGE; ++i)
                    {
                        //debug.Write(temp + "; ");
                        bool accepted = false;
                        SeatingChart working;
                        SeatingChart.Copy(current, out working);
                        working.SmallTweak();
                        if (working.score > current.score)
                        {
                            accepted = true;
                        }
                        else
                        {
                            accepted = LoveMeNot(working.score, current.score, temp);
                        }
                        if (accepted)
                        {
                            SeatingChart.Copy(working, out current);
                            if (current.score > bestSoFar.score)
                            {
                                SeatingChart.Copy(current, out bestSoFar);
                            }
                        }
                        //debug.WriteLine(working.ToString() + " " + accepted);
                    }
                    temp = temp * ALPHA;
                }
            }
            debug.Close();
            return bestSoFar;
        }

        private bool LoveMeNot(int working, int current, double temp)
        {
            Random rand = new Random();
            double test = rand.NextDouble();
            double delta = current - working;
            double probability = Math.Exp(delta / temp);
            if (test < probability)
            {
                return true;
            }
            return false;
        }
    }
}
