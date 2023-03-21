using EG.Models.MDM;
using EG.Services.BaseSystem;
using Microsoft.AspNetCore.Mvc;
using EG.Services.MasterData;

namespace EG.API.Siembra.Controllers.MDM
{
    [Route("Controllers/[controller]")]
    [ApiController]
    public class MDMController : ApiControllerBase
    {
        private readonly IMDMService _mdmService;
        public MDMController(IMDMService mdmService, ISystemService systemService) : base(systemService)
        {
            _mdmService = mdmService;
        }

        [HttpGet("table/{tableName}/referential-data")]
        public List<ReferentialRecord> GetMasterReferentialData(string tableName)
        {

            var result = _mdmService.GetTableReferentialData(tableName);
            if (result.Count > 0)
            {
                if (result.ElementAt(0).Estado == null)
                    return result;
                else
                    return result.Where(x => x.Estado.Equals(true)).ToList();
            }
            else
                return result;
        }

    }

}

