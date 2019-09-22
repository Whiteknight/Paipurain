using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace Paipurain
{
    public class Pipeline<TInput, TOutput>
    {
        internal readonly List<IDataflowBlock> _blocks = new List<IDataflowBlock>();

        //public Task ProcessAsync(TInput input)
        //{
        //    var firstStep = _blocks[0] as ITargetBlock<TransformWrapper<TInput, TOutput>>;
        //    var tcs = new TaskCompletionSource<TOutput>();
        //    firstStep.SendAsync(new TransformWrapper<TInput, TOutput>(input, tcs));
        //    return tcs.Task;
        //}

        //public void AddUnit<TTransformFuncInput, TTransformFuncOutput>(Func<TTransformFuncInput, TTransformFuncOutput> transformFunc)
        //{
        //    if (transformFunc == null)
        //        return;

        //    var block = new TransformBlock<TransformWrapper<TTransformFuncInput, TOutput>, TransformWrapper<TTransformFuncOutput, TOutput>>((unit) =>
        //        new TransformWrapper<TTransformFuncOutput, TOutput>(transformFunc(unit.Value), unit.Completion));

        //    LinkToLastBlock(block);
        //}

        //private void LinkToLastBlock<TFuncInput, TFuncOutput>(
        //    TransformBlock<TransformWrapper<TFuncInput, TOutput>, TransformWrapper<TFuncOutput, TOutput>> block)
        //{
        //    if (block == null)
        //        return;

        //    if (_blocks.Any())
        //    {
        //        if (!(_blocks.LastOrDefault() is ISourceBlock<TransformWrapper<TFuncInput, TOutput>> sourceBlock))
        //            throw new InvalidOperationException();

        //        sourceBlock.LinkTo(block, new DataflowLinkOptions());
        //    }

        //    _blocks.Add(block);
        //}
    }
}
