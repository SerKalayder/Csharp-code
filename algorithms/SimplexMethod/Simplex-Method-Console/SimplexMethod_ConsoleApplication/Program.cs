using System;

namespace SimplexMethod_ConsoleApplication
{
    class Program
    {
        static void Main(string[] args)
        {
            SimplexTable t = new SimplexTable();
            t.Show();

            t.Solve();

            Console.ReadKey();
        }
    }
}
