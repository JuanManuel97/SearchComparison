using System.Threading.Tasks;

namespace SearchComparison.Services
{
    public interface ISearchClient
    {
        string ClientName { get; }
        Task<long> GetResultsCountAsync(string query);
    }
}
