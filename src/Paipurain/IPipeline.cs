using System.Threading.Tasks;

namespace Paipurain
{
    public interface IPipeline<TInput, TOutput>
    {
        Task<TOutput> Process(TInput input);
    }
}
