using Microsoft.AspNetCore.Mvc;
using EG.DAL;
using EG.Services.BaseSystem;

namespace EG.API.Siembra
{
    public class ApiControllerBase : ControllerBase
    {
        private readonly ISystemService _systemService;
    
        public ApiControllerBase(ISystemService systemService)
        {
            _systemService = systemService;
        }

    }
}
