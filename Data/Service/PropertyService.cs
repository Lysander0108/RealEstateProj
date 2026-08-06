using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using RealEstateProj.Data.Interfaces;
using System.Reflection;
using System.Linq.Expressions;

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

        //--Filter--
        public async Task<List<Property>> FilterAsync(Property property, int? minPrice, int? maxPrice)
        {
            using var context = _dbContextFactory.CreateDbContext();
            IQueryable<Property> query = context.Properties.AsQueryable();

            foreach (var prop in property.GetType().GetProperties())
            {
                if( prop.Name.Equals("Price") || prop.GetValue(property) is null || prop.Name.Equals("ID")|| prop.Name.Equals("Images")) continue;
                
                var parameter = Expression.Parameter(typeof(Property), "p");
                var propertyExpresion = Expression.Property(parameter, prop.Name);
                var constand = Expression.Constant(prop.GetValue(property), prop.PropertyType);
                var equal = Expression.Equal(propertyExpresion, constand);
                var lambda = Expression.Lambda<Func<Property,bool>>(equal, parameter);
                
                query = query.Where(lambda);
                
            }
            
            if(minPrice.HasValue && maxPrice.HasValue && minPrice <= maxPrice)
            {
                query = query.Where(x => x.Price >= minPrice && x.Price <= maxPrice);
            }
            return await query.ToListAsync();
        }
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
