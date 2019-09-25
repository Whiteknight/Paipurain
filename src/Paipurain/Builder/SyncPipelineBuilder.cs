using System;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace Paipurain.Builder
{
    public partial class PipelineBuilder<TInput, TOutput>
    {
        public IPipelineBuilder<TInput, TOutput, TTransformFunctionOutput> AddBlock<TTransformFunctionOutput>(
            Func<TInput, TTransformFunctionOutput> transformFunction)
        {
            if (transformFunction == null)
                throw new ArgumentNullException();

            LinkToPredecessorBlock(
                CreateSynchronousBlock(transformFunction));

            return new IntermediatePipelineBuilder<TTransformFunctionOutput>(this);
        }

        private IDataflowBlock CreateSynchronousBlock<TTransformFunctionInput, TTransformFunctionOutput>(
            Func<TTransformFunctionInput, TTransformFunctionOutput> func)
        {
            return new TransformBlock<TransformWrapper<TOutput>, TransformWrapper<TOutput>>(
                (unit) => new TransformWrapper<TOutput>(func(unit.Value), unit.Completion));
        }

        private class IntermediatePipelineBuilder<TIn> : IPipelineBuilder<TInput, TOutput, TIn>
        {
            private readonly PipelineBuilder<TInput, TOutput> _builder;

            public IntermediatePipelineBuilder(PipelineBuilder<TInput, TOutput> builder)
            {
                _builder = builder;
            }


            public IPipelineBuilder<TInput, TOutput, TTransformFunctionOutput> AddBlock<TTransformFunctionOutput>(Func<TIn, TTransformFunctionOutput> transformFunction)
            {
                if (transformFunction == null)
                    throw new ArgumentNullException();

                _builder.LinkToPredecessorBlock(
                    _builder.CreateSynchronousBlock(transformFunction));

                return new IntermediatePipelineBuilder<TTransformFunctionOutput>(_builder);
            }

            public IPipelineBuilder<TInput, TOutput, TTransformFunctionOutput> AddBlockAsync<TTransformFunctionOutput>(Func<TIn, Task<TTransformFunctionOutput>> asyncTransformFunction)
            {
                if (asyncTransformFunction == null)
                    throw new ArgumentNullException();

                _builder.LinkToPredecessorBlock(
                    _builder.CreateAsynchronousBlock(asyncTransformFunction));

                return new IntermediatePipelineBuilder<TTransformFunctionOutput>(_builder);
            }

            public IPipeline<TInput, TOutput> Build()
            {
                return _builder.Build();
            }
        }
    }
}
