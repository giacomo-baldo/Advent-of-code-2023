using System.IO;

namespace Day17
{
    class Day17
    {
        [STAThread]
        static void Main(string[] args)
        {
            string path = "text.txt"; 

            Part1.Job(path);
            Part2.Job(path);

            return;
        }
    }
}