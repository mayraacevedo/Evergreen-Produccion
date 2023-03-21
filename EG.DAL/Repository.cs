using Microsoft.EntityFrameworkCore;
using EG.Models;
using EG.Models.Util;
using System.Collections.Specialized;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;

namespace EG.DAL
{
    public class Repository<TEntity, TContext> : IRepository<TEntity>
       where TEntity : EntityBase
       where TContext : EGDbContext
    {
        private readonly TContext context;
        private readonly TEntity entity;

        public Repository(
            TContext context)
        {
            this.context = context;
            this.entity = entity;
        }
        public virtual async Task<TEntity> Add(TEntity entity)
        {
            context.Set<TEntity>().Add(entity);
            await context.SaveChangesAsync();
            return entity;
        }

        public async Task<TEntity> Delete(object id)
        {
            id = EnsureIdType(id);

            var entity = await context.Set<TEntity>().FindAsync(id);

            if (entity == null)
            {
                return entity;
            }

            context.Set<TEntity>().Remove(entity);
            await context.SaveChangesAsync();

            return entity;
        }

        public async Task<TEntity> Get(object id, string includeProperties = "")
        {
            id = EnsureIdType(id);

            IQueryable<TEntity> query = context.Set<TEntity>();

            if (!string.IsNullOrEmpty(includeProperties))
            {
                foreach (var includeProperty in includeProperties.Split
                    (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProperty);
                }
            }

            Type type = typeof(TEntity);
            var instance = Activator.CreateInstance(type) as EntityBase;
            var schema = instance.GetTableSchema();

            var config = new ParsingConfig
            {
                UseParameterizedNamesInDynamicQuery = true
            };

            if (schema.KeyDataType == typeof(int).Name)
                query = query.Where(config, $"{schema.KeyName} = @0", id);
            else
                query = query.Where(config, $"{schema.KeyName} = @0", id);

            var result = await query.ToListAsync();

            return result.FirstOrDefault();
        }

        public virtual async Task<List<TEntity>> GetAll()
        {
            return await context.Set<TEntity>().ToListAsync();
        }

        public virtual async Task<TEntity> Update(TEntity entity)
        {
            context.Entry(entity).State = EntityState.Modified;
            await context.SaveChangesAsync();
            return entity;
        }

        public async Task<PagedEntityQueryResult<TEntity>> Get(Expression<Func<TEntity, bool>> filter = null,
           Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
           string includeProperties = "",
           int pageSize = 10,
           int pageIndex = 0)
        {
            IQueryable<TEntity> query = context.Set<TEntity>();

            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (!string.IsNullOrEmpty(includeProperties))
            {
                foreach (var includeProperty in includeProperties.Split
                    (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProperty);
                }
            }

            if (orderBy != null)
            {
                query = orderBy(query);
            }

            var pagedResult = new PagedEntityQueryResult<TEntity>();
            pagedResult.TotalRecords = query.Count(); //Ejecutamos el conteo por performance
            pagedResult.CurrentPage = pageIndex + 1;
            pagedResult.PageSize = pageSize;
            pagedResult.Data = query.Skip(pageIndex * pageSize).Take(pageSize).ToList(); //Se obtiene solo la página requerida

            return pagedResult;
        }

        public async Task<PagedEntityQueryResult<TEntity>> Get(
            int pageSize,
            int pageNumber,
            string sortField = "",
            string sortOrder = "",
            NameValueCollection? where = null,
            bool includeReferentialProperties = true)
        {

            Type type = typeof(TEntity);


            var instance = Activator.CreateInstance(type) as EntityBase;

            IQueryable<TEntity> q = context.Set<TEntity>();

            var schema = instance.GetTableSchema();

            if (!string.IsNullOrEmpty(sortField))
            {
                bool desc = sortOrder == "desc";
                q = q.OrderBy(EntityBase.ToPascalCase(sortField), desc);
            }

            if (where != null && where.Count > 0)
            {
                // Create a config object
                var config = new ParsingConfig
                {
                    UseParameterizedNamesInDynamicQuery = true
                };


                foreach (var field in where.AllKeys)
                {
                    if (field.ToLower() != "estado")
                    {
                        var tableField = schema.Fields.Where(f => f.Name.ToLower().Equals(field.ToLower())).FirstOrDefault();
                        if (tableField == null)
                        {
                            throw new ArgumentException("El campo no pertenece a la tabla");
                        }
                        else
                        {
                            if (tableField.Type == "text")
                                q = q.Where(config, $"{field}.Contains(@0)", where[field]);
                            else if (tableField.Type == "number")
                                q = q.Where(config, $"{field} = @0", int.Parse(where[field]));
                            else
                                q = q.Where(config, $"{field} = @0", where[field]);
                        }
                    }
                }
            }


            if (includeReferentialProperties && schema.ReferentialProperties.Count > 0)
            {
                foreach (var fk in schema.ReferentialProperties)
                {
                    q = q.Include(fk);
                }
            }

            var totalRecords = q.Count();

            PagedEntityQueryResult<TEntity> result = new PagedEntityQueryResult<TEntity>();
            result.TotalRecords = totalRecords;
            result.Data = await q.Skip(pageSize * pageNumber).Take(pageSize).ToListAsync();

            return result;
        }

        private object EnsureIdType(object id)
        {
            Type type = typeof(TEntity);
            var instance = Activator.CreateInstance(type) as EntityBase;
            var schema = instance.GetTableSchema();

            if (schema.KeyDataType == typeof(int).Name)
            {
                return int.Parse(id.ToString());
            }
            else
            {
                return id;
            }
        }

    }
}

