using System.Threading.Tasks;

namespace Paipurain.Application
{
    public interface IPipeline<TInput, TOutput>
    {
        Task<TOutput> Process(TInput input);
    }
}
