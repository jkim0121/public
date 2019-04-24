using System;
using System.Linq;

namespace Clear
{
    class Program
    {
        static void Main(string[] args)
        {
            var inst = new Measure();

            var output = inst.FizzBuzz(int.MaxValue);

            foreach (var entry in output)
            {
                Console.WriteLine(entry);
            }
        }
    }
}
