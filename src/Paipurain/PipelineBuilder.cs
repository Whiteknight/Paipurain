using Paipurain.Application;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks.Dataflow;

namespace Paipurain
{
    public class PipelineBuilder<TInput, TOutput>
    {
        private readonly List<IDataflowBlock> _blocks = new List<IDataflowBlock>();

        private ISourceBlock<TransformWrapper<dynamic, dynamic>> _lastBlock;

        public PipelineBuilder<TInput, TOutput> AddUnit<TTransformFunctionInput, TTransformFunctionOutput>(
            Func<TTransformFunctionInput, TTransformFunctionOutput> transformFunction)
        {
            if (transformFunction == null)
                throw new ArgumentNullException();

            var block = new TransformBlock<TransformWrapper<TTransformFunctionInput, TOutput>, TransformWrapper<TTransformFunctionOutput, TOutput>>(
                (unit) => new TransformWrapper<TTransformFunctionOutput, TOutput>(transformFunction(unit.Value), unit.Completion));

            _lastBlock = block as ISourceBlock<TransformWrapper<dynamic, dynamic>>;

            LinkToLastBlock(block);

            return this;
        }

        //public PipelineBuilder<TInput, TOutput> AddUnit<TTransformFunctionInput, TTransformFunctionOutput>(
        //    Func<TTransformFunctionInput, Task<TTransformFunctionOutput>> asyncTransformFunction)
        //{
        //    if (asyncTransformFunction == null)
        //        throw new ArgumentNullException();

        //    var block = new TransformBlock<TransformWrapper<TTransformFunctionInput, TOutput>, TransformWrapper<TTransformFunctionOutput, TOutput>>(
        //        async (unit) => new TransformWrapper<TTransformFunctionOutput, TOutput>(await asyncTransformFunction(unit.Value), unit.Completion));

        //    LinkToLastBlock(block);

        //    return this;
        //}

        public IPipeline<TInput, TOutput> Build()
        {
            var headUnit = _blocks.FirstOrDefault() as ITargetBlock<TransformWrapper<TInput, TOutput>>;
            CreateResultUnit();

            return new Pipeline<TInput, TOutput>(headUnit);
        }

        private bool CreateResultUnit()
        {
            var resultUnit = new ActionBlock<TransformWrapper<TOutput, TOutput>>((tc) =>
                tc.Completion.SetResult(tc.Value));

            if (!(_blocks.Last() is ISourceBlock<TransformWrapper<TOutput, TOutput>> tailSourceUnit))
                return false;

            tailSourceUnit.LinkTo(resultUnit);

            return true;
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
