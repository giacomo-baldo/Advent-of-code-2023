using System.Diagnostics;

namespace Day17
{
    class Part2
    {
        public static void Job(string path)
        {
            // create and start a Stopwatch instance
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            String line;
            try
            {

                //Pass the file path and file name to the StreamReader constructor
                StreamReader sr = new StreamReader(path);
                //Read the first line of text
                line = sr.ReadLine();
                //Continue to read until you reach end of file
                List<string> input = new List<string>();
                while (line != null)
                {
                    input.Add(line.ToString());
                    line = sr.ReadLine();
                }

                //close the file
                sr.Close();

                int cols = input[0].Count();
                int rows = input.Count();

                Console.WriteLine("There are {0} rows and {1} columns.", rows, cols);

                int[,] table = new int[rows, cols];

                for (int i = 0; i < rows; i++)
                {
                    for (int j = 0; j < cols; j++)
                    {
                        table[i, j] = Int32.Parse(input[i][j].ToString());
                    }
                }

                HashSet<Point>[,] tableTT = new HashSet<Point>[rows, cols];

                int maxV = table.Cast<int>().Sum();
                Console.WriteLine("Sum of all cells is {0}", maxV);

                for (int i = 0; i < rows; i++)
                {
                    for (int j = 0; j < cols; j++)
                    {
                        tableTT[i, j] = new HashSet<Point>();
                    };
                }

                var point0 = new Point(0, 0, 'V', 0, 0);
                var point1 = new Point(0, 0, 'H', 0, 0);

                tableTT[0, 0].Add(point0);
                tableTT[0, 0].Add(point1);

                HashSet<Point> points = [point0, point1];

                int min = 0;
                while (!tableTT[rows - 1, cols - 1].Any())// first reach the goal stops (does not guarante best solution but seems to work with given heuristic)
                {
                    points.RemoveWhere(p => p.Visited == 1);
                    min = points.Min(p => (p.Score - p.R - p.C)); // heuristic to choose point to visit
                    var p = points.Where(p => (p.Score - p.R - p.C) == min).First();
                    p.Visited = 1;
                    ProcessUnvisited(p, rows, cols, table, tableTT, points);
                }
                Console.WriteLine(" Final.. {0} .. ", tableTT[rows - 1, cols - 1].Min(p => p.Score));

            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: " + e.Message);
            }
            finally
            {
                stopWatch.Stop();
                TimeSpan ts = stopWatch.Elapsed;
                // Format and display the TimeSpan value.
                string elapsedTime = String.Format("{0:00}h:{1:00}m:{2:00}s.{3:0000}ms",
                    ts.Hours, ts.Minutes, ts.Seconds, ts.Milliseconds);
                Console.WriteLine("RunTime " + elapsedTime);
            }
            return;
        }


        public static int[] rs = [-10, -9, -8, -7, -6, -5, -4, 4, 5, 6, 7, 8, 9, 10];

        public static int score;

        public static int ScorerR(Point point, int[,] table, int i)
        {
            score = 0;
            switch (i > 0)
            {
                case true: for (int c = 1; c <= i; c++) { score += table[point.R + c, point.C]; }; break;
                case false: for (int c = i; c <= -1; c++) { score += table[point.R + c, point.C]; }; break;
            }
            return score;
        }
        public static int ScorerC(Point point, int[,] table, int i)
        {
            score = 0;
            switch (i > 0)
            {
                case true: for (int c = 1; c <= i; c++) { score += table[point.R, point.C + c]; }; break;
                case false: for (int c = i; c <= -1; c++) { score += table[point.R, point.C + c]; }; break;
            }
            return score;
        }
        public static void ProcessUnvisited(Point point, int rows, int cols, int[,] table, HashSet<Point>[,] tableTT, HashSet<Point> listOut)
        {
            switch (point.PreviousDir)
            {
                case 'H':
                    for (int i = 0; i < 14; i++)
                    {
                        if (point.R + rs[i] < rows && point.R + rs[i] >= 0)
                        {
                            score = ScorerR(point, table, rs[i]);
                            if (!tableTT[point.R + rs[i], point.C].Where(p => p.PreviousDir == 'V').Any())
                            {
                                // add point
                                var newP = new Point(point.R + rs[i], point.C, 'V', point.Score + score, 0);
                                listOut.Add(newP);
                                tableTT[point.R + rs[i], point.C].Add(newP);
                            }
                            else if (tableTT[point.R + rs[i], point.C].Where(p => p.PreviousDir == 'V').First().Score > point.Score + score)
                            {
                                tableTT[point.R + rs[i], point.C].Where(p => p.PreviousDir == 'V').First().Score = point.Score + score;
                                tableTT[point.R + rs[i], point.C].Where(p => p.PreviousDir == 'V').First().Visited = 0;
                            }

                        }
                    }
                    break;

                case 'V':
                    for (int i = 0; i < 14; i++)
                    {
                        if (point.C + rs[i] < cols && point.C + rs[i] >= 0)
                        {
                            score = ScorerC(point, table, rs[i]);
                            if (!tableTT[point.R, point.C + rs[i]].Where(p => p.PreviousDir == 'H').Any())
                            {
                                // add point
                                var newP = new Point(point.R, point.C + rs[i], 'H', point.Score + score, 0);
                                listOut.Add(newP);
                                tableTT[point.R, point.C + rs[i]].Add(newP);
                            }
                            else if (tableTT[point.R, point.C + rs[i]].Where(p => p.PreviousDir == 'H').First().Score > point.Score + score)
                            {
                                tableTT[point.R, point.C + rs[i]].Where(p => p.PreviousDir == 'H').First().Score = point.Score + score;
                                tableTT[point.R, point.C + rs[i]].Where(p => p.PreviousDir == 'H').First().Visited = 0;
                            }
                        }
                    }
                    break;
            }
        }

        public class Point
        {
            public int R { get; set; }
            public int C { get; set; }
            public char PreviousDir { get; set; }
            public int Score { get; set; }
            public int Visited { get; set; }
            public Point(int r, int c, char prvdir, int score, int visited)
            {
                this.R = r;
                this.C = c;
                this.PreviousDir = prvdir;
                this.Score = score;
                this.Visited = visited;
            }
        }
    }
}