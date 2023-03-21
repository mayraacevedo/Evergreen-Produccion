using EG.Models.MDM;

namespace EG.PWA.Proxies
{
    public class MasterDataServiceProxy : ApiProxy
    {

        private Dictionary<string, TableInfo[]> _tables = new Dictionary<string, TableInfo[]>();
        private Dictionary<string, List<ReferentialRecord>> _referentialDataCache = new Dictionary<string, List<ReferentialRecord>>();

        public MasterDataServiceProxy(IConfiguration configuration) : base(configuration)
        {
        }

        #region Tablas maestras

        public async Task<List<ReferentialRecord>> GetMasterTableReferentialData(string tableName)
        {

            if (!_referentialDataCache.ContainsKey(tableName))
            {
                HttpClient http2 = await PrepareHttpClient();
                _referentialDataCache.Add(tableName, await http2.GetFromJsonAsync<List<ReferentialRecord>>($"Controllers/MDM/table/{tableName}/referential-data"));
            }
            return _referentialDataCache[tableName].OrderBy(x => x.Nombre).ToList();

        }

        public void FlushCache()
        {
            _referentialDataCache.Clear();
            _tables.Clear();
        }

        #endregion
    }
}
