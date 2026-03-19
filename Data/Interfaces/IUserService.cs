namespace RealEstateProj.Data.Interfaces
{
    public interface IUserService
    {
        public void AddUser(User user);
        public bool VerifyPassword(User user, string password);
        public User GetUserByUserName(string userName);
        public void UpdateUserByName(User user, string newUsername);
        public void DeleteUserByName(string userName);
        public User GetUserByEmail(string email);


    }
}
