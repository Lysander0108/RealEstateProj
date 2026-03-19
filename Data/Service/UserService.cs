using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RealEstateProj.Data.Interfaces;

namespace RealEstateProj.Data.Service
{
    public class UserService : IUserService
    {
        private IDbContextFactory<AppDbContext> _dbContextFactory;
        private PasswordHasher<User> _passwordhasher = new();
        public UserService(IDbContextFactory<AppDbContext> dbContextFactory)
        {
            _dbContextFactory = dbContextFactory;
        }

        public void AddUser(User user)
        {
            using var context = _dbContextFactory.CreateDbContext();
            user.PasswordHashed = _passwordhasher.HashPassword(user, user.PasswordHashed);
            context.Users.Add(user);
            context.SaveChanges();
        }

        public void DeleteUserByName(string userName)
        {
            var user = GetUserByUserName(userName) ?? throw new Exception("No user found");
            using var context = _dbContextFactory.CreateDbContext();

            context.Remove(user);

        }

        public void UpdateUserByName(User user, string newUsername)
        {
            var User = GetUserByUserName(user.UserName) ?? throw new Exception("No User found");
            user.UserName = newUsername;

            using var context = _dbContextFactory.CreateDbContext();

            context.Update(user);
            context.SaveChanges();

        }

        public bool VerifyPassword(User user, string password)
        {
            var result = _passwordhasher.VerifyHashedPassword(user, user.PasswordHashed ,password);
            return result == PasswordVerificationResult.Success;
        }

        //--filter--
        public User GetUserByUserName(string userName)
        {
            using var context = _dbContextFactory.CreateDbContext();
            var User = context.Users.FirstOrDefault(x => x.UserName == userName);
            return User;
        }

        public User GetUserByEmail(string email)
        {
            using var context = _dbContextFactory.CreateDbContext();
            var user = context.Users.FirstOrDefault(x => x.Email == email);
            return user;
        }
    }
}
