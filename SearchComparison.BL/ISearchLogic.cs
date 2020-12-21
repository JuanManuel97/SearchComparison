using SearchComparison.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SearchComparison.BL
{
    public interface ISearchLogic
    {
        Task<string> GetResultsReport(System.Collections.Generic.List<string> querys);
        Task<List<SearchResult>> GetResults(IEnumerable<string> querys);
        IEnumerable<SearchWinner> GetWinners(List<SearchResult> searchResults);
        string GetTotalWinner(List<SearchResult> searchResults);
        IEnumerable<IGrouping<string, SearchResult>> GetMainResults(List<SearchResult> searchResults);
    }
}
