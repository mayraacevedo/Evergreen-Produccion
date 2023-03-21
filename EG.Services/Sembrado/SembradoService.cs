using EG.DAL;
using EG.DAL.Repositories;
using EG.Models.Util;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EG.Services.Sembrado
{
    public class SembradoService : ISembradoService
    {
        private readonly SembradoRepository _sembradoRepository;

        public SembradoService(SembradoRepository rep)
        {
            _sembradoRepository = rep;
        }
        public async Task<PagedEntityQueryResult<object>> Get(int pageSize, int pageNumber, NameValueCollection? where = null)
        {
            return await _sembradoRepository.Get(pageSize, pageNumber, where);
        }
        public async Task<EG.Models.Entities.Sembrado> GetDetail(int Id)
        {
            return await _sembradoRepository.GetDetail(Id);
        }
        public async Task<bool> Delete(int Id)
        {
            return await _sembradoRepository.Delete(Id);
        }
        public async Task<List<EG.Models.Entities.Sembrado>> GetExcelInfo(int pageSize, int pageNumber, NameValueCollection? where = null)
        {
            return await _sembradoRepository.GetExcelInfo(pageSize, pageNumber, where);
        }
    }
}
