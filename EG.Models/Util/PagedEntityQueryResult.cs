using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EG.Models.Util
{
    public class PagedEntityQueryResult<T>
    {
        public int PageSize { get; set; }
        public int TotalRecords { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages
        {
            get
            {
                if (PageSize > 0)
                {
                    //double totalPages = ;
                    return Convert.ToInt32(Math.Ceiling(Convert.ToDouble(TotalRecords) / Convert.ToDouble(PageSize)));
                }
                else
                {
                    return 0;
                }
            }
        }
        public List<T>? Data { get; set; }

    }
}
