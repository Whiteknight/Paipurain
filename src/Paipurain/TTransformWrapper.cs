using System.Threading.Tasks;

namespace Paipurain
{
    internal class TransformWrapper<TInput, TOutput>
    {
        public TInput Value { get; }
        public TaskCompletionSource<TOutput> Completion { get; set;  }

        internal TransformWrapper(
            TInput value,
            TaskCompletionSource<TOutput> taskCompletionSource)
        {
            Value = value;
            Completion = taskCompletionSource;
        }
    }
}
