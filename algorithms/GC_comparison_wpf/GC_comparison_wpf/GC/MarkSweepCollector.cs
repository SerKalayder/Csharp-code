using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GC_comparison_wpf.GC
{
    public class MarkSweepCollector : GarbageCollector
    {
        public Stack<MarkedObject> MarkTable { get; set; }

        public MarkSweepCollector()
        {
            MarkTable = new Stack<MarkedObject>();
        }

        public override MockHeap CollectGarbage(MockHeap h)
        {
            this.Heap = h;
            Mark();
            FreeMarked();
            Console.WriteLine("HEAP TO BE RETURNED");
            PrintToConsole();

            return Heap;
        }

        public void PrintToConsole()
        {
            Console.WriteLine("****************** REDRAWN");
            for (int i = 0; i < Heap.Length; i++)
            {
                for (int j = 0; j < Heap.Length; j++)
                {
                    string s = (Heap.Storage[i, j] != null) ? "   "
                        + Heap.Storage[i, j].ActiveReferences : "null";
                    Console.Write(s + " ");
                    //Convert.ToString(h.Storage[i, j].Id) 
                }
                Console.WriteLine();
            }
            Console.WriteLine("****************** END");
            Console.WriteLine("****************** END");
        }

        public void Mark()
        {
            for (int i = 0; i < Heap.Length; i++)
                for (int j = 0; j < Heap.Length; j++)
                    if (Heap.Storage[i, j] != null)
                        if (Heap.IsDead(i, j))
                        {
                            Console.WriteLine("Object is dead");
                            MarkedObject markedObject = new MarkedObject(i, j);
                            MarkTable.Push(markedObject);
                        }
        }

        public void FreeMarked()
        {
            MarkedObject markedObject = null;
            int x, y = 0;
            while (MarkTable.Count > 0)
            {
                markedObject = MarkTable.Pop();
                x = markedObject.X;
                y = markedObject.Y;
                Console.WriteLine("Disposeing {0} and {1}!!!", x, y);
                Heap.Dispose(x, y);
            }
        }
    }

    public class MarkedObject
    {
        public int X { get; set; }
        public int Y { get; set; }
        public MarkedObject(int x, int y)
        {
            X = x;
            Y = y;
        }
    }
}
