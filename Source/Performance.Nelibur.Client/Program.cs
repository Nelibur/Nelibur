using System;

namespace Performance.Nelibur.Client
{
    internal class Program
    {
        private const int Iterations = 5;

        private static void Main()
        {
            Console.WriteLine("Starting.. Total iterations: {0}", Iterations);

            var tests = new PerformanceTests();
            tests.Run(Iterations);

            Console.WriteLine("\nComplete");
            Console.ReadKey();
        }
    }
}
