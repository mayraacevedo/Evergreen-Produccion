using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EG.Core.Excel
{
    public interface IExcelProvider
    {
        public string GenerateExcel(string name, DataTable data);

    }
}
