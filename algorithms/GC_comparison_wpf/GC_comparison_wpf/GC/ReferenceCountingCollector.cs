using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GC_comparison_wpf.GC
{
    public class ReferenceCountingCollector : GarbageCollector
    {
        public override MockHeap CollectGarbage(MockHeap h)
        {
            MockHeap Heap = h;
            for (int i = 0; i < Heap.Length; i++)
                for (int j = 0; j < Heap.Length; j++)
                    if (Heap.Storage[i, j] != null)
                        if (Heap.IsDead(i, j))
                            Heap.Dispose(i, j);
            return Heap;
        }
    }
}
