using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using RealEstateProj.Data;


namespace RealEstateProj.Components.Pages.Identity
{
    public class AuthService
    {
        private readonly ProtectedLocalStorage _storage;
        private const string Key = "currentUser";

        public User? CurrentUser { get; private set; }

        public AuthService(ProtectedLocalStorage storage)
        {
            _storage = storage;
        }

        public async Task LoadAsync()
        {
            try
            {
                var result = await _storage.GetAsync<User>(Key);
                CurrentUser = result.Success ? result.Value : null;
            }
            catch
            {
                CurrentUser = null;
            }
        }

        public async Task Sighin(User user)
        {
            CurrentUser = user;
            await _storage.SetAsync(Key, user);
        }

        public async Task LogOut()
        {
            CurrentUser = null;
            await _storage.DeleteAsync(Key);
        }

        public bool IsLogged() => CurrentUser != null;
        public bool IsAgent() => CurrentUser?.Role == Role.AGENT;
        public bool IsUser() => CurrentUser?.Role == Role.USER;
    }
}