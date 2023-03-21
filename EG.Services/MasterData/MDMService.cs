using EG.DAL;
using EG.Models.MDM;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EG.Services.MasterData
{
    public class MDMService : IMDMService
    {
        private protected EGDbContext _dbContext = null;
        public MDMService(EGDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public List<ReferentialRecord> GetTableReferentialData(string tableName)
        {
            string typeName = $"EG.Models.Entities.{tableName},EG.Models";
            Type type = Type.GetType(typeName);

            IQueryable<object> q = (IQueryable<Object>)_dbContext.GetType().GetMethods().Where(x=> x.Name=="Set").FirstOrDefault().MakeGenericMethod(type).Invoke(_dbContext, null);

            //Serializamos el primer resultado
            //TODO: Mejorar como no depender de las propiedades Id y Nombre
            var result = System.Text.Json.JsonSerializer.Serialize(q.ToList());

            //Volvemos a cargar en la nueva lista
            var result2 = System.Text.Json.JsonSerializer.Deserialize<List<ReferentialRecord>>(result);

            return result2;
        }
    }
}
