using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GC_comparison_wpf.GC
{
    public class GarbageCollector
    {
        public MockHeap Heap { get; set; }

        public virtual MockHeap CollectGarbage(MockHeap h)
        {
            Heap = h;
            return Heap;
        }
    }
}
