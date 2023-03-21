using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EG.Models.Util
{
    [Serializable]
    public class ApiException
    {
        public DateTime TimeStamp { get; set; }
        public string Message { get; set; }
        public string StackTrace { get; set; }

        public ApiException InnerException { get; set; }

        public ApiException()
        {
            this.TimeStamp = DateTime.Now;
        }

        public ApiException(string Message) : this()
        {
            this.TimeStamp = DateTime.Now;
            this.Message = Message;
        }

        public ApiException(System.Exception ex)
        {
            this.TimeStamp = DateTime.Now;
            if (ex != null)
            {
                this.Message = ex.Message;
                this.StackTrace = ex.StackTrace;
                if (ex.InnerException != null)
                {
                    this.InnerException = new ApiException(ex.InnerException);
                }
            }
        }

        public override string ToString()
        {
            return this.Message + this.StackTrace;
        }
    }
}
