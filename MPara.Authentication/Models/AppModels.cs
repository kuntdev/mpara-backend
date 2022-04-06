using System;
namespace MPara.Authentication.Models
{
    public class AppUserModel
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }

    public class Token
    {
        public string AccessToken { get; set; }
        public DateTime Expiration { get; set; }
        public string RefreshToken { get; set; }
    }

}
