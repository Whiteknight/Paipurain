using System;
using System.Threading.Tasks.Dataflow;

namespace Paipurain.Builder
{
    public partial class PipelineBuilder<TInput, TOutput>
    {
        private IDataflowBlock _initialBlock;
        private IDataflowBlock _lastBlock;

        public IPipeline<TInput, TOutput> Build()
        {
            var initialBlock = _initialBlock as ITargetBlock<TransformWrapper<TOutput>>;
            CreateResultBlock();

            return new Pipeline<TInput, TOutput>(initialBlock);
        }

        private void CreateResultBlock()
        {
            var resultActionBlock = new ActionBlock<TransformWrapper<TOutput>>((tc) =>
                tc.Completion.SetResult((TOutput)tc.Value));

            if (!(_lastBlock is ISourceBlock<TransformWrapper<TOutput>> lastAsSourceBlock))
                throw new InvalidOperationException();

            lastAsSourceBlock.LinkTo(resultActionBlock);
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

            if (_initialBlock == null)
            {
                _initialBlock = block;
            }

            _lastBlock = block;
        }
    }
}
