using Paipurain.Builder;
using System;
using System.Threading.Tasks;

namespace Paipurain.Samples.ConsoleApp
{
    class Program
    {
        static async Task Main()
        {
            var bla = new PipelineBuilder<bool, string>()
                .Begin((b) => true)
                .AddUnit<bool, string>(x => "")
                .AddUnit<string, string>(async (t) => await Task.FromResult("hallo!"))
                .Build<string>(

            var value = await bla.Process(true);

            Console.WriteLine(value);
            Console.ReadLine();
        }
    }
}
