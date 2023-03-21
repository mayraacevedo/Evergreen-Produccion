using EG.Models;
using EG.Models.Util;
using System.Collections.Specialized;
using System.Linq.Expressions;

namespace EG.DAL
{
    public interface IRepository<T> where T : EntityBase
    {
        Task<List<T>> GetAll();
        Task<T> Get(object id, string includeProperties = "");
        Task<T> Add(T entity);
        Task<T> Update(T entity);
        Task<T> Delete(object id);

        Task<PagedEntityQueryResult<T>> Get(Expression<Func<T, bool>> filter = null,
          Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
          string includeProperties = "",
          int pageSize = 10,
          int pageIndex = 0);

        Task<PagedEntityQueryResult<T>> Get(int pageSize, int pageNumber, string sortField = "", string sortOrder = "", NameValueCollection? where = null, bool includeReferentialProperties = true);
    }
}
