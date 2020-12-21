using SearchComparison.Entities.Google;
using SearchComparison.Util;
using System;
using System.Configuration;
using System.Net.Http;
using System.Threading.Tasks;

namespace SearchComparison.Services
{
    class GoogleCustomSearchClient : ISearchClient
    {
        public string ClientName => "Google";
        private static readonly HttpClient _httpClient = new HttpClient();
        private readonly string _googleUrl;

        public GoogleCustomSearchClient()
        {
            _googleUrl = ConfigurationManager.AppSettings["GoogleUrl"].Replace("{0}", ConfigurationManager.AppSettings["GoogleAPIKey"]).Replace("{1}", ConfigurationManager.AppSettings["GoogleCEKey"]);
        }

        public async Task<long> GetResultsCountAsync(string query)
        {
            try
            {
                using (var response = await _httpClient.GetAsync(_googleUrl.Replace("{2}", query)))
                {
                    if (!response.IsSuccessStatusCode)
                        throw new Exception("Error while processing the request");

                    var result = await response.Content.ReadAsStringAsync();
                    var googleResponse = result.DeserializeJson<GoogleCustomSearchResponse>();
                    return long.Parse(googleResponse.SearchInformation.TotalResults);
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
    }
}