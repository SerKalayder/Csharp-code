using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GC_comparison_wpf.GC
{
    public class MockObject
    {
        public int Id { get; set; }
        public int ActiveReferences { get; set; }
        public int OccupiedCells { get; set; }

        public MockObject(int id)
        {
            Random rnd = new Random();
            ActiveReferences = rnd.Next(10);
            OccupiedCells = rnd.Next(15);
            Id = id;
        }

        public bool IsDead()
        {
            return ActiveReferences == 0 ? true : false;
        }
    }
}
