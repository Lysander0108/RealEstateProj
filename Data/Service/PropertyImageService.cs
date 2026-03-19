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
    }
}
