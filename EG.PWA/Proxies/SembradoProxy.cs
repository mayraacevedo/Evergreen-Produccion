using EG.Models.Entities;
using EG.Models.Util;
using System.Text;

namespace EG.PWA.Proxies
{
    public class SembradoProxy : ApiProxy
    {
        public SembradoProxy( IConfiguration configuration) : base(configuration)
        {
        }
        public async Task<PagedEntityQueryResult<Sembrado>> GetAll(Dictionary<string, object> filter, int pageIndex, int pageSize, string sortField = "", string sortOrder = "")
        {

            //var properties = EntityBase.ParseToKeyValueList("Portafolio", filter);
            StringBuilder filterString = new StringBuilder();

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

            string sortParameter = string.Empty;

            if (!string.IsNullOrEmpty(sortField))
            {
                sortParameter = $"/{sortField}/{sortOrder}";
            }

            HttpClient http2 = await PrepareHttpClient();
            return await http2.GetFromJsonAsync<PagedEntityQueryResult<Sembrado>>($"Controllers/Sembrado/GetInfo/{pageSize}/{pageIndex - 1}{filterString.ToString()}");
        }
        public async Task<Sembrado> Save(Sembrado item)
        {
            string jsonOject = System.Text.Json.JsonSerializer.Serialize(item);

            return await PostAsync<Sembrado>($"Controllers/Sembrado", new StringContent(jsonOject, Encoding.UTF8, "application/json"));

        }
        public async Task<Sembrado> GetDetail(int Id)
        {
            
            return await GetFromJsonAsync<Sembrado>($"Controllers/Sembrado/GetDetail/{Id}");
           
        }
        public async Task<HttpResponseMessage> Delete(int id)
        {
            return await DeleteAsync<Sembrado>($"Controllers/Sembrado/Delete/{id}");
        }
        public async Task<Stream> GetExcel(Dictionary<string, object> filter, int pageIndex, int pageSize)
        {

            //var properties = EntityBase.ParseToKeyValueList("Portafolio", filter);
            StringBuilder filterString = new StringBuilder();

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

          
            HttpClient http2 = await PrepareHttpClient();

            return await http2.GetStreamAsync($"Controllers/Sembrado/GetExcel/{pageSize}/{pageIndex - 1}{filterString.ToString()}");

        }
    }
}
