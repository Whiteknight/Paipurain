using Paipurain.Builder;

using System;
using System.Threading.Tasks;

namespace Paipurain.Samples.ConsoleApp
{
    class Program
    {
        static async Task Main()
        {
            var pipeline = new PipelineBuilder<bool>()
                .Begin((b) => true)
                .AddBlock<bool, string>(x => "")
                .AddBlock<string, string>(async (t) => await Task.FromResult("hallo!"))
                .Build<string>((t) => true);

            var value = await pipeline.Process(true);

            Console.WriteLine(value);
            Console.ReadLine();
        }
    }
}
