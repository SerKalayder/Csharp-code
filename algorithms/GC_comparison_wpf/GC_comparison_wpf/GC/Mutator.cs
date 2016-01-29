using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GC_comparison_wpf.GC
{
    class Mutator
    {
        public static void Do(MockHeap heap)
        {
            Console.WriteLine("MUTATOR IN WORK");
            for (int i = 0; i < heap.Length; i++)
                for (int j = 0; j < heap.Length; j++) { 
                    if (heap.Storage[i, j] != null && !heap.Storage[i, j].IsDead())
                        heap.Storage[i, j].ActiveReferences--;

                    /*
                    if (heap.Storage[i, j] != null && heap.Storage[i, j].IsDead())
                    {
                        heap.Storage[i, j] = null;
                    }
                    */
                }
        }
    }
}
