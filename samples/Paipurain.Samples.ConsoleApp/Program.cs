using System;
using System.Threading.Tasks;

namespace Paipurain.Samples.ConsoleApp
{
    class Program
    {
        static async Task Main()
        {
            var bla = new PipelineBuilder<string, bool>()
                //.AddUnit<string, bool>(async (t) => await Task.FromResult(false))
                .AddUnit<bool, bool>(x => true)
                .Build();

            var value = await bla.Process("bla");

            Console.WriteLine("Hello World!");
        }
    }
}
