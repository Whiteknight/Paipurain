using Paipurain.Builder;

using System;
using System.IO;
using System.Threading.Tasks;

namespace Paipurain.Samples.ConsoleApp
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var pipeline = new PipelineBuilder<string[], string>()
                .AddBlock<string[], string>(GetFilePath)
                .AddBlock<string, string>(ReadFileContent)
                .AddBlock<string, string>(AttachText)
                .Build();

            var value = await pipeline.Process(args);

            Console.WriteLine(value);
            Console.ReadLine();
        }

        private static string GetFilePath(string[] args) => args[0];
        private static string ReadFileContent(string path) => File.ReadAllText(path);
        private static string AttachText(string content) => $"{content} from the command line parsing sample!";
    }
}
