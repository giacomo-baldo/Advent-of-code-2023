using System.Diagnostics;

namespace Day17
{
    public class Part1
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

                // Create 2-d array table to store input data
                int[,] table = new int[rows, cols];
                for (int i = 0; i < rows; i++)
                {
                    for (int j = 0; j < cols; j++)
                    {
                        table[i, j] = Int32.Parse(input[i][j].ToString());
                    }
                }

                // Calculate sum of all entries - not essential
                int maxV = table.Cast<int>().Sum();
                Console.WriteLine("Sum of all cells is {0}", maxV);

                // Create table to store path finding progress
                // For each cell reached, store 

                HashSet<Point>[,] tableTT = new HashSet<Point>[rows, cols];
                for (int i = 0; i < rows; i++)
                {
                    for (int j = 0; j < cols; j++)
                    {
                        tableTT[i, j] = new HashSet<Point>();
                    };
                }

                // Initialize progress table with first entry
                var point0 = new Point(0, 0, 'V', 0, 0);
                var point1 = new Point(0, 0, 'H', 0, 0);
                HashSet<Point> points = [point0, point1];

                int min = 0;
                while (points.Count() > 1) // wait till only one point is left in queue to visit (does not guarante best solution)
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

        // list of possible steps count to loop through at each move
        public static int[] rs = [-3, -2, -1, 1, 2, 3];

        public static int score;
        public static void ProcessUnvisited(Point point, int rows, int cols, int[,] table, HashSet<Point>[,] tableTT, HashSet<Point> listOut)
        {
            switch (point.PreviousDir)
            {
                case 'H':
                    for (int i = 0; i < 6; i++)
                    {
                        if (point.R + rs[i] < rows && point.R + rs[i] >= 0)
                        {
                            switch (i)
                            {
                                case 0: score = table[point.R + rs[0], point.C] + table[point.R + rs[1], point.C] + table[point.R + rs[2], point.C]; break;
                                case 1: score = table[point.R + rs[1], point.C] + table[point.R + rs[2], point.C]; break;
                                case 2: score = table[point.R + rs[2], point.C]; break;
                                case 3: score = table[point.R + rs[3], point.C]; break;
                                case 4: score = table[point.R + rs[3], point.C] + table[point.R + rs[4], point.C]; break;
                                case 5: score = table[point.R + rs[3], point.C] + table[point.R + rs[4], point.C] + table[point.R + rs[5], point.C]; break;
                            }
                            if (!tableTT[point.R + rs[i], point.C].Where(p => p.PreviousDir == 'V').Any())
                            {
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
                    for (int i = 0; i < 6; i++)
                    {
                        if (point.C + rs[i] < cols && point.C + rs[i] >= 0)
                        {
                            switch (i)
                            {
                                case 0: score = table[point.R, point.C + rs[0]] + table[point.R, point.C + rs[1]] + table[point.R, point.C + rs[2]]; break;
                                case 1: score = table[point.R, point.C + rs[1]] + table[point.R, point.C + rs[2]]; break;
                                case 2: score = table[point.R, point.C + rs[2]]; break;
                                case 3: score = table[point.R, point.C + rs[3]]; break;
                                case 4: score = table[point.R, point.C + rs[3]] + table[point.R, point.C + rs[4]]; break;
                                case 5: score = table[point.R, point.C + rs[3]] + table[point.R, point.C + rs[4]] + table[point.R, point.C + rs[5]]; break;
                            }
                            if (!tableTT[point.R, point.C + rs[i]].Where(p => p.PreviousDir == 'H').Any())
                            {
                                // add point if no point reached the point from same direction before
                                var newP = new Point(point.R, point.C + rs[i], 'H', point.Score + score, 0);
                                listOut.Add(newP);
                                tableTT[point.R, point.C + rs[i]].Add(newP);
                            }
                            else if (tableTT[point.R, point.C + rs[i]].Where(p => p.PreviousDir == 'H').First().Score > point.Score + score)
                            {
                                // edit point if point reached from same direction before but there is a better score this time
                                tableTT[point.R, point.C + rs[i]].Where(p => p.PreviousDir == 'H').First().Score = point.Score + score;
                                tableTT[point.R, point.C + rs[i]].Where(p => p.PreviousDir == 'H').First().Visited = 0;
                            }
                        }
                    }
                    break;
            }
        }

        // Class to strore information about each cell once it has been reached
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