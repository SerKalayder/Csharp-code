using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KAA_task_1
{
    class WeightedQuickUnionUF
    {
        private int[] parent;   // parent[i] = parent of i
        private int[] size;     // size[i] = number of objects in subtree rooted at i
        private int count;      // number of components

        public WeightedQuickUnionUF(int N)
        {
            count = N;
            parent = new int[N];
            size = new int[N];
            for (int i = 0; i < N; i++)
            {
                parent[i] = i;
                size[i] = 1;
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

        // validate that p is a valid index
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

            // make smaller root point to larger one
            if (size[rootP] < size[rootQ])
            {
                parent[rootP] = rootQ;
                size[rootQ] += size[rootP];
            }
            else
            {
                parent[rootQ] = rootP;
                size[rootP] += size[rootQ];
            }
            count--;
        }

    }
}
