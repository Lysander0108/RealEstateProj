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
        public User GetUserById(int id);
        public void DeleteUserById(int id);
        public List<User> GetAllUsers();
        public void UpdateUserName(int id, string newUsername);
        public void UpdateUserEmail(int id, string newEmail);
    }
}
