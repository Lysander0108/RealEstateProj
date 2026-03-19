using Microsoft.EntityFrameworkCore;

namespace RealEstateProj.Data.Service
{
    public class PropertyImageService
    {
        private IDbContextFactory<AppDbContext> _dbContextFactory;
        public PropertyImageService(IDbContextFactory<AppDbContext> dbContextFactory)
        {
            _dbContextFactory = dbContextFactory;
        }
    }
}
