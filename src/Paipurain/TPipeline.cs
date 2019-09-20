using Paipurain.Domain;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks.Dataflow;

namespace Paipurain
{
    public class Pipeline<TInput, TOutput>
    {
        private readonly List<IDataflowBlock> _blocks = new List<IDataflowBlock>();

        public void AddUnit<TUnitInput, TUnitOutput>(Unit<TUnitInput, TUnitOutput> pipelineUnit)
        {
            if (pipelineUnit == null)
                return;

            var block = new TransformBlock<TransformWrapper<TUnitInput, TOutput>, TransformWrapper<TUnitOutput, TOutput>>((unit) =>
                new TransformWrapper<TUnitOutput, TOutput>(pipelineUnit.ProcessingFunction(unit.Value), unit.Completion));

            LinkToLastBlock(block);
        }

        private void LinkToLastBlock<TUnitInput, TUnitOutput>(
            TransformBlock<TransformWrapper<TUnitInput, TOutput>, TransformWrapper<TUnitOutput, TOutput>> block)
        {
            if (block == null)
                return;

            if (_blocks.Any())
            {
                var sourceBlock = _blocks.LastOrDefault() as ISourceBlock<TransformWrapper<TUnitInput, TOutput>>;

                if (sourceBlock == null)
                    throw new InvalidOperationException();

                sourceBlock.LinkTo(block, new DataflowLinkOptions());
            }

            _blocks.Add(block);
        }
    }
}
