using EG.Models.Util;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EG.Services.Sembrado
{
    public interface ISembradoService
    {
        Task<PagedEntityQueryResult<object>> Get(int pageSize, int pageNumber, NameValueCollection? where = null);
        Task<EG.Models.Entities.Sembrado> GetDetail(int Id);
        Task<bool> Delete(int Id);
        Task<List<EG.Models.Entities.Sembrado>> GetExcelInfo(int pageSize, int pageNumber, NameValueCollection? where = null);
    }
}
