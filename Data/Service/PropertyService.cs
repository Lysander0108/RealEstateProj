using Microsoft.EntityFrameworkCore;

namespace RealEstateProj.Data.Service
{
    public class PropertyService
    {
        private IDbContextFactory<AppDbContext> _dbContextFactory;
        public PropertyService(IDbContextFactory<AppDbContext> dbContextFactory) 
        {
            _dbContextFactory = dbContextFactory;
        }
<<<<<<< HEAD
=======

        public void AddProperty(Property property)
        {
            using var context = _dbContextFactory.CreateDbContext();
            context.Properties.Add(property);
            context.SaveChanges();

        }
>>>>>>> 1c2867a (updated UserService)
    }
}
