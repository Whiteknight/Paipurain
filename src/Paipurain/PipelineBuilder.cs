using Paipurain.Application;

using System;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace Paipurain
{
    public class PipelineBuilder<TInput, TOutput>
    {
        private IDataflowBlock _initialUnit;
        private IDataflowBlock _lastUnit;

        public PipelineBuilder<TInput, TOutput> AddUnit<TTransformFunctionInput, TTransformFunctionOutput>(
            Func<TTransformFunctionInput, TTransformFunctionOutput> transformFunction)
        {
            if (transformFunction == null)
                throw new ArgumentNullException();

            var block = new TransformBlock<TransformWrapper<TOutput>, TransformWrapper<TOutput>>(
                (unit) => new TransformWrapper<TOutput>(transformFunction(unit.Value), unit.Completion));

            LinkToPredecessorBlock(block);

            return this;
        }

        public PipelineBuilder<TInput, TOutput> AddUnit<TTransformFunctionInput, TTransformFunctionOutput>(
            Func<TTransformFunctionInput, Task<TTransformFunctionOutput>> asyncTransformFunction)
        {
            if (asyncTransformFunction == null)
                throw new ArgumentNullException();

            var block = new TransformBlock<TransformWrapper<TOutput>, TransformWrapper<TOutput>>(
                async (unit) => new TransformWrapper<TOutput>(await asyncTransformFunction(unit.Value), unit.Completion));

            LinkToPredecessorBlock(block);

            return this;
        }

        public IPipeline<TInput, TOutput> Build()
        {
            var initialUnit = _initialUnit as ITargetBlock<TransformWrapper<TOutput>>;

            var resultUnit = new ActionBlock<TransformWrapper<TOutput>>((tc) =>
                tc.Completion.SetResult((TOutput)tc.Value));

            if (!(_lastUnit is ISourceBlock<TransformWrapper<TOutput>> tailSourceUnit))
                throw new InvalidOperationException();

            tailSourceUnit.LinkTo(resultUnit);

            return new Pipeline<TInput, TOutput>(initialUnit);
        }

        private void LinkToPredecessorBlock(IDataflowBlock block)
        {
            if (block == null)
                return;

            if (_initialUnit == null)
                _initialUnit = block;

            if (_lastUnit == null)
            {
                _lastUnit = block;
                return;
            }

            if (!(_lastUnit is ISourceBlock<TransformWrapper<TOutput>> sourceBlock))
                return;

            sourceBlock.LinkTo(block as ITargetBlock<TransformWrapper<TOutput>>);
            _lastUnit = block;
        }
    }
}
