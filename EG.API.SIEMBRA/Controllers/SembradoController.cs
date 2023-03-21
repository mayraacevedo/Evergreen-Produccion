using EG.Models.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using EG.DAL.Repositories;
using EG.Services.BaseSystem;
using EG.Services.Sembrado;
using EG.Models.Util;
using System.Data;
using System.Web;
using EG.Core.Excel;

namespace EG.API.Siembra.Controllers
{
    [Route("Controllers/[controller]")]
    [ApiController]
    public class SembradoController : EntityControllerBase<Sembrado, SembradoRepository>
    {
        private readonly ISembradoService _sembradoService;
        public SembradoController(
           ISembradoService sembradoService,
           ISystemService systemService,
           SembradoRepository repository) : base(repository, systemService)
        {
            _sembradoService = sembradoService;
        }
        [HttpGet("GetInfo/{pageSize}/{pageNumber}")]
        [ProducesResponseType(typeof(ApiException), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(Task<List<ActionResult>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetInfo(int pageSize, int pageNumber)
        {
            try
            {
                var filter = HttpUtility.ParseQueryString(Request.QueryString.Value);

                return Ok(await _sembradoService.Get(pageSize, pageNumber, filter));
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiException(ex));
            }
        }
        [HttpGet("GetDetail/{Id}")]
        [ProducesResponseType(typeof(ApiException), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(Task<Sembrado>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetDetail(int Id)
        {
            try
            {
                return Ok(await _sembradoService.GetDetail(Id));
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiException(ex));
            }
        }
        [HttpDelete("Delete/{Id}")]
        [ProducesResponseType(typeof(ApiException), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(Task<bool>), StatusCodes.Status200OK)]
        public async Task<ActionResult> Delete(int Id)
        {
            try
            {
                return Ok(await _sembradoService.Delete(Id));
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiException(ex));
            }
        }
        [HttpGet("GetExcel/{pageSize}/{pageNumber}")]
        [ProducesResponseType(typeof(ApiException), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(Task<List<ActionResult>>), StatusCodes.Status200OK)]
        public async Task<ActionResult> GetExcel(int pageSize, int pageNumber)
        {
            try
            {
                ExcelProvider excel = new ExcelProvider();

                var filter = HttpUtility.ParseQueryString(Request.QueryString.Value);

                var list = await _sembradoService.GetExcelInfo(Int32.MaxValue, pageNumber, filter);

                DataTable table = new DataTable();


                table.Columns.Add("Codigo");
                table.Columns.Add("Estado Sembrado");
                table.Columns.Add("Observaciones");
                table.Columns.Add("Fecha Creación");
                table.Columns.Add("Parcela");
                table.Columns.Add("Semilla");
                table.Columns.Add("Estado Parcela");
                table.Columns.Add("Observaciones Parcela");
                table.Columns.Add("Fecha creación");

                foreach (var item in list)
                {
                    foreach (var r in item.SembradosDets)
                    {
                        DataRow row = table.NewRow();
                        row[0] = item.Codigo;
                        row[1] = item.IdEstadoNavigation.Nombre;
                        row[2] = item.Observaciones;
                        row[3] = item.FechaCreacion;
                        row[4] = r.IdParcelaNavigation.Nombre;
                        row[5] = r.IdSemillaNavigation.Nombre;
                        row[6] = r.IdEstadoNavigation.Nombre;
                        row[7] = r.Observaciones;
                        row[8] = r.FechaCreacion;

                        table.Rows.Add(row);
                    }
                }

                var filePath = excel.GenerateExcel("Sembrado", table);

                var contentType = "application/octet-stream";
                Response.Headers.Add("Content-Disposition", $"attachment; filename=\"{"Sembrado.xlsx"}\"");
                Response.ContentType = contentType;
                FileStream stream = System.IO.File.OpenRead(filePath);
                return File(stream, contentType, "Sembrado.xlsx");
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiException(ex));
            }
        }
    }
}
