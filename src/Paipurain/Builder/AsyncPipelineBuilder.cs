using System;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace Paipurain.Builder
{
    public partial class PipelineBuilder<TInput, TOutput>
    {
        public PipelineBuilder<TInput, TOutput> AddBlock<TTransformFunctionInput, TTransformFunctionOutput>(
            Func<TTransformFunctionInput, Task<TTransformFunctionOutput>> asyncTransformFunction)
        {
            if (asyncTransformFunction == null)
                throw new ArgumentNullException();

            LinkToPredecessorBlock(
                CreateAsynchronousBlock(asyncTransformFunction));

            return this;
        }

        private IDataflowBlock CreateAsynchronousBlock<TTransformFunctionInput, TTransformFunctionOutput>(
            Func<TTransformFunctionInput, Task<TTransformFunctionOutput>> func)
        {
            return new TransformBlock<TransformWrapper<TOutput>, TransformWrapper<TOutput>>(
                async (unit) => new TransformWrapper<TOutput>(await func(unit.Value), unit.Completion));
        }
    }
}
