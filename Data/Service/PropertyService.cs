using Microsoft.EntityFrameworkCore;
using RealEstateProj.Data.Interfaces;

namespace RealEstateProj.Data.Service
{
    public class PropertyService : IPropertyService
    {
        private IDbContextFactory<AppDbContext> _dbContextFactory;
        public PropertyService(IDbContextFactory<AppDbContext> dbContextFactory) 
        {
            _dbContextFactory = dbContextFactory;
        }

        public void AddProperty(Property property)
        {
            using var context = _dbContextFactory.CreateDbContext();
            context.Properties.Add(property);
            context.SaveChanges();

        }

        public void RemovePropertyById(int id)
        {
            using var context = _dbContextFactory.CreateDbContext();
            var property = GetPropertyById(id);
            context.Properties.Remove(property);
            context.SaveChanges();

        }

        //--Filter--
        public Property GetPropertyById(int id)
        {
            using var context = _dbContextFactory.CreateDbContext();
            var property = context.Properties.FirstOrDefault(x  => x.Id == id);
            return property;        
                }

        public List<Property> GetPropertysListByRooms(int rooms)
        {
            using var context = _dbContextFactory.CreateDbContext();
            return context.Properties.Where(x => x.Rooms == rooms).ToList();
        }

        public List<Property> GetAllProperties()
        {
            using var context = _dbContextFactory.CreateDbContext();
            return context.Properties.ToList();
        }

        public List<Property> GetAllForRent()
        {
            using var context = _dbContextFactory.CreateDbContext();
            return context.Properties.Where( x => x.Status == Status.Rent).ToList();

        }

        public List<Property> GetAllForSale()
        {
            using var context = _dbContextFactory.CreateDbContext();
            return context.Properties.Where(x => x.Status == Status.Sale).ToList();
        }

        public void UpdatePropertyBySmth(int id, Action<Property> update)
        {
            using var context = _dbContextFactory.CreateDbContext();
            var prop = context.Properties.Find(id);

            if (prop is null)
                return;

            update(prop);
            context.SaveChanges();

        }
    }
}
