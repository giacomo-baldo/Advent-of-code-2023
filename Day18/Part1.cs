using System.Diagnostics;

namespace Day18
{
    public class Part1
    {
        // this method verify if a point is inside or outside based on the number of edges to its left.
        // when count is odd, the point is interior, when count is even, the point is exterior
        // there are specific rules on how to account for corners.
        public static void Run(string path)
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
                string[] thisLine;
                HashSet<Point> border = new HashSet<Point>();
                int c = 0;
                int r = 0;
                int loop = 0;
                char last;
                int countEdge = 0;
                HashSet<BorderPoint> borderCompactList = new HashSet<BorderPoint>();
                while (line != null)
                {
                    thisLine = line.Split(" ");
                    if (loop > 0) 
                    {
                        border.Last().EdgeType = border.Last().EdgeType + thisLine[0].ToCharArray()[0];
                    }                    
                    switch (thisLine[0].ToCharArray()[0]) 
                    {
                        case 'U': for (int i = 0; i < Int32.Parse(thisLine[1]); i++) { r--; border.Add(new Point(r, c, "U")); }; break;
                        case 'D': for (int i = 0; i < Int32.Parse(thisLine[1]); i++) { r++; border.Add(new Point(r, c, "D")); }; break;
                        case 'R': borderCompactList.Add(new BorderPoint(r, c, Int32.Parse(thisLine[1]))); { c += Int32.Parse(thisLine[1]); border.Add(new Point(r, c, "R")); }; break;
                        case 'L': borderCompactList.Add(new BorderPoint(r, c, -Int32.Parse(thisLine[1]))); { c -= Int32.Parse(thisLine[1]); border.Add(new Point(r, c, "L")); }; break;
                    }
                    countEdge = countEdge + Int32.Parse(thisLine[1]);
                    line = sr.ReadLine();
                    loop++;
                }
                border.Last().EdgeType = border.Last().EdgeType + border.First().EdgeType[0];

                //close the file
                sr.Close();
                Console.WriteLine("The final coordinates are r:{0} and c:{1}.", r,c);
                Console.WriteLine("There are {0} points as edge. ", countEdge);

                int maxC = border.Max(p => p.C);
                int maxR = border.Max(p => p.R);
                int minC = border.Min(p => p.C);
                int minR = border.Min(p => p.R);

                Console.WriteLine("The further away MIN coordinates are r:{0} and c:{1} (count starts at 0).", minR, minC);
                Console.WriteLine("The further away MAX coordinates are r:{0} and c:{1} (count starts at 0).", maxR, maxC);

                int countInside = 0; // counter variable for inside area;
                for (int i = minR;i < maxR;i++)
                {
                    for (int j = minC; j < maxC; j++)
                    {
                        if (!borderCompactList.Any(c => c.R == i && c.Cmin <= j && j <= c.Cmax))
                        {
                            if (!border.Any(p => p.R == i && p.C == j))
                            {
                                if (border.Count(p =>
                                p.R == i && p.C < j && (p.EdgeType == "U" ||
                                                        p.EdgeType == "D" ||
                                                        p.EdgeType == "DR" ||
                                                        p.EdgeType == "RU" ||
                                                        p.EdgeType == "DL" ||
                                                        p.EdgeType == "LU")) % 2 == 1
                                                )
                                { countInside++; }
                            }
                        }
                    }
                }
                Console.WriteLine("There are {0} points as internal. ", countInside);
                Console.WriteLine("The total volume is {0}. ", countInside+countEdge);
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

        public class Dir
        {
            public char HV { get; set; }
            public int Steps { get; set; }
        }

        public class Point
        {
            public int C { get; set; }
            public int R { get; set; }
            public string EdgeType { get; set; }
            public Point(int r, int c, string edgeType) 
            {
                C = c;
                R = r;
                EdgeType = edgeType;
            }
        }

        public class BorderPoint
        {
            public int R { get; set; }
            public int Cmin { get; set; }
            public int Cmax { get; set; }

            public BorderPoint(int r, int cmin, int step)
            {
                R = r;
                switch (step > 0)
                {
                    case true:
                        Cmin = cmin;
                        Cmax = cmin + step; 
                        break;

                    case false:
                        Cmin = cmin + step;
                        Cmax = cmin; 
                        break;
                }
                
            }
        }
    }
}