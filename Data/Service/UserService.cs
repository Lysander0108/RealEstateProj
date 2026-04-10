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
          
            using var context = _dbContextFactory.CreateDbContext();
            var user = context.Users.FirstOrDefault() ?? throw new Exception("No user found");

            context.Remove(user);
            context.SaveChanges();

        }

        public void DeleteUserById(int id)
        {
            using var context = _dbContextFactory.CreateDbContext();
            var user = context.Users.FirstOrDefault() ?? throw new Exception("No user found"); ;

            context.Remove(user);
            context.SaveChanges();
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

        public User GetUserById(int id)
        {
            using var context = _dbContextFactory.CreateDbContext();
            var user = context.Users.FirstOrDefault(x => x.ID == id);
            return user;
        }

        public List<User> GetAllUsers()
        {
            using var context = _dbContextFactory.CreateDbContext();
            return context.Users.ToList();
        }

        public void UpdateUserName(int id, string newUsername)
        {
            using var context = _dbContextFactory.CreateDbContext();
            var user = context.Users.Find(id) ?? throw new Exception("no user found hehheehhe");
            user.UserName = newUsername;
            context.SaveChanges();
        }

        public void UpdateUserEmail(int id, string newEmail)
        {
            using var context = _dbContextFactory.CreateDbContext();
            var user = context.Users.Find(id) ?? throw new Exception("no user found hehheehhe");
            user.UserName = newEmail;
            context.SaveChanges();
        }
    }
 }


