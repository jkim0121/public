using System;
using System.Linq;

namespace Clear
{
    class Program
    {
        static void Main(string[] args)
        {
            var inst = new Measure();

            //var output = inst.FizzBuzz(int.MaxValue);
            var output = inst.FizzBuzz(100, 
                i => i % 3 == 0 && i % 5 == 0 ? (true, "fizz buzz") : (false, null), 
                i => i % 3 == 0 ? (true, "fizz") : (false, null)
                );

            foreach (var entry in output)
            {
                Console.WriteLine(entry);
            }
        }
    }
}
