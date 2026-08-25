using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using RealEstateProj.Data.Interfaces;
using System.Reflection;
using System.Linq.Expressions;
using System.Linq.Dynamic.Core;

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

        //--Filter--
        public async Task<List<Property>> FilterAsync(PropertyFilterDTO propertyFilter)
        {
            using var context = _dbContextFactory.CreateDbContext();
            IQueryable<Property> query = context.Properties.AsQueryable();

            foreach (var prop in propertyFilter.GetType().GetProperties())
            {
                var value = prop.GetValue(propertyFilter);
                if (value is null) continue;
                if (prop.Name is "MinPrice" or "MaxPrice") continue;

                var targetProp = typeof(Property).GetProperty(prop.Name);
                if (targetProp is null) continue;

                switch (value)
                {
                    case string searchString when string.IsNullOrWhiteSpace(searchString):
                        continue;

                    case string searchString:
                        query = query.Where($"{prop.Name}.ToLower().Contains(@0)", new object[] { searchString.ToLower() });
                        break;

                    case Array arr when arr.Length == 0:
                        continue;

                    case Array:
                        query = query.Where($"@0.Contains({prop.Name})", new object[] { value });
                        break;

                    default:
                        query = query.Where($"{prop.Name} == @0", new object[] { value });
                        break;
                }
            }

            if (propertyFilter.MinPrice is not null)
            {
                query = query.Where(p => p.Price >= propertyFilter.MinPrice.Value);
            }
            if (propertyFilter.MaxPrice is not null)
            {
                query = query.Where(p => p.Price <= propertyFilter.MaxPrice.Value);
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
