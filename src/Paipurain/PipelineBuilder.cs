using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks.Dataflow;

namespace Paipurain
{
    public class PipelineBuilder<TInput, TOutput>
    {
        private readonly List<IDataflowBlock> _blocks = new List<IDataflowBlock>();

        public PipelineBuilder<TInput, TOutput> AddUnit<TTransformFuncInput, TTransformFuncOutput>(
            Func<TTransformFuncInput, TTransformFuncOutput> transformFunc)
        {
            if (transformFunc == null)
                throw new ArgumentNullException();

            var block = new TransformBlock<TransformWrapper<TTransformFuncInput, TOutput>, TransformWrapper<TTransformFuncOutput, TOutput>>(
                (unit) => new TransformWrapper<TTransformFuncOutput, TOutput>(transformFunc(unit.Value), unit.Completion));

            LinkToLastBlock(block);

            return this;
        }

        private void LinkToLastBlock<TFuncInput, TFuncOutput>(
            TransformBlock<TransformWrapper<TFuncInput, TOutput>, TransformWrapper<TFuncOutput, TOutput>> block)
        {
            if (block == null)
                return;

            if (_blocks.Any())
            {
                if (!(_blocks.LastOrDefault() is ISourceBlock<TransformWrapper<TFuncInput, TOutput>> sourceBlock))
                    throw new InvalidOperationException();

                sourceBlock.LinkTo(block, new DataflowLinkOptions());
            }

            _blocks.Add(block);
        }
    }
}
