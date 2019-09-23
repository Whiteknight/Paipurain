﻿using Paipurain.Application;

using System;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace Paipurain
{
    internal class Pipeline<TInput, TOutput> : IPipeline<TInput, TOutput>
    {
        private readonly ITargetBlock<TransformWrapper<TOutput>> _headUnit;
        private readonly TaskCompletionSource<TOutput> _completion;

        internal Pipeline(ITargetBlock<TransformWrapper<TOutput>> headUnit)
        {
            _headUnit = headUnit ?? throw new ArgumentNullException();
            _completion = new TaskCompletionSource<TOutput>();
        }

        public Task<TOutput> Process(TInput input)
        {
            _headUnit.SendAsync(new TransformWrapper<TOutput>(input, _completion));

            return _completion.Task;
        }
    }
}
