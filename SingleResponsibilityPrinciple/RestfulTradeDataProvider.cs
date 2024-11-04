using SingleResponsibilityPrinciple.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace SingleResponsibilityPrinciple
{
    public class RestfulTradeDataProvider : ITradeDataProvider
    {
        string url;
        ILogger logger;
        private readonly HttpClient _httpClient;

        public RestfulTradeDataProvider(string url, ILogger logger)
        {
            this.url = url;
            this.logger = logger;
            _httpClient = new HttpClient();
        }

        public IEnumerable<string> GetTradeData()
        {
            // URL for the API endpoint
            string url = "https://unit9trader.azurewebsites.net/api/TradeData";

            // Send the GET request and wait for the result
            var response = _httpClient.GetAsync(url).Result;

            // Ensure the response was successful
            if (!response.IsSuccessStatusCode)
            {
                // log error and throw an exception if the URL fails
                logger.LogInfo("Failed to read URL.");
                throw new HttpRequestException();
            }

            // Read the response content as a string synchronously
            string jsonString = response.Content.ReadAsStringAsync().Result;

            // Deserialize the JSON string into a list of strings
            List<string> trades = JsonSerializer.Deserialize<List<string>>(jsonString);

            return trades;
        }

    }
}
