using Microsoft.EntityFrameworkCore;

namespace RealEstateProj.Data.Service
{
    public class UserService
    {
        private IDbContextFactory<AppDbContext> _dbContextFactory;
        public UserService(IDbContextFactory<AppDbContext> dbContextFactory)
        {
            _dbContextFactory = dbContextFactory;
        }

        public void AddUser(User user)
        {
            using var context = _dbContextFactory.CreateDbContext();
            context.Users.Add(user);
            context.SaveChanges();
        }
    }
}
