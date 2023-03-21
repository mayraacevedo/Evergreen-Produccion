using System.Diagnostics;
using System.Runtime.InteropServices;
using EG.Core.API;
using Microsoft.AspNetCore.Http;
using EG.DAL;
using EG.Models.Util;

namespace EG.Services.BaseSystem
{
    public class SystemService : ISystemService
    {

        private readonly IApiVersion _apiVersion;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly EGDbContext _dbContext = null;

        public SystemService(
            EGDbContext dbContext,
            IApiVersion apiVersion,
            IHttpContextAccessor httpContextAccessor
            )
        {
            _dbContext = dbContext;
            _apiVersion = apiVersion;
            _httpContextAccessor = httpContextAccessor;
           
        }

        public SystemInfo PrepareSystemInfoModel(SystemInfo model)
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            model.ApiVersion = _apiVersion.FULL_VERSION;
            model.ServerTimeZone = TimeZoneInfo.Local.StandardName;
            model.ServerLocalTime = DateTime.Now;
            model.UtcTime = DateTime.UtcNow;
            //model.CurrentUserTime = await _dateTimeHelper.ConvertToUserTimeAsync(DateTime.Now);
            model.CurrentUserTime = DateTime.UtcNow;
            //model.HttpHost = _httpContextAccessor.HttpContext.Request.Headers[HeaderNames.Host];

            //ensure no exception is thrown
            try
            {
                model.OperatingSystem = Environment.OSVersion.VersionString;
                model.AspNetInfo = RuntimeInformation.FrameworkDescription;
                model.IsFullTrust = AppDomain.CurrentDomain.IsFullyTrusted;
            }
            catch
            {
                // ignored
            }

            foreach (var header in _httpContextAccessor.HttpContext.Request.Headers)
            {
                model.Headers.Add(new SystemInfo.HeaderModel
                {
                    Name = header.Key,
                    Value = header.Value
                });
            }

            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                var loadedAssemblyModel = new SystemInfo.LoadedAssembly
                {
                    FullName = assembly.FullName
                };

                //ensure no exception is thrown
                try
                {
                    loadedAssemblyModel.Location = assembly.IsDynamic ? null : assembly.Location;
                    loadedAssemblyModel.IsDebug = assembly.GetCustomAttributes(typeof(DebuggableAttribute), false)
                        .FirstOrDefault() is DebuggableAttribute attribute && attribute.IsJITOptimizerDisabled;

                   
                   // loadedAssemblyModel.BuildDate = assembly.IsDynamic ? null : (DateTime?)TimeZoneInfo.ConvertTimeFromUtc(_fileProvider.GetLastWriteTimeUtc(assembly.Location), TimeZoneInfo.Local);

                }
                catch
                {
                    // ignored
                }

                model.LoadedAssemblies.Add(loadedAssemblyModel);
            }

            return model;
        }



     
       
    }
}
