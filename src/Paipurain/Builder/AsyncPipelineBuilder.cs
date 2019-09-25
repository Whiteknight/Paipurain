using System;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace Paipurain.Builder
{
    public partial class PipelineBuilder<TInput, TOutput>
    {
        public IPipelineBuilder<TInput, TOutput, TTransformFunctionOutput> AddBlockAsync<TTransformFunctionOutput>(
            Func<TInput, Task<TTransformFunctionOutput>> asyncTransformFunction)
        {
            if (asyncTransformFunction == null)
                throw new ArgumentNullException();

            LinkToPredecessorBlock(
                CreateAsynchronousBlock(asyncTransformFunction));

            return new IntermediatePipelineBuilder<TTransformFunctionOutput>(this);
        }

        private IDataflowBlock CreateAsynchronousBlock<TTransformFunctionInput, TTransformFunctionOutput>(
            Func<TTransformFunctionInput, Task<TTransformFunctionOutput>> func)
        {
            return new TransformBlock<TransformWrapper<TOutput>, TransformWrapper<TOutput>>(
                async (unit) => new TransformWrapper<TOutput>(await func(unit.Value), unit.Completion));
        }
    }
}
