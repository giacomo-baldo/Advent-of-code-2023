using System.Diagnostics;

namespace Day18
{
    public class Part2
    {
        // this method computes the internal area based on two theorems.
        // 1. Pick's theorem
        // 2. Shoelace formula
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
                List<Point> edges = new List<Point>();
                Int64 c = 0;
                Int64 r = 0;
                Int64 perimeter = 0;
                Int64 steps;
                string str;
                char[] move;
                while (line != null)
                {                    
                    move = line.Split(" ")[2].Replace("(#", "").Replace(")", "").ToCharArray();
                    str = String.Concat(move.Take(5));
                    steps = Convert.ToInt64(str, 16);
                    switch (move[5])
                    {
                        case '0': c += steps; edges.Add(new Point(r, c)); break;
                        case '1': r += steps; edges.Add(new Point(r, c)); break;
                        case '2': c -= steps; edges.Add(new Point(r, c)); break;
                        case '3': r -= steps; edges.Add(new Point(r, c)); break;
                    }
                    perimeter += steps;
                    line = sr.ReadLine();
                }
                //close the file
                sr.Close();
                Console.WriteLine("The final coordinates are r:{0} and c:{1}.", r,c);
                Console.WriteLine("The perimeter length is {0} ", perimeter);
                Console.WriteLine("There are {0} edges ", edges.Count());

                Int64 maxC = edges.Max(p => p.C);
                Int64 maxR = edges.Max(p => p.R);
                Int64 minC = edges.Min(p => p.C);
                Int64 minR = edges.Min(p => p.R);

                Console.WriteLine("The further away MIN coordinates are r:{0} and c:{1} (count starts at 0).", minR, minC);
                Console.WriteLine("The further away MAX coordinates are r:{0} and c:{1} (count starts at 0).", maxR, maxC);
                
                Int64 area = 0;
                // put edges in anti clockwise order, as required to apply Shoelace theorem.
                edges.Reverse(); 
                int ec = edges.Count(); // use as loop limit
                for (int i = 0; i < ec-1; i++) 
                {
                    area += (edges[i].C * edges[i+1].R - edges[i + 1].C * edges[i].R );
                }
                area = area + (edges[ec - 1].C * edges[0].R);
                area = area + (edges[0].C * edges[ec-1].R);
                area /= 2;
                area  = Math.Abs(area);

                Console.WriteLine("The internal area of the shape is {0}. ", area);

                Int64 n_of_points;
                // Pick's theorem
                n_of_points = area + 1 + perimeter / 2; 

                Console.WriteLine("Total area (internal area + perimeter) is: {0}", n_of_points);
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

        public class Point
        {
            public Int64 C { get; set; }
            public Int64 R { get; set; }
            public Point(Int64 r, Int64 c) 
            {
                C = c;
                R = r;
            }
        }
    }
}