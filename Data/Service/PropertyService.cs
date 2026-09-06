using System.Collections;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using RealEstateProj.Data.Interfaces;
using System.Reflection;
using System.Linq.Expressions;
using System.Linq.Dynamic.Core;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore; 


using RealEstateProj.Data.DTO;

namespace RealEstateProj.Data.Service
{
    public class PropertyService : IPropertyService
    {
        private IDbContextFactory<AppDbContext> _dbContextFactory;
        public PropertyService(IDbContextFactory<AppDbContext> dbContextFactory) 
        {
            _dbContextFactory = dbContextFactory;
        }

        //CRUD
        public async Task AddProperty(Property property)
        {
            using var context = _dbContextFactory.CreateDbContext();
            context.Properties.Add(property);
            await context.SaveChangesAsync();

        }

        public async Task RemovePropertyById(int id)
        {
            using var context = _dbContextFactory.CreateDbContext();
            var property = await GetPropertyByIdAsync(id);
            context.Properties.Remove(property);
            await context.SaveChangesAsync();

        }

        public async Task<List<Property>> GetAllPropertiesAsync()
        {
            using var context = _dbContextFactory.CreateDbContext();
            return await context.Properties.Include(p => p.Images).ToListAsync();
        }

        public async Task UpdatePropertyAsync(int id, Action<Property> update)
        {
            using var context = _dbContextFactory.CreateDbContext();
            var prop = context.Properties.Find(id);

            if (prop is null)
                return;

            update(prop);
            await context.SaveChangesAsync();

        }

        public async Task<Property> GetPropertyByIdAsync(int id)
        {
            using var context = _dbContextFactory.CreateDbContext();
            return await context.Properties.Include(p => p.Images).FirstOrDefaultAsync(x => x.Id == id); //null if not found
        }
        public async Task<List<Property>> FilterProperties(PropertyFilterDTO f)
        {
            using var context = _dbContextFactory.CreateDbContext();
            IQueryable<Property> query = context.Properties;

            foreach (var prop in typeof(PropertyFilterDTO).GetProperties())
            {
                var val = prop.GetValue(f);
                if (val == null || (val is ICollection c && c.Count == 0)) continue;

                var entityProp = typeof(Property).GetProperty(prop.Name);
                if (entityProp == null) continue; // FreeSearch, MinPrice, MaxPrice no match, handle below

                var param = Expression.Parameter(typeof(Property), "p");
                var left = Expression.Property(param, entityProp);
                var right = Expression.Constant(val);
                var eq = Expression.Equal(left, right);
                query = query.Where(Expression.Lambda<Func<Property, bool>>(eq, param));
            }

            if (!string.IsNullOrWhiteSpace(f.FreeSearch))
                query = query.Where(BuildFreeSearch<Property>(f.FreeSearch));

            if (f.MinPrice != null) query = query.Where(p => p.Price >= f.MinPrice);
            if (f.MaxPrice != null) query = query.Where(p => p.Price <= f.MaxPrice);

            return await query.ToListAsync();
        }
        private static Expression<Func<T, bool>> BuildFreeSearch<T>(string term)
        {
            var param = Expression.Parameter(typeof(T), "p");
            var stringProps = typeof(T).GetProperties().Where(pr => pr.PropertyType == typeof(string));
            var containsMethod = typeof(string).GetMethod(nameof(string.Contains), new[] { typeof(string) })!;
            var termExpr = Expression.Constant(term);

            Expression? body = null;
            foreach (var prop in stringProps)
            {
                var propAccess = Expression.Property(param, prop);
                var notNull = Expression.NotEqual(propAccess, Expression.Constant(null, typeof(string)));
                var contains = Expression.Call(propAccess, containsMethod, termExpr);
                var safe = Expression.AndAlso(notNull, contains);
                body = body == null ? safe : Expression.OrElse(body, safe);
            }

            return Expression.Lambda<Func<T, bool>>(body ?? Expression.Constant(false), param);
        }
        
        //--Filter--
        // public async Task<List<Property>> FilterAsync(PropertyFilterDTO propertyFilter)
        // {//tjngjntjcng
        //     using var context = _dbContextFactory.CreateDbContext();
        //     IQueryable<Property> query = context.Properties.AsQueryable();
        //
        //     foreach (var prop in propertyFilter.GetType().GetProperties())
        //     {
        //         var value = prop.GetValue(propertyFilter);
        //         if (value is null) continue;
        //         if (prop.Name is "MinPrice" or "MaxPrice") continue;
        //
        //         var targetProp = typeof(Property).GetProperty(prop.Name);
        //         if (targetProp is null) continue;
        //
        //         switch (value)
        //         {
        //             case string searchString when string.IsNullOrWhiteSpace(searchString):
        //                 continue;
        //
        //             case string searchString:
        //                 query = query.Where($"{prop.Name}.ToLower().Contains(@0)", new object[] { searchString.ToLower() });
        //                 break;
        //
        //             // Handle any enumerable (arrays, lists, etc.) in one Contains call
        //             case System.Collections.IEnumerable enumerable and not string:
        //                 // convert to object[] (or to the element type array if needed)
        //                 var elements = enumerable.Cast<object>().ToArray();
        //                 if (elements.Length == 0) continue;
        //
        //                 // Pass the whole collection as a single parameter so the dynamic Where can do @0.Contains(Property)
        //                 query = query.Where($"@0.Contains({prop.Name})", new object[] { elements });
        //                 break;
        //
        //             default:
        //                 query = query.Where($"{prop.Name} == @0", new object[] { value });
        //                 break;
        //         }
        //     }
        //
        //     if (propertyFilter.MinPrice is not null)
        //     {
        //         query = query.Where(p => p.Price >= propertyFilter.MinPrice.Value);
        //     }
        //     if (propertyFilter.MaxPrice is not null)
        //     {
        //         query = query.Where(p => p.Price <= propertyFilter.MaxPrice.Value);
        //     }
        //
        //     return await query.ToListAsync();
        // }
        //
        public List<Property> GetPropertiesByRooms(int rooms)
        {
            using var context = _dbContextFactory.CreateDbContext();
            return context.Properties.Where(x => x.Rooms == rooms).ToList();
        }

     
        public List<Property> GetAllForRent()
        {
            using var context = _dbContextFactory.CreateDbContext();
            return context.Properties.Include(p => p.Images).Where(x => x.Status == Status.Rent).ToList();
        }

        public List<Property> GetAllForSale()
        {
            using var context = _dbContextFactory.CreateDbContext();
            return context.Properties.Include(p => p.Images).Where(x => x.Status == Status.Sale).ToList();
        }

      
    }
}
