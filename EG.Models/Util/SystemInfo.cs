using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EG.Models.Util
{
    public class SystemInfo
    {
        public SystemInfo()
        {
            Headers = new List<HeaderModel>();
            LoadedAssemblies = new List<LoadedAssembly>();
            NotificationServiceParameters = new Dictionary<string, string>();
        }

        public string AspNetInfo { get; set; }

        public bool IsFullTrust { get; set; }

        public string ApiVersion { get; set; }

        public string OperatingSystem { get; set; }

        public DateTime ServerLocalTime { get; set; }

        public string ServerTimeZone { get; set; }

        public DateTime UtcTime { get; set; }

        public DateTime CurrentUserTime { get; set; }

        public string CurrentStaticCacheManager { get; set; }

        public string HttpHost { get; set; }

        public IList<HeaderModel> Headers { get; set; }

        public IList<LoadedAssembly> LoadedAssemblies { get; set; }

        public bool AzureBlobStorageEnabled { get; set; }

        public Dictionary<string, string> NotificationServiceParameters { get; set; }

        public partial record HeaderModel
        {
            public string Name { get; set; }
            public string Value { get; set; }
        }

        public partial record LoadedAssembly
        {
            public string FullName { get; set; }
            public string Location { get; set; }
            public bool IsDebug { get; set; }
            public DateTime? BuildDate { get; set; }
        }
    }
}
