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
    }
}
