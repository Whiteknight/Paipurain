using Paipurain.Application;

using System;
using System.Threading.Tasks.Dataflow;

namespace Paipurain.Builder
{
    public partial class PipelineBuilder<TInput, TOutput>
    {
        private IDataflowBlock _beginUnit;
        private IDataflowBlock _lastUnit;

        private IPipeline<TInput, TOutput> CreatePipeline()
        {
            var initialUnit = _beginUnit as ITargetBlock<TransformWrapper<TOutput>>;
            CreateResultBlock();

            return new Pipeline<TInput, TOutput>(initialUnit);
        }

        private void CreateResultBlock()
        {
            var resultUnit = new ActionBlock<TransformWrapper<TOutput>>((tc) =>
                tc.Completion.SetResult((TOutput)tc.Value));

            if (!(_lastUnit is ISourceBlock<TransformWrapper<TOutput>> tailSourceUnit))
                throw new InvalidOperationException();

            tailSourceUnit.LinkTo(resultUnit);
        }

        private void LinkToPredecessorBlock(IDataflowBlock block)
        {
            if (block == null)
                return;

            if (_lastUnit != null)
            {
                if (!(_lastUnit is ISourceBlock<TransformWrapper<TOutput>> sourceBlock))
                    return;

                var targetBlock = block as ITargetBlock<TransformWrapper<TOutput>>;

                sourceBlock.LinkTo(targetBlock);
            }

            _lastUnit = block;
        }
    }
}
