using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KAA_task_1
{
    class QuickUnionUF
    {
        private int[] parent;  // parent[i] = parent of i
        private int count;     // number of components

        public QuickUnionUF(int N)
        {
            parent = new int[N];
            count = N;
            for (int i = 0; i < N; i++)
            {
                parent[i] = i;
            }
        }

        public int Count()
        {
            return count;
        }

        public int find(int p)
        {
            validate(p);
            while (p != parent[p])
                p = parent[p];
            return p;
        }

        private void validate(int p)
        {
            int N = parent.Length;
            if (p < 0 || p >= N)
            {
                throw new IndexOutOfRangeException("index " + p + " is not between 0 and " + N);
            }
        }

        public bool connected(int p, int q)
        {
            return find(p) == find(q);
        }


        public void union(int p, int q)
        {
            int rootP = find(p);
            int rootQ = find(q);
            if (rootP == rootQ) return;
            parent[rootP] = rootQ;
            count--;
        }

    }
}
