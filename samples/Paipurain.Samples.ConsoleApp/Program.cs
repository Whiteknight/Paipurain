using System;
using System.Threading.Tasks;

namespace Paipurain.Samples.ConsoleApp
{
    class Program
    {
        static async Task Main()
        {
            var bla = new PipelineBuilder<bool, string>()
                .AddUnit<bool, bool>(x => true)
                .AddUnit<bool, string>(async (t) => await Task.FromResult("hallo!"))
                .Build();

            var value = await bla.Process(true);

            Console.WriteLine("Hello World!");
        }
    }
}
