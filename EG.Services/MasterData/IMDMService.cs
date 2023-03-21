using EG.Models.MDM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EG.Services.MasterData
{
    public interface IMDMService
    {
        public List<ReferentialRecord> GetTableReferentialData(string tableName);
    }
}
