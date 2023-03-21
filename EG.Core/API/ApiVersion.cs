using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EG.Core.API
{
    public class ApiVersion : IApiVersion
    {
        public string CURRENT_VERSION { get; set; }

        /// <summary>
        /// Gets the minor store version
        /// </summary>
        public string MINOR_VERSION { get; set; }

        /// <summary>
        /// Gets the full store version
        /// </summary>
        public string FULL_VERSION
        {
            get
            {
                return CURRENT_VERSION + "." + MINOR_VERSION;
            }
        }

        public ApiVersion()
        {
            CURRENT_VERSION = "0.00";
            MINOR_VERSION = "0";
        }
        public ApiVersion(string currentVersion, string minorVersion)
        {
            CURRENT_VERSION = currentVersion;
            MINOR_VERSION = minorVersion;
        }
    }
}
