using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Drawing;
namespace KAA_task_1
{
    public class QuickFindUF
    {
        private int[] id;    // id[i] = component identifier of i
        private int count;   // number of components

        /*
         * Initializes an empty union-find data structure with N isolated components 0 through N-1.
         * @throws java.lang.IllegalArgumentException if N < 0
         * @param N the number of objects
         */
        public QuickFindUF(int N)
        {
            count = N;
            id = new int[N];
            for (int i = 0; i < N; i++)
                id[i] = i;
        }

        /**
         * Returns the number of components.
         * @return the number of components (between 1 and N)
         */
        public int Count()
        {
            return count;
        }

        /**
         * Returns the component identifier for the component containing site p.
         * @param p the integer representing one site
         * @return the component identifier for the component containing site p
         */
        public int find(int p)
        {
            validate(p);
            return id[p];
        }

        // validate that p is a valid index
        private void validate(int p)
        {
            int N = id.Length;
            if (p < 0 || p >= N)
            {
                throw new IndexOutOfRangeException("index " + p + " is not between 0 and " + N);
            }
        }

        /**
         * Are the two sites <tt>p</tt> and <tt>q/tt> in the same component?
         * @param p the integer representing one site
         * @param q the integer representing the other site
         * @return <tt>true</tt> if the two sites <tt>p</tt> and <tt>q</tt> are in
         *    the same component, and <tt>false</tt> otherwise
         * @throws java.lang.IndexOutOfBoundsException unless both 0 <= p < N and 0 <= q < N
         */
        public bool connected(int p, int q)
        {
            validate(p);
            validate(q);
            return id[p] == id[q];
        }

        /**
         * Merges the component containing site<tt>p</tt> with the component
         * containing site <tt>q</tt>.
         * @param p the integer representing one site
         * @param q the integer representing the other site
         * @throws java.lang.IndexOutOfBoundsException unless both 0 <= p < N and 0 <= q < N
         */
        public void union(int p, int q)
        {
            if (connected(p, q)) return;
            int pid = id[p];
            for (int i = 0; i < id.Length; i++)
                if (id[i] == pid) id[i] = id[q];
            count--;
        }

        /**
         * Reads in a sequence of pairs of integers (between 0 and N-1) from standard input, 
         * where each integer represents some object;
         * if the objects are in different components, merge the two components
         * and print the pair to standard output.
         */
        public void Wrapper()
        {
            //string fileName = "tinyUF.txt";
            //string fileName = "mediumUF.txt";
            string fileName = "largeUF.txt";

            StreamReader reader = new StreamReader(fileName);
            int N = int.Parse(reader.ReadLine());
            QuickFindUF uf = new QuickFindUF(N);
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
        }

    }

}
