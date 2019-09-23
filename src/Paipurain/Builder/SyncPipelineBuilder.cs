﻿using System;
using System.Threading.Tasks.Dataflow;

namespace Paipurain.Builder
{
    public partial class PipelineBuilder<TInput, TOutput>
    {
        public PipelineBuilder<TInput, TOutput> Begin<TTransformFunctionOutput>(
            Func<TInput, TTransformFunctionOutput> transformFunction)
        {
            if (transformFunction == null)
                throw new ArgumentNullException();

            if (_initialBlock != null)
                throw new InvalidOperationException();

            _initialBlock = CreateSynchronousBlock(transformFunction);
            LinkToPredecessorBlock(_initialBlock);

            return this;
        }

        public IPipeline<TInput, TOutput> Build<TTransformFuncInput>(
            Func<TTransformFuncInput, TOutput> transformFunction)
        {
            if (transformFunction == null)
                throw new ArgumentNullException();

            AddBlock(transformFunction);

            return CreatePipeline();
        }

        public PipelineBuilder<TInput, TOutput> AddBlock<TTransformFunctionInput, TTransformFunctionOutput>(
            Func<TTransformFunctionInput, TTransformFunctionOutput> transformFunction)
        {
            if (transformFunction == null)
                throw new ArgumentNullException();

            if (_initialBlock == null)
                throw new InvalidOperationException();

            LinkToPredecessorBlock(
                CreateSynchronousBlock(transformFunction));

            return this;
        }

        private IDataflowBlock CreateSynchronousBlock<TTransformFunctionInput, TTransformFunctionOutput>(
            Func<TTransformFunctionInput, TTransformFunctionOutput> func)
        {
            return new TransformBlock<TransformWrapper<TOutput>, TransformWrapper<TOutput>>(
                (unit) => new TransformWrapper<TOutput>(func(unit.Value), unit.Completion));
        }
    }
}
