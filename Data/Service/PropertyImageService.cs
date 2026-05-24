using Microsoft.EntityFrameworkCore;
using RealEstateProj.Data.Interfaces;

namespace RealEstateProj.Data.Service
{
    public class PropertyImageService : IPropertyImageService
    {
        private IDbContextFactory<AppDbContext> _dbContextFactory;
        public PropertyImageService(IDbContextFactory<AppDbContext> dbContextFactory)
        {
            _dbContextFactory = dbContextFactory;
        }

        public void Add(PropertyImage pi)
        {
            using var context = _dbContextFactory.CreateDbContext();
            context.Entry(pi.property).State = EntityState.Unchanged;
            context.PropertyImages.Add(pi);
            context.SaveChanges();
        }

        public void Delete(int imageID)
        {
            using var context = _dbContextFactory.CreateDbContext();
           var image =  context.PropertyImages.FirstOrDefault(x => x.Id == imageID);
            context.Remove(image);
            context.SaveChanges();
        }

        public PropertyImage GetById(int imageID)
        {
            using var context = _dbContextFactory.CreateDbContext();
            return context.PropertyImages.FirstOrDefault(x => x.Id == imageID);
        }

        public List<PropertyImage> GetByPropertyId(int propertyId)
        {
            using var context = _dbContextFactory.CreateDbContext();
            return context.PropertyImages.Where(x => x.PropertyId == propertyId).ToList();
        }
    }
}
