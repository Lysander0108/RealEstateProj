using System.Linq.Expressions;
using RealEstateProj.Data.DTO;

namespace RealEstateProj.Data.Interfaces
{
    public interface IPropertyService
    {
        // CRUD

        Task AddProperty(Property property);

        Task RemovePropertyById(int id);

        public Task<List<Property>> GetAllPropertiesAsync();

        Task UpdatePropertyAsync(int id, Action<Property> update);

        Task<Property?> GetPropertyByIdAsync(int id);


        // THE filter!!!!! 
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

        public Task<List<Property>> FilterProperties(PropertyFilterDTO f);
        //public Task<List<Property>> FilterAsync(PropertyFilterDTO propertyFilter);
        
        // Filters

        List<Property> GetPropertiesByRooms(int rooms);

        List<Property> GetAllForRent();

        List<Property> GetAllForSale();
    }
}
