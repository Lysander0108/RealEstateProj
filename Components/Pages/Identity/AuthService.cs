using RealEstateProj.Data;

namespace RealEstateProj.Components.Pages.Identity
{
    public class AuthService
    {
        public User? currentUser  {get;private set;}
    
        public bool IsLogged() => currentUser != null;

        public bool IsAgent() => currentUser?.Role == Role.AGENT;

        public bool IsUser() => currentUser?.Role == Role.USER;

        public void Sighin(User user) => currentUser = user;

        public void LogOut() => currentUser = null;

        
    }
}
