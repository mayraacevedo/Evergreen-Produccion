using Microsoft.AspNetCore.Mvc;
using System.Web;
using EG.Models;
using EG.Models.Util;
using EG.DAL;
using EG.Services.BaseSystem;

namespace EG.API.Siembra
{
    public class EntityControllerBase<TEntity, TRepository> : ApiControllerBase
         where TEntity : EntityBase
         where TRepository : IRepository<TEntity>
    {
        private readonly TRepository _repository;
        protected string _getIncludedProperties = string.Empty;
        protected string _detailIncludedProperties = string.Empty;

        protected TRepository Repository => _repository;

        public EntityControllerBase(
            TRepository repository,
            ISystemService systemService) : base(systemService)
        {
            this._repository = repository;
        }

        // GET: api/[controller]
        [HttpGet("all")]
        public virtual async Task<ActionResult<IEnumerable<TEntity>>> GetAll()
        {
            return await Repository.GetAll();
        }
        // GET: api/[controller]
        [HttpGet("{pageSize}/{pageNumber}/{sortField?}/{sortOrder?}")]
        [ProducesResponseType(typeof(ApiException), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ActionResult<PagedQueryResult>), StatusCodes.Status200OK)]
        public virtual async Task<IActionResult> Get(int pageSize, int pageNumber, string? sortField = "", string? sortOrder = "")
        {
            try
            {
                var filter = HttpUtility.ParseQueryString(Request.QueryString.Value);
               
                return Ok(await Repository.Get(pageSize, pageNumber, sortField, sortOrder, filter, false));
            }
            catch (Exception ex)
            {
                
                return BadRequest(new ApiException(ex));
            }
        }

        // GET: api/[controller]/5
        [HttpGet("{id}")]
        public virtual async Task<ActionResult<TEntity>> Get(int id)
        {
            var movie = await Repository.Get(id, _detailIncludedProperties);
            if (movie == null)
            {
             
                return NotFound();
            }
            return movie;
        }

        // PUT: api/[controller]/5
        [HttpPut("{id}")]
        public virtual async Task<IActionResult> Put(string id, TEntity entity)
        {
            try
            {
                await Repository.Update(entity);
                return Ok(entity);
            }
            catch (Exception ex)
            {
               
                return BadRequest(new ApiException(ex));
            }
        }

        // POST: api/[controller]
        [HttpPost]
        public virtual async Task<ActionResult<TEntity>> Post(TEntity entity)
        {
            try
            {
               entity= await Repository.Add(entity);

                return Ok(entity);
            }
            catch (Exception ex)
            {
             
                return BadRequest(new ApiException(ex));
            }
        }

        // DELETE: api/[controller]/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<TEntity>> Delete(string id)
        {
            var movie = await Repository.Delete(id);
            if (movie == null)
            {
                return NotFound();
            }
            return movie;
        }

    }
}
