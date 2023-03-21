using Microsoft.Extensions.Configuration;
using EG.Models.Util;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using EG.Models.Util;

namespace EG.PWA.Proxies
{
    public class ApiProxy
    {

        private readonly IConfiguration _configuration;

        public ApiProxy( IConfiguration configuration)
        {
            _configuration = configuration;
         
        }

        internal async Task<HttpResponseMessage> GetAsync(string? requestUri)
        {
            HttpClient client = await PrepareHttpClient();
            var response = await client.GetAsync(requestUri);
            await ValidateResponse(response);
            return response;
        }

        internal async Task<TValue> GetPagedFromJsonAsync<TValue>(string? requestUri, Dictionary<string, object> filter, int pageIndex, int pageSize, string sortField = "", string sortOrder = "")
        {
            //var properties = EntityBase.ParseToKeyValueList("Portafolio", filter);
            StringBuilder filterString = new StringBuilder();

            if (filter != null)
            {
                foreach (var property in filter)
                {
                    //Logger.LogInformation(property.Key, property.Value);
                    if (property.Value == null || property.Value.ToString() == string.Empty || (property.Value.ToString() == "0"))
                    {
                        filter.Remove(property.Key);
                    }
                    else
                    {
                        filterString.Append($"{property.Key}={property.Value}&");
                    }

                }

                if (filterString.Length > 0)
                {
                    filterString = new StringBuilder($"?{filterString.Remove(filterString.Length - 1, 1)}");
                }

            }

            string sortParameter = string.Empty;

            if (!string.IsNullOrEmpty(sortField))
            {
                sortOrder = sortOrder == "Descending" || sortOrder == "desc" ? "desc" : "asc";

                sortParameter = $"/{sortField}/{sortOrder}";
            }

            HttpClient client = await PrepareHttpClient();
            var response = await client.GetAsync($"{requestUri}/{pageSize}/{pageIndex}{sortParameter}{filterString.ToString()}");
            await ValidateResponse(response);
            return await response.Content.ReadFromJsonAsync<TValue>();
        }

        internal async Task<TValue> GetFromJsonAsync<TValue>(string? requestUri)
        {
            HttpClient client = await PrepareHttpClient();
            client.Timeout = TimeSpan.FromMinutes(10);
            var response = await client.GetAsync(requestUri);
            await ValidateResponse(response);
            return await response.Content.ReadFromJsonAsync<TValue>();
        }

        internal async Task<TValue> PostAsync<TValue>(string? requestUri, TValue content)
        {
            string jsonOject = JsonSerializer.Serialize(content);
            var jsonContent = new StringContent(jsonOject, Encoding.UTF8, "application/json");
            HttpClient client = await PrepareHttpClient();
            var response = await client.PostAsync(requestUri, jsonContent);
            await ValidateResponse(response);
            return await response.Content.ReadFromJsonAsync<TValue>();
        }

        internal async Task<TValue> PostAsync<TValue>(string? requestUri, HttpContent? content)
        {
            HttpClient client = await PrepareHttpClient();
            client.Timeout = TimeSpan.FromMinutes(10);
            var response = await client.PostAsync(requestUri, content);
            await ValidateResponse(response);
            return await response.Content.ReadFromJsonAsync<TValue>();
        }

        internal async Task<TValue> PutAsync<TValue>(string? requestUri, TValue content)
        {
            string jsonOject = JsonSerializer.Serialize(content);
            var jsonContent = new StringContent(jsonOject, Encoding.UTF8, "application/json");
            HttpClient client = await PrepareHttpClient();
            var response = await client.PutAsync(requestUri, jsonContent);
            await ValidateResponse(response);
            return await response.Content.ReadFromJsonAsync<TValue>();
        }

        internal async Task<TValue> PutAsync<TValue>(string? requestUri, HttpContent? content)
        {
            HttpClient client = await PrepareHttpClient();
            var response = await client.PutAsync(requestUri, content);
            await ValidateResponse(response);
            return await response.Content.ReadFromJsonAsync<TValue>();
        }
        internal async Task<HttpResponseMessage> DeleteAsync<TValue>(string? requestUri)
        {
            HttpClient client = await PrepareHttpClient();
            var response = await client.DeleteAsync(requestUri);
            await ValidateResponse(response);
            return response;
        }

        #region Util
        private async Task ValidateResponse(HttpResponseMessage? httpResponseMessage)
        {
            if (!httpResponseMessage.IsSuccessStatusCode)
            {
                var exception = await httpResponseMessage.Content.ReadFromJsonAsync<ApiException>();
                throw new Exception(exception.Message);
            }
        }

        internal async Task<HttpClient> PrepareHttpClient()
        {
            HttpClient http2 = new HttpClient { BaseAddress = new Uri(_configuration["EGApiUri"]) };
            return http2;

        }
        #endregion
    }
}
