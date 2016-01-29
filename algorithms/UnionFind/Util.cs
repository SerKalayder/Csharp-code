using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Diagnostics;

namespace KAA_task_1
{
    class Util
    {
        public static void Main(String[] args)
        {
            //string fileName = "tinyUF.txt";
            //string fileName = "mediumUF.txt";
            //string fileName = "largeUF.txt";
            string fileName = "mUF.txt";
            //Util.FileFill(fileName, 1000000);

            Stopwatch timer = new Stopwatch();

            timer.Start();
            StreamReader reader = new StreamReader(fileName);
            int N = int.Parse(reader.ReadLine());
            WeightedQuickUnionUF uf = new WeightedQuickUnionUF(N);
            for (int i = 0; i < N; i++)
            {
                string x = reader.ReadLine();
                //Console.WriteLine(x);
                int p = Convert.ToInt32(x);
                int q = Convert.ToInt32(reader.ReadLine());
                if (uf.connected(p, q)) continue;
                uf.union(p, q);
                Console.WriteLine(p + " " + q);
            }
            Console.WriteLine(uf.Count() + " components");
            reader.Dispose();
            timer.Stop();
            Console.WriteLine(MillisecondsConvertor(timer.ElapsedMilliseconds));
            

            Console.Read();
        }

        public static void FileFill(string file, int m)
        {
            StreamWriter writer = new StreamWriter(file);
            writer.WriteLine(m / 2);
            Random random = new Random();
            for (int i = 0; i < m; i++)
            {
                writer.WriteLine(random.Next(0, m / 2));
            }
            writer.Dispose();
        }

        public static string MillisecondsConvertor(long milliseconds)
        {
            long minutes = 0;
            long seconds = 0;
            seconds = milliseconds / 1000;
            milliseconds %= 1000;
            minutes = seconds / 60;
            seconds %= 60;
            return "\n ======>>>>> Elapsed time: " + minutes + ":" + seconds + ":" + milliseconds + "\n";
        }
    }
}
