using Paipurain.Domain;
using System;
using System.Threading.Tasks;

namespace Paipurain.Samples.ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var pipeline = new Pipeline<string, Task>();
            pipeline.AddUnit(new Unit<string, bool>((t) =>
            {
                return false;
            }));

            pipeline.AddUnit(new Unit<string, bool>((t) =>
            {
                return false;
            }));

            Console.WriteLine("Hello World!");
        }
    }
}
