using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EG.Core.API
{
    public interface IApiVersion
    {
        string CURRENT_VERSION { get; set; }
        string FULL_VERSION { get; }
        string MINOR_VERSION { get; set; }
    }
}
