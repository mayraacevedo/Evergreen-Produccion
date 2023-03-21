using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EG.Core.Excel
{
    public class ExcelProvider : IExcelProvider
    {
       
        //==================================================================
        //======================= Generar un excel genérico
        //==================================================================
        public async Task<Stream> GenerateExcelStream(string name, DataTable data)
        {
            Stream fs = new MemoryStream();
            // Creating a new workbook
            var wb = new XLWorkbook();
            //Adding a worksheet
            var ws = wb.Worksheets.Add(name);
            int f = 1, c = 1, i = 1;
            //Encabezado
            foreach (DataColumn colum in data.Columns)
            {
                ws.Cell(1, i).Value = colum.ColumnName;
                i++;
            }
            f = 2;
            i = 0;
            foreach (DataRow row in data.Rows)
            {
                c = 1;
                i = 0;
                foreach (DataColumn colum in data.Columns)
                {
                    ws.Cell(f, c).Value = row[i].ToString();
                    c++;
                    i++;
                }
                f++;
            }
            wb.SaveAs(fs);
            return fs;
        }

        public string GenerateExcel(string name, DataTable data)
        {
            // Creating a new workbook
            var wb = new XLWorkbook();

            //Adding a worksheet
            var ws = wb.Worksheets.Add(name);
            int f = 1, c = 1, i = 1;

            //Encabezado
            foreach (DataColumn colum in data.Columns)
            {
                ws.Cell(1, i).Value = colum.ColumnName;
                i++;
            }

            f = 2;
            i = 0;
            foreach (DataRow row in data.Rows)
            {
                c = 1;
                i = 0;
                foreach (DataColumn colum in data.Columns)
                {
                    ws.Cell(f, c).Value =row[i].ToString();
                    c++;
                    i++;
                }
                f++;
            }

            if (!System.IO.Directory.Exists("Files"))
                System.IO.Directory.CreateDirectory("Files");

            string fileName = Guid.NewGuid().ToString() + ".xlsx";
            string filePath = System.IO.Path.Combine("Files", fileName);
            wb.SaveAs(filePath);
            wb.Dispose();


            return filePath;
        }
    }
}
