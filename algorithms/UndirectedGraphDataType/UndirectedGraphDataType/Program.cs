using System;
using System.Collections.Generic;

namespace UndirectedGraphDataType
{
    class Program
    {
        static void Main(string[] args)
        {
            Graph g = new Graph("tinyG.txt");
            Console.Write(g.toString());

            Console.Read();
        }
    }
}
