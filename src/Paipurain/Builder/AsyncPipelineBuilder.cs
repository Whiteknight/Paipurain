using Paipurain.Application;

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

            if (_beginUnit != null)
                throw new InvalidOperationException();

            _beginUnit = CreateAsynchronousBlock(asyncTransformFunction);
            LinkToPredecessorBlock(_beginUnit);

            return this;
        }

        public IPipeline<TInput, TOutput> Build<TTransformFuncInput>(
            Func<TTransformFuncInput, Task<TOutput>> asyncTransformFunction)
        {
            if (asyncTransformFunction == null)
                throw new ArgumentNullException();

            AddUnit(asyncTransformFunction);

            return CreatePipeline();
        }

        public PipelineBuilder<TInput, TOutput> AddUnit<TTransformFunctionInput, TTransformFunctionOutput>(
            Func<TTransformFunctionInput, Task<TTransformFunctionOutput>> asyncTransformFunction)
        {
            if (asyncTransformFunction == null)
                throw new ArgumentNullException();

            if (_beginUnit == null)
                throw new InvalidOperationException();

            LinkToPredecessorBlock(
                CreateAsynchronousBlock(asyncTransformFunction));

            return this;
        }

        private IDataflowBlock CreateAsynchronousBlock<TTransformFunctionInput, TTransformFunctionOutput>(
            Func<TTransformFunctionInput, Task<TTransformFunctionOutput>> func,
            bool asInitialUnit = false)
        {
            return new TransformBlock<TransformWrapper<TOutput>, TransformWrapper<TOutput>>(
                async (unit) => new TransformWrapper<TOutput>(await func(unit.Value), unit.Completion));
        }
    }
}
