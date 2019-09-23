using System;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace Paipurain.Builder
{
    public partial class PipelineBuilder<TInput, TOutput>
    {
        public PipelineBuilder<TInput, TOutput> Begin<TTransformFunctionOutput>(
            Func<TInput, Task<TTransformFunctionOutput>> asyncTransformFunction)
        {
            if (asyncTransformFunction == null)
                throw new ArgumentNullException();

            if (_initialBlock != null)
                throw new InvalidOperationException();

            _initialBlock = CreateAsynchronousBlock(asyncTransformFunction);
            LinkToPredecessorBlock(_initialBlock);

            return this;
        }

        public IPipeline<TInput, TOutput> Build<TTransformFuncInput>(
            Func<TTransformFuncInput, Task<TOutput>> asyncTransformFunction)
        {
            if (asyncTransformFunction == null)
                throw new ArgumentNullException();

            AddBlock(asyncTransformFunction);

            return CreatePipeline();
        }

        public PipelineBuilder<TInput, TOutput> AddBlock<TTransformFunctionInput, TTransformFunctionOutput>(
            Func<TTransformFunctionInput, Task<TTransformFunctionOutput>> asyncTransformFunction)
        {
            if (asyncTransformFunction == null)
                throw new ArgumentNullException();

            if (_initialBlock == null)
                throw new InvalidOperationException();

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
