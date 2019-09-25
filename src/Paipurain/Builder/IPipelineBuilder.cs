using System;
using System.Threading.Tasks;

namespace Paipurain.Builder
{
    public interface IPipelineBuilder<TPipeInput, TPipeOutput, TInput>
    {
        // TPipeInput is the initial input to the entire pipeline, which we need for the .Build() method
        // TPipeOutput is the final output of the entire pipeline, which we need for the .Build() method
        // TInput is the input to this stage (output from the previous stage)
        // TOutput is the output of each individual stage.

        IPipelineBuilder<TPipeInput, TPipeOutput, TOutput> AddBlock<TOutput>(
            Func<TInput, TOutput> transformFunction);

        IPipelineBuilder<TPipeInput, TPipeOutput, TOutput> AddBlockAsync<TOutput>(
            Func<TInput, Task<TOutput>> asyncTransformFunction);

        IPipeline<TPipeInput, TPipeOutput> Build();
    }
}