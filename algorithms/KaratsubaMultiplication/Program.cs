using System;
using System.Numerics;
using System.IO;

namespace KaratsubaMultiplication
{
    class Program
    {
        /// Karatzuba formula:
        /// A * B = A0*B0 + ((A0+A1)*(B0+B1) - A0*B0 - A1*B1)*BASE^K + A1+B1*BASE^N,
        /// where
        /// A = A0 + A1 * BASE^M
        /// B = B0 + B1 * BASE^M
        static void Main(string[] args)
        {
            BigInteger A0, A1, B0, B1, R;
            int K, N;
            Read(out A0, out A1, out B0, out B1, out K, out N);

            R = (A0 * B0 +
                BigInteger.Pow(10, K) * ((A0 + A1) * (B0 + B1) - (A0 * B0 + A1 * B1))
                + A1 * B1 * BigInteger.Pow(10, N));

            Console.WriteLine("{0}", R);
            Console.Read();
        }
        /// Read long numbers and initialize parameters
        /// A0 - The right particle of the first part of the number
        /// A1 - The left particle of the first part of the number
        /// B0 - The right particle of the second part of the number
        /// B1 - The left particle of the second part of the number
        /// K - Middle of the numbers
        /// N - Number of bits
        public static void Read(out BigInteger A0, out BigInteger A1,
                                out BigInteger B0, out BigInteger B1,
                                out int K, out int N)
        {
            using (StreamReader sr = new StreamReader("test.txt"))
            {
                string a, a0, a1, b, b0, b1;
                a = sr.ReadLine();
                b = sr.ReadLine();

                K = a.Length / 2;
                N = a.Length;

                // making strings to parse
                a0 = a.Substring(0, K);
                a1 = a.Substring(K);
                b0 = b.Substring(0, K);
                b1 = b.Substring(K);

                A0 = BigInteger.Parse(a1);
                A1 = BigInteger.Parse(a0);
                B0 = BigInteger.Parse(b1);
                B1 = BigInteger.Parse(b0);
            }

        }
    }
}