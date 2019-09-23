using System;
using System.Threading.Tasks.Dataflow;

namespace Paipurain
{
    public partial class PipelineBuilder<TInput, TOutput>
    {
        private IDataflowBlock _initialUnit;
        private IDataflowBlock _lastUnit;

        public PipelineBuilder<TInput, TOutput> AddUnit<TTransformFunctionInput, TTransformFunctionOutput>(
            Func<TTransformFunctionInput, TTransformFunctionOutput> transformFunction)
        {
            if (transformFunction == null)
                throw new ArgumentNullException();

            var block = new TransformBlock<TransformWrapper<TOutput>, TransformWrapper<TOutput>>(
                (unit) => new TransformWrapper<TOutput>(transformFunction(unit.Value), unit.Completion));

            LinkToPredecessorBlock(block);

            return this;
        }
    }
}
