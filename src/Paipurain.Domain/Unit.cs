using System;

namespace Paipurain.Domain
{
    public class Unit<TInput, TOutput>
    {
        public Func<TInput, TOutput> ProcessingFunction { get; }

        public Unit(Func<TInput, TOutput> processingFunc)
        {
            ProcessingFunction = processingFunc ?? throw new ArgumentNullException();
        }
    }
}
