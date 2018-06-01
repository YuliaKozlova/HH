using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace CalculateService
{
    public class Calculator
    {
        struct Point
        {
            public float GameCount;
            public float BookCount;
            public float AlcoCount;
            public float MakeUpCount;
            public float DinnerCount;
        }

        static bool PointIsPositive(Point point)
        {
            if (point.AlcoCount >= 0 && point.BookCount >= 0 && point.DinnerCount >= 0 && point.GameCount >= 0 && point.MakeUpCount >= 0)
                return true;
            else
                return false;
        }

        static void WriteToFile(Point point, int step, string MethodName)
        {
            string writePath = @"..\..\..\..\..\Files\" + MethodName + ".txt";
            try
            {
                using (StreamWriter sw = new StreamWriter(writePath, true, System.Text.Encoding.Default))
                {
                    string thisOne = step.ToString() + ";" + point.GameCount + ";" + point.BookCount + ";" + point.AlcoCount + ";" + point.MakeUpCount + ";" + point.DinnerCount;
                    sw.WriteLine(thisOne);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        static int OurMoneys = 50000;
        static int radius = 20;
        //X0, X1, X2, X3, X4, X5, X6, Salary
        static int[] Game = new int[8] { 3, 5, 3, 0, 0, 0, 9, 1500 };
        static int[] Book = new int[8] { 10, 7, 7, 0, 0, 0, 2, 1300 };
        static int[] Alco = new int[8] { 1, 6, 4, 0, 0, 10, 1, 1700 };
        static int[] MakeUp = new int[8] { 0, 0, 9, 9, 0, 0, 0, 2000 };
        static int[] Dinner = new int[8] { 1, 4, 7, 0, 10, 7, 0, 3000 };

        //для функции Z = 3*X0 + 4*X4 + 3*X6 + X5
        static int Benefits(Point point)
        {
            int X0 = Game[0] * (int)point.GameCount + Book[0] * (int)point.BookCount + Alco[0] * (int)point.AlcoCount + MakeUp[0] * (int)point.MakeUpCount + Dinner[0] * (int)point.DinnerCount;
            int X4 = Game[4] * (int)point.GameCount + Book[4] * (int)point.BookCount + Alco[4] * (int)point.AlcoCount + MakeUp[4] * (int)point.MakeUpCount + Dinner[4] * (int)point.DinnerCount;
            int X6 = Game[6] * (int)point.GameCount + Book[6] * (int)point.BookCount + Alco[6] * (int)point.AlcoCount + MakeUp[6] * (int)point.MakeUpCount + Dinner[6] * (int)point.DinnerCount;
            int X5 = Game[5] * (int)point.GameCount + Book[5] * (int)point.BookCount + Alco[5] * (int)point.AlcoCount + MakeUp[5] * (int)point.MakeUpCount + Dinner[5] * (int)point.DinnerCount;
            return 3 * X0 + 4 * X4 + 3 * X6 + X5;
        }

        static int Salary(Point point)
        {
            return Game[7] * (int)point.GameCount + Book[7] * (int)point.BookCount + Alco[7] * (int)point.AlcoCount + MakeUp[7] * (int)point.MakeUpCount + Dinner[7] * (int)point.DinnerCount;
        }

        static void Worker(string[] args)
        {
            Random rnd = new Random();

            ///
            ///
            ////
            //////////////////////________________________________________________________________________________________________________________________________________________________
            ////
            ///
            ////Monte-Karlo
            Point first = new Point() { GameCount = rnd.Next(0, 5), AlcoCount = rnd.Next(0, 5), BookCount = rnd.Next(0, 5), DinnerCount = rnd.Next(0, 5), MakeUpCount = rnd.Next(0, 5) };
            int step = 1;
            while (Salary(first) >= OurMoneys)
            {
                first = new Point() { GameCount = rnd.Next(0, 5), AlcoCount = rnd.Next(0, 5), BookCount = rnd.Next(0, 5), DinnerCount = rnd.Next(0, 5), MakeUpCount = rnd.Next(0, 5) };
            }
            WriteToFile(first, step, "Monte-Karlo");
            int efg = Salary(first);
            Point current = first;
            int GiantSteps = 20;
            while (GiantSteps > 0)
            {
                int BigSteps = 30;
                while (BigSteps > 0)
                {
                    int errorCount = 0;
                    while (Benefits(first) >= Benefits(current) || Salary(current) >= OurMoneys)
                    {
                        float GC = -1;
                        while (GC < 0)
                        {
                            GC = current.GameCount + rnd.Next(0, radius) - radius / 2;
                        }
                        float AC = -1;
                        while (AC < 0)
                        {
                            AC = current.AlcoCount + rnd.Next(0, radius) - radius / 2;
                        }
                        float BC = -1;
                        while (BC < 0)
                        {
                            BC = current.BookCount + rnd.Next(0, radius) - radius / 2;
                        }
                        float DC = -1;
                        while (DC < 0)
                        {
                            DC = current.DinnerCount + rnd.Next(0, radius) - radius / 2;
                        }
                        float MC = -1;
                        while (MC < 0)
                        {
                            MC = current.MakeUpCount + rnd.Next(0, radius) - radius / 2;
                        }

                        current = new Point()
                        {
                            GameCount = GC,
                            AlcoCount = AC,
                            BookCount = BC,
                            DinnerCount = DC,
                            MakeUpCount = MC
                        };
                        errorCount++;
                        if (errorCount > 20000)
                            break;
                    }
                    if (errorCount <= 20000)
                    {
                        step++;
                        first = current;
                        WriteToFile(first, step, "Monte-Karlo");
                    }

                    errorCount = 0;
                    BigSteps--;
                }
                GiantSteps--;
                radius = radius / 2;
            }

            ///
            ///
            ////
            //////////////////////________________________________________________________________________________________________________________________________________________________
            ////
            ///
            //Nelder-Mid
            Point p1 = new Point() { GameCount = rnd.Next(0, 5), AlcoCount = rnd.Next(0, 5), BookCount = rnd.Next(0, 5), DinnerCount = rnd.Next(0, 5), MakeUpCount = rnd.Next(0, 5) };
            Point p2 = new Point() { GameCount = rnd.Next(0, 5), AlcoCount = rnd.Next(0, 5), BookCount = rnd.Next(0, 5), DinnerCount = rnd.Next(0, 5), MakeUpCount = rnd.Next(0, 5) };
            Point p3 = new Point() { GameCount = rnd.Next(0, 5), AlcoCount = rnd.Next(0, 5), BookCount = rnd.Next(0, 5), DinnerCount = rnd.Next(0, 5), MakeUpCount = rnd.Next(0, 5) };
            while (Salary(p1) >= OurMoneys)
            {
                p1 = new Point() { GameCount = rnd.Next(0, 5), AlcoCount = rnd.Next(0, 5), BookCount = rnd.Next(0, 5), DinnerCount = rnd.Next(0, 5), MakeUpCount = rnd.Next(0, 5) };
            }
            while (Salary(p2) >= OurMoneys)
            {
                p2 = new Point() { GameCount = rnd.Next(0, 5), AlcoCount = rnd.Next(0, 5), BookCount = rnd.Next(0, 5), DinnerCount = rnd.Next(0, 5), MakeUpCount = rnd.Next(0, 5) };
            }
            while (Salary(p3) >= OurMoneys)
            {
                p3 = new Point() { GameCount = rnd.Next(0, 5), AlcoCount = rnd.Next(0, 5), BookCount = rnd.Next(0, 5), DinnerCount = rnd.Next(0, 5), MakeUpCount = rnd.Next(0, 5) };
            }

            int limitSteps = 1000;

            while (limitSteps > 0)
            {
                int worstIdentifier = 1;
                Point best = new Point(), another = new Point(), worst = new Point();
                limitSteps--;
                if (Benefits(p1) <= Benefits(p2) && Benefits(p1) <= Benefits(p3))
                {
                    worstIdentifier = 1;
                    worst = p1;
                    if (Benefits(p2) <= Benefits(p3))
                    {
                        another = p2;
                        best = p3;
                    }
                    else
                    {
                        best = p2;
                        another = p3;
                    }
                }
                else if (Benefits(p2) <= Benefits(p1) && Benefits(p2) <= Benefits(p3))
                {
                    worstIdentifier = 2;
                    worst = p2;
                    if (Benefits(p1) <= Benefits(p3))
                    {
                        another = p1;
                        best = p3;
                    }
                    else
                    {
                        best = p1;
                        another = p3;
                    }
                }
                else
                {
                    worstIdentifier = 3;
                    worst = p3;
                    if (Benefits(p1) <= Benefits(p2))
                    {
                        another = p1;
                        best = p2;
                    }
                    else
                    {
                        best = p1;
                        another = p2;
                    }
                }

                Point Centroid = new Point
                {
                    AlcoCount = (best.AlcoCount + another.AlcoCount) / 2,
                    BookCount = (best.BookCount + another.BookCount) / 2,
                    DinnerCount = (best.DinnerCount + another.DinnerCount) / 2,
                    GameCount = (best.GameCount + another.GameCount) / 2,
                    MakeUpCount = (best.MakeUpCount + another.MakeUpCount) / 2
                };

                Point Compression = new Point
                {
                    AlcoCount = ((best.AlcoCount + another.AlcoCount) / 2 + worst.AlcoCount) / 2,
                    BookCount = ((best.BookCount + another.BookCount) / 2 + worst.BookCount) / 2,
                    DinnerCount = ((best.DinnerCount + another.DinnerCount) / 2 + worst.DinnerCount) / 2,
                    GameCount = ((best.GameCount + another.GameCount) / 2 + worst.GameCount) / 2,
                    MakeUpCount = ((best.MakeUpCount + another.MakeUpCount) / 2 + worst.MakeUpCount) / 2
                };

                Point Reflected = new Point
                {
                    AlcoCount = ((best.AlcoCount + another.AlcoCount) / 2 - worst.AlcoCount) * 2 + worst.AlcoCount,
                    BookCount = ((best.BookCount + another.BookCount) / 2 - worst.BookCount) * 2 + worst.BookCount,
                    DinnerCount = ((best.DinnerCount + another.DinnerCount) / 2 - worst.DinnerCount) * 2 + worst.DinnerCount,
                    GameCount = ((best.GameCount + another.GameCount) / 2 - worst.GameCount) * 2 + worst.GameCount,
                    MakeUpCount = ((best.MakeUpCount + another.MakeUpCount) / 2 - worst.MakeUpCount) * 2 + worst.MakeUpCount
                };

                Point Extented = new Point
                {
                    AlcoCount = ((best.AlcoCount + another.AlcoCount) / 2 - worst.AlcoCount) * 3 + worst.AlcoCount,
                    BookCount = ((best.BookCount + another.BookCount) / 2 - worst.BookCount) * 3 + worst.BookCount,
                    DinnerCount = ((best.DinnerCount + another.DinnerCount) / 2 - worst.DinnerCount) * 3 + worst.DinnerCount,
                    GameCount = ((best.GameCount + another.GameCount) / 2 - worst.GameCount) * 3 + worst.GameCount,
                    MakeUpCount = ((best.MakeUpCount + another.MakeUpCount) / 2 - worst.MakeUpCount) * 3 + worst.MakeUpCount
                };

                Point Random = new Point() { GameCount = rnd.Next(0, 50), AlcoCount = rnd.Next(0, 50), BookCount = rnd.Next(0, 50), DinnerCount = rnd.Next(0, 50), MakeUpCount = rnd.Next(0, 50) };
                while (Salary(Random) >= OurMoneys)
                {
                    Random = new Point() { GameCount = rnd.Next(0, 50), AlcoCount = rnd.Next(0, 50), BookCount = rnd.Next(0, 50), DinnerCount = rnd.Next(0, 50), MakeUpCount = rnd.Next(0, 50) };
                }

                if (Benefits(Extented) > Benefits(worst) && Salary(Extented) <= OurMoneys && PointIsPositive(Extented))
                {
                    if (worstIdentifier == 1)
                    {
                        p1 = Extented;
                        worst = p1;
                    }
                    else if (worstIdentifier == 2)
                    {
                        p2 = Extented;
                        worst = p2;
                    }
                    else
                    {
                        p3 = Extented;
                        worst = p3;
                    }
                }

                if (Benefits(Reflected) > Benefits(worst) && Salary(Reflected) <= OurMoneys && PointIsPositive(Reflected))
                {
                    if (worstIdentifier == 1)
                    {
                        p1 = Reflected;
                        worst = p1;
                    }
                    else if (worstIdentifier == 2)
                    {
                        p2 = Reflected;
                        worst = p2;
                    }
                    else
                    {
                        p3 = Reflected;
                        worst = p3;
                    }
                }

                if (Benefits(Compression) > Benefits(worst) && Salary(Compression) <= OurMoneys && PointIsPositive(Compression))
                {
                    if (worstIdentifier == 1)
                    {
                        p1 = Compression;
                        worst = p1;
                    }
                    else if (worstIdentifier == 2)
                    {
                        p2 = Compression;
                        worst = p2;
                    }
                    else
                    {
                        p3 = Compression;
                        worst = p3;
                    }
                }

                if (Benefits(Random) > Benefits(worst) && Salary(Random) <= OurMoneys && PointIsPositive(Random))
                {
                    if (worstIdentifier == 1)
                    {
                        p1 = Random;
                        worst = p1;
                    }
                    else if (worstIdentifier == 2)
                    {
                        p2 = Random;
                        worst = p2;
                    }
                    else
                    {
                        p3 = Random;
                        worst = p3;
                    }
                }

                WriteToFile(worst, 1000 - limitSteps, "Nelder-Mid");
            }


            Console.WriteLine("Выполнение упешно завершено");
            Console.ReadLine();
        }
    }
}
