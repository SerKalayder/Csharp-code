using System.Collections.Generic;
using GC_comparison_wpf;
using System;

namespace GC_comparison_wpf.GC
{
    public class CopyingCollector : GarbageCollector
    {
        public Stack<MarkedObject> MarkTable { get; set; }

        public CopyingCollector()
        {
            MarkTable = new Stack<MarkedObject>();
        }

        public override MockHeap CollectGarbage(MockHeap h)
        {
            Heap = h;
            Mark();
            FreeMarked();
            Compact();
            return Heap;
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
                Heap.Dispose(x, y);
            }
        }

        public void Compact()
        {
            MockHeap mh = new MockHeap(Heap.Length);

            Stack<MockObject> mockObjectStack = new Stack<MockObject>();
            for (int i = 0; i < Heap.Length; i++)
                for (int j = 0; j < Heap.Length; j++)
                    if (Heap.Storage[i, j] != null)
                        mockObjectStack.Push(Heap.Storage[i, j]);
            Heap = new MockHeap(Heap.Length);
            while (mockObjectStack.Count > 0)
            {
                MockObject mockObject = mockObjectStack.Pop();
                Heap.Add(mockObject);
            }
        }

    }
}
