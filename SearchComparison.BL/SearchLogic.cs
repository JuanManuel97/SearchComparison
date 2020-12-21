using SearchComparison.Entities;
using SearchComparison.Services;
using SearchComparison.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SearchComparison.BL
{
    public class SearchLogic : ISearchLogic
    {
        private readonly IEnumerable<ISearchClient> _searchClients;
        private readonly StringBuilder _stringBuilder;

        public SearchLogic(IEnumerable<ISearchClient> searchClients)
        {
            _searchClients = searchClients;
            _stringBuilder = new StringBuilder();
        }

        public IEnumerable<IGrouping<string, SearchResult>> GetMainResults(List<SearchResult> searchResults)
        {
            if (searchResults == null)
                throw new ArgumentNullException(nameof(searchResults));

            var results = searchResults.OrderBy(result => result.SearchClient).ToLookup(result => result.Query, result => result);
            return results;
        }

        public async Task<List<SearchResult>> GetResults(IEnumerable<string> querys)
        {
            var results = new List<SearchResult>();

            foreach (var keyword in querys)
            {
                foreach (var searchClient in _searchClients)
                {
                    results.Add(new SearchResult
                    {
                        SearchClient = searchClient.ClientName,
                        Query = keyword,
                        TotalResults = await searchClient.GetResultsCountAsync(keyword)
                    });
                }
            }

            return results;
        }

        public async Task<string> GetResultsReport(List<string> querys)
        {
            if (querys == null)
                throw new ArgumentNullException(nameof(querys));

            try
            {
                var searchResults = await GetResults(querys);

                var winners = GetWinners(searchResults);
                var totalWinner = GetTotalWinner(searchResults);
                var mainResults = GetMainResults(searchResults);


                var clientResultsString = mainResults
                    .Select(resultsGroup =>
                        $"{resultsGroup.Key}: {string.Join(" ", resultsGroup.Select(client => $"{client.SearchClient}: {client.TotalResults}"))}")
                    .ToList();

                var winnerString = winners.Select(client => $"{client.SearchClient} winner: {client.Winner}")
                    .ToList();

                var totalWinnerStr = $"Total winner: {totalWinner}";

                clientResultsString.ForEach(queryResults => _stringBuilder.AppendLine(queryResults));
                winnerString.ForEach(w => _stringBuilder.AppendLine(w));

                _stringBuilder.AppendLine(totalWinnerStr);

                return _stringBuilder.ToString();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message, e);
            }
        }

        public string GetTotalWinner(List<SearchResult> searchResults)
        {
            if (searchResults == null)
                throw new ArgumentNullException(nameof(searchResults));

            var totalWinner = searchResults.OrderBy(result => result.SearchClient)
                                            .GroupBy(result => result.Query, result => result, (query, result) => new { Query = query, Total = result.Sum(r => r.TotalResults) })
                                            .MaxValue(r => r.Total).Query;

            return totalWinner;
        }

        public IEnumerable<SearchWinner> GetWinners(List<SearchResult> searchResults)
        {
            if (searchResults == null)
                throw new ArgumentNullException(nameof(searchResults));

            var winners = searchResults.OrderBy(result => result.SearchClient).GroupBy(result => result.SearchClient, result => result,
                                                                                (client, result) => new SearchWinner
                                                                                {
                                                                                    SearchClient = client,
                                                                                    Winner = result.MaxValue(r => r.TotalResults).Query
                                                                                });

            return winners;
        }
    }
}
