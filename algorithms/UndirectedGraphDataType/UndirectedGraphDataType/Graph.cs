using System;
using System.Collections.Generic;
using System.Collections;
using System.IO;

namespace UndirectedGraphDataType
{
    class Graph : IEnumerable
    {
        #region Variables
        private int _V; // number of vertices
        private int _E; // number of edges
        private List<int>[] adj; // adjaceny lists

        public int V { get { return _V; } }
        public int E { get { return _E; } }
        #endregion

        #region Skeleton
        public Graph(int V)
        {
            this._V = V;
            this._E = 0;
            adj = new List<int>[V];         // Create array of lists.
            for (int v = 0; v < V; v++)     // Initialize all lists
                adj[v] = new List<int>();   // to empty.
        }
        public Graph(string path)
        {
            using (StreamReader sr = new StreamReader(path))
            {
                this._V = int.Parse(sr.ReadLine()); // Read V and construct this graph.
                this._E = 0;
                adj = new List<int>[_V];            // Create array of lists.
                for (int v = 0; v < _V; v++)        // Initialize all lists
                    adj[v] = new List<int>();       // to empty.

                int E = int.Parse(sr.ReadLine());   // Read E
                for (int i = 0; i < E; i++)
                {
                    string[] bits = sr.ReadLine().Split(' ');
                    int v = int.Parse(bits[0]);     // Read a vertex,
                    int w = int.Parse(bits[1]);     // read another vertex,
                    AddEdge(v, w);                  // and add edge connecting them.
                }
            }
        }
        public void AddEdge(int v, int w)
        {
            adj[v].Add(w);      // Add w to v's list
            adj[w].Add(v);      // Add v to w's list
            _E++;
        }
        public IEnumerator GetEnumerator()
        {
            for (int v = 0; v < _V; v++)
                yield return adj[v];
        }
        #endregion

        #region Graph-processing methods
        public string toString()
        {
            string s = _V + " vertices, " + _E + " edges \n";
            for (int v = 0; v < _V; v++)
            {
                s += v + ": ";
                foreach (int w in this.adj[v])
                    s += w + " ";
                s += "\n";
            }
            return s;
        }
        public int Degree(int v)
        {
            int degree = 0;
            foreach (int w in this.adj[v])
                degree++;
            return degree;
        }
        public int MaxDegree()
        {
            int max = 0;
            for (int v = 0; v < _V; v++)
                if (this.Degree(v) > max)
                    max = this.Degree(v);
            return max;
        }
        public int AvgDegree()
        {
            return 2 * _E / _V;
        }
        public int NumberOfSelfLoops()
        {
            int count = 0;
            for (int v = 0; v < _V; v++)
                foreach (int w in this.adj[v])
                    if (v == w) count++;
            return count / 2;
        }
        #endregion
    }
}
