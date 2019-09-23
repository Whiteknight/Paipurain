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
                .AddBlock<bool, string>(BeginMethod)
                .AddBlock<string, bool>(async (t) => await Task.FromResult(true))
                .Build();

            var value = await pipeline.Process(true);

            Console.WriteLine(value);
            Console.ReadLine();
        }

        private static string BeginMethod(bool input)
        {
            return input ? ":)" : ":(";
        }
    }
}
