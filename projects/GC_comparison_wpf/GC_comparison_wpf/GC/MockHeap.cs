using System;

namespace GC_comparison_wpf.GC
{
    public class MockHeap
    {
        public MockObject[,] Storage { get; set; }
        public int Length { get; set; }

        public MockHeap(int elementsAmount)
        {
            Length = elementsAmount;
            Storage = new MockObject[Length, Length];
        }

        public void Allocate(MockObject mockObject)
        {
            Random rnd = new Random();
            int i = rnd.Next(Length);
            int j = rnd.Next(Length);
            bool allocationSuccess = false;

            while (!allocationSuccess)
            {
                if (Storage[i, j] == null)
                {
                    Storage[i, j] = mockObject;
                    allocationSuccess = true;
                }
                else { 
                    i = rnd.Next(Length);
                    j = rnd.Next(Length);
                }
            }
        }

        public void Dispose(int x, int y)
        {
            Storage[x, y] = null;
            Console.WriteLine("Disposed!!!", x, y);
        }

        public void Add(MockObject mockObject)
        {
            for (int i = 0; i < Length; i++)
            {
                for (int j = 0; j < Length; j++)
                    if (Storage[i, j] == null)
                    {
                        Storage[i, j] = mockObject;
                        break;
                    }
                break;
            }           
        }

        public bool IsEmpty()
        {
            bool empty = false;
            for (int i = 0; i < Length; i++)
                for (int j = 0; j < Length; j++)
                    if (Storage[i, j] == null) empty = true;
                    else return false;
            return empty;
        }

        public int MemoryOccupied()
        {
            int total = 0;
            for (int i = 0; i < Length; i++)
                for (int j = 0; j < Length; j++)
                    if (Storage[i, j] != null)
                        total += Storage[i, j].OccupiedCells;
            return total;
        }

        public int ObjectsInMemory()
        {
            int total = 0;
            for (int i = 0; i < Length; i++)
                for (int j = 0; j < Length; j++)
                    if (Storage[i, j] != null)
                        total++;
            return total;
        }

        public bool IsDead(int i, int j)
        {
            return (Storage[i,j].ActiveReferences == 0) ? true : false;
        }

    }
}
