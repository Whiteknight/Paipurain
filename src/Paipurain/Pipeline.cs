using Paipurain.Application;

using System;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace Paipurain
{
    internal class Pipeline<TInput, TOutput> : IPipeline<TInput, TOutput>
    {
        private readonly ITargetBlock<TransformWrapper<TOutput>> _beginBlock;
        private readonly TaskCompletionSource<TOutput> _completion;

        internal Pipeline(ITargetBlock<TransformWrapper<TOutput>> beginBlock)
        {
            _beginBlock = beginBlock ?? throw new ArgumentNullException();
            _completion = new TaskCompletionSource<TOutput>();
        }

        public Task<TOutput> Process(TInput input)
        {
            _beginBlock.SendAsync(new TransformWrapper<TOutput>(input, _completion));

            return _completion.Task;
        }
    }
}
