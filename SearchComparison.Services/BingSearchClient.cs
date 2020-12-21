using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Configuration;
using SearchComparison.Entities.Bing;
using SearchComparison.Util;

namespace SearchComparison.Services
{
    public class BingSearchClient : ISearchClient
    {
        public string ClientName => "Bing";
        private static readonly HttpClient _httpClient;

        static BingSearchClient()
        {
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri(ConfigurationManager.AppSettings["BingURL"]),
                DefaultRequestHeaders = { { "Ocp-Apim-Subscription-Key", ConfigurationManager.AppSettings["BingKey"] } }
            };
        }

        public async Task<long> GetResultsCountAsync(string query)
        {
            try
            {
                using (var response = await _httpClient.GetAsync($"?q={query}"))
                {
                    if (!response.IsSuccessStatusCode)
                        throw new Exception("Error while processing the request");

                    var result = await response.Content.ReadAsStringAsync();
                    var bingResponse = result.DeserializeJson<BingSearchResponse>();
                    return long.Parse(bingResponse.WebPages.TotalEstimatedMatches);
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
    }
}
