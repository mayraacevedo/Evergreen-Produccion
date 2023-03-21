using System;
using System.Linq;
using System.Reflection;

namespace Microsoft.EntityFrameworkCore
{
    public static partial class CustomExtensions
    {

        public static IQueryable Query(this DbContext context, string entityName) =>
            context.Query(context.Model.FindEntityType(entityName).ClrType);

        static readonly MethodInfo SetMethod = typeof(DbContext).GetMethod(nameof(DbContext.Set));

        public static IQueryable Query(this DbContext context, Type entityType) =>
            (IQueryable)SetMethod.MakeGenericMethod(entityType).Invoke(context, null);


        public static IQueryable<Object> Set(this DbContext _context, Type t)
        {
            //return (IQueryable<Object>)_context.GetType().GetMethod("Set").MakeGenericMethod(t).Invoke(_context, null);
            return (IQueryable<Object>)_context.GetType().GetMethods().Where(x => x.Name == "Set").FirstOrDefault(x => x.IsGenericMethod).MakeGenericMethod(t).Invoke(_context, null);

        }

        public static dynamic GetDbSet(this DbContext _context, Type t)
        {
            //return (IQueryable<Object>)_context.GetType().GetMethod("Set").MakeGenericMethod(t).Invoke(_context, null);
            return _context.GetType().GetMethods().Where(x => x.Name == "Set").FirstOrDefault(x => x.IsGenericMethod).MakeGenericMethod(t).Invoke(_context, null);

        }

        public static dynamic GetDbReadOnlySet(this DbContext _context, Type t)
        {
            //return (IQueryable<Object>)_context.GetType().GetMethod("Set").MakeGenericMethod(t).Invoke(_context, null);
            return _context.GetType().GetMethods().Where(x => x.Name == "Set").FirstOrDefault(x => x.IsGenericMethod).MakeGenericMethod(t).Invoke(_context, null);

        }

    }
}