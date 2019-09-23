using System.Threading.Tasks;

namespace Paipurain
{
    internal class TransformWrapper<TOutput>
    {
        public dynamic Value { get; }
        public TaskCompletionSource<TOutput> Completion { get; set;  }

        internal TransformWrapper(
            dynamic value,
            TaskCompletionSource<TOutput> taskCompletionSource)
        {
            Value = value;
            Completion = taskCompletionSource;
        }
    }
}
