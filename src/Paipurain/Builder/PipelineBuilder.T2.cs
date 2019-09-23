using Paipurain.Application;

using System;
using System.Threading.Tasks.Dataflow;

namespace Paipurain.Builder
{
    public partial class PipelineBuilder<TInput, TOutput>
    {
        private IDataflowBlock _initialBlock;
        private IDataflowBlock _lastBlock;

        private IPipeline<TInput, TOutput> CreatePipeline()
        {
            var initialBlock = _initialBlock as ITargetBlock<TransformWrapper<TOutput>>;
            CreateResultBlock();

            return new Pipeline<TInput, TOutput>(initialBlock);
        }

        private void CreateResultBlock()
        {
            var resultActionBlock = new ActionBlock<TransformWrapper<TOutput>>((tc) =>
                tc.Completion.SetResult((TOutput)tc.Value));

            if (!(_lastBlock is ISourceBlock<TransformWrapper<TOutput>> lastBlockAsSource))
                throw new InvalidOperationException();

            lastBlockAsSource.LinkTo(resultActionBlock);
        }

        private void LinkToPredecessorBlock(IDataflowBlock block)
        {
            if (block == null)
                return;

            if (_lastBlock != null)
            {
                if (!(_lastBlock is ISourceBlock<TransformWrapper<TOutput>> sourceBlock))
                    return;

                var targetBlock = block as ITargetBlock<TransformWrapper<TOutput>>;

                sourceBlock.LinkTo(targetBlock);
            }

            _lastBlock = block;
        }
    }
}
