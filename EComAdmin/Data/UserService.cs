namespace EComAdmin.Data
{
    public class UserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }


        public void Logout()
        {
            SetUsername(string.Empty);
        }
        public bool IsSignedIn()
        {
            return !string.IsNullOrEmpty(_httpContextAccessor?.HttpContext?.Session.GetString("Username"));
        }

        public void SetUsername(string username)
        {
            _httpContextAccessor?.HttpContext?.Session.SetString("Username", username);
        }

        public string GetUsername()
        {
            return _httpContextAccessor?.HttpContext?.Session.GetString("Username") ?? string.Empty;
        }
    }
}
