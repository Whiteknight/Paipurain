using System;
using System.Threading.Tasks;

namespace Paipurain.Samples.ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            //var pipeline = new Pipeline<string, Task>();

            //pipeline.AddUnit<string, bool>((t) =>
            //{
            //    return false;
            //});

            //pipeline.AddUnit<bool, bool>((t) =>
            //{
            //    return false;
            //});

            //pipeline.ProcessAsync("test").Wait();

            var bla = new PipelineBuilder<string, bool>()
                .AddUnit<string, bool>((t) => false)
                .AddUnit<bool, bool>(x => true);

            Console.WriteLine("Hello World!");
        }
    }
}
