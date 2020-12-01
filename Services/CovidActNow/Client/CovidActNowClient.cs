using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

using Services.Common;
using Services.CovidActNow.Models;

namespace Services.CovidActNow.Client
{
    public class CovidActNowClient : ICovidActNowClient
    {
        #region Static Fields

        private static readonly HttpClient _httpClient = new HttpClient();
        private static readonly string _apiKey = "78a3565e6e9d4bf0b1fd33473b18a159";

        #endregion

        #region API Methods

        public async Task<ServiceResponse<SingleCountySummary>> GetSingleCountySummary(string countyFipsCode)
        {
            try
            {
                var url = $@"https://api.covidactnow.org/v2/county/{countyFipsCode}.json?apiKey={_apiKey}";
                var response = await _httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();

                var responseData = await response.Content.ReadAsStringAsync();
                var deserializedResponseObj = JsonSerializer.Deserialize<SingleCountySummary>(responseData);
                return new ServiceResponse<SingleCountySummary>(deserializedResponseObj);
            }
            catch (Exception ex)
            {
                return new ServiceResponse<SingleCountySummary>("An error occurred loading county data", ex);
            }
        }

        #endregion
    }
}
